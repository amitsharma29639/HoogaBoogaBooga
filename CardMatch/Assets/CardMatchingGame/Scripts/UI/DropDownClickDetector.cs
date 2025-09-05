using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Detects click events on a UI element (e.g., dropdown) and plays a click sound.
/// Implements <see cref="IPointerClickHandler"/> to handle pointer click events.
/// </summary>
public class DropDownClickDetector : MonoBehaviour, IPointerClickHandler
{
    #region Public Methods

    /// <summary>
    /// Called when the UI element is clicked.
    /// Plays the button click sound effect.
    /// </summary>
    /// <param name="eventData">Event data associated with the pointer click.</param>
    public void OnPointerClick(PointerEventData eventData)
    {
        // Play button click sound using the AudioManager
        AudioManager.Instance.PlayOneShot(
            AudioGroupConstants.GAMEPLAYSFX, 
            AudioGroupConstants.BUTTON_CLICK, 
            AudioGroupConstants.GAMEPLAYSFX
        );
    }

    #endregion
}