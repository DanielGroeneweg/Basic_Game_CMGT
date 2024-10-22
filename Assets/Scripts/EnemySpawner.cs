using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public PlayerMovement _PlayerMovement;
    public float spawnCooldown;
    public float cooldownReduction;
    public float minCooldown;
    public float SpawnsForIncrease;

    public List<GameObject> spawnerList;

    private float cooldownTimer = 0;
    private float howManyEnemiesToSpawn = 1;
    private float maxEnemySpawnAmount;
    private float timesSpawned;
    private bool canSpawn = true;

    private void Start()
    {
        List<GameObject> spawners = new List<GameObject>();
        foreach (GameObject spawner in spawnerList) spawners.Add(spawner);
        maxEnemySpawnAmount = spawners.Count - 2f;
    }
    // Update is called once per frame
    void Update()
    {
        if (canSpawn) SpawnEnemies();
        else DoCooldown();
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
        }
        Debug.Log("Spawned " + howManyEnemiesToSpawn + " enemies!");
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
        if (spawnCooldown > minCooldown) spawnCooldown -= cooldownReduction;
        else if (spawnCooldown < minCooldown) spawnCooldown = minCooldown;
    }
}
