using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages all active power-ups in the game.
/// Provides activation, deactivation, and tracking of power-ups.
/// Implements a Singleton pattern.
/// </summary>
public class PowerUpManager : MonoBehaviour
{
    #region Singleton

    /// <summary>
    /// Global instance of the PowerUpManager.
    /// </summary>
    public static PowerUpManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    #endregion


    #region Fields

    /// <summary>
    /// List of currently active power-ups.
    /// </summary>
    private List<PowerUp> _activePowerUps = new List<PowerUp>();

    #endregion


    #region Public Methods

    /// <summary>
    /// Initializes the manager with a set of power-ups.
    /// </summary>
    /// <param name="powerUps">The list of power-ups to initialize with.</param>
    public void Init(List<PowerUp> powerUps)
    {
        _activePowerUps = powerUps;
    }

    /// <summary>
    /// Activates the first available power-up in the list and then deactivates it.
    /// </summary>
    public void ActivatePowerUp()
    {
        if (_activePowerUps.Count <= 0)
            return;

        PowerUp powerUp = _activePowerUps[0];
        powerUp.Activate();
        DeactivatePowerUp(powerUp);
    }

    #endregion


    #region Private Methods

    /// <summary>
    /// Deactivates and removes a specific power-up from the active list.
    /// </summary>
    /// <param name="powerUp">The power-up to deactivate.</param>
    private void DeactivatePowerUp(PowerUp powerUp)
    {
        if (_activePowerUps.Contains(powerUp))
        {
            powerUp.Deactivate();
            _activePowerUps.Remove(powerUp);
        }
    }

    #endregion


    #region Properties

    /// <summary>
    /// Gets the number of currently active power-ups.
    /// </summary>
    public int PowerUpCount => _activePowerUps.Count;

    #endregion
}
