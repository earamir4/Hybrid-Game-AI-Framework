/// <summary>
/// <see cref="IState"/> denotes the properties and methods that all states should contain.
/// </summary>
public interface IState
{
    /// <summary>
    /// All states should have an <see cref="IUnit"/> associated with it.
    /// <para>
    ///     Any class should be able to get a state's parent, but only the <see cref="IState"/> 
    ///     should be able to set its own parent <see cref="IUnit"/> when instantiated.
    /// </para>
    /// </summary>
    IUnit Parent { get; }

    /// <summary>
    /// All states should have a name that can be referenced.
    /// </summary>
    string Name { get; }

    #region Interface Methods
    /// <summary>
    /// Logic that should be set up before the state runs should be done here first.
    /// </summary>
    void EnterState();

    /// <summary>
    /// The main logic / decision-making of the state should go here.
    /// </summary>
	void UpdateState();

    /// <summary>
    /// Logic that needs to occur before the state ends should go here.
    /// </summary>
	void ExitState();

    /// <summary>
    /// If another method needs to stop a state, this should be called.
    /// </summary>
	void SignalStopState();
    #endregion
}
