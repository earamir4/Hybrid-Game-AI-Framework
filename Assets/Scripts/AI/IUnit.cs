using System;
using UnityEngine;

/// <summary>
/// <see cref="IUnit"/> describes the properties and methods that all entities in the game share.
/// 
/// <para>
/// In this AI framework, <see cref="PlayerStats"/>, allies, and enemies (i.e. <see cref="RageUnit"/>)
/// all implement <see cref="IUnit"/>.
/// </para>
/// <para>
/// In addition to standard RPG stats (i.e. Health, Attack), <see cref="IUnit"/> also describes hidden
/// personality traits that influence their state transitions.
/// </para>
/// </summary>
public interface IUnit
{
    /// <summary>
    /// All <see cref="IUnit"/>s should have a Blackboard to manage their local <see cref="IState"/>.
    /// </summary>
    Blackboard Blackboard { get; }

    /// <summary>
    /// Each <see cref="IUnit"/> should have an <see cref="UnityEngine.Animator"/> to update their
    /// UI.
    /// </summary>
    Animator Animator { get; set; }

    #region Stat Properties
    string Name { get; }
    float MaxHP { get; }
	float CurrentHP { get; set; }
	float Attack { get; set; }
	float Defense { get; set; }
	float Speed { get; set; }
    #endregion

    #region Hidden Traits
    /// <summary>
    /// If an <see cref="IUnit"/> has a high Rage trait, then decisions involving reckless
    /// attacks will take priority.
    /// </summary>
    float Rage { get; }

    /// <summary>
    /// If an <see cref="IUnit"/> has a high Healing trait, then decisions involving healing
    /// other units will take priority.
    /// </summary>
    float Healing { get; }

    /// <summary>
    /// If an <see cref="IUnit"/> has a high Guarding trait, then decisions involving protecting
    /// other units will take priority.
    /// </summary>
    float Guarding { get; }
    #endregion

    #region Interface Methods
    /// <summary>
    /// Every <see cref="IUnit"/> should be able to take damage.
    /// </summary>
    /// <param name="damage"></param>
    void DamageTaken(float damage);

    /// <summary>
    /// When <see cref="CurrentHP"/> reaches zero, this should be called.
    /// </summary>
    void Death();

    /// <summary>
    /// An <see cref="IUnit"/> should have a graphical representation of their health.
    /// </summary>
    void UpdateHealthBar();

    /// <summary>
    /// Updates the state information displayed for an <see cref="IUnit"/>.
    /// </summary>
    [Obsolete("Used for displaying state information before Animator UI implementation.")]
    void UpdateStateText();
    #endregion
}
