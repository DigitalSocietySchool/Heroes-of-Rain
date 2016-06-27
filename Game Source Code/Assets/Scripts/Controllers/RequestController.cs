using UnityEngine;
using System.Collections.Generic;

public class RequestController : MonoBehaviour
{
    public float requestInterval;

    public delegate void ReceivedDataEvent();
    public event ReceivedDataEvent OnReceivedData;

    static RequestController _instance;
    public static RequestController instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.Find("Request Controller").GetComponent<RequestController>();

            return _instance;
        }
    }

    [HideInInspector]
    public List<WebData> webData = new List<WebData>();

    float _timer;
    WWW _www;
	string _url;
	
	void Start()
	{
		_url = "[insert url here]";
	}

    public void ClearOnlineQueue()
    {
        // do web stuff
        WWWForm wwwForm = new WWWForm();
        wwwForm.AddField("secret_key", "dj_rainfalla");
        wwwForm.AddField("action", "clear");

        _www = new WWW(_url, wwwForm);
    }

    void Update()
    {
        if (TimeController.instance.timeProgress == 1f)
            return;

        if (_timer < requestInterval)
            _timer += Time.deltaTime;
        else
        {
            _timer = 0f;

            // do web stuff
            WWWForm wwwForm = new WWWForm();
            wwwForm.AddField("secret_key", "dj_rainfalla");

            _www = new WWW(_url, wwwForm);
        }

        if (_www != null)
        {
            if (_www.isDone)
            {
                if (_www.text.Length > 1)
                {
                    if (!GameController.instance.hasStarted)
                        GameController.instance.StartTheRound();

                    string[] dataSet = _www.text.Split(new char[] { ';' });
                    foreach (string data in dataSet)
                    {
                        string characterName = "";
                        WebData webDataObj;

                        switch (data[0])
                        {
                            case 'c':
                                string[] characterDataSet = data.Split(new char[] { ',' });
                                characterName = characterDataSet[1];
                                int rainproofMeasureID = -1;

                                switch (characterDataSet[2])
                                {
                                    case "GreenGarden": rainproofMeasureID = 0; break;
                                    case "RainBarrel": rainproofMeasureID = 1; break;
                                    case "GreenRoof": rainproofMeasureID = 2; break;
                                    case "DrainPipe": rainproofMeasureID = 3; break;
                                    case "Threshold": rainproofMeasureID = 4; break;
                                    case "VerticalGarden": rainproofMeasureID = 5; break;
                                    case "TemporalDams": rainproofMeasureID = 6; break;
                                }

                                webDataObj = new WebData("0;" + rainproofMeasureID + ";" + characterName);
                                webData.Add(webDataObj);
                                break;

                            case 'r':
                                string[] rewardDataSet = data.Split(new char[] { ',' });
                                characterName = rewardDataSet[1];

                                int rewardID = -1;

                                switch (rewardDataSet[2])
                                {
                                    case "SendRainwater": rewardID = 0; break;
                                    case "SellProducts": rewardID = 1; break;
                                    case "SellRainwater": rewardID = 2; break;
                                    case "CleanFloods": rewardID = 3; break;
                                    case "Discount": rewardID = 4; break;
                                    case "StartStopInitiative": rewardID = 5; break;
                                }

                                webDataObj = new WebData("1;" + rewardID + ";" + characterName);
                                webData.Add(webDataObj);
                                break;
                        }
                    }
                }

                _www = null;

                if (OnReceivedData != null)
                    OnReceivedData();
            }

            
        }

        HandleInput();
    }
    
    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // builder with GreenGarden
            webData.Add(new WebData("0;0;Rainproof"));
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            // builder with RainBarrel
            webData.Add(new WebData("0;1;Rainproof"));
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            // builder with GreenRoof
            webData.Add(new WebData("0;2;Rainproof"));
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            // builder with DrainPipe
            webData.Add(new WebData("0;3;Rainproof"));
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            // builder with Threshold
            webData.Add(new WebData("0;4;Rainproof"));
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            // builder with VerticalGarden
            webData.Add(new WebData("0;5;Rainproof"));
        }

        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            // builder with TemporalDams
            webData.Add(new WebData("0;6;Rainproof"));
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            // reward with SendRainwater
            webData.Add(new WebData("1;0;Rainproof"));
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            // reward with SellProducts
            webData.Add(new WebData("1;1;Rainproof"));
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            // reward with SellRainwater
            webData.Add(new WebData("1;2;Rainproof"));
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            // reward with CleanFloods
            webData.Add(new WebData("1;3;Rainproof"));
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            // reward with Discount
            webData.Add(new WebData("1;4;Rainproof"));
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            // reward with StartStopInitiative
            webData.Add(new WebData("1;5;Rainproof"));
        }
    }
}