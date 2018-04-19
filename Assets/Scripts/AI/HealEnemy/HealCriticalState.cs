/// <summary>
/// <see cref="HealCriticalState"/> is the initial state for the <see cref="HealUnit"/>.
/// <para>
///     Before any other decision is made, if the <see cref="IState.Parent"/> is low on health
///     (as indicated by <see cref="CRITICAL_HEALTH"/>), the Parent Unit will prioritize healing itself.
/// </para>
/// </summary>
public class HealCriticalState : DecisionState, IState
{
    private const float CRITICAL_HEALTH = 0.25f;
    private const string HEAL_SELF_TRIGGER = "HealSelfTrigger";
    private const string HEAL_DECISION_TRIGGER = "HealDecisionTrigger";

    public HealCriticalState(IUnit Parent)
        :base(Parent)
    {
    }

    /// <summary>
    /// If the health of the <see cref="IState.Parent"/> reaches a critical level,
    /// then the Parent Unit will transition to <see cref="HealState"/> to heal itself.
    /// <para>
    ///     Otherwise, the Parent Unit will use <see cref="HealDecisionState"/> to decide
    ///     what to do next.
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
            Parent.Blackboard.ChangeState(new HealDecisionState(Parent));
            Parent.Animator.SetTrigger(HEAL_DECISION_TRIGGER);
        }
    }
}
