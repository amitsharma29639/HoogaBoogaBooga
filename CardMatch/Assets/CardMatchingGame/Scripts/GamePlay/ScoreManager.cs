using System;
using UnityEngine;

/// <summary>
/// Manages the player's score, provides methods to update and notify listeners about score changes.
/// Handles listener management and score updates for the card game.
/// </summary>
[Serializable]
public class ScoreManager
{
    #region Events

    /// <summary>
    /// Event triggered when the score changes.
    /// </summary>
    public Action<int> OnScoreChanged = delegate { };

    #endregion

    #region Private Fields

    private int score; // Current score value

    #endregion

    #region Constructor

    /// <summary>
    /// Initializes a new instance of ScoreManager and sets the initial score.
    /// </summary>
    /// <param name="score">Initial score value.</param>
    public ScoreManager(int score)
    {
        SetScore(0);
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets the current score.
    /// </summary>
    public int Score => score;

    #endregion

    #region Listener Management

    /// <summary>
    /// Adds a listener for score changes.
    /// </summary>
    /// <param name="listner">The listener to add.</param>
    public void AddScoreListner(Action<int> listner)
    {
        OnScoreChanged += listner;
    }

    /// <summary>
    /// Removes a listener for score changes.
    /// </summary>
    /// <param name="listner">The listener to remove.</param>
    public void RemoveListner(Action<int> listner)
    {
        OnScoreChanged -= listner;
    }

    /// <summary>
    /// Removes all listeners for score changes.
    /// </summary>
    public void RemoveAllListner()
    {
        OnScoreChanged = delegate { };
    }

    #endregion

    #region Score Management

    /// <summary>
    /// Adds the specified amount to the score and notifies listeners.
    /// </summary>
    /// <param name="amount">Amount to add to the score.</param>
    public void AddScore(int amount)
    {
        score += amount;
        OnScoreChanged(score);
        Debug.Log("score : " + score);
    }

    /// <summary>
    /// Sets the score to the specified value and notifies listeners.
    /// </summary>
    /// <param name="score">The new score value.</param>
    public void SetScore(int score)
    {
        this.score = score;
        OnScoreChanged(score);
    }

    #endregion

    #region Cleanup

    /// <summary>
    /// Cleans up listeners when the ScoreManager is destroyed.
    /// </summary>
    public void OnDestroy()
    {
        OnScoreChanged = delegate { };
        RemoveAllListner();
    }

    #endregion
}