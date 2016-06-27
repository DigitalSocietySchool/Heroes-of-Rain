using UnityEngine;
using System.Collections;

public class Building : MonoBehaviour
{
    int _worth;
    public int worth
    {
        get { return _worth; }
    }
    
    void Awake()
    {
        _worth = Random.Range(SettingsController.instance.minimumBuildingWorth, SettingsController.instance.maximumBuildingWorth);
        _worth = (int)(Mathf.Round(_worth / 100f) * 100f);
    }
}