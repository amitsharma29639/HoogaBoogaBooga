/// <summary>
/// Holds configuration settings for the card game, such as grid size and load state.
/// </summary>
public class GameConfig
{
    #region Private Fields

    /// <summary>
    /// Number of rows in the card grid.
    /// </summary>
    private int rows;

    /// <summary>
    /// Number of columns in the card grid.
    /// </summary>
    private int cols;

    /// <summary>
    /// Indicates whether to load a saved game.
    /// </summary>
    private bool loadSavedGame;

    #endregion

    #region Constructor

    /// <summary>
    /// Initializes a new instance of GameConfig with default values.
    /// </summary>
    public GameConfig()
    {
        this.rows = 2;
        this.cols = 2;
        this.loadSavedGame = false;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets whether to load a saved game.
    /// </summary>
    public bool LoadSavedGame
    {
        get { return loadSavedGame; }
        set { loadSavedGame = value; }
    }

    /// <summary>
    /// Gets or sets the number of rows in the grid.
    /// </summary>
    public int Rows
    {
        get => rows;
        set => rows = value;
    }

    /// <summary>
    /// Gets or sets the number of columns in the grid.
    /// </summary>
    public int Cols
    {
        get => cols;
        set => cols = value;
    }

    #endregion
}