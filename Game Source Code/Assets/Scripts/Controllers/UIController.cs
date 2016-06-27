using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIController : MonoBehaviour
{
    public Image rainwaterMeter;
    public Text rainwaterLabel;
    public Text moneyLabel;
    public Text vegetableCountLabel;
    public Text beerCountLabel;
    public Text timeLabel;
    public GameObject initativeHolder;
    public Text initativeText;
    public GameObject moneyIncreaseLabel;
    public GameObject moneyDecreaseLabel;
    public Canvas uiCanvas;
    public Text damageCounter;
    public Text currentDamage;
    public Text totalDamage;
    public Text hDamageTotal;
    public Text hDamageCurrent;
    public Text hDamagePrevented;
    public Text hTeamNumber;
    public GameObject hRankList;
    public GameObject uiExplaination;
    public GameObject newsLabelPrefab;

    [Header("Settings")]
    public float marqueeSpeed;

    static UIController _instance;
    public static UIController instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.Find("UI Controller").GetComponent<UIController>();

            return _instance;
        }
    }

    Gradient _gradient;
    GameObject _news;
    Queue<GameObject> _newsQueue;

    void Awake()
    {
        GradientColorKey[] colorKeys = new GradientColorKey[2];
        colorKeys[0].time = 0f;
        colorKeys[0].color = new Color(215 / 255f, 163 / 255f, 63 / 255f);
        colorKeys[1].time = 1f;
        colorKeys[1].color = new Color(67 / 255f, 215 / 255f, 63 / 255f);

        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
        alphaKeys[0].time = 0f;
        alphaKeys[0].alpha = 1f;
        alphaKeys[1].time = 1f;
        alphaKeys[1].alpha = 1f;

        _gradient = new Gradient();
        _gradient.SetKeys(colorKeys, alphaKeys);

        _newsQueue = new Queue<GameObject>();
    }

    void Start()
    {
        UpdateRainwaterMeter();
    }

    void Update()
    {
        if (_news == null && _newsQueue.Count > 0)
        {
            _news = _newsQueue.Dequeue();
            _news.SetActive(true);
        }
    }
    
    public void AddNews(string name, string news)
    {
        GameObject newsLabel = Instantiate(newsLabelPrefab);
        newsLabel.transform.SetParent(GameController.instance.uiCanvas.transform, false);
        newsLabel.transform.SetSiblingIndex(1);
        newsLabel.SetActive(false);

        newsLabel.GetComponent<RectTransform>().localPosition = new Vector2(1000f, -513f);

        string nameColor = ColorToHex(new Color(Random.value, Random.value, Random.value));
        RectTransform nameLabel = newsLabel.transform.FindChild("Name").GetComponent<RectTransform>();
        nameLabel.GetComponent<Text>().text = "<color='#" + nameColor + "'>" + name + "</color> ";

        RectTransform descriptionLabel = newsLabel.transform.FindChild("Description").GetComponent<RectTransform>();
        descriptionLabel.localPosition = new Vector2(nameLabel.GetComponent<Text>().preferredWidth, 0f);
        descriptionLabel.GetComponent<Text>().text = news;
        
        _newsQueue.Enqueue(newsLabel);
    }

    public void DisableUIExplaination()
    {
        uiExplaination.SetActive(false);
    }

    public void AddMoneyIncreaseFeedback(Vector2 screenPosition, int money)
    {
        GameObject feedbackObj = Instantiate(moneyIncreaseLabel);
        feedbackObj.transform.SetParent(uiCanvas.transform, false);
        feedbackObj.GetComponent<RectTransform>().localPosition = screenPosition;
        feedbackObj.GetComponent<MoneyFeedback>().targetLocation = moneyLabel.rectTransform;
        feedbackObj.GetComponent<MoneyFeedback>().moneyMutation = money;
        feedbackObj.GetComponent<Text>().text = "+ " + money;

        GameController.instance.AddMoney(money);
        UpdateMoneyLabel();
    }

    public void AddMoneyDecreaseFeedback(Vector2 screenPosition, int money)
    {
        GameObject feedbackObj = Instantiate(moneyDecreaseLabel);
        feedbackObj.transform.SetParent(uiCanvas.transform, false);
        feedbackObj.GetComponent<RectTransform>().localPosition = screenPosition;
        feedbackObj.GetComponent<MoneyFeedback>().targetLocation = moneyLabel.rectTransform;
        feedbackObj.GetComponent<MoneyFeedback>().moneyMutation = -money;
        feedbackObj.GetComponent<Text>().text = "- " + money;

        GameController.instance.TakeMoney(money);
        UpdateMoneyLabel();
    }

    public void UpdateMoneyLabel()
    {
        moneyLabel.text = GameController.instance.money.ToString();
    }

    public void UpdateProductLabels()
    {
        vegetableCountLabel.text = GameController.instance.storedVegetableProducts.ToString();
        beerCountLabel.text = GameController.instance.storedBeerProducts.ToString();
    }

    public void ToggleInitiativeAlert()
    {
        initativeHolder.SetActive(!initativeHolder.activeInHierarchy);
        UpdateInitiativeLabel();
    }

    public void UpdateInitiativeLabel()
    {
        int liters = (int)(InitiativeController.instance.storedRainwater / 1000f);
        initativeText.text = liters + " liters stored.";
    }

    public void UpdateRainwaterMeter()
    {
        RainproofMeasures[] measures = GameObject.FindObjectsOfType<RainproofMeasures>();
        if (measures.Length < 1)
            return;

        int collected = 0;
        int rainbarrelCount = 0;
        foreach (RainproofMeasures measure in measures)
        {
            collected += measure.storedRainwater;

            if (measure.appliedMeasures.Contains(MeasureType.RainBarrel))
                rainbarrelCount++;
        }

        if (rainbarrelCount == 0)
        {
            rainwaterMeter.rectTransform.localScale = new Vector3(0f, 1f, 1f);
            rainwaterLabel.text = "0 / 0 liters";
            return;
        }

        float maxCapacity = rainbarrelCount * SettingsController.instance.maximumRainbarrelStorage;
        float scale = collected / maxCapacity;

        rainwaterMeter.rectTransform.localScale = new Vector3(scale, 1f, 1f);
        rainwaterLabel.text = (int)(collected / 1000f) + " / " + (int)(maxCapacity / 1000f) + " liters";
    }

    public void UpdateTimeLabel(int days, int weeks, int months, int years)
    {
        timeLabel.text = Mathf.Round(TimeController.instance.timeProgress * 100f) + "%";
        //timeLabel.text = "Day " + days + " | Week " + weeks + " | Month " + months + " | Year " + years;
    }

    public void UpdateDamageCounter()
    {
        if (GameController.instance.currentDamage == 0 || GameController.instance.totalDamage == 0)
            return;

        float percentageSaved = (float)GameController.instance.currentDamage / (float)GameController.instance.totalDamage;
        damageCounter.text = Mathf.Round((1f - percentageSaved) * 100f) + "%";
        damageCounter.color = _gradient.Evaluate(1f - percentageSaved);

        currentDamage.text = "€ " + string.Format("{0:0,0}", GameController.instance.currentDamage);
        totalDamage.text = "€ " + string.Format("{0:0,0}", GameController.instance.totalDamage);

        //Debug.Log(GameController.instance.currentDamage + ", " + GameController.instance.totalDamage);
    }

    public void UpdateHighscoreInfo()
    {
        float percentageSaved = (float)GameController.instance.currentDamage / (float)GameController.instance.totalDamage;
        int percentageSavedInt = (int)Mathf.Round((1f - percentageSaved) * 100f);
        int teamNumber = HighscoreController.scoreCount + 1;
        HighscoreController.AddScore(teamNumber, percentageSavedInt);

        List<KeyValuePair<int, int>> top10 = HighscoreController.GetTop10();
        for (int i = 0; i < top10.Count; i++)
        {
            int rank = i + 1;

            Text currentRankText = hRankList.transform.FindChild(rank.ToString()).GetComponent<Text>();
            currentRankText.text = "#" + rank + " - Team " + top10[i].Key + ": <color='#" + ColorToHex(_gradient.Evaluate(top10[i].Value / 100f)) + "'>" + top10[i].Value + "</color>%";
        }

        hDamagePrevented.text = "<color='#" + ColorToHex(_gradient.Evaluate(1f - percentageSaved)) + "'>" + percentageSavedInt + "</color>%";
        hDamageTotal.text = "€ " + string.Format("{0:0,0}", GameController.instance.totalDamage);
        hDamageCurrent.text = "€ " + string.Format("{0:0,0}", GameController.instance.currentDamage);
        hTeamNumber.text = teamNumber.ToString();
    }

    public string ColorToHex(Color color)
    {
        Color32 color32 = color;
        return color32.r.ToString("X2") + color32.g.ToString("X2") + color32.b.ToString("X2");
    }
}