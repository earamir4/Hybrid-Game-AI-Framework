using UnityEngine;

/// <summary>
/// <see cref="DecisionState"/> serves as the base <see cref="IState"/> for all states with "Decision"
/// in their name.
/// <para>
/// All states deriving from <see cref="DecisionState"/> should override <see cref="UpdateState"/>
/// with their own decision-making logic.
/// </para>
/// </summary>
public class DecisionState : IState
{
    private const string STATE_NAME = "Decision";

    public IUnit Parent
    {
        get;
        private set;
    }

    public string Name
    {
        get { return STATE_NAME; }
    }

    public DecisionState(IUnit parent)
    {
        Parent = parent;
    }

    public void EnterState()
    {
        Parent.UpdateStateText();
        //Debug.Log(Parent.Name + ": Entered " + Name);
    }

    /// <summary>
    /// If <see cref="IUnit"/> has less health than their max health, transition to
    /// <see cref="RageAttackState"/>.
    /// </summary>
    public virtual void UpdateState()
    {
        if (Parent.CurrentHP < Parent.MaxHP)
        {
            Parent.Blackboard.ChangeState(new RageAttackState(Parent));
        }
    }

    public virtual void ExitState()
    {
        //Debug.Log(Parent.Name + ": Left Rage Decision State.");
    }

    public void SignalStopState()
    {
        Debug.Log(Parent.Name + ": Signaled Rage Decision State to stop.");
    }
}
