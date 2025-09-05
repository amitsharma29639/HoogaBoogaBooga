/// <summary>
/// A specific power-up that reveals all unmatched and hidden cards.
/// Inherits from <see cref="PowerUp"/>.
/// </summary>
public class RevealAllCardsPowerUp : PowerUp
{
    #region Fields

    private readonly CardsManager _cardManager; // Reference to the CardsManager that controls card state

    #endregion


    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="RevealAllCardsPowerUp"/>.
    /// </summary>
    /// <param name="manager">The card manager responsible for revealing cards.</param>
    public RevealAllCardsPowerUp(CardsManager manager)
    {
        PowerUpName = "Reveal All Cards";
        _cardManager = manager;
    }

    #endregion


    #region Public Methods

    /// <summary>
    /// Activates the power-up by revealing all unmatched hidden cards.
    /// </summary>
    public override void Activate()
    {
        _cardManager.RevealAllUnMatchedHiddenCards();
    }

    #endregion
}