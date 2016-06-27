using UnityEngine;
using System.Collections;

public class FloodDisplay : MonoBehaviour
{
    public GameObject floodsRender;
    public string sortingLayerName;
    public int sortingLayerOrder;

    void Awake()
    {
        floodsRender.GetComponent<MeshRenderer>().sortingLayerName = sortingLayerName;
        floodsRender.GetComponent<MeshRenderer>().sortingOrder = sortingLayerOrder;

        if (!floodsRender.activeSelf)
            floodsRender.SetActive(true);
    }
}