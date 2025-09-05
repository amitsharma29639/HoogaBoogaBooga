using System;
using UnityEngine;

/// <summary>
/// Manages the turn count for the card game.
/// Provides methods to update turns, notify listeners, and handle listener management.
/// </summary>
public class TurnsManager
{
    #region Events

    /// <summary>
    /// Event triggered when the turn count changes.
    /// </summary>
    public Action<int> OnTurnCountChanged = delegate { };

    #endregion

    #region Private Fields

    private int turn = 0; // Current turn count

    #endregion

    #region Constructor

    /// <summary>
    /// Initializes a new instance of TurnsManager with the specified turn count.
    /// </summary>
    /// <param name="turn">Initial turn count.</param>
    public TurnsManager(int turn)
    {
        this.turn = turn;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets the current turn count.
    /// </summary>
    public int Turn => turn;

    #endregion

    #region Listener Management

    /// <summary>
    /// Adds a listener for turn count changes.
    /// </summary>
    /// <param name="listner">The listener to add.</param>
    public void AddTurnsListner(Action<int> listner)
    {
        OnTurnCountChanged += listner;
    }

    /// <summary>
    /// Removes a listener for turn count changes.
    /// </summary>
    /// <param name="listner">The listener to remove.</param>
    public void RemoveListner(Action<int> listner)
    {
        OnTurnCountChanged -= listner;
    }

    /// <summary>
    /// Removes all listeners for turn count changes.
    /// </summary>
    public void RemoveAllListner()
    {
        OnTurnCountChanged = delegate { };
    }

    #endregion

    #region Turn Management

    /// <summary>
    /// Increments the turn count and notifies listeners.
    /// </summary>
    public void IncreamentTurnCount()
    {
        turn++;
        OnTurnCountChanged(turn);
        Debug.Log("Turn count " + turn);
    }

    /// <summary>
    /// Sets the turn count to the specified value and notifies listeners.
    /// </summary>
    /// <param name="turns">The new turn count.</param>
    public void SetTurns(int turns)
    {
        this.turn = turns;
        OnTurnCountChanged(turn);
    }

    #endregion

    #region Cleanup

    /// <summary>
    /// Cleans up listeners when the TurnsManager is destroyed.
    /// </summary>
    public void OnDestroy()
    {
        RemoveAllListner();
    }

    #endregion
}