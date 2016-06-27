using UnityEngine;
using System.Collections;

public enum DataType
{
    Builder,
    Reward
}

public class WebData
{
    DataType _type;
    public DataType dataType
    {
        get { return _type; }
    }

    MeasureType? _measureType = null;
    public MeasureType? measureType
    {
        get { return _measureType; }
    }

    RewardType? _rewardType = null;
    public RewardType? rewardType
    {
        get { return _rewardType; }
    }

    string _sender;
    public string sender
    {
        get { return _sender; }
    }

    bool _isComplete;
    public bool isComplete
    {
        get { return _isComplete; }
    }

    public WebData(string webData)
    {
        if (webData.Length < 5)
            return;

        string[] data = webData.Split(new char[] { ';' });
 
        if (data.Length < 3)
            return;

        int parseResult = -1;

        if (!int.TryParse(data[0], out parseResult))
            return;

        if (parseResult < 0 || parseResult > System.Enum.GetValues(typeof(DataType)).Length - 1)
            return;

        _type = (DataType)parseResult;

        if (!int.TryParse(data[1], out parseResult))
            return;

        switch (_type)
        {
            case DataType.Builder:
                if (parseResult < 0 || parseResult > System.Enum.GetValues(typeof(MeasureType)).Length - 1)
                    return;

                _measureType = (MeasureType)parseResult;
                break;

            case DataType.Reward:
                if (parseResult < 0 || parseResult > System.Enum.GetValues(typeof(RewardType)).Length - 1)
                    return;

                _rewardType = (RewardType)parseResult;
                break;
        }

        _sender = data[2];
        _isComplete = true;
    }
}