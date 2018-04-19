using UnityEngine;

/// <summary>
/// <see cref="Blackboard"/> serves as a local manager for an individual <see cref="IUnit"/>.
/// This system keeps track of the current <see cref="IState"/> of a <see cref="IUnit"/> and is
/// called when a transition occurs between states.
/// </summary>
public class Blackboard
{
	public IState currentState;
    public IState prevState;

	/// <summary>
	/// Instantiates the Blackboard with the given startState
	/// </summary>
	/// <param name="startState">The first state upon starting the machine</param>
	public Blackboard(IState startState)
	{
		currentState = startState;
        prevState = null;
	}

	/// <summary>
	/// Calls the current state's <see cref="IState.EnterState"> method
	/// </summary>
	public void Start()
	{
		if (currentState != null)
		{
			currentState.EnterState();
		}
		else
		{
			Debug.Log("No Available State to Start!");
		}
	}

	/// <summary>
	/// Calls the current state's <see cref="IState.UpdateState"> method
	/// </summary>
	public void Update()
	{
		currentState.UpdateState();
	}

	/// <summary>
	/// If there is a current state running, the current state's <see cref="IState.ExitState">
	/// method will be called before transitioning to the new state.
	/// </summary>
	/// <param name="newState">The next state to transition into</param>
	public void ChangeState(IState newState)
	{
		if (currentState != null)
		{
			currentState.ExitState();
		}

        prevState = currentState;
		currentState = newState;
        Start();
	}
}
