using System.Collections.Generic;
using CardGame.GamePlay;
using UnityEngine;

/// <summary>
/// Evaluates the result of card reveals, manages match logic, and notifies listeners about game events.
/// Handles card matching, game completion, and event propagation.
/// </summary>
public class GameResultEvaluator : ICardEventsListner
{
    #region Private Fields

    private List<GameObject> cards; // All card GameObjects in the grid
    private List<IGameResultListner> listeners; // Listeners for game result events
    private Queue<Card> queue; // Queue for revealed cards awaiting evaluation

    #endregion

    #region Constructor

    /// <summary>
    /// Initializes the evaluator with the card list and sets up listeners.
    /// </summary>
    public GameResultEvaluator(List<GameObject> cards)
    {
        this.cards = cards;
        listeners = new List<IGameResultListner>();
        queue = new Queue<Card>();
        Init();
    }

    #endregion

    #region Initialization

    /// <summary>
    /// Adds this evaluator as a listener to all cards.
    /// </summary>
    public void Init()
    {
        foreach (var cardObj in cards)
        {
            Card card = cardObj.GetComponent<Card>();
            card.AddListener(this);
        }
    }

    #endregion

    #region Listener Management

    /// <summary>
    /// Adds a game result listener.
    /// </summary>
    public void AddListener(IGameResultListner listener)
    {
        listeners.Add(listener);
    }

    /// <summary>
    /// Removes a game result listener.
    /// </summary>
    public void RemoveListener(IGameResultListner listener)
    {
        listeners.Remove(listener);
    }

    /// <summary>
    /// Clears all listeners.
    /// </summary>
    private void ClearListeners()
    {
        listeners.Clear();
    }

    #endregion

    #region Card Evaluation Logic

    /// <summary>
    /// Called when a card is revealed; enqueues and evaluates pairs.
    /// </summary>
    private void OnCardRevealed(Card card)
    {
        if (card.CurrentFace == CardFace.frontFace)
        {
            queue.Enqueue(card);
        }

        // Evaluate pairs while at least two cards are in the queue
        while (queue.Count >= 2)
        {
            var firstCard = queue.Dequeue();
            var secondCard = queue.Dequeue();

            NotifyResultEvaluationStarted(firstCard, secondCard);
            EvaluateRevealedCards(firstCard, secondCard);
        }
    }

    /// <summary>
    /// Evaluates two revealed cards for a match and notifies listeners.
    /// </summary>
    private async void EvaluateRevealedCards(Card firstRevealedCard, Card secondRevealedCard)
    {
        if (firstRevealedCard.GetCardData().Equals(secondRevealedCard.GetCardData()))
        {
            AudioManager.Instance.PlayOneShot(AudioGroupConstants.GAMEPLAYSFX, AudioGroupConstants.MATCHED, AudioGroupConstants.GAMEPLAYSFX);
            await System.Threading.Tasks.Task.Delay(200); // Delay to show second card
            Debug.Log("Match Found!");
            NotifyMatchFound(firstRevealedCard, secondRevealedCard);
            if (CheckForGameFinish())
            {
                AudioManager.Instance.PlayOneShot(AudioGroupConstants.GAMEPLAYSFX, AudioGroupConstants.GAME_COMPLETE, AudioGroupConstants.GAMEPLAYSFX);
                await System.Threading.Tasks.Task.Delay(200);
                NotifyGameFinished();
            }
        }
        else
        {
            AudioManager.Instance.PlayOneShot(AudioGroupConstants.GAMEPLAYSFX, AudioGroupConstants.WRONG_MATCH, AudioGroupConstants.GAMEPLAYSFX);
            await System.Threading.Tasks.Task.Delay(400);
            Debug.Log("No Match. Hiding cards again.");
            NotifyNoMatch(firstRevealedCard, secondRevealedCard);
        }

        firstRevealedCard = null;
        secondRevealedCard = null;
    }

    /// <summary>
    /// Checks if all cards are matched and the game is finished.
    /// </summary>
    private bool CheckForGameFinish()
    {
        foreach (var cardObj in cards)
        {
            if (cardObj.activeSelf)
            {
                return false;
            }
        }
        return true;
    }

    #endregion

    #region Event Notification

    /// <summary>
    /// Notifies listeners that result evaluation has started.
    /// </summary>
    private void NotifyResultEvaluationStarted(Card firstCard, Card secondCard)
    {
        foreach (var listener in listeners)
        {
            listener.OnResultEvaluationStarted(firstCard, secondCard);
        }
    }

    /// <summary>
    /// Notifies listeners that a match was found.
    /// </summary>
    private void NotifyMatchFound(Card firstCard, Card secondCard)
    {
        foreach (var listener in listeners)
        {
            listener.OnMatchFound(firstCard, secondCard);
        }
    }

    /// <summary>
    /// Notifies listeners that no match was found.
    /// </summary>
    private void NotifyNoMatch(Card firstCard, Card secondCard)
    {
        foreach (var listener in listeners)
        {
            listener.OnNoMatch(firstCard, secondCard);
        }
    }

    /// <summary>
    /// Notifies listeners that the game is finished.
    /// </summary>
    private void NotifyGameFinished()
    {
        foreach (var listener in listeners)
        {
            listener.OnGameFinished();
        }
    }

    #endregion

    #region ICardEventsListner Implementation

    /// <summary>
    /// Called when the evaluator is disabled; clears listeners.
    /// </summary>
    public void OnDisable()
    {
        ClearListeners();
    }

    public void OnCardClicked(Card card)
    {
        // Optional: Add logic for card click if needed
    }

    public void OnCardFlipped(Card card)
    {
        // Optional: Add logic for card flip if needed
    }

    /// <summary>
    /// Called when card face-up animation finishes; triggers evaluation.
    /// </summary>
    public void OnCardFaceUpAnimationFinished(Card card)
    {
        OnCardRevealed(card);
    }

    public void OnCardFaceDownAnimationFinished(Card card)
    {
        // Optional: Add logic for face-down animation if needed
    }

    public void OnCardDisableAnimationFinished(Card card)
    {
        // Optional: Add logic for disable animation if needed
    }

    #endregion
}