/// <summary>
/// Base abstract class for all power-up types.
/// Defines the contract for activation and deactivation behavior.
/// </summary>
public abstract class PowerUp
{
    #region Properties

    /// <summary>
    /// The name of the power-up.
    /// </summary>
    public string PowerUpName { get; protected set; }

    #endregion


    #region Public Methods

    /// <summary>
    /// Executes the power-up logic.
    /// Must be implemented by derived classes.
    /// </summary>
    public abstract void Activate();

    /// <summary>
    /// Optionally cleans up or reverses effects when the power-up ends.
    /// Can be overridden by derived classes.
    /// </summary>
    public virtual void Deactivate() { }

    #endregion
}