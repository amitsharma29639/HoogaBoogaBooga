using UnityEngine;

/// <summary>
/// Handles the initialization and starting of the card game.
/// Manages grid configuration, responds to UI events, and launches gameplay.
/// </summary>
public class StartGame : MonoBehaviour, IHomeScreenEventsListner
{
    #region Serialized Fields

    [SerializeField] private GameObject gamePlayPrefab; // Prefab for the gameplay grid

    #endregion

    #region Private Fields

    private GameObject cardGridObj; // Instance of the card grid
    private int rows;               // Number of grid rows
    private int cols;               // Number of grid columns
    private bool isOn;              // Flag for loading saved game

    #endregion

    #region Unity Methods

    /// <summary>
    /// Initializes the main menu screen and sets default grid size.
    /// </summary>
    private void Start()
    {
        rows = 2;
        cols = 2;
        MainMenuScreen screen = (MainMenuScreen)UiScreenFactory.Instance.GetUIScreen(Constants.HOME_SCREEN);
        screen.Init();
        screen.AddListner(this);
        UIManager.Instance.PushScreen(screen);
    }

    #endregion

    #region IHomeScreenEventsListner Implementation

    /// <summary>
    /// Handles dropdown value changes to set grid size.
    /// </summary>
    /// <param name="value">Selected dropdown index.</param>
    public void OnDropDownValueChanged(int value)
    {
        switch (value)
        {
            case 0:
                rows = 2;
                cols = 2;
                break;
            case 1:
                rows = 2;
                cols = 3;
                break;
            case 2:
                rows = 3;
                cols = 4;
                break;
            case 3:
                rows = 3;
                cols = 6;
                break;
            case 4:
                rows = 4;
                cols = 4;
                break;
            case 5:
                rows = 4;
                cols = 6;
                break;
        }
    }

    /// <summary>
    /// Handles play button click, instantiates the grid and starts the game.
    /// </summary>
    public void OnPlayButtonClicked()
    {
        cardGridObj = GameObject.Instantiate(gamePlayPrefab, transform);
        CardsGridManager manager = cardGridObj.GetComponent<CardsGridManager>();
        GameConfig config = new GameConfig
        {
            Rows = rows,
            Cols = cols,
            LoadSavedGame = isOn
        };
        manager.Init(config);
    }

    /// <summary>
    /// Handles toggle value changes for loading saved game.
    /// </summary>
    /// <param name="isOn">Toggle state.</param>
    public void OnToggleValueChanged(bool isOn)
    {
        this.isOn = isOn;
    }

    #endregion
}