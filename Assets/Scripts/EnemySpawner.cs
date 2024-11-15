using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    #region public variables
    // A reference to the playermovement script
    public PlayerMovement _PlayerMovement;

    // A reference to the enemy prefab it should spawn
    public GameObject enemyPrefab;

    // the time that is between each series of enemy spawns
    public float spawnCooldown;

    // The rate at which the spawn cooldown shortens over time
    public float cooldownReduction;

    // The minimum value of the spawn cooldown
    public float minCooldown;

    // The amount of series of enemy spawns before more enemies will be spawned in one serie of enemy spawns
    public float SpawnsForIncrease;

    // A list with all spawners
    public List<GameObject> spawnerList;

    // A list of all enemies in the scene
    public int enemiesInScene;
    #endregion

    #region private variables
    // A float to keep track of the cooldown time
    private float cooldownTimer = 0;

    // A float to keep track of how enemies should be spawned in one serie of enemy spawns
    private float howManyEnemiesToSpawn = 1;

    // The max amount of enemies spawned in one series of enemy spawns
    private float maxEnemySpawnAmount;

    // A float to keep track of how many series of enemy spawns there have been
    private float timesSpawned;

    // A bool to check if we can spawn enemies
    private bool canSpawn = true;

    private List<GameObject> spawners;

    // A reference to the gamemanager script
    private GameManager _GameManager;

    #endregion
    private void Start()
    {
        _GameManager = GameManager.instance;
        // Set the max eneny spawn amount to the amount of spawners minus two. This because each room has 2 spawners, I don't want to spawn enemies in the same room as the
        // player, and with this limit there will be no enemies spawning inside of each other (or errors for going through an empty list)
        maxEnemySpawnAmount = spawnerList.Count - 2f;
        spawners = new List<GameObject>();
    }
    // Update is called once per frame
    void Update()
    {
        if (_GameManager.gameState == GameManager.gameStates.Playing)
        {
            // Make enemies immediately spawn if the player killed all enemies
            if (enemiesInScene == 0)
            {
                SpawnEnemies();
                cooldownTimer = 0;
            }

            // Else use the spawn cooldown
            if (canSpawn) SpawnEnemies();
            else DoCooldown();
        }
    }
    private void SpawnEnemies()
    {
        Spawn();
        IncreaseSpawnAmount();
        IncreaseSpawnRate();
        canSpawn = false;
    }
    private void DoCooldown()
    {
        cooldownTimer += Time.deltaTime;
        if (cooldownTimer >= spawnCooldown)
        {
            canSpawn = true;
            cooldownTimer = 0;
        }
    }

    private void Spawn()
    {
        // Get a list of all spawners, then remove the ones in the same room as the player
        // Every room has 2 hitboxes that need to be removed from the list to make enemies not spawn in the same room as the player
        spawners.Clear();
        foreach (GameObject spawner in spawnerList) spawners.Add(spawner);

        switch (_PlayerMovement.isInRoom)
        {
            case PlayerMovement.rooms.TopLeft:
                spawners.RemoveAt(0);
                spawners.RemoveAt(0);
                break;
            case PlayerMovement.rooms.TopRight:
                spawners.RemoveAt(2);
                spawners.RemoveAt(2);
                break;
            case PlayerMovement.rooms.BottomLeft:
                spawners.RemoveAt(4);
                spawners.RemoveAt(4);
                break;
            case PlayerMovement.rooms.BottomRight:
                spawners.RemoveAt(6);
                spawners.RemoveAt(6);
                break;
            case PlayerMovement.rooms.Center:
                break;
        }

        // Spawn enemies equal to the value of "howManyEnemiesToSpawn"
        for (int i = 1; i <= howManyEnemiesToSpawn; i++)
        {
            // Spawn an enemy on a random spawner chosen from the list of spawners
            int rand = Random.Range(0, spawners.Count);
            GameObject enemy = Instantiate(enemyPrefab, spawners[rand].transform.position, spawners[rand].transform.rotation);

            // Remove the spawner from the list to prevent two enemies spawning at the same spot
            spawners.RemoveAt(rand);

            // increase enemies in scene count
            enemiesInScene++;
        }
    }

    private void IncreaseSpawnAmount()
    {
        // Increase the enemies spawned for every "SpawnsForIncrease" times we have spawned a series of enemies
        timesSpawned++;
        if (timesSpawned >= SpawnsForIncrease)
        {
            timesSpawned = 0;
            if (howManyEnemiesToSpawn < maxEnemySpawnAmount) howManyEnemiesToSpawn++;
        }
    }

    private void IncreaseSpawnRate()
    {
        // Shorten the spawn cooldown
        if (spawnCooldown > minCooldown) spawnCooldown -= cooldownReduction;

        // Make sure that if the spawn cooldown somehow comes under the minimum, it's set back to the minimum
        else if (spawnCooldown < minCooldown) spawnCooldown = minCooldown;
    }
}
