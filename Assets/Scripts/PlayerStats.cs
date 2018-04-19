using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Represents the Player <see cref="IUnit"/> that the Player will be in control of.
/// <para>
///     By having the Player script implement the <see cref="IUnit"/> interface, the Player
///     has the same stat types as enemy <see cref="IUnit"/>s.
/// </para>
/// <para>
///     Theoretically, by implementing <see cref="IUnit"/>, the Player can have their own
///     <see cref="Blackboard"/> and use <see cref="IState"/>s.
/// </para>
/// </summary>
public class PlayerStats : MonoBehaviour, IUnit
{
    #region Player UI Variables
    public Slider HealthBar;
    public Slider CooldownBar;
    public GameObject DebugPanel;
    public Text HPText;
    public Text CPText;
    public Text TargetText;
    #endregion

    #region Other Player Variables
    public IUnit TargetUnit;
    #endregion

    #region Player Stat Constants
    private const float MAX_HEALTH = 250f;
    private const float BASE_ATTACK = 15f;
    private const float BASE_DEFENSE = 10f;
    private const float BASE_SPEED = 60f;

    private const string PLAYER_NAME = "Player";
    private const float COOLDOWN_LIMIT = 100f;
    private const float HEAL_RATE = 75f;

    private const float RAGE_STAT = 0f;
    private const float HEAL_STAT = 0f;
    private const float GUARD_STAT = 0f;

    private const string HP_PREFIX = "Player HP: ";
    private const string CP_PREFIX = "Player CP: ";
    private const string TARGET_PREFIX = "Player Target: ";
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
        get;
        set;
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

    public float Cooldown
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

    #region Unity Start & Update
    void Start ()
    {
        // Initialize Player Stats
        Name = PLAYER_NAME;
        CurrentHP = MAX_HEALTH;
        Attack = BASE_ATTACK;
        Defense = BASE_DEFENSE;
        Speed = BASE_SPEED;
        Cooldown = COOLDOWN_LIMIT;

        Blackboard = new Blackboard(null);

        // Set Target
        TargetUnit = GameManager.EnemyUnits[0];

        // Initialize Player UI
        HealthBar.maxValue = MaxHP;
        HealthBar.value = CurrentHP;

        CooldownBar.maxValue = COOLDOWN_LIMIT;
        CooldownBar.value = Cooldown;

        // Initialize Debug Panel Text
        HPText.text = HP_PREFIX + CurrentHP + "/" + MaxHP;
        CPText.text = CP_PREFIX + Cooldown;
        TargetText.text = TARGET_PREFIX + TargetUnit.Name;
	}

    /// <summary>
    /// Check if the Player is in a cooldown state.
    /// <para>
    ///     If the player pressed left mouse clicked, check to see if they changed targets.
    /// </para>
    /// </summary>
    void Update()
    {
        CooldownRecharge();

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            ChangeTargets();
        }
    }
    #endregion

    #region Player Logic (Non-Attack Functions)
    /// <summary>
    /// Called by an opposing <see cref="IUnit"/> when dealing damage to the Player.
    /// </summary>
    /// <param name="damage">Damage dealt by enemy <see cref="IUnit"/> attack</param>
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

        // Check if Player is dead
        if (CurrentHP <= 0)
        {
            Death();
        }

        // Debug
        Debug.Log(Name + ": Took " + damage + " damage!");
        Debug.Log(Name + " Current HP: " + CurrentHP + "/" + MaxHP);
    }

    /// <summary>
    /// Called when the Player reaches 0 health.
    /// </summary>
    public void Death()
    {
        Debug.Log(Name + ": is dead!");
    }

    /// <summary>
    /// The Player also has a cooldown effect and must wait before using another skill.
    /// <para>
    ///     <see cref="Cooldown"/> rate is determined by <see cref="IUnit.Speed"/>.
    /// </para>
    /// </summary>
    private void CooldownRecharge()
    {
        if (Cooldown < COOLDOWN_LIMIT)
        {
            Cooldown += Speed * Time.deltaTime;
            CooldownBar.value = Cooldown;
            CPText.text = CP_PREFIX + Cooldown;
        }
        else if (Cooldown >= COOLDOWN_LIMIT)
        {
            Cooldown = COOLDOWN_LIMIT;
            CooldownBar.value = Cooldown;
            CPText.text = CP_PREFIX + Cooldown;
        }
    }
    #endregion

    #region Targeting
    /// <summary>
    /// Allows the Player to change targets based on mouse position.
    /// </summary>
    public void ChangeTargets()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray);

        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.CompareTag("Enemy"))
            {
                SelectTarget(hit.transform);
                break;
            }
        }
    }

    /// <summary>
    /// Sets the Player's <see cref="TargetUnit"/>.
    /// </summary>
    /// <param name="transform">Used to get an enemy <see cref="IUnit"/>.</param>
    private void SelectTarget(Transform transform)
    {
        GameObject targetObject = transform.gameObject;
        TargetUnit = targetObject.GetComponentInChildren<IUnit>();
        TargetText.text = TARGET_PREFIX + TargetUnit.Name;
    }
    #endregion

    #region Player UI
    /// <summary>
    /// Called when the <see cref="IUnit"/> needs to update its healthbar.
    /// </summary>
    public void UpdateHealthBar()
    {
        HealthBar.value = CurrentHP;
        HPText.text = HP_PREFIX + CurrentHP + "/" + MaxHP;
    }

    /// <summary>
    /// Not used for the Player unit.
    /// </summary>
    public void UpdateStateText()
    {
    }

    /// <summary>
    /// Toggles the visibility of the <see cref="DebugPanel"/> in the scene.
    /// </summary>
    public void ToggleDebugMenu()
    {
        DebugPanel.SetActive(!DebugPanel.activeInHierarchy);
    }
    #endregion

    #region Player Commands
    /// <summary>
    /// Called when the Player deals damage to their <see cref="TargetUnit"/>.
    /// </summary>
    public void BasicAttack()
    {
        if (TargetUnit != null && !(Cooldown < COOLDOWN_LIMIT))
        {
            Debug.Log(Name + ": attacks " + TargetUnit.Name);
            TargetUnit.DamageTaken(Attack);

            Cooldown = 0;
        }
    }

    /// <summary>
    /// Heals the Player's <see cref="CurrentHP"/> by a small amount.
    /// </summary>
    public void Heal()
    {
        if (!(Cooldown < COOLDOWN_LIMIT))
        {
            CurrentHP += HEAL_RATE;
            Cooldown = 0;

            if (CurrentHP > MaxHP)
            {
                CurrentHP = MaxHP;
            }

            UpdateHealthBar();
        }
    }

    /// <summary>
    /// Reduces the Target <see cref="IUnit"/> health by 75% of their
    /// <see cref="IUnit.MaxHP"/>.
    /// </summary>
    public void CutAttack()
    {
        if (TargetUnit != null && !(Cooldown < COOLDOWN_LIMIT))
        {
            Debug.Log(Name + ": attacks " + TargetUnit.Name);
            float damage = (TargetUnit.MaxHP * 0.75f) + TargetUnit.Defense;

            TargetUnit.DamageTaken(damage);

            Cooldown = 0;
        }
    }
    #endregion

    #region Debug Commands
    /// <summary>
    /// Fully restores the Player's health.
    /// </summary>
    public void FullHeal()
    {
        CurrentHP = MaxHP;
        Cooldown = 0;

        UpdateHealthBar();
    }

    /// <summary>
    /// Fully charges the Player's Cooldown meter.
    /// </summary>
    public void FullCharge()
    {
        Cooldown = COOLDOWN_LIMIT;
    }
    #endregion
}
