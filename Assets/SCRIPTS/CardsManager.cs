using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class CardsManager : MonoBehaviour
{
    [SerializeField] private List<ProfileData> profiles;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private RectTransform cardSpawnPoint;
    [SerializeField] private float cardSpawnDelay = 0.5f;
    [SerializeField] private int maxVisibleCards = 2;

    [SerializeField] private GameManager gameManager;

    private List<ProfileData> availableProfiles;
    private List<Card> cardStack = new List<Card>();

    private int acceptedImpostors = 0;

    private int minDifficulty;
    private int maxDifficulty;

    private void Start()
    {
        availableProfiles = new List<ProfileData>(profiles);
        SpawnInitialCards();
    }

    private void SpawnInitialCards()
    {
        for (int i = 0; i < maxVisibleCards; i++)
        {
            SpawnNewCard();
        }
        if (cardStack.Count > 0)
        {
            cardStack[cardStack.Count - 1].SetVisible(true);
        }
    }

    private void SpawnNewCard()
    {
        if (availableProfiles.Count == 0 || !gameManager.IsDayInProgress())
        {
            return;
        }

        List<ProfileData> suitableProfiles = availableProfiles.FindAll(p => p.difficulty >= minDifficulty && p.difficulty <= maxDifficulty);
        if (suitableProfiles.Count == 0)
        {
            Debug.Log("No suitable profiles available!");
            return;
        }

        int randomIndex = Random.Range(0, suitableProfiles.Count);
        ProfileData randomProfile = suitableProfiles[randomIndex];
        availableProfiles.Remove(randomProfile);

        GameObject cardObject = Instantiate(cardPrefab, cardSpawnPoint);
        Card newCard = cardObject.GetComponent<Card>();
        newCard.Initialize(randomProfile);
        newCard.SetVisible(false);
        cardStack.Insert(0, newCard);

        PositionCards();
    }

    private void PositionCards()
    {
        for (int i = 0; i < cardStack.Count; i++)
        {
            float yOffset = i * 10f; // Adjust this value to change the stacking effect
            cardStack[i].SetPosition(new Vector3(0, -yOffset, 0));
        }
    }

    public void AcceptCard()
    {
        if (cardStack.Count > 0 && gameManager.IsDayInProgress())
        {
            Card currentCard = cardStack[cardStack.Count - 1];
            if (currentCard.IsImpostor)
                acceptedImpostors++;
            else
                gameManager.AcceptValidPerson();

            currentCard.SwipeRight(() => RemoveTopCard());
        }
    }

    public void DiscardCard()
    {
        if (cardStack.Count > 0 && gameManager.IsDayInProgress())
        {
            Card currentCard = cardStack[cardStack.Count - 1];
            currentCard.SwipeLeft(() => RemoveTopCard());
        }
    }

    private void RemoveTopCard()
    {
        if (cardStack.Count > 0)
        {
            cardStack.RemoveAt(cardStack.Count - 1);
            if (cardStack.Count > 0)
            {
                cardStack[cardStack.Count - 1].SetVisible(true);
            }
            SpawnNewCard();
            PositionCards();
        }
    }

    public void SetDifficultyRange(int min, int max)
    {
        minDifficulty = min;
        maxDifficulty = max;
    }

    public int GetAcceptedImpostorCount()
    {
        return acceptedImpostors;
    }

    public void ResetForNewDay()
    {
        foreach (Card card in cardStack)
        {
            Destroy(card.gameObject);
        }
        cardStack.Clear();
        SpawnInitialCards();
    }
}
