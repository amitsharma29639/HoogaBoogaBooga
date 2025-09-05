using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles the Game Complete popup UI.
/// Manages animation, button interactions, and event notifications to listeners.
/// Inherits from <see cref="UIScreen"/>.
/// </summary>
public class GameCompletePopup : UIScreen
{
    #region Fields

    private Vector2 originalPos;                          // Original anchored position of the popup
    private RectTransform rectTransform;                  // RectTransform component for animation

    [SerializeField] private Button homeBtn;             // Home button in the popup
    private List<IGameCompletePopupEventListner> listners; // List of listeners for popup events

    #endregion


    #region Unity Callbacks

    private void Awake()
    {
        rectTransform = (RectTransform)transform;
        originalPos = rectTransform.anchoredPosition;
    }

    private void OnDestroy()
    {
        // Clean up listeners to avoid memory leaks
        homeBtn.onClick.RemoveAllListeners();
        listners.Clear();
    }

    #endregion


    #region Public Methods

    /// <summary>
    /// Initializes the popup by adding button listeners and creating the listener list.
    /// </summary>
    public void Init()
    {
        homeBtn.onClick.AddListener(OnHomeButtonClicked);
        listners = new List<IGameCompletePopupEventListner>();
    }

    /// <summary>
    /// Adds a listener to receive popup events.
    /// </summary>
    /// <param name="listener">Listener to add.</param>
    public void AddListener(IGameCompletePopupEventListner listener)
    {
        listners.Add(listener);
    }

    /// <summary>
    /// Shows the popup with animation from the top of the screen to its original position.
    /// </summary>
    public override void Show()
    {
        base.Show();

        // Start above the screen
        rectTransform.anchoredPosition = new Vector2(originalPos.x, Screen.height);

        // Animate into center
        rectTransform.DOAnchorPos(originalPos, 0.5f)
            .SetEase(Ease.InBack)
            .OnComplete(() => Debug.Log("✅ Popup Arrived at Center"));
    }

    #endregion


    #region Private Methods

    /// <summary>
    /// Handles the Home button click event.
    /// Plays click sound, navigates to home screen, and notifies listeners.
    /// </summary>
    private void OnHomeButtonClicked()
    {
        AudioManager.Instance.PlayOneShot(
            AudioGroupConstants.GAMEPLAYSFX, 
            AudioGroupConstants.BUTTON_CLICK, 
            AudioGroupConstants.GAMEPLAYSFX
        );

        UIManager.Instance.PopScreen();
        UIManager.Instance.PushScreen(UiScreenFactory.Instance.GetUIScreen(Constants.HOME_SCREEN));

        // Notify all listeners
        foreach (IGameCompletePopupEventListner listner in listners)
        {
            listner.OnClickHomeButton();
        }
    }

    #endregion
}

/// <summary>
/// Interface for listeners to receive GameCompletePopup events.
/// </summary>
public interface IGameCompletePopupEventListner
{
    /// <summary>
    /// Called when the Home button is clicked on the popup.
    /// </summary>
    void OnClickHomeButton();
}
