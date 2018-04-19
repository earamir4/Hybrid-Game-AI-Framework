using UnityEngine;

/// <summary>
/// <see cref="HealAllyState"/> allows an <see cref="IUnit"/> to heal another <see cref="IUnit"/>.
/// </summary>
public class HealAllyState : IState
{
    #region Variables
    public IUnit DamagedUnit;

    private const float HEAL_RATE = 7.5f;
    private const string STATE_NAME = "Heal Ally";
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

    public HealAllyState (IUnit parent, IUnit damagedUnit)
    {
        Parent = parent;
        DamagedUnit = damagedUnit;
    }

    public void EnterState()
    {
        Parent.UpdateStateText();
        Debug.Log(Parent.Name + ": Entered " + Name);
    }

    /// <summary>
    /// The targeted <see cref="IUnit"/> will recover health based on a set rate.
    /// <para>
    ///     The parent <see cref="IUnit"/> will then transition to <see cref="WaitState"/>.
    /// </para>
    /// </summary>
    public void UpdateState()
    {
        Debug.Log(Parent.Name + ": Updated " + Name);
        DamagedUnit.CurrentHP += HEAL_RATE;

        if (DamagedUnit.CurrentHP > DamagedUnit.MaxHP)
        {
            DamagedUnit.CurrentHP = DamagedUnit.MaxHP;
        }

        DamagedUnit.UpdateHealthBar();
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
