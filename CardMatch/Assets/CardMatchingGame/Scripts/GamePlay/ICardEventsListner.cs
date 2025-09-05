using CardGame.GamePlay;

/// <summary>
/// Interface for listening to card events such as clicks, flips, and animation completions.
/// Implement this to handle card interactions in the game.
/// </summary>
public interface ICardEventsListner
{
    #region Card Interaction Events

    /// <summary>
    /// Called when a card is clicked.
    /// </summary>
    /// <param name="card">The card that was clicked.</param>
    void OnCardClicked(Card card);

    /// <summary>
    /// Called when a card is flipped.
    /// </summary>
    /// <param name="card">The card that was flipped.</param>
    void OnCardFlipped(Card card);

    /// <summary>
    /// Called when the card face-up animation finishes.
    /// </summary>
    /// <param name="card">The card whose animation finished.</param>
    void OnCardFaceUpAnimationFinished(Card card);

    /// <summary>
    /// Called when the card face-down animation finishes.
    /// </summary>
    /// <param name="card">The card whose animation finished.</param>
    void OnCardFaceDownAnimationFinished(Card card);

    /// <summary>
    /// Called when the card disable animation finishes.
    /// </summary>
    /// <param name="card">The card whose animation finished.</param>
    void OnCardDisableAnimationFinished(Card card);

    #endregion
}