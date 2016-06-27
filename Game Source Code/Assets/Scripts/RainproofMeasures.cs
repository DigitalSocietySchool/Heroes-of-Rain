using UnityEngine;
using System.Collections.Generic;

public enum MeasureType
{
    GreenGarden,
    RainBarrel,
    GreenRoof,
    DrainPipe,
    Threshold,
    VerticalGarden,
    TemporalDams
}

public class RainproofMeasures : MonoBehaviour
{
    public List<MeasureType> appliedMeasures = new List<MeasureType>();

    public bool isRainbarrelFull
    {
        get
        {
            if (appliedMeasures.Contains(MeasureType.RainBarrel))
                return _storedRainwater == _maxRainwaterStorage;

            return true;
        }
    }

    static Dictionary<MeasureType, int> _costValues = new Dictionary<MeasureType, int>();

    public static void AddCostValue(MeasureType type, int cost)
    {
        if (!_costValues.ContainsKey(type))
            _costValues.Add(type, cost);
    }
    
    public static int GetCost(MeasureType type)
    {
        return _costValues[type];
    }

    int _storedRainwater;
    public int storedRainwater
    {
        get { return _storedRainwater; }
    }

    float _maxRainwaterStorage;

    List<MeasureType> _plannedMeasures = new List<MeasureType>();
    public List<MeasureType> plannedMeasures
    {
        get { return _plannedMeasures; }
    }

    void Awake()
    {
        TimeController.OnMonthPassed += TimeController_OnMonthPassed;
        _maxRainwaterStorage = SettingsController.instance.maximumRainbarrelStorage;
    }

    void Start()
    {
        //ApplyMeasure(MeasureType.DrainPipe);
        //ApplyMeasure(MeasureType.GreenGarden);
        //ApplyMeasure(MeasureType.GreenRoof);
        //ApplyMeasure(MeasureType.RainBarrel);
        //ApplyMeasure(MeasureType.TemporalDams);
        //ApplyMeasure(MeasureType.Threshold);
        //ApplyMeasure(MeasureType.VerticalGarden);
    }

    void TimeController_OnMonthPassed()
    {
        if (TimeController.instance.timeProgress == 1f)
            return;

        int profit = CalculateProfit();
        if (profit > 0)
        {
            Vector2 screenSize = new Vector2(Screen.width, Screen.height);
            Vector2 canvasLocation = Camera.main.WorldToViewportPoint(new Vector2(transform.position.x * screenSize.x, transform.position.y * screenSize.y));
            UIController.instance.AddMoneyIncreaseFeedback(canvasLocation, profit);
        }
    }

    public void PlanMeasure(MeasureType type)
    {
        if (!_plannedMeasures.Contains(type))
            _plannedMeasures.Add(type);
    }

    public void RemovePlannedMeasure(MeasureType type)
    {
        _plannedMeasures.Remove(type);
    }

    void TakeDamage(int mm)
    {
        float singleFactor = SettingsController.instance.maximumDamageReduction / System.Enum.GetValues(typeof(MeasureType)).Length;
        float combinedReversedFactor = 1f - (appliedMeasures.Count * singleFactor);
        int calculatedMM = Mathf.FloorToInt(combinedReversedFactor * mm);

        GameController.instance.currentDamage += (int)(calculatedMM / 10f);
        UIController.instance.UpdateDamageCounter();
    }

    public int TakeRainwater(int amount)
    {
        if (amount < _storedRainwater)
        {
            _storedRainwater -= amount;
            return amount;
        }
        else
        {
            int temp = _storedRainwater;
            _storedRainwater = 0;

            return temp;
        }
    }

    public void RainUpon(int amount)
    {
        TakeDamage(amount);

        if (!isRainbarrelFull)
            _storedRainwater = (int)Mathf.Clamp(_storedRainwater + amount, 0f, _maxRainwaterStorage);

        UIController.instance.UpdateRainwaterMeter();
    }

    public void ApplyMeasure(MeasureType type)
    {
        if (!appliedMeasures.Contains(type))
        {
            Transform child = transform.FindChild(type.ToString());
            if (child != null)
                child.gameObject.SetActive(true);

            appliedMeasures.Add(type);

            UIController.instance.UpdateRainwaterMeter();
        }
    }

    public bool CanApply(MeasureType type)
    {
        return !appliedMeasures.Contains(type) && !plannedMeasures.Contains(type);
    }

    public int CalculateProfit()
    {
        int amount = 0;

        foreach (MeasureType type in appliedMeasures)
            amount += (int)(RainproofMeasures.GetCost(type) * SettingsController.instance.costPercentageProfitPerMonth);

        return amount;
    }
}