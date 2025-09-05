using System.Collections.Generic;
using CardGame.GamePlay;
using UnityEngine;
using UnityEngine.U2D;

/// <summary>
/// Manages the creation, shuffling, and state of card objects in the game grid.
/// Handles card instantiation, revealing, matching logic, and listener management.
/// </summary>
public class CardsManager : IGameResultListner
{
    #region Private Fields

    private List<GameObject> cards; // List of card GameObjects in the grid
    private Transform cardsParent; // Parent transform for card objects
    private SpriteAtlas spriteAtlas; // Sprite atlas for card faces
    private GameObject cardPrefab; // Prefab used to instantiate card objects

    #endregion

    #region Constructor

    /// <summary>
    /// Initializes the CardsManager, generates and shuffles cards, and instantiates them.
    /// </summary>
    public CardsManager(GameObject cardPrefab, Transform parent, int cardsCount, SpriteAtlas spriteAtlas)
    {
        this.cardPrefab = cardPrefab;
        this.cardsParent = parent;
        this.spriteAtlas = spriteAtlas;
        cards = new List<GameObject>();
        List<CardData> cardsData = GenerateUniqueRandomCards();
        List<CardData> gridCards = ShuffleCardsData(cardsData.GetRange(0, cardsCount));
        InstantiateCardObjects(cardPrefab, parent, gridCards);
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Returns the list of card GameObjects.
    /// </summary>
    public List<GameObject> GetCards()
    {
        return cards;
    }

    /// <summary>
    /// Instantiates cards from saved state data and updates their state.
    /// </summary>
    public void InstantiateFromSavedData(List<CardStateData> cardsStateData, GameResultEvaluator evaluator)
    {
        DestroyAllCards();

        foreach (CardStateData cardState in cardsStateData)
        {
            GameObject cardObj = GameObject.Instantiate(cardPrefab, cardsParent);
            Card card = cardObj.GetComponent<Card>();
            CardData cardData = new CardData(cardState.id, cardState.suit, cardState.rank, spriteAtlas);
            card.Init(cardData);
            cards.Add(cardObj);
            cardObj.SetActive(!cardState.isMatched);
            if (!cardState.isMatched)
            {
                card.CurrentFace = cardState.cardFace;
            }
        }

        evaluator.Init();

        foreach (GameObject cardObj in cards)
        {
            Card card = cardObj.GetComponent<Card>();
            if (card.CurrentFace == CardFace.frontFace)
            {
                card.SetCardFrontFacing();
            }
        }
    }

    /// <summary>
    /// Reveals all unmatched and hidden cards for hint purposes.
    /// </summary>
    public void RevealAllUnMatchedHiddenCards()
    {
        foreach (GameObject cardObj in cards)
        {
            Card card = cardObj.GetComponent<Card>();
            if (card.CurrentFace == CardFace.backFace && cardObj.activeSelf)
            {
                card.FlipCardForHint();
            }
        }
    }

    /// <summary>
    /// Handles result evaluation started event (optional logic).
    /// </summary>
    public void OnResultEvaluationStarted(Card firstCard, Card secondCard)
    {
        // Optional: Add logic if needed when evaluation starts
    }

    /// <summary>
    /// Handles match found event by disabling matched cards.
    /// </summary>
    public void OnMatchFound(Card firstCard, Card secondCard)
    {
        firstCard.gameObject.SetActive(false);
        secondCard.gameObject.SetActive(false);
    }

    /// <summary>
    /// Handles no match event by flipping both cards back.
    /// </summary>
    public void OnNoMatch(Card firstCard, Card secondCard)
    {
        firstCard.FlipCard();
        secondCard.FlipCard();
    }

    /// <summary>
    /// Handles game finished event.
    /// </summary>
    public void OnGameFinished()
    {
        Debug.Log("Game Finished! All matches found.");
    }

    /// <summary>
    /// Cleans up listeners on destroy.
    /// </summary>
    public void OnDestroy()
    {
        ClearListeners();
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Generates a deck of unique random cards (pairs for each suit and rank).
    /// </summary>
    private List<CardData> GenerateUniqueRandomCards()
    {
        int id = 0;
        List<CardData> deck = new List<CardData>();

        foreach (var suit in Constants.SUITS)
        {
            foreach (var rank in Constants.RANKS)
            {
                for (int i = 0; i < 2; i++)
                {
                    deck.Add(new CardData(id++, suit, rank, spriteAtlas));
                }
            }
        }
        return deck;
    }

    /// <summary>
    /// Instantiates card GameObjects and initializes them with card data.
    /// </summary>
    private void InstantiateCardObjects(GameObject cardPrefab, Transform parent, List<CardData> cardsData)
    {
        foreach (var cardData in cardsData)
        {
            GameObject cardObj = GameObject.Instantiate(cardPrefab, parent);
            Card card = cardObj.GetComponent<Card>();
            card.Init(cardData);
            cards.Add(cardObj);
        }
    }

    /// <summary>
    /// Shuffles the provided deck of cards.
    /// </summary>
    private List<CardData> ShuffleCardsData(List<CardData> deck)
    {
        for (int i = 0; i < deck.Count; i++)
        {
            CardData temp = deck[i];
            int randomIndex = Random.Range(i, deck.Count);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }
        return deck;
    }

    /// <summary>
    /// Destroys all card GameObjects and clears the list.
    /// </summary>
    private void DestroyAllCards()
    {
        foreach (var gameObject in cards)
        {
            GameObject.Destroy(gameObject);
        }
        cards.Clear();
    }

    /// <summary>
    /// Clears all listeners from card objects.
    /// </summary>
    private void ClearListeners()
    {
        foreach (var cardObj in cards)
        {
            Card card = cardObj.GetComponent<Card>();
            card.ClearListeners();
        }
    }

    #endregion
}