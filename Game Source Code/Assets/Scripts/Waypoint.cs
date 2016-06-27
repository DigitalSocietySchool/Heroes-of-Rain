using UnityEngine;
using System.Collections;

public class Waypoint : MonoBehaviour
{
    public GameObject[] neighbours;

    [HideInInspector]
    public string id;

    void Awake()
    {
        id = Random.Range(-int.MaxValue, int.MaxValue).ToString();
        name += " [" + id + "]";
    }
}