using UnityEngine;
using System.Collections;

public class VegetableGarden : MonoBehaviour
{
    int _weeksPassed;
    int _rainwaterStored;

    void Awake()
    {

    }

    void Start()
    {
        TimeController.OnDayPassed += TimeController_OnDayPassed;
        TimeController.OnWeekPassed += TimeController_OnWeekPassed;
    }

    void TimeController_OnDayPassed()
    {
        if (_rainwaterStored > 0)
            _rainwaterStored = (int)Mathf.Clamp(_rainwaterStored - SettingsController.instance.vegetablesUseRainwaterPerDay, 0f, int.MaxValue);
    }

    void TimeController_OnWeekPassed()
    {
        SettingsController settings = SettingsController.instance;

        _weeksPassed++;
        if (_weeksPassed == settings.generateVegetableEveryWeek)
        {
            _weeksPassed -= settings.generateVegetableEveryWeek;

            int amount = Random.Range(settings.minVegetablesGenerated, settings.maxVegetablesGenerated);
            if (_rainwaterStored > 0)
                amount = (int)(amount * (1f + SettingsController.instance.vegetablesBonusProfit));
            
            GameController.instance.AddVegetableProducts(amount);
        }
    }

    public void StoreRainwater(int amount)
    {
        if (amount > 0)
            _rainwaterStored += amount;
    }
}