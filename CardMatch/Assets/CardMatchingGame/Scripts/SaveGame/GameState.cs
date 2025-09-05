using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents the overall state of the game.
/// Stores the current score, number of turns, power-up count, and grid state.
/// </summary>
[Serializable]
public class GameState
{
    #region Fields

    [SerializeField]
    public int score;          // Current game score

    [SerializeField]
    public int turns;          // Number of turns taken

    [SerializeField]
    public int powerCount;     // Number of power-ups available

    [SerializeField]
    public GameGridStateData gridData;  // Current state of the game grid

    #endregion


    #region Constructor

    /// <summary>
    /// Initializes a new instance of <see cref="GameState"/> with a grid of given dimensions.
    /// </summary>
    /// <param name="row">Number of rows in the grid.</param>
    /// <param name="col">Number of columns in the grid.</param>
    public GameState(int row, int col)
    {
        gridData = new GameGridStateData(row, col);
    }

    #endregion


    #region Public Methods

    /// <summary>
    /// Sets the card data for the grid.
    /// </summary>
    /// <param name="cardsData">List of card state data.</param>
    public void SetGridData(List<CardStateData> cardsData)
    {
        gridData.SetGridData(cardsData);
    }

    /// <summary>
    /// Updates the current score.
    /// </summary>
    /// <param name="score">New score value.</param>
    public void SetScore(int score)
    {
        this.score = score;
    }

    /// <summary>
    /// Updates the current turn count.
    /// </summary>
    /// <param name="turns">New number of turns.</param>
    public void SetTurns(int turns)
    {
        this.turns = turns;
    }

    /// <summary>
    /// Updates the current power-up count.
    /// </summary>
    /// <param name="powerCount">New power-up count.</param>
    public void SetPowerCount(int powerCount)
    {
        this.powerCount = powerCount;
    }

    #endregion
}