using UnityEngine;
using System.Collections.Generic;
using System;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<DayData> days;
    [SerializeField] private CardsManager cardsManager;
    [SerializeField] private PanelManager panelManager;
    [SerializeField] private TimerUI timerUI;

    private int currentDayIndex = 0;
    private float currentDayTimer;
    private int acceptedValidPeople = 0;
    private bool isDayInProgress = false;

    public event Action<int> OnDayStart;
    public event Action<int> OnDayEnd;

    private void Start()
    {
        StartDay(0);
    }

    private void Update()
    {
        if (currentDayIndex >= days.Count || !isDayInProgress) return;

        currentDayTimer -= Time.deltaTime;
        timerUI.UpdateTimer(currentDayTimer, days[currentDayIndex].lengthInSeconds);
        
        if (currentDayTimer <= 0)
        {
            EndDay();
        }
    }

    private void StartDay(int dayIndex)
    {
        if (dayIndex >= days.Count) return;

        currentDayIndex = dayIndex;
        DayData currentDay = days[currentDayIndex];
        currentDayTimer = currentDay.lengthInSeconds;
        acceptedValidPeople = 0;
        isDayInProgress = true;

        cardsManager.SetDifficultyRange(currentDay.minDifficulty, currentDay.maxDifficulty);
        cardsManager.ResetForNewDay();
        timerUI.SetupTimer(currentDay.lengthInSeconds);
        OnDayStart?.Invoke(currentDayIndex);
    }

    private void EndDay()
    {
        isDayInProgress = false;
        DayData currentDay = days[currentDayIndex];
        OnDayEnd?.Invoke(currentDayIndex);

        if (acceptedValidPeople >= currentDay.requiredPeople && cardsManager.GetAcceptedImpostorCount() == 0)
        {
            if (currentDayIndex + 1 < days.Count)
            {
                panelManager.ShowDayPassedPanel();
                StartDay(currentDayIndex + 1);
            }
            else
            {
                panelManager.ShowGameWonPanel();
            }
        }
        else if (cardsManager.GetAcceptedImpostorCount() > 0)
        {
            panelManager.ShowDayFailedSpyPanel();
        }
        else
        {
            panelManager.ShowDayFailedNotEnoughPeoplePanel();
        }
    }

    public void AcceptValidPerson()
    {
        acceptedValidPeople++;
    }

    public float GetRemainingDayTime()
    {
        return currentDayTimer;
    }

    public int GetCurrentDayRequiredPeople()
    {
        return days[currentDayIndex].requiredPeople;
    }

    public int GetAcceptedValidPeople()
    {
        return acceptedValidPeople;
    }

    public bool IsDayInProgress()
    {
        return isDayInProgress;
    }
}