using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <see cref="GameManager"/> acts as the global Blackboard that keeps track of all active <see cref="IUnit"/>s.
/// These units include both the player and enemies.
/// </summary>
public class GameManager : MonoBehaviour
{
    #region Static Variables
    public static GameObject Player;
    public static PlayerStats PlayerStats;
    public static List<GameObject> EnemyObjects;
    public static List<IUnit> EnemyUnits;
    #endregion

    #region Constants
    private const string PLAYER_TAG = "Player";
    private const string ENEMY_TAG = "Enemy";
    #endregion
    
    /// <summary>
    /// Get references to Player and Enemies before Start methods are called!
    /// </summary>
    void Awake ()
    {
        Player = GameObject.FindGameObjectWithTag(PLAYER_TAG);
        PlayerStats = Player.GetComponent<PlayerStats>();

        EnemyObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag(ENEMY_TAG));
        EnemyUnits = new List<IUnit>();

        for (int index = 0; index < EnemyObjects.Count; index++)
        {
            EnemyUnits.Add(EnemyObjects[index].GetComponent<IUnit>());
            Debug.Log("Added to EnemyUnits: " + EnemyUnits[index].Name);
        }
    }
    
    /// <summary>
    /// Called to get a new list of enemies currently active.
    /// </summary>
    public void UpdateEnemyList()
    {
        EnemyObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag(ENEMY_TAG));

        for (int index = 0; index < EnemyObjects.Count; index++)
        {
            EnemyUnits.Add(EnemyObjects[index].GetComponent<IUnit>());
            Debug.Log("Added to EnemyUnits: " + EnemyUnits[index].Name);
        }
    }
}
