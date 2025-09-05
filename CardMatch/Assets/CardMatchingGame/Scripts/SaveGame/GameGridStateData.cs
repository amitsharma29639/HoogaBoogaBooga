using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents the state of a game grid.
/// Stores the number of rows and columns, and the list of card states in the grid.
/// </summary>
[Serializable]
public class GameGridStateData
{
    #region Fields

    [SerializeField]
    public int row;   // Number of rows in the grid

    [SerializeField]
    public int col;   // Number of columns in the grid

    [SerializeField]
    public List<CardStateData> cardsData;  // List of all card states in this grid

    #endregion


    #region Constructor

    /// <summary>
    /// Initializes a new instance of <see cref="GameGridStateData"/> with specified row and column count.
    /// </summary>
    /// <param name="row">Number of rows in the grid.</param>
    /// <param name="col">Number of columns in the grid.</param>
    public GameGridStateData(int row, int col)
    {
        this.row = row;
        this.col = col;
    }

    #endregion


    #region Public Methods

    /// <summary>
    /// Sets the card data for this grid.
    /// </summary>
    /// <param name="cardsDate">List of card state data to assign.</param>
    public void SetGridData(List<CardStateData> cardsDate)
    {
        this.cardsData = cardsDate;
    }

    #endregion
}