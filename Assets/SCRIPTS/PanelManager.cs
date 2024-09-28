using UnityEngine;
using System.Collections;
using DG.Tweening;
using TMPro;

public class PanelManager : MonoBehaviour
{
    [SerializeField] private GameObject basePanel;
    [SerializeField] private GameObject dayPassedPanel;
    [SerializeField] private GameObject dayFailedSpyPanel;
    [SerializeField] private GameObject dayFailedNotEnoughPeoplePanel;
    [SerializeField] private GameObject gameWonPanel;
    
    [SerializeField] private GameObject dayIntroPanel;
    [SerializeField] private TextMeshProUGUI dayNumberText;
    [SerializeField] private TextMeshProUGUI personsNeededText;
    
    [SerializeField] private float fadeDuration = 1f;

    private CanvasGroup basePanelCanvasGroup;
    private CanvasGroup dayPassedCanvasGroup;
    private CanvasGroup dayFailedSpyCanvasGroup;
    private CanvasGroup dayFailedNotEnoughPeopleCanvasGroup;
    private CanvasGroup gameWonCanvasGroup;
    private CanvasGroup dayIntroPanelCanvasGroup;

    private void Awake()
    {
        basePanelCanvasGroup = GetOrAddCanvasGroup(basePanel);
        dayPassedCanvasGroup = GetOrAddCanvasGroup(dayPassedPanel);
        dayFailedSpyCanvasGroup = GetOrAddCanvasGroup(dayFailedSpyPanel);
        dayFailedNotEnoughPeopleCanvasGroup = GetOrAddCanvasGroup(dayFailedNotEnoughPeoplePanel);
        gameWonCanvasGroup = GetOrAddCanvasGroup(gameWonPanel);
        dayIntroPanelCanvasGroup = GetOrAddCanvasGroup(dayIntroPanel);

        HideAllPanels();
    }

    private CanvasGroup GetOrAddCanvasGroup(GameObject obj)
    {
        CanvasGroup cg = obj.GetComponent<CanvasGroup>();
        if (cg == null)
        {
            cg = obj.AddComponent<CanvasGroup>();
        }
        return cg;
    }

    private void HideAllPanels()
    {
        SetPanelActive(basePanelCanvasGroup, false);
        SetPanelActive(dayPassedCanvasGroup, false);
        SetPanelActive(dayFailedSpyCanvasGroup, false);
        SetPanelActive(dayFailedNotEnoughPeopleCanvasGroup, false);
        SetPanelActive(gameWonCanvasGroup, false);
        SetPanelActive(dayIntroPanelCanvasGroup, false);
    }

    private void SetPanelActive(CanvasGroup cg, bool active)
    {
        cg.alpha = active ? 1 : 0;
        cg.gameObject.SetActive(active);
    }

    public IEnumerator ShowDayIntroPanel(int dayNumber, int personsNeeded)
    {
        dayNumberText.text = dayNumber.ToString();
        personsNeededText.text = personsNeeded.ToString();
        
        dayIntroPanel.SetActive(true);
        dayIntroPanelCanvasGroup.alpha = 0;
        
        yield return dayIntroPanelCanvasGroup.DOFade(2, fadeDuration).WaitForCompletion();
    }

    public IEnumerator FadeOutDayIntroPanel()
    {
        yield return dayIntroPanelCanvasGroup.DOFade(0, fadeDuration).WaitForCompletion();
        dayIntroPanel.SetActive(false);
    }

    private IEnumerator ShowPanelWithDelay(CanvasGroup panelToShow)
    {
        HideAllPanels();
        
        SetPanelActive(basePanelCanvasGroup, true);
        yield return FadeInPanel(basePanelCanvasGroup);

        yield return FadeInPanel(panelToShow);
    }

    private IEnumerator FadeInPanel(CanvasGroup canvasGroup)
    {
        canvasGroup.gameObject.SetActive(true);
        canvasGroup.alpha = 0;
        yield return canvasGroup.DOFade(1, fadeDuration).WaitForCompletion();
    }

    public void ShowDayPassedPanel()
    {
        StartCoroutine(ShowPanelWithDelay(dayPassedCanvasGroup));
    }

    public void ShowDayFailedSpyPanel()
    {
        StartCoroutine(ShowPanelWithDelay(dayFailedSpyCanvasGroup));
    }

    public void ShowDayFailedNotEnoughPeoplePanel()
    {
        StartCoroutine(ShowPanelWithDelay(dayFailedNotEnoughPeopleCanvasGroup));
    }

    public void ShowGameWonPanel()
    {
        StartCoroutine(ShowPanelWithDelay(gameWonCanvasGroup));
    }

    public IEnumerator FadeOutAllPanels()
    {
        Sequence fadeOutSequence = DOTween.Sequence();
        fadeOutSequence.Join(basePanelCanvasGroup.DOFade(0, fadeDuration));
        fadeOutSequence.Join(dayPassedCanvasGroup.DOFade(0, fadeDuration));
        fadeOutSequence.Join(dayFailedSpyCanvasGroup.DOFade(0, fadeDuration));
        fadeOutSequence.Join(dayFailedNotEnoughPeopleCanvasGroup.DOFade(0, fadeDuration));
        fadeOutSequence.Join(gameWonCanvasGroup.DOFade(0, fadeDuration));

        yield return fadeOutSequence.WaitForCompletion();

        HideAllPanels();
    }
}