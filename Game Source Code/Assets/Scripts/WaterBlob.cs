using UnityEngine;
using System.Collections;
using System.Reflection;

public class WaterBlob : MonoBehaviour
{
    public float lifeTime;
    public float startScale;
    public float endScale;
    public float scaleTime;
    public GameObject ducky;

    float _timer;
    float _scaleTimer;
    float _scaleStep;
    Material _quadMaterial;
    GameObject _ducky;

    void Awake()
    {
        transform.localScale = new Vector3(startScale, startScale, startScale);
        _scaleStep = endScale - startScale;
        _quadMaterial = GetComponentInChildren<MeshRenderer>().material;
    }

    void Start()
    {
        if (Random.value < 0.025f)
        {
            _ducky = Instantiate(ducky);
            _ducky.transform.SetParent(transform, false);

            if (Random.value < 0.5f)
                _ducky.GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    void Update()
    {
        if (_scaleTimer < scaleTime)
        {
            _scaleTimer += Time.deltaTime;

            float percentage = _scaleTimer / scaleTime;
            float scale = startScale + _scaleStep * percentage;
            transform.localScale = new Vector3(+scale, scale, scale);
        }

        if (_timer < lifeTime)
            _timer += Time.deltaTime;
        else
        {
            Color quadColor = _quadMaterial.color;

            if (quadColor.a > 0.01f)
            {
                _quadMaterial.color = new Color(quadColor.r, quadColor.g, quadColor.b, quadColor.a * 0.99f);

                if (_ducky != null)
                    _ducky.transform.localScale *= 0.99f;
            }
            else
                Destroy(gameObject);
        }
    }

    public void EndLife()
    {
        _timer = lifeTime;
    }
}