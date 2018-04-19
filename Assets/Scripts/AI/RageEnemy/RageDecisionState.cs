/// <summary>
/// <see cref="RageDecisionState"/> acts as an idle state while the <see cref="IUnit"/> decides whether or not
/// to attack with <see cref="RageAttackState"/>. 
/// <para>
///     Transition: When the <see cref="IUnit"/> is below <see cref="IUnit.MaxHP"/>
/// </para>
/// </summary>
public class RageDecisionState : DecisionState, IState
{
    private const string ATTACK_TRIGGER = "AttackTrigger";

    public RageDecisionState(IUnit parent)
        :base(parent)
    {
    }
    
    /// <summary>
    /// If <see cref="IUnit"/> has less health than their max health, transition to
    /// <see cref="RageAttackState"/>.
    /// </summary>
    public override void UpdateState()
    {
        if (Parent.CurrentHP < Parent.MaxHP)
        {
            Parent.Blackboard.ChangeState(new RageAttackState(Parent));
            Parent.Animator.SetTrigger(ATTACK_TRIGGER);
        }
    }
}
