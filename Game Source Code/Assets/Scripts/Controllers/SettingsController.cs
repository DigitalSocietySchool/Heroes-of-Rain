using UnityEngine;
using System.Collections;

public class SettingsController : MonoBehaviour
{
    [Header("Rainproof Measures")]
    public int greenGardenCost;
    public int rainBarrelCost;
    public int greenRoofCost;
    public int drainPipeCost;
    public int thresholdCost;
    public int verticalGardenCost;
    public int temporalDamsCost;
    public float costPercentageProfitPerMonth;

    [Header("Builder")]
    public float builderMovementSpeed;
    public float builderTimeToBuildMeasure;

    [Header("Building")]
    public int minimumBuildingWorth;
    public int maximumBuildingWorth;

    [Header("Beer")]
    public float rainwaterToBeerRatio;
    public int beerSellValue;
    public float beerBottleCapacity;
    
    [Header("Vegetable")]
    public int vegetableSellValue;
    public int minVegetablesGenerated;
    public int maxVegetablesGenerated;
    public int generateVegetableEveryWeek;
    public int vegetablesUseRainwaterPerDay;
    public float vegetablesBonusProfit;

    [Header("Rewards")]
    public float sellPercentageOfProducts;
    public int sellFlatAmountOfProducts;
    public float rainwaterSellValuePerLiter;
    public int sellFlatAmountOfRainwater;
    public int sendFlatAmountOfRainwater;

    [Header("Other Settings")]
    public int startingMoney;
    public int maximumRainbarrelStorage;
    [Range(0, 1)] public float maximumDamageReduction;
    public float cleaningTime;
    
    static SettingsController _instance;
    public static SettingsController instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.Find("Settings Controller").GetComponent<SettingsController>();

            return _instance;
        }
    }
}