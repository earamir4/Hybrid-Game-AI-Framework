/// <summary>
/// <see cref="HealDecisionState"/> acts as an idle state while the <see cref="IUnit"/> decides whether or not
/// to heal.
/// <para>
///     Transition: When the <see cref="IUnit"/> is below <see cref="IUnit.MaxHP"/>
/// </para>
/// </summary>
public class HealDecisionState : DecisionState, IState
{
    private const float CRITICAL_HEALTH = 0.25f;
    private const string HEAL_SELF_TRIGGER = "HealSelfTrigger";
    private const string HEAL_ALLY_TRIGGER = "HealAllyTrigger";

    public HealDecisionState(IUnit parent)
        :base(parent)
    {
    }

    /// <summary>
    /// If another enemy's health is less than the parent <see cref="IUnit"/>'s health, then transition to
    /// <see cref="HealAllyState"/>.
    /// <para>
    ///     Else if the parent <see cref="IUnit"/> has less health than their max health, then transition to
    ///     <see cref="HealState"/>.    
    /// </para>
    /// </summary>
    public override void UpdateState()
    {
        float parentHealthPercent = Parent.CurrentHP / Parent.MaxHP;

        int index = 0;
        bool isDamaged = false;
        float allyHealthPercent = 0f;
        IUnit enemyUnit = GameManager.EnemyUnits[index];

        while (index < GameManager.EnemyUnits.Count && !isDamaged)
        {
            enemyUnit = GameManager.EnemyUnits[index];

            if (enemyUnit.CurrentHP < enemyUnit.MaxHP && !string.Equals(enemyUnit.Name, Parent.Name))
            {
                isDamaged = true;
                allyHealthPercent = enemyUnit.CurrentHP / enemyUnit.MaxHP;
            }
            else
            {
                index++;
            }
        }

        if (isDamaged && allyHealthPercent < parentHealthPercent)
        {
            Parent.Blackboard.ChangeState(new HealAllyState(Parent, enemyUnit));
            Parent.Animator.SetTrigger(HEAL_ALLY_TRIGGER);
        }
        else if (Parent.CurrentHP < Parent.MaxHP)
        {
            Parent.Blackboard.ChangeState(new HealState(Parent));
            Parent.Animator.SetTrigger(HEAL_SELF_TRIGGER);
        }
    }
}
