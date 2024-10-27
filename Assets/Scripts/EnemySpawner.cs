using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // A reference to the gamemanager script
    public GameManager gameManager;
    // A reference to the enemy prefab it should spawn
    public GameObject enemyPrefab;

    // A reference to the playermovement script
    public PlayerMovement _PlayerMovement;

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

    // A reference to the enemiesInScene scriptable object
    public IntCount enemiesInScene;

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

    private void Start()
    {
        // Set the max eneny spawn amount to the amount of spawners minus two. This because each room has 2 spawners, I don't want to spawn enemies in the same room as the
        // player, and with this limit there will be no enemies spawning inside of each other (or errors for going through an empty list)
        maxEnemySpawnAmount = spawnerList.Count - 2f;
    }
    // Update is called once per frame
    void Update()
    {
        if (gameManager.gameState == GameManager.gameStates.Playing)
        {
            // Make enemies immediately spawn if the player killed all enemies
            if (enemiesInScene.value == 0)
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
        // Spawn the enemies
        Spawn();

        // Increase how many enemies are spawned over time
        IncreaseSpawnAmount();

        // Increase how often enemies are spawned over time
        IncreaseSpawnRate();

        // Disable spawning for next frame
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
        List<GameObject> spawners = new List<GameObject>();
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
            Instantiate(enemyPrefab, spawners[rand].transform.position, spawners[rand].transform.rotation);

            // Remove the spawner from the list to prevent two enemies spawning at the same spot
            spawners.RemoveAt(rand);

            // Increase the enemiesinscene value by 1
            enemiesInScene.ChangeValue(1);
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
