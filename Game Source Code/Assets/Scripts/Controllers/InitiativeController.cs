using UnityEngine;
using System.Collections;

public class InitiativeController : MonoBehaviour
{
    static InitiativeController _instance;
    public static InitiativeController instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.Find("Initiative Controller").GetComponent<InitiativeController>();

            return _instance;
        }
    }

    bool _isStarted;
    public bool isStarted
    {
        get { return _isStarted; }
    }

    int _storedRainwater;
    public int storedRainwater
    {
        get { return _storedRainwater; }
    }

    public void StoreRainwater(int amount)
    {
        if (amount > 0)
            _storedRainwater += amount;
    }

    public void StartStop()
    {
        _isStarted = !_isStarted;

        if (_isStarted)
            Start();
        else
            Stop();

        UIController.instance.ToggleInitiativeAlert();
    }

    void Start()
    {
        _storedRainwater = 0;
    }

    void Stop()
    {
        float litersRainwater = Mathf.Round(_storedRainwater / 1000f);
        float beer = litersRainwater * SettingsController.instance.rainwaterToBeerRatio;
        int products = Mathf.FloorToInt(beer / SettingsController.instance.beerBottleCapacity);

        if (products > 0)
        {
            GameController.instance.AddBeerProducts(products);
            UIController.instance.AddNews("", "The neighbourhood initiative has created " + products + " beer.");
        }
    }
}