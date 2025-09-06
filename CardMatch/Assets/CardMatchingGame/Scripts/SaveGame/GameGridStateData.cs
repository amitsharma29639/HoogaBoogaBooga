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
    private int row;   // Number of rows in the grid

    [SerializeField]
    private int col;   // Number of columns in the grid

    [SerializeField]
    private List<CardStateData> cardsData;  // List of all card states in this grid

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

    public int Row
    {
        get => row;
    }

    public int Col
    {
        get => col;
    }
    
    public List<CardStateData> CardsData
    {
        get => cardsData;
        set => cardsData = value;
    }
    
}