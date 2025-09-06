using System;
using CardGame.GamePlay;
using UnityEngine;

/// <summary>
/// Represents the state of a single card in the game.
/// Contains its id, suit, rank, matched status, and current face.
/// </summary>
[Serializable]
public class CardStateData
{
    #region Fields

    [SerializeField] 
    private int id;               // Unique identifier for the card
    
    [SerializeField] 
    private string suit;          // Suit of the card (e.g., Hearts, Spades)

    [SerializeField] 
    private string rank;          // Rank of the card (e.g., Ace, 2, King)

    [SerializeField] 
    private bool isMatched;       // Whether the card has been matched

    [SerializeField] 
    private CardFace cardFace;    // Current face of the card (Front/Back)

    #endregion


    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CardStateData"/> class.
    /// </summary>
    /// <param name="id">Unique identifier of the card.</param>
    /// <param name="suit">Suit of the card.</param>
    /// <param name="rank">Rank of the card.</param>
    /// <param name="isMatched">Whether the card is matched.</param>
    /// <param name="cardFace">The current face of the card.</param>
    public CardStateData(int id, string suit, string rank, bool isMatched, CardFace cardFace)
    {
        this.id = id;
        this.suit = suit;
        this.rank = rank;
        this.isMatched = isMatched;
        this.cardFace = cardFace;
    }

    #endregion
    
    public int ID
    {
        get => id;
    }

    public string Suit
    {
        get => suit;
    }

    public string Rank
    {
        get => rank;
    }

    public bool IsMatched
    {
        get => isMatched;
    }

    public CardFace CardFace
    {
        get => cardFace;
    }

}