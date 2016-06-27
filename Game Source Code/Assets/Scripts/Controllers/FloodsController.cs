using UnityEngine;
using System.Collections.Generic;

public class FloodsController : MonoBehaviour
{
    public GameObject waterBlobPrefab;

    static FloodsController _instance;
    public static FloodsController instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.Find("Floods Controller").GetComponent<FloodsController>();

            return _instance;
        }
    }

    public void SpawnBlob(Vector2 location)
    {
        GameObject obj = Instantiate(waterBlobPrefab);
        obj.transform.position = location;
        obj.transform.SetParent(GameObject.Find("Flood Holder").transform);
    }

    public void CleanFloods(Vector2 location, float radius)
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(location, radius, Vector2.zero, 10f, 1 << LayerMask.NameToLayer("Meta Balls"));
        foreach (RaycastHit2D hit in hits)
            hit.collider.GetComponent<WaterBlob>().EndLife();
    }

    public List<GameObject> GetBlobsAt(Vector2 location, float radius)
    {
        List<GameObject> result = new List<GameObject>();

        RaycastHit2D[] hits = Physics2D.CircleCastAll(location, radius, Vector2.zero, 10f, 1 << LayerMask.NameToLayer("Meta Balls"));
        foreach (RaycastHit2D hit in hits)
            result.Add(hit.collider.gameObject);

        return result;
    }
}