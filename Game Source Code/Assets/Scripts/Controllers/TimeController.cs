using UnityEngine;
using System.Collections;

public class TimeController : MonoBehaviour
{
    public int totalYears;
    public float totalGameplayMinutes;

    static TimeController _instance;
    public static TimeController instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.Find("Time Controller").GetComponent<TimeController>();

            return _instance;
        }
    }

    public delegate void DayPassedEvent();
    public static event DayPassedEvent OnDayPassed;

    public delegate void WeekPassedEvent();
    public static event WeekPassedEvent OnWeekPassed;

    public delegate void MonthPassedEvent();
    public static event MonthPassedEvent OnMonthPassed;

    public delegate void YearPassedEvent();
    public static event YearPassedEvent OnYearPassed;

    int _days;
    int _weeks;
    int _months;
    int _years;

    float _secondsPerDay;
    public float secondsPerDay
    {
        get { return _secondsPerDay; }
    }

    public float timeProgress
    {
        get { return Mathf.Clamp01(_timer / (totalGameplayMinutes * 60f)); }
    }

    float _dayTimer;
    float _timer;

    void Awake()
    {
        _secondsPerDay = (totalYears / 336f) * (totalGameplayMinutes * 60f);
        UIController.instance.UpdateTimeLabel(_days, _weeks, _months, _years);
    }

    void Update()
    {
        if (!GameController.instance.hasStarted)
            return;

        _timer += Time.deltaTime;

        if (_dayTimer < _secondsPerDay)
            _dayTimer += Time.deltaTime;
        else
        {
            _dayTimer = 0f;
            _days++;

            UIController.instance.UpdateTimeLabel(_days, _weeks, _months, _years);

            if (OnDayPassed != null)
                OnDayPassed();

            if (_days == 7)
            {
                _days -= 7;
                _weeks++;

                if (OnWeekPassed != null)
                    OnWeekPassed();

                if (_weeks == 4)
                {
                    _weeks -= 4;
                    _months++;

                    if (OnMonthPassed != null)
                        OnMonthPassed();

                    if (_months == 12)
                    {
                        _months -= 12;
                        _years++;

                        if (OnYearPassed != null)
                            OnYearPassed();
                    }
                }
            }
        }
    }

    public void ResetEventHandlers()
    {
        if (OnDayPassed != null)
        {
            foreach (System.Delegate del in OnDayPassed.GetInvocationList())
                OnDayPassed -= (DayPassedEvent)del;
        }

        if (OnWeekPassed != null)
        {
            foreach (System.Delegate del in OnWeekPassed.GetInvocationList())
                OnWeekPassed -= (WeekPassedEvent)del;
        }

        if (OnMonthPassed != null)
        {
            foreach (System.Delegate del in OnMonthPassed.GetInvocationList())
                OnMonthPassed -= (MonthPassedEvent)del;
        }

        if (OnYearPassed != null)
        {
            foreach (System.Delegate del in OnYearPassed.GetInvocationList())
                OnYearPassed -= (YearPassedEvent)del;
        }
    }
}