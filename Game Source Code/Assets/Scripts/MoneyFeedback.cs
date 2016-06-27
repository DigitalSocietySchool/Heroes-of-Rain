using UnityEngine;
using System.Collections;

public class MoneyFeedback : MonoBehaviour
{
    public float scaleTime;
    public RectTransform targetLocation;
    public int moneyMutation;

    RectTransform myTransform;

    float _timer;
    float _maxMagnitude;

    void Awake()
    {
        myTransform = GetComponent<RectTransform>();
        myTransform.localScale = new Vector3(0f, 0f, 0f);
    }

    void Start()
    {
        _maxMagnitude = (targetLocation.localPosition - myTransform.localPosition).magnitude;
    }
    
    void FixedUpdate()
    {
        if (targetLocation != null)
        {
            if (_timer < scaleTime)
                _timer = Mathf.Clamp(_timer + Time.deltaTime, 0f, scaleTime);

            float scale = _timer / scaleTime;
            myTransform.localScale = new Vector3(scale, scale, scale);

            if (scale == 1f)
            {
                Vector3 delta = (targetLocation.localPosition - myTransform.localPosition);
                myTransform.localPosition += delta * 0.025f;

                scale = (delta.magnitude / _maxMagnitude);
                myTransform.localScale = new Vector3(scale, scale, scale);

                if (scale < 0.025f)
                {
                    UIController.instance.UpdateMoneyLabel();
                    Destroy(gameObject);
                }
            }
        }
    } 
}