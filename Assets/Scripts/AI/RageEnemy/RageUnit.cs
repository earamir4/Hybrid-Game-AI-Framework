using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// <see cref="RageUnit"/> is a basic enemy <see cref="IUnit"/>.
/// <para>
/// This unit has three possible states:
///     <see cref="RageDecisionState"/>,
///     <see cref="RageAttackState"/>,
///     and <see cref="WaitState"/>.
/// </para>
/// </summary>
public class RageUnit : MonoBehaviour, IUnit
{
    #region Enemy UI
    public Slider HealthBar;
    public Text CurrentStateText;
    public Text PrevStateText;

    private string CURRENT_STATE = "State: ";
    private string PREV_STATE = "Prev State: ";
    private string NULL_STATE = "null state";
    #endregion

    #region Private constants
    private const string ENEMY_NAME = "Rage Enemy";

    private const float MAX_HEALTH = 35f;
    private const float BASE_ATTACK = 25f;
    private const float BASE_DEFENSE = 4f;
    private const float BASE_SPEED = 1.75f;

    private const float RAGE_STAT = 1f;
    private const float HEAL_STAT = 0f;
    private const float GUARD_STAT = 0f;

    private const string DEATH_TRIGGER = "DeathTrigger";
    #endregion

    #region Stat Properties
    public Blackboard Blackboard
    {
        get;
        private set;
    }

    public Animator Animator
    {
        get;
        set;
    }

    public string Name
    {
        get { return ENEMY_NAME; }
    }

    public float MaxHP
    {
        get { return MAX_HEALTH; }
    }

    public float CurrentHP
    {
        get;
        set;
    }

    public float Attack
    {
        get;
        set;
    }

    public float Defense
    {
        get;
        set;
    }

    public float Speed
    {
        get;
        set;
    }
    #endregion

    #region Personality Properties
    public float Rage
    {
        get { return RAGE_STAT; }
    }

    public float Healing
    {
        get { return HEAL_STAT; }
    }

    public float Guarding
    {
        get { return HEAL_STAT; }
    }
    #endregion

    #region Start & Update
    // Use this for initialization
    void Start()
    {
        // Initialize Stats
        CurrentHP = MAX_HEALTH;
        Attack = BASE_ATTACK;
        Defense = BASE_DEFENSE;
        Speed = BASE_SPEED;

        // Initialize Blackboard and StartState
        RageDecisionState startState = new RageDecisionState(this);
        Blackboard = new Blackboard(startState);
        Blackboard.Start();

        // Initialize UI
        HealthBar.maxValue = MaxHP;
        HealthBar.value = CurrentHP;
        CurrentStateText.text = CURRENT_STATE + startState.Name;

        // Initialize Animator
        Animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Blackboard.Update();
    }
    #endregion

    #region Damage & Death
    public void DamageTaken(float damage)
    {
        damage -= Defense;

        if (damage <= 0)
        {
            CurrentHP--;
        }
        else
        {
            CurrentHP -= damage;
        }

        UpdateHealthBar();

        // Check if Enemy is dead
        if (CurrentHP <= 0)
        {
            Death();
        }

        // Debug
        Debug.Log(Name + ": Took " + damage + " damage!");
        Debug.Log(Name + " Current HP: " + CurrentHP + "/" + MaxHP);
    }

    /// <summary>
    /// Called when the enemy reaches 0 health.
    /// </summary>
    public void Death()
    {
        Animator.SetTrigger(DEATH_TRIGGER);
        Debug.Log(Name + ": is dead!");
        GameManager.EnemyObjects.Remove(gameObject);
        GameManager.EnemyUnits.Remove(this);
        Destroy(gameObject);
    }
    #endregion

    #region Unit UI
    /// <summary>
    /// Called when the <see cref="IUnit"/> needs to update its healthbar.
    /// </summary>
    public void UpdateHealthBar()
    {
        HealthBar.value = CurrentHP;
    }

    /// <summary>
    /// Called when the <see cref="IUnit"/> changes states to update the states displayed in the UI.
    /// </summary>
    public void UpdateStateText()
    {
        if (Blackboard.currentState != null)
        {
            CurrentStateText.text = CURRENT_STATE + Blackboard.currentState.Name;
        }
        else
        {
            CurrentStateText.text = CURRENT_STATE + NULL_STATE;
        }
        
        if (Blackboard.prevState != null)
        {
            PrevStateText.text = PREV_STATE + Blackboard.prevState.Name;
        }
        else
        {
            PrevStateText.text = PREV_STATE + NULL_STATE;
        }
    }
    #endregion
}
