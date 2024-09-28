using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using DG.Tweening;

public class Card : MonoBehaviour
{
    [SerializeField] private Image profileImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI ageText;
    [SerializeField] private TextMeshProUGUI occupationText;
    [SerializeField] private TextMeshProUGUI bioText;
    [SerializeField] private TextMeshProUGUI likesText;
    [SerializeField] private TextMeshProUGUI dislikesText;

    [SerializeField] private float swipeDuration = 0.5f;
    [SerializeField] private float swipeDistance = 1000f;
    [SerializeField] private float rotationAngle = 20f;

    private RectTransform rectTransform;
    private ProfileData profileData;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    public void Initialize(ProfileData data)
    {
        profileData = data;
        profileImage.sprite = data.profilePicture;
        nameText.text = data.name;
        ageText.text = data.age.ToString();
        occupationText.text = data.occupation;
        bioText.text = data.bio;
        likesText.text = $"Likes: {data.likes}";
        dislikesText.text = $"Dislikes: {data.dislikes}";
    }

    public bool IsImpostor => profileData.isImpostor;

    public void SetVisible(bool visible)
    {
        canvasGroup.alpha = visible ? 1 : 0;
        canvasGroup.blocksRaycasts = visible;
    }

    public void SetPosition(Vector3 position)
    {
        rectTransform.anchoredPosition = position;
    }

    public void SwipeRight(Action onComplete)
    {
        Sequence swipeSequence = DOTween.Sequence();
        swipeSequence.Append(rectTransform.DOAnchorPos(new Vector2(swipeDistance, 0), swipeDuration).SetEase(Ease.InBack));
        swipeSequence.Join(rectTransform.DORotate(new Vector3(0, 0, -rotationAngle), swipeDuration));
        swipeSequence.Join(canvasGroup.DOFade(0, swipeDuration));
        swipeSequence.OnComplete(() => 
        {
            onComplete?.Invoke();
            Destroy(gameObject);
        });
    }

    public void SwipeLeft(Action onComplete)
    {
        Sequence swipeSequence = DOTween.Sequence();
        swipeSequence.Append(rectTransform.DOAnchorPos(new Vector2(-swipeDistance, 0), swipeDuration).SetEase(Ease.InBack));
        swipeSequence.Join(rectTransform.DORotate(new Vector3(0, 0, rotationAngle), swipeDuration));
        swipeSequence.Join(canvasGroup.DOFade(0, swipeDuration));
        swipeSequence.OnComplete(() => 
        {
            onComplete?.Invoke();
            Destroy(gameObject);
        });
    }
}