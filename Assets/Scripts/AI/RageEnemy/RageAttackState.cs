using UnityEngine;

/// <summary>
/// <see cref="RageAttackState"/> is a basic attack that enemy <see cref="IUnit"/> can use.
/// </summary>
public class RageAttackState : IState
{
    #region State Constants & Properties
    private const string STATE_NAME = "Rage Attack";
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

    public RageAttackState(IUnit parent)
    {
        Parent = parent;
    }

    public void EnterState()
    {
        Parent.UpdateStateText();
        Debug.Log(Parent.Name + ": Entered Rage Attack State.");
    }

    /// <summary>
    /// Upon updating <see cref="RageAttackState"/>, the enemy deals damage to the player.
    /// After damage is dealt, the enemy transitions to <see cref="WaitState"/>.
    /// <para>
    ///     Damage = Enemy's Attack - Player's Defense
    /// </para>
    /// </summary>
    public void UpdateState()
    {
        Debug.Log(Parent.Name + ": Updated Rage Attack State.");
        GameManager.PlayerStats.DamageTaken(Parent.Attack);

        Parent.Blackboard.ChangeState(new WaitState(Parent));
        Parent.Animator.SetTrigger(COOLDOWN_TRIGGER);
    }

    public void ExitState()
    {
        Debug.Log(Parent.Name + ": Left Rage Attack State.");
    }

    public void SignalStopState()
    {
        Debug.Log(Parent.Name + ": Signaled Rage Attack State to stop.");
    }
}
