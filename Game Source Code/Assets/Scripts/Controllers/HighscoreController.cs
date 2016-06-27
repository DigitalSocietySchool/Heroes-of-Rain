using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class HighscoreController
{
    static Dictionary<int, int> _scores = new Dictionary<int, int>();

    public static int scoreCount
    {
        get { return _scores.Count; }
    }

    public static void AddScore(int teamName, int damagePrevented)
    {
        if (_scores.ContainsKey(teamName))
        {
            if (_scores[teamName] < damagePrevented)
                _scores[teamName] = damagePrevented;
        }
        else
            _scores.Add(teamName, damagePrevented);
    }

    public static List<KeyValuePair<int, int>> GetTop10()
    {
        List<KeyValuePair<int, int>> scoresList = _scores.ToList();
        scoresList.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));
        scoresList.Reverse();

        scoresList = scoresList.GetRange(0, (scoresList.Count > 10) ? 10 : scoresList.Count);

        return scoresList;
    }
}