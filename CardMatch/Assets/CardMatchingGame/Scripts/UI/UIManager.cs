using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages UI screens in a stack-based system.
/// Handles showing, hiding, and back navigation for UIScreens.
/// Implements a singleton pattern.
/// </summary>
public class UIManager : MonoBehaviour
{
    #region Singleton

    public static UIManager Instance { get; private set; }  // Singleton instance

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Ensure only one UIManager exists
            return;
        }

        Instance = this;
    }

    #endregion


    #region Fields

    private Stack<UIScreen> screenStack = new Stack<UIScreen>();  // Stack to manage active screens

    #endregion


    #region Public Methods

    /// <summary>
    /// Pushes a new screen onto the stack and displays it.
    /// </summary>
    /// <param name="screen">The UIScreen to display.</param>
    public void PushScreen(UIScreen screen)
    {
        screenStack.Push(screen);
        screen.Show();
    }

    /// <summary>
    /// Pops the top screen from the stack and hides it.
    /// Shows the previous screen if any exists.
    /// </summary>
    public void PopScreen()
    {
        if (screenStack.Count == 0) return;

        UIScreen top = screenStack.Pop();
        top.Hide();

        if (screenStack.Count > 0)
        {
            screenStack.Peek().Show();
        }
    }

    /// <summary>
    /// Handles back button press.
    /// Delegates the event to the top screen; if not handled, pops the screen.
    /// </summary>
    public void HandleBackPress()
    {
        if (screenStack.Count == 0) return;

        UIScreen top = screenStack.Peek();
        bool handled = top.OnBackPressed();

        if (!handled)
        {
            PopScreen();
        }
    }

    #endregion
}