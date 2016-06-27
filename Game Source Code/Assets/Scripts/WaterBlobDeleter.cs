using UnityEngine;
using System.Collections;

public class WaterBlobDeleter : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.name == "Water Blob(Clone)")
            Destroy(collider.gameObject);
    }
}