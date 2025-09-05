using System;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Component that enables click detection on a GameObject and notifies listeners via an event.
/// Implements IPointerClickHandler for Unity UI event handling.
/// </summary>
public class ClickableObject : MonoBehaviour, IPointerClickHandler
{
    #region Events

    /// <summary>
    /// Event triggered when the object is clicked.
    /// </summary>
    public Action OnClick = delegate { };

    #endregion

    #region Interface Implementation

    /// <summary>
    /// Called by Unity when the object is clicked.
    /// Invokes the OnClick event.
    /// </summary>
    /// <param name="eventData">Pointer event data.</param>
    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick();
    }

    #endregion
}