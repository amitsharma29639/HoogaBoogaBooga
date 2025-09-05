using System.Collections.Generic;
using System.IO;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles the Main Menu screen UI.
/// Manages dropdown selection, play button, saved game toggle, and fade transitions.
/// Notifies listeners about user interactions.
/// </summary>
public class MainMenuScreen : UIScreen
{
    #region Serialized Fields

    [SerializeField] TMP_Dropdown dropdown;           // Dropdown UI for selecting options
    [SerializeField] private Button playBtn;           // Play button
    [SerializeField] private Toggle playSavedGame;    // Toggle to play saved game
    [SerializeField] private Image fadeImage;         // Fade image for transitions

    #endregion


    #region Private Fields

    private List<IHomeScreenEventsListner> listners;   // Listeners for main menu events

    #endregion


    #region Public Methods

    /// <summary>
    /// Initializes the main menu screen by adding UI listeners and creating listener list.
    /// </summary>
    public void Init()
    {
        dropdown.onValueChanged.AddListener(OnDropDownValueChange);
        playBtn.onClick.AddListener(OnPlayButtonClicked);
        playSavedGame.onValueChanged.AddListener(OnToggleValueChanged);

        listners = new List<IHomeScreenEventsListner>();
    }

    /// <summary>
    /// Adds a listener to receive main menu events.
    /// </summary>
    /// <param name="listner">Listener to add.</param>
    public void AddListner(IHomeScreenEventsListner listner)
    {
        listners.Add(listner);
    }

    #endregion


    #region UI Callbacks

    /// <summary>
    /// Called when the dropdown value changes.
    /// Plays click sound and notifies listeners.
    /// </summary>
    /// <param name="value">Selected dropdown index.</param>
    private void OnDropDownValueChange(int value)
    {
        AudioManager.Instance.PlayOneShot(AudioGroupConstants.GAMEPLAYSFX, AudioGroupConstants.BUTTON_CLICK, AudioGroupConstants.GAMEPLAYSFX);

        foreach (var listner in listners)
        {
            listner.OnDropDownValueChanged(value);
        }
    }

    /// <summary>
    /// Called when the Play button is clicked.
    /// Handles fade transition and notifies listeners. Checks for saved game if toggle is on.
    /// </summary>
    private void OnPlayButtonClicked()
    {
        AudioManager.Instance.PlayOneShot(AudioGroupConstants.GAMEPLAYSFX, AudioGroupConstants.BUTTON_CLICK, AudioGroupConstants.GAMEPLAYSFX);

        if (playSavedGame.isOn)
        {
            if (!CheckSavedGame())
            {
                UIManager.Instance.PushScreen(UiScreenFactory.Instance.GetUIScreen(Constants.NO_SAVED_GAME_POPUP));
                playSavedGame.isOn = false;
                return;
            }
        }

        // Show fade effect and trigger play event
        fadeImage.gameObject.SetActive(true);
        Sequence seq = DOTween.Sequence();
        seq.Append(fadeImage.DOFade(1f, 0.5f).OnComplete(() =>
        {
            foreach (var listner in listners)
            {
                listner.OnPlayButtonClicked();
            }

            fadeImage.color = new Color(0, 0, 0, 0);
            fadeImage.gameObject.SetActive(false);
            UIManager.Instance.PopScreen();
        }));
    }

    /// <summary>
    /// Called when the saved game toggle value changes.
    /// Plays click sound and notifies listeners.
    /// </summary>
    /// <param name="isOn">Toggle state.</param>
    private void OnToggleValueChanged(bool isOn)
    {
        AudioManager.Instance.PlayOneShot(AudioGroupConstants.GAMEPLAYSFX, AudioGroupConstants.BUTTON_CLICK, AudioGroupConstants.GAMEPLAYSFX);

        foreach (var listner in listners)
        {
            listner.OnToggleValueChanged(isOn);
        }
    }

    #endregion


    #region Unity Callbacks

    protected override void OnShow()
    {
        // Optionally handle additional logic when screen shows
    }

    public override bool OnBackPressed()
    {
        Debug.Log("Main menu on back pressed");
        return false; 
    }

    private void OnDestroy()
    {
        // Clean up UI listeners and listener list
        dropdown.onValueChanged.RemoveAllListeners();
        playBtn.onClick.RemoveAllListeners();
        playSavedGame.onValueChanged.RemoveAllListeners();
        listners.Clear();
    }

    #endregion


    #region Private Methods

    /// <summary>
    /// Checks if a saved game exists in persistent storage.
    /// </summary>
    /// <returns>True if saved game exists, false otherwise.</returns>
    private bool CheckSavedGame()
    {
        return File.Exists(Application.persistentDataPath + "/SaveGameData.json");
    }

    #endregion
}

/// <summary>
/// Interface for listeners to receive Main Menu screen events.
/// </summary>
public interface IHomeScreenEventsListner
{
    void OnDropDownValueChanged(int value);
    void OnPlayButtonClicked();
    void OnToggleValueChanged(bool isOn);
}
