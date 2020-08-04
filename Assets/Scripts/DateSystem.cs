using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DateSystem : MonoBehaviour
{
    public static DateSystem Instance;
    private int CurrentDay;
    const int ConventionTimeLimit=30;
    const int DayTimeUnitLength = 10;
    public GameEvent OnDateChange;
    public GameEvent OnDateChangeTrigger;
    private int TimeLineCursor;
    public int getCurrentDay()
    {
        return CurrentDay;
    }
    // Start is called before the first frame update
    void Start()
    {
        CurrentDay = 0;
        if (Instance == null)
        {
            Instance = this;
        }
        OnDateChange.Raise();
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (OnDateChange != null) OnDateChange.Raise();
    }
    public void IncreaseTime(int i)
    {
        TimeLineCursor += i;
        if ((int)TimeLineCursor / DayTimeUnitLength != CurrentDay) ChangeDate();
      
    }
    private void ChangeDate()
    {
        CurrentDay= (int)TimeLineCursor / DayTimeUnitLength;
        //Quand on change de jour, on revient toujours au debut du jour.
        TimeLineCursor = CurrentDay* DayTimeUnitLength;
        OnDateChangeTrigger.Raise();
    }
    public int GetCurrentTime()
    {
        return TimeLineCursor - CurrentDay * DayTimeUnitLength;
    }
    public int GetRemainingDays()
    {
        return ConventionTimeLimit - CurrentDay;
    }
    public void ChangeDay()
    {

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
