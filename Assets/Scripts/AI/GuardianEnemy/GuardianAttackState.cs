using UnityEngine;

/// <summary>
/// <see cref="GuardianAttackState"/> is a unique attack for the <see cref="GuardianUnit"/>.
/// <para>
///     In this state, the <see cref="GuardianUnit"/> attacks the Player for a high amount
///     of damage that bypasses the Player's Defense stat and recover half of the
///     Unit's <see cref="IUnit.MaxHP"/>.
/// </para>
/// </summary>
public class GuardianAttackState : IState
{
    #region State Constants & Properties
    private PlayerStats player;

    private const float GUARDIAN_ATTACK_MOD = 2f;
    private const float GUARDIAN_HEAL_MOD = 0.33f;
    private const string STATE_NAME = "Guardian Attack State";
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

    public GuardianAttackState(IUnit parent)
    {
        Parent = parent;
        player = GameManager.PlayerStats;
    }

    public void EnterState()
    {
        Parent.UpdateStateText();
        Debug.Log(Parent.Name + ": Entered " + Name);
    }

    /// <summary>
    /// When <see cref="GuardianAttackState"/> updates, the <see cref="GuardianUnit"/>
    /// deals a large amount of damage to the player that bypasses the Player's Defense.
    /// <para>
    ///     After dealing damage, the <see cref="GuardianUnit"/> transitions to the
    ///     <see cref="WaitState"/>.
    /// </para>
    /// <para>
    ///     Damage = Enemy's Attack * Guardian Modifier (ignores Player's Defense)
    /// </para>
    /// </summary>
    public void UpdateState()
    {
        Debug.Log(Parent.Name + ": Updated " + Name);
        float damage = (Parent.Attack * GUARDIAN_ATTACK_MOD) + player.Defense;
        player.DamageTaken(damage);

        Parent.Blackboard.ChangeState(new WaitState(Parent));
        Parent.Animator.SetTrigger(COOLDOWN_TRIGGER);
    }

    /// <summary>
    /// When the <see cref="GuardianUnit"/> exits this state, the <see cref="GuardianUnit"/>
    /// will recover a third of its <see cref="IUnit.MaxHP"/>.
    /// </summary>
    public void ExitState()
    {
        float healthBonus = GUARDIAN_HEAL_MOD * Parent.MaxHP;
        Parent.CurrentHP += healthBonus;

        if (Parent.CurrentHP > Parent.MaxHP)
        {
            Parent.CurrentHP = Parent.MaxHP;
        }

        Parent.UpdateHealthBar();

        Debug.Log(Parent.Name + ": Recovered " + healthBonus + " HP!");
        Debug.Log(Parent.Name + ": Exited" + Name);
    }

    public void SignalStopState()
    {
        Debug.Log(Parent.Name + "Signaled " + Name + " to stop");
    }
}
