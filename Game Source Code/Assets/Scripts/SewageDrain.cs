using UnityEngine;
using System.Collections;

public class SewageDrain : MonoBehaviour
{
    public int drainMMPerDay;

    [HideInInspector]
    public string id;

    int _receivingPerDay;

    public bool isFlooding
    {
        get { return _receivingPerDay > drainMMPerDay; }
    }

    void Awake()
    {
        id = Random.Range(-int.MaxValue, int.MaxValue).ToString();
        name += " [" + id + "]";
    }

    void Start()
    {
        TimeController.OnDayPassed += TimeController_OnDayPassed;
    }

    void TimeController_OnDayPassed()
    {
        _receivingPerDay = (int)Mathf.Clamp(_receivingPerDay - drainMMPerDay, 0f, int.MaxValue);
    }

    public void RainUpon(int amount)
    {
        if (amount > 0)
            _receivingPerDay += amount;

        if (isFlooding)
            FloodsController.instance.SpawnBlob((Vector2)transform.position + (Vector2)Random.insideUnitCircle * 0.001f);
    }
}