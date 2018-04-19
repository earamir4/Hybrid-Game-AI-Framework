using UnityEngine;

/// <summary>
/// <see cref="HealState"/> allows an <see cref="IUnit"/> to heal itself.
/// </summary>
public class HealState : IState
{
    #region Variables
    private const float HEAL_RATE = 25f;
    private const string STATE_NAME = "Heal Self";
    private const string COOLDOWN_TRIGGER = "CooldownTrigger";

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
    /// Instantiates a new <see cref="HealState"/>.
    /// </summary>
    /// <param name="parent">The <see cref="IUnit"/> entering this state</param>
	public HealState(IUnit parent)
    {
        Parent = parent;
    }
    
	public void EnterState()
    {
        Parent.UpdateStateText();
        Debug.Log(Parent.Name + ": Entered " + Name);
    }

    /// <summary>
    /// Upon updating <see cref="HealState"/>, heal the Parent <see cref="IUnit"/> by a fixed rate.
    /// After healing, transition to <see cref="WaitState"/>.
    /// </summary>
	public void UpdateState()
    {
        Debug.Log(Parent.Name + ": Updated " + Name);

        Parent.CurrentHP += HEAL_RATE;

        if (Parent.CurrentHP > Parent.MaxHP)
        {
            Parent.CurrentHP = Parent.MaxHP;
        }

        Parent.UpdateHealthBar();
        Parent.Blackboard.ChangeState(new WaitState(Parent));
        Parent.Animator.SetTrigger(COOLDOWN_TRIGGER);
    }

    public void ExitState()
    {
        Debug.Log(Parent.Name + ": Exited " + Name);
    }

    public void SignalStopState()
    {
        Debug.Log(Parent.Name + ": Signaled " + Name + " to stop.");
    }
}
