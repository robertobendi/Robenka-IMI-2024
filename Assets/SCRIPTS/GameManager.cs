using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<DayData> days;
    [SerializeField] private CardsManager cardsManager;
    [SerializeField] private PanelManager panelManager;
    [SerializeField] private TimerUI timerUI;
    [SerializeField] private float dayIntroDisplayTime = 3f;
    [SerializeField] private string menuSceneName = "Menu";
    [SerializeField] private TextMeshProUGUI acceptedPeopleText;

    private int currentDayIndex = 0;
    private float currentDayTimer;
    private int acceptedValidPeople = 0;
    private int totalAcceptedPeople = 0;
    private bool isDayInProgress = false;
    private bool hasAcceptedImpostor = false;

    public event System.Action<int> OnDayStart;
    public event System.Action<int> OnDayEnd;

    private void Start()
    {
        StartDay(0);
        UpdateAcceptedPeopleDisplay();
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
        
        StartCoroutine(ShowDayIntroAndStartDay(currentDay));
    }

    private IEnumerator ShowDayIntroAndStartDay(DayData currentDay)
    {
        yield return StartCoroutine(panelManager.ShowDayIntroPanel(currentDayIndex + 1, currentDay.requiredPeople));

        yield return new WaitForSeconds(dayIntroDisplayTime);

        yield return StartCoroutine(panelManager.FadeOutDayIntroPanel());

        SetupDay(currentDay);
    }

    private void SetupDay(DayData currentDay)
    {
        currentDayTimer = currentDay.lengthInSeconds;
        acceptedValidPeople = 0;
        totalAcceptedPeople = 0;
        isDayInProgress = true;
        hasAcceptedImpostor = false;

        cardsManager.SetDifficultyRange(currentDay.minDifficulty, currentDay.maxDifficulty);
        cardsManager.ResetForNewDay();
        timerUI.SetupTimer(currentDay.lengthInSeconds);
        UpdateAcceptedPeopleDisplay();
        OnDayStart?.Invoke(currentDayIndex);
    }

    private void EndDay()
    {
        isDayInProgress = false;
        DayData currentDay = days[currentDayIndex];
        OnDayEnd?.Invoke(currentDayIndex);

        if (hasAcceptedImpostor)
        {
            panelManager.ShowDayFailedSpyPanel();
        }
        else if (acceptedValidPeople >= currentDay.requiredPeople)
        {
            if (currentDayIndex + 1 < days.Count)
            {
                panelManager.ShowDayPassedPanel();
            }
            else
            {
                panelManager.ShowGameWonPanel();
            }
        }
        else
        {
            panelManager.ShowDayFailedNotEnoughPeoplePanel();
        }
    }

    public void AcceptPerson(bool isValid)
    {
        if (isValid)
        {
            acceptedValidPeople++;
        }
        else
        {
            hasAcceptedImpostor = true;
        }
        totalAcceptedPeople++;
        UpdateAcceptedPeopleDisplay();
    }

    private void UpdateAcceptedPeopleDisplay()
    {
        if (acceptedPeopleText != null)
        {
            acceptedPeopleText.text = totalAcceptedPeople.ToString();
        }
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

    public void ReturnToMenu()
    {
        StartCoroutine(ReturnToMenuCoroutine());
    }

    private IEnumerator ReturnToMenuCoroutine()
    {
        yield return StartCoroutine(panelManager.FadeOutAllPanels());
        SceneManager.LoadScene(menuSceneName);
    }

    public void ContinueToNextDay()
    {
        StartCoroutine(ContinueToNextDayCoroutine());
    }

    private IEnumerator ContinueToNextDayCoroutine()
    {
        yield return StartCoroutine(panelManager.FadeOutAllPanels());
        StartDay(currentDayIndex + 1);
    }
}