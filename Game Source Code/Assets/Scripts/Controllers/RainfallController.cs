using UnityEngine;
using System.Collections.Generic;

public class RainfallController : MonoBehaviour
{
    public GameObject smallCloudPrefab;
    public GameObject mediumCloudPrefab;
    public GameObject heavyCloudPrefab;
    public GameObject superHeavyCloudPrefab;
    [Range(0, 1f)] public float mediumChanceStartPercentage;
    [Range(0, 1f)] public float spawnChancePerWeek;
    public float cloudMovementSpeed;
    public float spawnHeavyCloudAfter;
    public GameObject spawnLocation;
    public GameObject rainCloudDeleter;

    static RainfallController _instance;
    public static RainfallController instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.Find("Rainfall Controller").GetComponent<RainfallController>();

            return _instance;
        }
    }

    bool _superHeavyActive;
    GameObject _superHeavyCloud;
    float _timer;
    List<GameObject> _queuedClouds = new List<GameObject>();
    float _spawnChancePerWeek;

    void Awake()
    {
        TimeController.OnWeekPassed += TimeController_OnWeekPassed;
    }

    void Start()
    {
        _spawnChancePerWeek = spawnChancePerWeek;
    }

    GameObject SpawnCloud(GameObject prefab)
    {
        GameObject cloud = Instantiate(prefab);
        cloud.transform.position = spawnLocation.transform.position;
        cloud.transform.SetParent(GameObject.Find("Cloud Holder").transform, false);

        BoxCollider2D collider = rainCloudDeleter.GetComponent<BoxCollider2D>();
        Vector2 targetLocation = collider.transform.position + (Vector3)Random.insideUnitCircle * (collider.size.x * 0.25f);

        // 0.7 - 1.3
        cloud.GetComponent<Rigidbody2D>().velocity = targetLocation.normalized * cloudMovementSpeed * (1f + Random.value * 0.5f);

        return cloud;
    }
    
    public void SpawnSuperHeavyCloud()
    {
        _superHeavyCloud = SpawnCloud(superHeavyCloudPrefab);
        _superHeavyCloud.GetComponent<Rigidbody2D>().velocity *= 1.5f; // give the heavy one some extra speed, cus it was slow
    }

    void FixedUpdate()
    {
        if (!GameController.instance.hasStarted)
            return;

        if (_queuedClouds.Count > 0)
        {
            // some noise in spawning
            if (Random.value < 0.05f)
            {
                SpawnCloud(_queuedClouds[0]);
                _queuedClouds.RemoveAt(0);
            }
        }
    }

    void Update()
    {
        if (!GameController.instance.hasStarted)
            return;

        if (TimeController.instance.timeProgress == 1f)
        {
            if (_superHeavyActive)
            {
                if (_superHeavyCloud == null)
                {
                    if (GameController.instance.hasRoundEnded)
                        return;

                    GameController.instance.StopRound();
                }

                return;
            }

            if (_timer < spawnHeavyCloudAfter)
                _timer += Time.deltaTime;
            else
            { 
                _superHeavyActive = true;

                RainCloud[] clouds = GameObject.FindObjectsOfType<RainCloud>();
                foreach (RainCloud cloud in clouds)
                    cloud.Despawn();

                SpawnSuperHeavyCloud();
                _queuedClouds.Clear();
            }
        }
    }

    void TimeController_OnWeekPassed()
    {
        if (!GameController.instance.hasStarted)
            return;

        _spawnChancePerWeek = Mathf.Lerp(spawnChancePerWeek, 1f, TimeController.instance.timeProgress);
        if (Random.value > _spawnChancePerWeek || TimeController.instance.timeProgress == 1f)
            return;

        float inverseProgress = 1f - TimeController.instance.timeProgress;
        float difference = 1f - inverseProgress;
        float mediumChance = Mathf.Lerp(mediumChanceStartPercentage, 0f, TimeController.instance.timeProgress) * difference;
        float heavyChance = (1f - (inverseProgress + mediumChance));

        float randomValue = Random.value;
        if (randomValue <= inverseProgress)
        {
            int randomCount = Random.Range(2, 4);
            for (int i = 0; i < randomCount; i++)
                _queuedClouds.Add(smallCloudPrefab);

            return;
        }

        randomValue -= inverseProgress;
        if (randomValue <= mediumChance)
        {
            int randomCount = Random.Range(1, 2);
            for (int i = 0; i < randomCount; i++)
                _queuedClouds.Add(mediumCloudPrefab);

            return;
        }

        randomValue -= mediumChance;
        if (randomValue <= heavyChance)
        {
            int randomCount = 1; // Random.Range(1, 2);
            for (int i = 0; i < randomCount; i++)
                _queuedClouds.Add(heavyCloudPrefab);

            return;
        }
    }
}