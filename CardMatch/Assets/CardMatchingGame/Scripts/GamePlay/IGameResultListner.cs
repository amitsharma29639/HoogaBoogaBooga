using CardGame.GamePlay;

/// <summary>
/// Interface for listening to game result events such as evaluation start, match found, no match, and game completion.
/// Implement this to handle game result notifications in the card game.
/// </summary>
public interface IGameResultListner
{
    #region Game Result Events

    /// <summary>
    /// Called when result evaluation starts for two revealed cards.
    /// </summary>
    /// <param name="firstCard">The first revealed card.</param>
    /// <param name="secondCard">The second revealed card.</param>
    void OnResultEvaluationStarted(Card firstCard, Card secondCard);

    /// <summary>
    /// Called when a match is found between two cards.
    /// </summary>
    /// <param name="firstCard">The first matched card.</param>
    /// <param name="secondCard">The second matched card.</param>
    void OnMatchFound(Card firstCard, Card secondCard);

    /// <summary>
    /// Called when no match is found between two cards.
    /// </summary>
    /// <param name="firstCard">The first revealed card.</param>
    /// <param name="secondCard">The second revealed card.</param>
    void OnNoMatch(Card firstCard, Card secondCard);

    /// <summary>
    /// Called when the game is finished.
    /// </summary>
    void OnGameFinished();

    #endregion
}