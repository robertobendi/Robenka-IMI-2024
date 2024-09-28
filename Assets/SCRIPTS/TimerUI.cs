using UnityEngine;
using UnityEngine.UI;

public class TimerUI : MonoBehaviour
{
    [SerializeField] private Slider timerSlider;
    [SerializeField] private Image fillImage;
    [SerializeField] private Gradient colorGradient;

    public void SetupTimer(float maxTime)
    {
        timerSlider.maxValue = maxTime;
        timerSlider.value = maxTime;
        UpdateFillColor(1f);
    }

    public void UpdateTimer(float currentTime, float maxTime)
    {
        timerSlider.value = currentTime;
        float normalizedTime = currentTime / maxTime;
        UpdateFillColor(normalizedTime);
    }

    private void UpdateFillColor(float normalizedTime)
    {
        fillImage.color = colorGradient.Evaluate(normalizedTime);
    }
}