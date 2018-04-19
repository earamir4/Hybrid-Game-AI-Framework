using UnityEngine;

/// <summary>
/// <see cref="GuardianUnit"/> is an enemy <see cref="IUnit"/> that is more complex than basic enemy types.
/// <para>
///     This unit has the following potential states:
///         <see cref="GuardianInitialDecisionState"/>
///         <see cref="HealState"/>
///         <see cref="GuardAttackDecisionState"/>
///         <see cref="RageAttackState"/>
///         <see cref="GuardianAttackState"/>
///         <see cref="WaitState"/>
/// </para>
/// </summary>
public class GuardianUnit : DefaultUnit
{
    #region Guardian Stats
    private const string GUARDIAN_NAME = "Guardian Enemy";

    private const float GUARDIAN_MAX_HEALTH = 100f;
    private const float GUARDIAN_BASE_ATTACK = 25f;
    private const float GUARDIAN_BASE_DEFENSE = 5f;
    private const float GUARDIAN_BASE_SPEED = 2f;

    private const float RAGE_STAT = 0f;
    private const float HEAL_STAT = 0f;
    private const float GUARD_STAT = 1.25f;
    #endregion

    public override string Name
    {
        get { return GUARDIAN_NAME; }
    }

    public override void Start ()
    {
        // Initialize Guardian Stats
        MaxHP = GUARDIAN_MAX_HEALTH;
        CurrentHP = GUARDIAN_MAX_HEALTH;
        Attack = GUARDIAN_BASE_ATTACK;
        Defense = GUARDIAN_BASE_DEFENSE;
        Speed = GUARDIAN_BASE_SPEED;

        Rage = RAGE_STAT;
        Healing = HEAL_STAT;
        Guarding = GUARD_STAT;

        // Initialize Blackboard and start state
        GuardianInitialDecisionState startState = new GuardianInitialDecisionState(this);
        Blackboard = new Blackboard(startState);
        Blackboard.Start();

        // Initialize UI
        HealthBar.maxValue = GUARDIAN_MAX_HEALTH;
        HealthBar.value = CurrentHP;
        CurrentStateText.text = CURRENT_STATE + startState.Name;

        // Initialize Animator
        Animator = GetComponent<Animator>();
    }
}
