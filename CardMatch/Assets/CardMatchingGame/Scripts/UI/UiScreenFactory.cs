using UnityEngine;

/// <summary>
/// Factory class to provide references to UIScreens by ID.
/// Implements a singleton pattern.
/// </summary>
public class UiScreenFactory : MonoBehaviour
{
    #region Singleton

    public static UiScreenFactory Instance { get; private set; } // Singleton instance

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Ensure only one factory exists
            return;
        }

        Instance = this;
    }

    #endregion


    #region Serialized Fields

    [SerializeField] private MainMenuScreen homeScreen;         // Reference to the Home screen
    [SerializeField] private GameCompletePopup gameCompletePopup; // Reference to Game Complete popup
    [SerializeField] private NoSavedGamePopup noSavedGamePopup;   // Reference to No Saved Game popup

    #endregion


    #region Public Methods

    /// <summary>
    /// Returns a UIScreen instance based on the provided ID.
    /// </summary>
    /// <param name="id">The screen ID defined in <see cref="Constants"/>.</param>
    /// <returns>The UIScreen instance if found; otherwise null.</returns>
    public UIScreen GetUIScreen(int id)
    {
        switch (id)
        {
            case Constants.HOME_SCREEN:
                return homeScreen;
            case Constants.GAME_COMPLETE_POPUP:
                return gameCompletePopup;
            case Constants.NO_SAVED_GAME_POPUP:
                return noSavedGamePopup;
            default:
                return null;
        }
    }

    #endregion
}