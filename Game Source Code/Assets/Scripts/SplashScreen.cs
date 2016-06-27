using UnityEngine;
using System.Collections;

public class SplashScreen : MonoBehaviour
{
    public GameObject[] logos;

    int _currentIndex;
    float angle;

    void Update()
    {
        if (logos[_currentIndex].transform.position.x < 20f)
        {
            float percentage = (logos[_currentIndex].transform.position.x + 20f) / 40f;
            float angle = percentage * Mathf.PI * 2f;
            float delta = 0.5f + Mathf.Cos(angle) * 0.5f;

            logos[_currentIndex].transform.Translate(new Vector2(0.05f + (delta * 30f) * Time.deltaTime, 0f));
        }
        else
        {
            _currentIndex++;
            if (_currentIndex == logos.Length)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("Gameplay");
            }
        }
    }
}