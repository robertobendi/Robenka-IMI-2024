using UnityEngine;
using System.Collections.Generic;

public class CardsManager : MonoBehaviour
{
    [SerializeField] private List<ProfileData> profiles;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private RectTransform cardSpawnPoint;
    [SerializeField] private float cardSpawnDelay = 0.5f;

    private List<ProfileData> availableProfiles;
    private Card currentCard;

    private int likedImpostors = 0;
    private int likedRealProfiles = 0;

    private void Start()
    {
        availableProfiles = new List<ProfileData>(profiles);
        SpawnNewCard();
    }

    private void SpawnNewCard()
    {
        if (availableProfiles.Count == 0)
        {
            Debug.Log("No more profiles available!");
            return;
        }

        int randomIndex = Random.Range(0, availableProfiles.Count);
        ProfileData randomProfile = availableProfiles[randomIndex];
        availableProfiles.RemoveAt(randomIndex);

        GameObject cardObject = Instantiate(cardPrefab, cardSpawnPoint);
        currentCard = cardObject.GetComponent<Card>();
        currentCard.Initialize(randomProfile);
        currentCard.AnimateIn();
    }

    public void AcceptCard()
    {
        if (currentCard != null)
        {
            if (currentCard.IsImpostor)
                likedImpostors++;
            else
                likedRealProfiles++;

            currentCard.SwipeRight(() => Invoke(nameof(SpawnNewCard), cardSpawnDelay));
        }
    }

    public void DiscardCard()
    {
        if (currentCard != null)
        {
            currentCard.SwipeLeft(() => Invoke(nameof(SpawnNewCard), cardSpawnDelay));
        }
    }

    public (int impostors, int realProfiles) GetLikedCounts()
    {
        return (likedImpostors, likedRealProfiles);
    }
}