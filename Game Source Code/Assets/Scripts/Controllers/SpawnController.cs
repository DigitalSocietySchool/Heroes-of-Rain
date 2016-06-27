using UnityEngine;
using System.Collections;

public class SpawnController : MonoBehaviour
{
    public GameObject builderPrefab;
    public GameObject cleanerPrefab;

    static SpawnController _instance;
    public static SpawnController instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.Find("Spawn Controller").GetComponent<SpawnController>();

            return _instance;
        }
    }

    void Awake()
    {
        RequestController.instance.OnReceivedData += Instance_OnReceivedData;
    }

    public GameObject SpawnBuilder(WebData data)
    {
        GameObject builderObj = Instantiate(builderPrefab);
        builderObj.name = data.sender;
        builderObj.transform.SetParent(GameObject.Find("Character Holder").transform, false);

        Builder builder = builderObj.GetComponent<Builder>();
        builder.SpawnAt(GameController.instance.GetRandomStartingWaypoint());

        return builderObj;
    }

    public GameObject SpawnCleaner()
    {
        GameObject cleanerObj = Instantiate(cleanerPrefab);
        cleanerObj.transform.SetParent(GameObject.Find("Character Holder").transform, false);
        cleanerObj.name = "Waternet";

        Cleaner cleaner = cleanerObj.GetComponent<Cleaner>();
        cleaner.SpawnAt(GameController.instance.GetRandomStartingWaypoint());

        return cleanerObj;
    }

    void Instance_OnReceivedData()
    {
        // go reverse, so we can just remove without messing up the order
        for (int i = RequestController.instance.webData.Count - 1; i >= 0; i--)
        {
            WebData data = RequestController.instance.webData[i];

            if (data.dataType != DataType.Builder)
                continue;

            RequestController.instance.webData.RemoveAt(i);

            GameObject builderObj = SpawnBuilder(data);
            Builder builder = builderObj.GetComponent<Builder>();

            if (!builder.AssignJob((MeasureType)data.measureType))
            {
                Destroy(builderObj);
                continue;
            }

            builderObj.SetActive(true);
        }
    }
}