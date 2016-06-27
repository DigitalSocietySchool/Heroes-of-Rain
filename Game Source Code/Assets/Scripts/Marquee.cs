using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Marquee : MonoBehaviour
{
    RectTransform _rectTransform;
    RectTransform _canvasRectTransform;
    float _totalWidth;

    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvasRectTransform = GameController.instance.uiCanvas.GetComponent<RectTransform>();

        _totalWidth = transform.FindChild("Name").GetComponent<Text>().preferredWidth;
        _totalWidth += transform.FindChild("Description").GetComponent<Text>().preferredWidth;
    }

    void Update()
    {
        float movement = Time.deltaTime * 250f * _canvasRectTransform.localScale.x;
        _rectTransform.Translate(new Vector2(-movement, 0f));

        if (_rectTransform.localPosition.x + _totalWidth < _canvasRectTransform.rect.xMin)
            Destroy(gameObject);
    }
}