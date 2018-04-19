/// <summary>
/// <see cref="GuardianInitialDecisionState"/> is the start state for the <see cref="GuardianUnit"/>.
/// <para>
///     The <see cref="GuardianUnit"/> will check to see if it should heal or attack based on how
///     low its <see cref="IUnit.CurrentHP"/> is.
/// </para>
/// </summary>
public class GuardianInitialDecisionState : DecisionState, IState
{
    private const float CRITICAL_HEALTH = 0.25f;
    private const string HEAL_SELF_TRIGGER = "HealSelfTrigger";
    private const string ATTACK_DECISION_TRIGGER = "AttackDecisionTrigger";

    public GuardianInitialDecisionState(IUnit parent)
        :base(parent)
    {
    }

    /// <summary>
    /// If the health of the <see cref="GuardianUnit"/> reaches a critical level,
    /// then the unit will transition to the <see cref="HealState"/> to heal itself.
    /// <para>
    ///     Otherwise, the unit will use <see cref="GuardAttackDecisionState"/>
    ///     to decide what to do next.
    /// </para>
    /// </summary>
    public override void UpdateState()
    {
        float parentHealthPercent = Parent.CurrentHP / Parent.MaxHP;

        if (parentHealthPercent <= CRITICAL_HEALTH)
        {
            Parent.Blackboard.ChangeState(new HealState(Parent));
            Parent.Animator.SetTrigger(HEAL_SELF_TRIGGER);
        }
        else
        {
            Parent.Blackboard.ChangeState(new GuardAttackDecisionState(Parent));
            Parent.Animator.SetTrigger(ATTACK_DECISION_TRIGGER);
        }
    }
}
