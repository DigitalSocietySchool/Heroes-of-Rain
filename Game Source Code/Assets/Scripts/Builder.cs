using UnityEngine;
using System.Collections.Generic;

public class Builder : MonoBehaviour
{
    public GameObject nameTagPrefab;
    public Sprite almarSprite;
    public Sprite jeffreySprite;
    public Sprite dymphieSprite;
    public Sprite iratiSprite;
    public Sprite jorisSprite;
    public GameObject measureIconPrefab;

    MeasureType _measureToInstall;
    Waypoint _target;
    Waypoint _lastReachedWaypoint;
    List<Waypoint> _path = new List<Waypoint>();
    float _movementSpeed;
    float _timeToBuild;
    float _buildTimer;
    GameObject _nameTag;
    GameObject _measureIcon;
    float _initialIconScale;
    Vector2? _initialMeasurePosition;
    Vector2 _measureToInstalPosition;

    void Awake()
    {
        _movementSpeed = SettingsController.instance.builderMovementSpeed;
        _timeToBuild = SettingsController.instance.builderTimeToBuildMeasure;

        _nameTag = Instantiate(nameTagPrefab);
        _nameTag.transform.position = transform.position;
        _nameTag.transform.SetParent(GameObject.Find("World Canvas").transform);
        _nameTag.GetComponentInChildren<UnityEngine.UI.Text>().text = name;
    }

    void FixedUpdate()
    {
        if (_path.Count > 0)
            FollowPath();
        else
        {
            if (_target == null)
            {
                Destroy(_nameTag);
                Destroy(gameObject);
            }
            else
            {
                if (_buildTimer < _timeToBuild)
                {
                    if (_initialMeasurePosition == null)
                        _initialMeasurePosition = _measureIcon.transform.position;

                    _buildTimer += Time.deltaTime;
                    GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.75f);

                    float percentage = _buildTimer / _timeToBuild;

                    _measureIcon.transform.position = Vector2.Lerp((Vector2)_initialMeasurePosition, _measureToInstalPosition, percentage);

                    float scale = Mathf.Lerp(_initialIconScale, _target.transform.localScale.x, percentage);
                    _measureIcon.transform.localScale = new Vector3(scale, scale, scale);
                }
                else
                {
                    GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
                    _target.GetComponent<RainproofMeasures>().ApplyMeasure(_measureToInstall);
                    _target.GetComponent<RainproofMeasures>().RemovePlannedMeasure(_measureToInstall);
                    _target = null;

                    Destroy(_measureIcon);

                    _path = AStar.FindPath(_lastReachedWaypoint, GameController.instance.GetRandomStartingWaypoint());
                }
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
        Vector2 movement = Vector2.MoveTowards(transform.position, next.transform.position, _movementSpeed);

        _nameTag.transform.position = transform.position + new Vector3(0f, -0.5f, 0f);

        transform.position = new Vector3(movement.x, movement.y, transform.position.z);
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

    public Waypoint FindBestBuilding(MeasureType typeToInstall)
    {
        Waypoint[] allBuildings = GameController.instance.GetAllBuildingWaypoints();
        Waypoint target = null;
        int lowestCount = int.MaxValue;

        foreach (Waypoint building in allBuildings)
        {
            RainproofMeasures measures = building.GetComponent<RainproofMeasures>();
            int count = measures.appliedMeasures.Count + measures.plannedMeasures.Count;

            if (count < lowestCount && measures.CanApply(typeToInstall))
            {
                target = building;
                lowestCount = count;
            }
        }

        return target;
    }

    void ApplyEasterEgg()
    {
        SpriteRenderer sp = GetComponent<SpriteRenderer>();
        if (name.ToLower() == "almar")
            sp.sprite = almarSprite;
        else if (name.ToLower() == "jeffrey")
            sp.sprite = jeffreySprite;
        else if (name.ToLower() == "dymphie")
            sp.sprite = dymphieSprite;
        else if (name.ToLower() == "irati")
            sp.sprite = iratiSprite;
        else if (name.ToLower() == "joris")
            sp.sprite = jorisSprite;
    }

    void CreateMeasureIcon()
    {
        _measureToInstalPosition = _target.transform.FindChild(_measureToInstall.ToString()).transform.position;

        Sprite measureSprite = _target.transform.FindChild(_measureToInstall.ToString()).GetComponent<SpriteRenderer>().sprite;
        _measureIcon = Instantiate(measureIconPrefab);
        _measureIcon.transform.SetParent(transform, false);
        _measureIcon.GetComponent<SpriteRenderer>().sprite = measureSprite;

        Color currentColor = _measureIcon.GetComponent<SpriteRenderer>().color;
        _measureIcon.GetComponent<SpriteRenderer>().color = new Color(currentColor.r, currentColor.g, currentColor.b, 0.75f);

        float area = measureSprite.bounds.extents.x * measureSprite.bounds.extents.y * 4f;
        float percentage = area / 7f;
        float scale = Mathf.Lerp(1f, 0.2f, percentage);
        _measureIcon.transform.localScale = new Vector3(scale, scale, scale);

        _initialIconScale = scale;
    }

    public bool AssignJob(MeasureType typeToInstall)
    {
        _target = FindBestBuilding(typeToInstall);
        if (_target == null)
        {
            UIController.instance.AddNews(name, " wants to install a " + typeToInstall.ToString() + ", but there is no room left.");
            return false;
        }

        if (GameController.instance.money < RainproofMeasures.GetCost(typeToInstall))
        {
            UIController.instance.AddNews(name, " tried to buy a " + typeToInstall.ToString() + ", but there is currently not enough money.");
            return false;
        }

        _measureToInstall = typeToInstall;
        _path = AStar.FindPath(_lastReachedWaypoint, _target);
        _target.GetComponent<RainproofMeasures>().PlanMeasure(_measureToInstall);

        Vector2 screenSize = new Vector2(Screen.width, Screen.height);
        Vector2 canvasLocation = Camera.main.WorldToViewportPoint(new Vector2(_target.transform.position.x * screenSize.x, _target.transform.position.y * screenSize.y));

        UIController.instance.AddMoneyDecreaseFeedback(canvasLocation, RainproofMeasures.GetCost(typeToInstall));
        UIController.instance.AddNews(name, " bought a " + typeToInstall.ToString() + " and is now going to install it.");

        CreateMeasureIcon();
        ApplyEasterEgg();

        return true;
    }

    public void AssignJob(MeasureType typeToInstall, Waypoint building)
    {
        _target = building;
        _measureToInstall = typeToInstall;
        _path = AStar.FindPath(_lastReachedWaypoint, _target);
        _target.GetComponent<RainproofMeasures>().PlanMeasure(_measureToInstall);

        CreateMeasureIcon();
        ApplyEasterEgg();
    }
}