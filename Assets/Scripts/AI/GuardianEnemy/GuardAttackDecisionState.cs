using UnityEngine;

/// <summary>
/// <see cref="GuardAttackDecisionState"/> is another decision-making state for
/// the <see cref="GuardianUnit"/>.
/// <para>
///     The <see cref="GuardianUnit"/> has two possible attacks:
///     <see cref="GuardianAttackState"/> and <see cref="RageAttackState"/>    
/// </para>
/// </summary>
public class GuardAttackDecisionState : DecisionState, IState
{
    #region State Constants
    private const float SPECIAL_MIN = 0f;
    private const float SPECIAL_MAX = 1f;
    private const float SPECIAL_LIMIT = 0.75f;
    private const string RAGE_ATTACK_TRIGGER = "RageAttackTrigger";
    private const string GUARDIAN_ATTACK_TRIGGER = "GuardianAttackTrigger";
    #endregion

    public GuardAttackDecisionState(IUnit parent)
        :base(parent)
    {
    }

    /// <summary>
    /// A modifier is calculated based on the <see cref="IUnit.Guarding"/> stat of the
    /// <see cref="GuardianUnit"/> and a random value.
    /// <para>
    ///     If the modifier equals or exceeds a limit, then the <see cref="GuardianUnit"/>
    ///     will transition to its unique <see cref="GuardianAttackState"/>.
    /// </para>
    /// <para>
    ///     Otherwise, the Unit will transition to <see cref="RageAttackState"/> to deal
    ///     standard damage to the Player.
    /// </para>
    /// </summary>
    public override void UpdateState()
    {
        float specialMod = Parent.Guarding * Random.Range(SPECIAL_MIN, SPECIAL_MAX);

        if (specialMod >= SPECIAL_LIMIT)
        {
            Parent.Blackboard.ChangeState(new GuardianAttackState(Parent));
            Parent.Animator.SetTrigger(GUARDIAN_ATTACK_TRIGGER);
        }
        else
        {
            Parent.Blackboard.ChangeState(new RageAttackState(Parent));
            Parent.Animator.SetTrigger(RAGE_ATTACK_TRIGGER);
        }
    }
}
