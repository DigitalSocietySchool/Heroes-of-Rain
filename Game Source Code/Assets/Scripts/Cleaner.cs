using UnityEngine;
using System.Collections.Generic;

public class Cleaner : MonoBehaviour
{
    public GameObject nameTagPrefab;

    Vector2? _targetBlobLocation = null;
    Waypoint _target;
    Waypoint _lastReachedWaypoint;
    List<Waypoint> _path = new List<Waypoint>();
    float _movementSpeed;
    float _timer;
    List<GameObject> _blobsToClean;
    GameObject _nameTag;

    void Awake()
    {
        _movementSpeed = SettingsController.instance.builderMovementSpeed;

        _nameTag = Instantiate(nameTagPrefab);
        _nameTag.transform.position = transform.position;
        _nameTag.transform.SetParent(GameObject.Find("World Canvas").transform);
        _nameTag.GetComponentInChildren<UnityEngine.UI.Text>().text = "Waternet";
    }

    void FixedUpdate()
    {
        _nameTag.transform.position = transform.position + new Vector3(0f, -0.5f, 0f);

        if (_path == null)
        {
            Destroy(_nameTag);
            Destroy(gameObject);
            return;
        }

        if (_path.Count > 0)
            FollowPath();
        else
        {
            if (_targetBlobLocation == null)
            {
                Destroy(_nameTag);
                Destroy(gameObject);
            }
            else
            {
                if ((Vector2)(transform.position) == _targetBlobLocation)
                {
                    if (_blobsToClean == null)
                        _blobsToClean = FloodsController.instance.GetBlobsAt((Vector2)_targetBlobLocation, 2f);
                    else
                    {
                        if (_blobsToClean.Count == 0)
                        {
                            _targetBlobLocation = null;
                            _path = AStar.FindPath(_lastReachedWaypoint, GameController.instance.GetRandomStartingWaypoint());
                        }
                        else
                        {
                            if (_timer < SettingsController.instance.cleaningTime)
                                _timer += Time.deltaTime;
                            else
                            {
                                _timer = 0f;

                                if (_blobsToClean[0] != null)
                                    _blobsToClean[0].GetComponent<WaterBlob>().EndLife();

                                _blobsToClean.RemoveAt(0);
                            }
                        }
                    }
                }
                else
                    transform.position = Vector2.MoveTowards(transform.position, (Vector2)_targetBlobLocation, _movementSpeed);
            }
        }
    }

    public void FollowPath()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, 0.5f, Vector2.zero, 99f, 1 << LayerMask.NameToLayer("Meta Balls"));
        if (hit.collider != null)
        {
            if (_movementSpeed > (SettingsController.instance.builderMovementSpeed * 0.35f))
                _movementSpeed *= 0.99f;
        }
        else
            _movementSpeed = SettingsController.instance.builderMovementSpeed;

        Waypoint next = _path[0];
        transform.position = Vector2.MoveTowards(transform.position, next.transform.position, _movementSpeed);

        if ((Vector2)transform.position == (Vector2)next.transform.position)
        {
            _lastReachedWaypoint = next;
            _path.RemoveAt(0);
        }
    }

    public void SpawnAt(Waypoint waypoint)
    {
        _lastReachedWaypoint = waypoint;
        transform.position = new Vector3(waypoint.transform.position.x, waypoint.transform.position.y, transform.position.z);
    }

    public bool AssignJob()
    {
        WaterBlob[] blobs = GameObject.FindObjectsOfType<WaterBlob>();
        if (blobs.Length == 0)
            return false;

        _targetBlobLocation = blobs[(int)(Random.value * blobs.Length)].transform.position;

        float lowestDistance = float.MaxValue;
        Waypoint closestWaypoint = null;
        foreach (Waypoint waypoint in GameController.instance.GetAllWaypoints())
        {
            float dist = Vector2.Distance((Vector2)waypoint.transform.position, (Vector2)_targetBlobLocation);
            if (dist < lowestDistance)
            {
                lowestDistance = dist;
                closestWaypoint = waypoint;
            }
        }

        if (closestWaypoint == null)
            return false;

        _path = AStar.FindPath(_lastReachedWaypoint, closestWaypoint);

        return true;
    }
}