using UnityEngine;

/// <summary>
/// Base class for all UI screens.
/// Provides methods for showing, hiding, and handling back button events.
/// </summary>
public abstract class UIScreen : MonoBehaviour
{
    #region Properties

    /// <summary>
    /// Returns true if the screen is currently visible (active in hierarchy).
    /// </summary>
    public bool IsVisible => gameObject.activeSelf;

    #endregion


    #region Public Methods

    /// <summary>
    /// Shows the screen and calls <see cref="OnShow"/> for additional logic.
    /// </summary>
    public virtual void Show()
    {
        gameObject.SetActive(true);
        OnShow();
    }

    /// <summary>
    /// Hides the screen and calls <see cref="OnHide"/> for additional logic.
    /// </summary>
    public virtual void Hide()
    {
        OnHide();
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Called when back button (or escape key) is pressed.
    /// Returns true if the screen handled the back press; otherwise false.
    /// </summary>
    public virtual bool OnBackPressed()
    {
        return false; // Default: not handled
    }

    #endregion


    #region Protected Methods

    /// <summary>
    /// Called when the screen is shown.
    /// Override in derived classes to add custom show logic.
    /// </summary>
    protected virtual void OnShow() { }

    /// <summary>
    /// Called when the screen is hidden.
    /// Override in derived classes to add custom hide logic.
    /// </summary>
    protected virtual void OnHide() { }

    #endregion
}