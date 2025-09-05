using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the Heads-Up Display (HUD) for the card game, including score, turns, power-ups, and button interactions.
/// Handles UI updates and event propagation to listeners.
/// </summary>
public class HUDManager : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField] private TextMeshProUGUI scoreText;           // Displays the current score
    [SerializeField] private TextMeshProUGUI turnsText;           // Displays the number of turns
    [SerializeField] private Button saveGameBtn;                  // Button to save the game
    [SerializeField] private Button homeBtn;                      // Button to return to home screen
    [SerializeField] private Button powerUpBtn;                   // Button to use a power-up
    [SerializeField] private TextMeshProUGUI powerUpCountText;    // Displays the number of power-ups

    #endregion

    #region Private Fields

    private List<IHUDEventsListner> listeners;                    // Listeners for HUD events

    #endregion

    #region Unity Methods

    /// <summary>
    /// Initializes button listeners and the HUD event listeners list.
    /// </summary>
    private void Awake()
    {
        saveGameBtn.onClick.AddListener(OnClickSaveBtn);
        homeBtn.onClick.AddListener(OnClickHomeBtn);
        powerUpBtn.onClick.AddListener(OnClickPowerUpBtn);
        listeners = new List<IHUDEventsListner>();
    }

    /// <summary>
    /// Cleans up listeners when the HUDManager is destroyed.
    /// </summary>
    private void OnDestroy()
    {
        ClearListeners();
    }

    #endregion

    #region Button Event Handlers

    /// <summary>
    /// Handles the power-up button click event.
    /// Notifies listeners and updates the power-up count.
    /// </summary>
    private void OnClickPowerUpBtn()
    {
        foreach (var listener in listeners)
        {
            listener.OnClickPowerUp();
        }
        UpdatePowerUpCount();
    }

    /// <summary>
    /// Handles the home button click event.
    /// Plays audio and notifies listeners.
    /// </summary>
    private void OnClickHomeBtn()
    {
        AudioManager.Instance.PlayOneShot(AudioGroupConstants.GAMEPLAYSFX, AudioGroupConstants.BUTTON_CLICK, AudioGroupConstants.GAMEPLAYSFX);
        foreach (var listener in listeners)
        {
            listener.OnHomeBtnClicked();
        }
    }

    /// <summary>
    /// Handles the save game button click event.
    /// Plays audio and notifies listeners.
    /// </summary>
    private void OnClickSaveBtn()
    {
        AudioManager.Instance.PlayOneShot(AudioGroupConstants.GAMEPLAYSFX, AudioGroupConstants.BUTTON_CLICK, AudioGroupConstants.GAMEPLAYSFX);
        foreach (var listener in listeners)
        {
            listener.OnSaveBtnClicked();
        }
    }

    #endregion

    #region HUD Update Methods

    /// <summary>
    /// Updates the displayed score.
    /// </summary>
    public void OnScoreUpdate(int score)
    {
        scoreText.text = "Score:" + score;
    }

    /// <summary>
    /// Updates the displayed number of turns.
    /// </summary>
    public void OnTurnsUpdate(int turns)
    {
        turnsText.text = "Turns:" + turns;
    }

    /// <summary>
    /// Updates the displayed power-up count.
    /// </summary>
    public void UpdatePowerUpCount()
    {
        powerUpCountText.text = PowerUpManager.Instance.PowerUpCount.ToString();
    }

    #endregion

    #region Listener Management

    /// <summary>
    /// Adds a HUD event listener.
    /// </summary>
    public void AddListener(IHUDEventsListner listener)
    {
        listeners.Add(listener);
    }

    /// <summary>
    /// Removes a HUD event listener.
    /// </summary>
    public void RemoveListener(IHUDEventsListner listener)
    {
        listeners.Remove(listener);
    }

    /// <summary>
    /// Clears all HUD event listeners.
    /// </summary>
    private void ClearListeners()
    {
        listeners.Clear();
    }

    #endregion
}

/// <summary>
/// Interface for listening to HUD button events.
/// </summary>
public interface IHUDEventsListner
{
    void OnHomeBtnClicked();
    void OnSaveBtnClicked();
    void OnClickPowerUp();
}