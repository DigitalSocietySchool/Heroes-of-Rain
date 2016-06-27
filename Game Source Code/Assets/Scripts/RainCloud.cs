using UnityEngine;
using System.Collections.Generic;

public class RainCloud : MonoBehaviour
{
    public int mmPerHour;

    bool _isDespawning;
    SpriteRenderer _spriteRenderer;
    int _mmPerDay;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _mmPerDay = mmPerHour * 24;
    }

    void Start()
    {
        TimeController.OnDayPassed += TimeController_OnDayPassed;
    }

    void TimeController_OnDayPassed()
    {
        ProcessRaining();
    }

    void Update()
    {
        _spriteRenderer.sortingOrder = 100; // + (int)(transform.position.y) * 1000;

        if (_isDespawning)
        {
            transform.localScale *= 0.99f;
            if (transform.localScale.x < 0.01f)
                Destroy(gameObject);
        }
    }

    void ProcessRaining()
    {
        float radius = 5f * transform.localScale.x;

        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, radius, Vector2.zero);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.tag == "Building Waypoint")
            {
                RainproofMeasures measures = hit.collider.GetComponent<RainproofMeasures>();
                measures.RainUpon(_mmPerDay);

                GameController.instance.totalDamage += (int)(_mmPerDay / 10f);
                UIController.instance.UpdateDamageCounter();
            }
            else if (hit.collider.tag == "Sewage Drain")
            {
                SewageDrain drain = hit.collider.GetComponent<SewageDrain>();
                if (!drain.isFlooding)
                    drain.RainUpon(_mmPerDay);
            }
            else if (hit.collider.name == "Rain Cloud Deleter")
            {
                DestroyNow();
            }
        }

        //if (count == 0)
        //    return;

        //int maxCount = (buildingMeasures.Count * System.Enum.GetValues(typeof(MeasureType)).Length);
        //float maxReduction = SettingsController.instance.maximumDamageReduction;
        //float factor = (1f - (count / maxCount)) * maxReduction;
        //int mmPerDay = Mathf.FloorToInt(_mmPerDay * factor);

        ////Debug.Log(factor);

        //foreach (RainproofMeasures measure in buildingMeasures)
        //{
        //    measure.RainUpon(mmPerDay);
        //}

        //UIController.instance.UpdateDamageCounter();

        //int calculatedMMPerFrame = _mmPerDay;
        //int count = 0;

        //foreach (Waypoint building in _buildings.Values)
        //    count += building.GetComponent<RainproofMeasures>().appliedMeasures.Count;

        //foreach (SewageDrain drain in _drains.Values)
        //{
        //    if (!drain.isFlooding)
        //        count++;
        //}

        //if (count > 1)
        //{
        //    float factor = 1f - (SettingsController.instance.maximumDamageReduction / 35f) * count;
        //    calculatedMMPerFrame = Mathf.FloorToInt(_mmPerDay * factor);
        //}

        //calculatedMMPerFrame = (calculatedMMPerFrame < 1) ? 1 : calculatedMMPerFrame;

        //foreach (Waypoint building in _buildings.Values)
        //    building.GetComponent<RainproofMeasures>().RainUpon(calculatedMMPerFrame);

        //foreach (SewageDrain drain in _drains.Values)
        //    drain.GetComponent<SewageDrain>().RainUpon(calculatedMMPerFrame);
    }

    public void DestroyNow()
    {
        TimeController.OnDayPassed -= TimeController_OnDayPassed;
        Destroy(gameObject);
    }

    public void Despawn()
    {
        TimeController.OnDayPassed -= TimeController_OnDayPassed;
        _isDespawning = true;
    }
}