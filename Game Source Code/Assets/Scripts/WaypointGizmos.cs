using UnityEngine;
using System.Collections;

public class WaypointGizmos : MonoBehaviour
{
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Waypoint waypoint = GetComponent<Waypoint>();
        if (waypoint.neighbours.Length > 0)
        {
            for (int i = 0; i < waypoint.neighbours.Length; i++)
            {
                GameObject other = waypoint.neighbours[i];
                if (other != null)
                {
                    float xDist = other.transform.position.x - transform.position.x;
                    float yDist = other.transform.position.y - transform.position.y;

                    // going right or left
                    if (Mathf.Abs(xDist) > Mathf.Abs(yDist))
                    {
                        // right
                        if (xDist > 0)
                        {
                            Vector3 offset = new Vector3(0f, 0.05f);

                            Gizmos.color = Color.red;
                            Gizmos.DrawLine(transform.position + offset, other.transform.position + offset);
                        }
                        else
                        {
                            Vector3 offset = new Vector3(0f, -0.05f);

                            Gizmos.color = Color.blue;
                            Gizmos.DrawLine(transform.position + offset, other.transform.position + offset);
                        }
                    }
                    // going up or down
                    else if (Mathf.Abs(yDist) > Mathf.Abs(xDist))
                    {
                        // up
                        if (yDist > 0)
                        {
                            Vector3 offset = new Vector3(0.05f, 0f);

                            Gizmos.color = Color.green;
                            Gizmos.DrawLine(transform.position + offset, other.transform.position + offset);
                        }
                        else
                        {
                            Vector3 offset = new Vector3(-0.05f, 0f);

                            Gizmos.color = Color.yellow;
                            Gizmos.DrawLine(transform.position + offset, other.transform.position + offset);
                        }
                    }
                }
            }
        }
    }
}