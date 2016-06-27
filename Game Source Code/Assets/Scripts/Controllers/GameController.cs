using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject scoreCanvas;
    public GameObject uiCanvas;

    Waypoint[] _allWaypoints;
    public Waypoint[] GetAllWaypoints()
    {
        if (_allWaypoints == null)
            _allWaypoints = FindObjectsOfType<Waypoint>();

        return _allWaypoints;
    }

    Waypoint[] _allStartingWaypoints;
    public Waypoint[] GetAllStartingWaypoints()
    {
        if (_allStartingWaypoints == null)
        {
            _allStartingWaypoints = new Waypoint[1];

            GameObject[] startingWaypoints = GameObject.FindGameObjectsWithTag("Starting Waypoint");
            for (int i = 0; i < startingWaypoints.Length; i++)
            {
                if (i > 0)
                    System.Array.Resize<Waypoint>(ref _allStartingWaypoints, _allStartingWaypoints.Length + 1);

                _allStartingWaypoints[i] = startingWaypoints[i].GetComponent<Waypoint>();
            }
        }

        return _allStartingWaypoints;
    }

    public Waypoint GetRandomStartingWaypoint()
    {
        return GetAllStartingWaypoints()[(int)(Random.value * GetAllStartingWaypoints().Length)];
    }

    public Waypoint GetRandomBuildingWaypoint()
    {
        return GetAllBuildingWaypoints()[(int)(Random.value * GetAllBuildingWaypoints().Length)];
    }

    Waypoint _vegetabelGardenWaypoint;
    public Waypoint GetVegetableGardenWaypoint()
    {
        if (_vegetabelGardenWaypoint == null)
            _vegetabelGardenWaypoint = GameObject.FindObjectOfType<VegetableGarden>().GetComponent<Waypoint>();

        return _vegetabelGardenWaypoint;
    }

    Waypoint[] _allBuildingWaypoints;
    public Waypoint[] GetAllBuildingWaypoints()
    {
        if (_allBuildingWaypoints == null)
        {
            _allBuildingWaypoints = new Waypoint[1];

            GameObject[] buildingWaypoints = GameObject.FindGameObjectsWithTag("Building Waypoint");
            for (int i = 0; i < buildingWaypoints.Length; i++)
            {
                if (i > 0)
                    System.Array.Resize<Waypoint>(ref _allBuildingWaypoints, _allBuildingWaypoints.Length + 1);

                _allBuildingWaypoints[i] = buildingWaypoints[i].GetComponent<Waypoint>();
            }
        }

        return _allBuildingWaypoints;
    }

    static GameController _instance;
    public static GameController instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.Find("Game Controller").GetComponent<GameController>();

            return _instance;
        }
    }

    int _money;
    public int money
    {
        get { return _money; }
    }

    int _storedBeerProducts;
    public int storedBeerProducts
    {
        get { return _storedBeerProducts; }
    }

    int _storedVegetableProducts;
    public int storedVegetableProducts
    {
        get { return _storedVegetableProducts; }
    }

    bool _hasRoundEnded;
    public bool hasRoundEnded
    {
        get { return _hasRoundEnded; }
        set { _hasRoundEnded = true; }
    }

    bool _hasStarted;
    public bool hasStarted
    {
        get { return _hasStarted; }
        set { _hasStarted = value; }
    }

    public int _currentDamage;
    public int currentDamage
    {
        get { return _currentDamage; }
        set { _currentDamage = value; }
    }

    public int _totalDamage;
    public int totalDamage
    {
        get { return _totalDamage; }
        set { _totalDamage = value; }
    }

    float _highScoreTimer;

    void Awake()
    {
        RainproofMeasures.AddCostValue(MeasureType.DrainPipe, SettingsController.instance.drainPipeCost);
        RainproofMeasures.AddCostValue(MeasureType.GreenGarden, SettingsController.instance.greenGardenCost);
        RainproofMeasures.AddCostValue(MeasureType.GreenRoof, SettingsController.instance.greenRoofCost);
        RainproofMeasures.AddCostValue(MeasureType.RainBarrel, SettingsController.instance.rainBarrelCost);
        RainproofMeasures.AddCostValue(MeasureType.TemporalDams, SettingsController.instance.temporalDamsCost);
        RainproofMeasures.AddCostValue(MeasureType.Threshold, SettingsController.instance.thresholdCost);
        RainproofMeasures.AddCostValue(MeasureType.VerticalGarden, SettingsController.instance.verticalGardenCost);

        UIController.instance.UpdateProductLabels();
        UIController.instance.UpdateRainwaterMeter();

        TimeController.OnMonthPassed += TimeController_OnMonthPassed;
        RequestController.instance.ClearOnlineQueue();
    }

    void TimeController_OnMonthPassed()
    {
        if (!hasStarted)
            return;

        GameObject cleanerObj = SpawnController.instance.SpawnCleaner();
        Cleaner cleaner = cleanerObj.GetComponent<Cleaner>();

        if (!cleaner.AssignJob())
            Destroy(cleanerObj);
        else
        {
            cleaner.gameObject.SetActive(true);
            UIController.instance.AddNews(cleaner.name, " is going to clean up some of the floods.");
        }
    }

    void Update()
    {
        if (!hasStarted)
        {
            if (Input.GetKeyDown(KeyCode.Space))
                StartTheRound();

            return;
        }

        if (_hasRoundEnded)
        {
            _highScoreTimer += Time.deltaTime;
            if (_highScoreTimer > 30f)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("Splash");
                return;
            }

            if (Input.GetKeyDown(KeyCode.Space))
                UnityEngine.SceneManagement.SceneManager.LoadScene("Gameplay");
        }
    }

    public void StartTheRound()
    {
        UIController.instance.AddMoneyIncreaseFeedback(Vector2.zero, SettingsController.instance.startingMoney);
        UIController.instance.DisableUIExplaination();
        hasStarted = true;
    }

    public string GenerateRandomColor()
    {
        int r = (int)(Random.value * 256);
        int g = (int)(Random.value * 256);
        int b = (int)(Random.value * 256);

        return string.Format("{0:X2}{1:X2}{2:X2}", r, g, b);
    }

    public void AddVegetableProducts(int amount)
    {
        if (amount > 0)
            _storedVegetableProducts += amount;

        UIController.instance.UpdateProductLabels();
    }

    public float[] SellVegetableProducts()
    {
        float[] result = new float[2];

        int amountPercentage = Mathf.FloorToInt(_storedVegetableProducts * SettingsController.instance.sellPercentageOfProducts);
        int amountFlat = SettingsController.instance.sellFlatAmountOfProducts;
        int amount = 0;

        if (amountFlat > amountPercentage)
            amount = (_storedVegetableProducts < amountFlat) ? _storedVegetableProducts : amountFlat;
        else
            amount = amountPercentage;

        if (amount > 0)
        {
            float moneyEarned = amount * SettingsController.instance.vegetableSellValue;

            result[0] = amount;
            result[1] = moneyEarned;

            _storedVegetableProducts -= amount;
            UIController.instance.AddMoneyIncreaseFeedback(UIController.instance.vegetableCountLabel.rectTransform.localPosition, (int)moneyEarned);
        }

        UIController.instance.UpdateProductLabels();

        return result;
    }

    public void AddBeerProducts(int amount)
    {
        if (amount > 0)
            _storedBeerProducts += amount;

        UIController.instance.UpdateProductLabels();
    }

    public float[] SellBeerProducts()
    {
        float[] result = new float[2];

        int amountPercentage = Mathf.FloorToInt(_storedBeerProducts * SettingsController.instance.sellPercentageOfProducts);
        int amountFlat = SettingsController.instance.sellFlatAmountOfProducts;
        int amount = 0;

        if (amountFlat > amountPercentage)
            amount = (_storedBeerProducts < amountFlat) ? _storedBeerProducts : amountFlat;
        else
            amount = amountPercentage;

        if (amount > 0)
        {
            float moneyEarned = amount * SettingsController.instance.beerSellValue;

            result[0] = amount;
            result[1] = moneyEarned;

            _storedBeerProducts -= amount;
            UIController.instance.AddMoneyIncreaseFeedback(UIController.instance.beerCountLabel.rectTransform.localPosition, (int)moneyEarned);
        }

        UIController.instance.UpdateProductLabels();

        return result;
    }

    public void StopRound()
    {
        hasRoundEnded = true;

        TimeController.instance.ResetEventHandlers();
        UIController.instance.UpdateHighscoreInfo();

        uiCanvas.gameObject.SetActive(false);
        scoreCanvas.gameObject.SetActive(true);
    }

    public void AddMoney(int amount)
    {
        if (amount > 0f)
            _money += amount;
    }

    public bool TakeMoney(int amount)
    {
        if (_money >= amount)
        {
            _money -= amount;

            return true;
        }

        return false;
    }
}