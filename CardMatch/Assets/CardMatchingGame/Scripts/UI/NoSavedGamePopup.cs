using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles the "No Saved Game" popup UI.
/// Plays click sound and closes the popup when the Home button is clicked.
/// </summary>
public class NoSavedGamePopup : UIScreen
{
    #region Serialized Fields

    [SerializeField] private Button homeBtn;        // Home button in the popup

    #endregion


    #region Private Fields

    private Vector2 originalPos;                    // Original anchored position of the popup
    private RectTransform rectTransform;            // RectTransform component for animation

    #endregion


    #region Unity Callbacks

    private void Awake()
    {
        rectTransform = (RectTransform)transform;
        originalPos = rectTransform.anchoredPosition;

        // Add click listener to home button
        homeBtn.onClick.AddListener(OnHomeButtonClicked);
    }

    #endregion


    #region Public Methods

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
    /// Plays click sound and closes the popup.
    /// </summary>
    private void OnHomeButtonClicked()
    {
        AudioManager.Instance.PlayOneShot(
            AudioGroupConstants.GAMEPLAYSFX, 
            AudioGroupConstants.BUTTON_CLICK, 
            AudioGroupConstants.GAMEPLAYSFX
        );

        UIManager.Instance.PopScreen();
    }

    #endregion
}