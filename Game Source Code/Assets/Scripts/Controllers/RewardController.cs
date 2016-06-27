using UnityEngine;
using System;
using System.Collections.Generic;

public enum RewardType
{
    SendRainwater,
    SellProducts,
    SellRainwater,
    CleanFloods,
    Discount,
    StartStopInitiative
}

public class RewardController : MonoBehaviour
{
    public float doRewardInterval;

    float _timer;

    static RewardController _instance;
    public static RewardController instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.Find("Reward Controller").GetComponent<RewardController>();

            return _instance;
        }
    }

    void Update()
    {
        if (!GameController.instance.hasStarted)
            return;

        if (_timer < doRewardInterval)
            _timer += Time.deltaTime;
        else
        {
            _timer = 0f;
            
            for (int i = RequestController.instance.webData.Count - 1; i >= 0; i--)
            {
                WebData data = RequestController.instance.webData[i];

                if (data.dataType != DataType.Reward)
                    continue;

                switch (data.rewardType)
                {
                    case RewardType.CleanFloods: CleanFloodsReward(data); break;
                    case RewardType.Discount: DiscountReward(data); break;
                    case RewardType.SellProducts: SellProductsReward(data); break;
                    case RewardType.SellRainwater: SellRainwaterReward(data); break;
                    case RewardType.SendRainwater: SendRainwaterReward(data); break;
                    case RewardType.StartStopInitiative: StartStopInitiativeReward(data); break;
                }

                RequestController.instance.webData.RemoveAt(i);
                break;
            }
        }
    }

    void SendRainwaterReward(WebData data)
    {
        int foundRainwater = GatherAvailableRainwater(SettingsController.instance.sendFlatAmountOfRainwater);
        if (foundRainwater > 0)
        {
            if (InitiativeController.instance.isStarted)
            {
                InitiativeController.instance.StoreRainwater(foundRainwater);

                UIController.instance.UpdateInitiativeLabel();
                UIController.instance.AddNews(data.sender, " has send " + (int)(foundRainwater / 1000f) + " liters rainwater to the initiative.");
            }
            else
            {
                GameController.instance.GetVegetableGardenWaypoint().GetComponent<VegetableGarden>().StoreRainwater(foundRainwater);
                UIController.instance.AddNews(data.sender, " has send " + (int)(foundRainwater / 1000f) + " liters rainwater to the vegetable garden.");
            }
        }
        else
        {
            UIController.instance.AddNews(data.sender, " tried to send rainwater to a initiative or vegetable garden, but there was no rainwater left.");
        }
    }

    void SellProductsReward(WebData data)
    {
        if (GameController.instance.storedBeerProducts > 0)
        {
            float[] resultData = GameController.instance.SellBeerProducts();
            UIController.instance.AddNews(data.sender, " has sold " + (int)resultData[0] + " beers for " + (int)resultData[1] + " euros.");
        }
        else if (GameController.instance.storedVegetableProducts > 0)
        {
            float[] resultData = GameController.instance.SellVegetableProducts();
            UIController.instance.AddNews(data.sender, " has sold " + (int)resultData[0] + " vegetables for " + (int)resultData[1] + " euros.");
        }
        else
        {
            UIController.instance.AddNews(data.sender, " tried to sell any products, but there were none left.");
        }
    }

    void SellRainwaterReward(WebData data)
    {
        int foundRainwater = GatherAvailableRainwater(SettingsController.instance.sellFlatAmountOfRainwater);
        if (foundRainwater > 0)
        {
            float liters = foundRainwater / 1000f;
            int moneyEarned = (int)(liters * SettingsController.instance.rainwaterSellValuePerLiter);
            UIController.instance.AddMoneyIncreaseFeedback(UIController.instance.rainwaterLabel.rectTransform.localPosition, (int)moneyEarned);
            UIController.instance.AddNews(data.sender, " has sold " + (int)liters + " for " + moneyEarned + " euroes.");
        }
    }

    void CleanFloodsReward(WebData data)
    {
        GameObject cleanerObj = SpawnController.instance.SpawnCleaner();
        Cleaner cleaner = cleanerObj.GetComponent<Cleaner>();

        if (!cleaner.AssignJob())
        {
            UIController.instance.AddNews(data.sender, " wants to clean some of the floods, but there was none left.");
            Destroy(cleanerObj);
        }

        cleaner.gameObject.SetActive(true);
        UIController.instance.AddNews(data.sender, " is going to clean up some of the floods.");
    }

    void DiscountReward(WebData data)
    {
        Waypoint chosenBuilding = null;
        MeasureType? chosenType = null;

        Waypoint[] buildingsCopy = (Waypoint[])GameController.instance.GetAllBuildingWaypoints().Clone();
        MeasureType[] measureTypesCopy = (MeasureType[])Enum.GetValues(typeof(MeasureType));
        int totalMeasureCount = Enum.GetValues(typeof(MeasureType)).Length;

        MiscUtils.ShuffleArray(buildingsCopy);

        for (int i = 0; i < buildingsCopy.Length; i++)
        {
            RainproofMeasures measures = buildingsCopy[i].GetComponent<RainproofMeasures>();

            if (measures.appliedMeasures.Count == totalMeasureCount)
                continue;

            chosenBuilding = buildingsCopy[i];

            MiscUtils.ShuffleArray(measureTypesCopy);

            for (int j = 0; j < measureTypesCopy.Length; j++)
            {
                MeasureType type = measureTypesCopy[j];
                if (!measures.CanApply(type))
                    continue;

                if (GameController.instance.money < (int)(RainproofMeasures.GetCost((MeasureType)type) * 0.5f))
                    continue;

                chosenType = type;
                break;
            }

            if (chosenType != null)
                break;
        }

        if (chosenBuilding == null || chosenType == null)
        {
            UIController.instance.AddNews(data.sender, " got a a 50% discount on a random rainproofing measure, but there was no room or money.");
            return;
        }

        GameObject builderObj = SpawnController.instance.SpawnBuilder(data);
        builderObj.transform.SetParent(GameObject.Find("Character Holder").transform, false);

        Builder builder = builderObj.GetComponent<Builder>();
        builder.SpawnAt(GameController.instance.GetRandomStartingWaypoint());
        builder.AssignJob((MeasureType)chosenType, chosenBuilding);
        builderObj.SetActive(true);

        UIController.instance.AddNews(data.sender, " got a 50% discount on a " + chosenType.ToString() + " and is going to install it.");
        UIController.instance.AddMoneyDecreaseFeedback(Vector2.zero, (int)(RainproofMeasures.GetCost((MeasureType)chosenType) * 0.5f));
    }

    void StartStopInitiativeReward(WebData data)
    {
        InitiativeController.instance.StartStop();
        UIController.instance.AddNews(data.sender, " has " + ((InitiativeController.instance.isStarted) ? " started " : " stopped") + " the neighbourhood initiative.");
    }


    int GatherAvailableRainwater(int amount)
    {
        int foundRainwater = 0;

        Waypoint[] allBuildings = GameController.instance.GetAllBuildingWaypoints();
        foreach (Waypoint building in allBuildings)
        {
            RainproofMeasures measures = building.GetComponent<RainproofMeasures>();
            if (measures.appliedMeasures.Contains(MeasureType.RainBarrel))
                foundRainwater += measures.TakeRainwater(amount - foundRainwater);
        }

        return foundRainwater;
    }
}