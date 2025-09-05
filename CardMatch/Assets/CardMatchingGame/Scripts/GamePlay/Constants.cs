using System.Collections.Generic;

/// <summary>
/// Contains constant values and collections used throughout the card game,
/// including card suits, ranks, and screen identifiers.
/// </summary>
public class Constants
{
    #region Card Suits

    // Card suit names
    private const string SUIT_HEARTS = "hearts";
    private const string SUIT_DIAMONDS = "diamonds";
    private const string SUIT_CLUBS = "clubs";
    private const string SUIT_SPADES = "spades";

    /// <summary>
    /// List of all card suits.
    /// </summary>
    public static readonly List<string> SUITS = new List<string>
    {
        SUIT_HEARTS,
        SUIT_DIAMONDS,
        SUIT_CLUBS,
        SUIT_SPADES
    };

    #endregion

    #region Card Ranks

    // Card rank names
    private const string RANK_ACE = "A";
    private const string RANK_2 = "02";
    private const string RANK_3 = "03";
    private const string RANK_4 = "04";
    private const string RANK_5 = "05";
    private const string RANK_6 = "06";
    private const string RANK_7 = "07";
    private const string RANK_8 = "08";
    private const string RANK_9 = "09";
    private const string RANK_10 = "10";
    private const string RANK_JACK = "J";
    private const string RANK_QUENN = "Q";
    private const string RANK_KING = "K";

    /// <summary>
    /// List of all card ranks.
    /// </summary>
    public static readonly List<string> RANKS = new List<string>
    {
        RANK_ACE,
        RANK_2,
        RANK_3,
        RANK_4,
        RANK_5,
        RANK_6,
        RANK_7,
        RANK_8,
        RANK_9,
        RANK_10,
        RANK_JACK,
        RANK_QUENN,
        RANK_KING
    };

    #endregion

    #region Screen Identifiers

    /// <summary>
    /// Screen ID for the home screen.
    /// </summary>
    public const int HOME_SCREEN = 0;

    /// <summary>
    /// Screen ID for the game complete popup.
    /// </summary>
    public const int GAME_COMPLETE_POPUP = 1;

    /// <summary>
    /// Screen ID for the no saved game popup.
    /// </summary>
    public const int NO_SAVED_GAME_POPUP = 2;

    #endregion
}