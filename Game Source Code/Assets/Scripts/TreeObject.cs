using UnityEngine;
using System.Collections;

public class TreeObject : MonoBehaviour
{
    public Sprite[] sprites;

    float _finalScale;
    float _growTime;
    float _growProgress;

    void Awake()
    {
        _finalScale = 0.5f + Random.value * 0.5f;

        GetComponent<SpriteRenderer>().sprite = sprites[(int)(Random.value * sprites.Length)];

        transform.localScale = new Vector3(0f, 0f, 0f);
    }

    void Start()
    {
        _growTime = TimeController.instance.secondsPerDay * 30f * Random.Range(2f, 8f);
    }

    void Update()
    {
        if (!GameController.instance.hasStarted)
            return;

        if (_growProgress < _growTime)
        {
            _growProgress += Time.deltaTime;

            float scale = Mathf.Lerp(0.2f, _finalScale, _growProgress / _growTime);
            transform.localScale = new Vector3(scale, scale, scale);
        }
    }
}