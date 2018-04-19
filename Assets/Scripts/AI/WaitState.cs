using UnityEngine;

/// <summary>
/// <see cref="WaitState"/> acts a cooldown after an <see cref="IUnit"/> uses a skill.
/// The cooldown time is dependent on the unit's <see cref="IUnit.Speed"/> stat.
/// </summary>
public class WaitState : IState
{
    #region Constants & Properties
    public float Cooldown;

    private const float COOLDOWN_BASE = 10f;
    private const string STATE_NAME = "Wait";
    private const string COOLDOWN_ANIMATOR = "Cooldown";

    public IUnit Parent
    {
        get;
        private set;
    }

    public string Name
    {
        get { return STATE_NAME; }
    }
    #endregion

    /// <summary>
    /// Instantiates a new <see cref="WaitState"/>.
    /// </summary>
    /// <param name="parent">The <see cref="IUnit"/> entering this state</param>
    public WaitState(IUnit parent)
	{
		Parent = parent;
	}

    /// <summary>
    /// Upon entering <see cref="WaitState"/>, calculate the cooldown time for <see cref="IUnit"/>.
    /// </summary>
	public void EnterState()
	{
        Parent.UpdateStateText();
        Cooldown = Mathf.Round(COOLDOWN_BASE / Parent.Speed);
        Parent.Animator.SetFloat("Cooldown", Cooldown);

        Debug.Log(Parent.Name + ": Entered Wait State.");
    }

    /// <summary>
    /// Upon updating <see cref="WaitState"/>, reduce the cooldown. If cooldown is over, transition to
    /// a decision state using <see cref="SwitchDecisionState"/>.
    /// </summary>
	public void UpdateState()
	{
        if (Cooldown <= 0)
		{
            SwitchDecisionState();
		}
		else
		{
			Cooldown -= Parent.Speed * Time.deltaTime;
            Parent.Animator.SetFloat(COOLDOWN_ANIMATOR, Cooldown);
		}
	}

	public void ExitState()
	{
        Debug.Log(Parent.Name + ": Exited Wait State.");
	}

	public void SignalStopState()
	{
        Debug.Log(Parent.Name + ": Signaled Wait State to stop.");
    }

    /// <summary>
    /// When <see cref="IUnit"/> is switching states, check personality traits to determine which
    /// decision state to transition into.
    /// </summary>
    private void SwitchDecisionState()
    {
        if (Parent.Guarding > Parent.Healing)
        {
            Parent.Blackboard.ChangeState(new GuardianInitialDecisionState(Parent));
        }
        else if (Parent.Healing > Parent.Rage)
        {
            Parent.Blackboard.ChangeState(new HealCriticalState(Parent));
        }
        else
        {
            Parent.Blackboard.ChangeState(new RageDecisionState(Parent));
        }
    }
}
