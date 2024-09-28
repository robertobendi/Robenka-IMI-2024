using UnityEngine;

public class PanelManager : MonoBehaviour
{
    [SerializeField] private GameObject dayPassedPanel;
    [SerializeField] private GameObject dayFailedSpyPanel;
    [SerializeField] private GameObject dayFailedNotEnoughPeoplePanel;
    [SerializeField] private GameObject gameWonPanel;

    private void HideAllPanels()
    {
        dayPassedPanel.SetActive(false);
        dayFailedSpyPanel.SetActive(false);
        dayFailedNotEnoughPeoplePanel.SetActive(false);
        gameWonPanel.SetActive(false);
    }

    public void ShowDayPassedPanel()
    {
        HideAllPanels();
        dayPassedPanel.SetActive(true);
    }

    public void ShowDayFailedSpyPanel()
    {
        HideAllPanels();
        dayFailedSpyPanel.SetActive(true);
    }

    public void ShowDayFailedNotEnoughPeoplePanel()
    {
        HideAllPanels();
        dayFailedNotEnoughPeoplePanel.SetActive(true);
    }

    public void ShowGameWonPanel()
    {
        HideAllPanels();
        gameWonPanel.SetActive(true);
    }
}