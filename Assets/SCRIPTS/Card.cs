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
    [SerializeField] private Vector2 swipeRightEndAnchor = new Vector2(2, 0.5f);
    [SerializeField] private Vector2 swipeLeftEndAnchor = new Vector2(-1, 0.5f);
    [SerializeField] private float rotationAngle = 20f;

    private RectTransform rectTransform;
    private ProfileData profileData;
    private Vector2 originalAnchorPos;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalAnchorPos = rectTransform.anchorMin = rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.anchoredPosition = Vector2.zero;
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

    public void AnimateIn()
    {
        rectTransform.localScale = Vector3.zero;
        rectTransform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack);
    }

    public void SwipeRight(Action onComplete)
    {
        Sequence swipeSequence = DOTween.Sequence();
        swipeSequence.Append(rectTransform.DOAnchorPos(swipeRightEndAnchor, swipeDuration).SetEase(Ease.InBack));
        swipeSequence.Join(rectTransform.DORotate(new Vector3(0, 0, -rotationAngle), swipeDuration));
        swipeSequence.OnComplete(() => 
        {
            onComplete?.Invoke();
            Destroy(gameObject);
        });
    }

    public void SwipeLeft(Action onComplete)
    {
        Sequence swipeSequence = DOTween.Sequence();
        swipeSequence.Append(rectTransform.DOAnchorPos(swipeLeftEndAnchor, swipeDuration).SetEase(Ease.InBack));
        swipeSequence.Join(rectTransform.DORotate(new Vector3(0, 0, rotationAngle), swipeDuration));
        swipeSequence.OnComplete(() => 
        {
            onComplete?.Invoke();
            Destroy(gameObject);
        });
    }
}