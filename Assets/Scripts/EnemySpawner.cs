using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    #region public variables
    public PlayerMovement _PlayerMovement;

    public GameObject enemyPrefab;

    public float spawnCooldown;

    public float cooldownReduction;

    public float minCooldown;

    public float SpawnsForIncrease;

    public List<GameObject> spawnerList;

    public int enemiesInScene;
    #endregion

    #region private variables
    private float cooldownTimer = 0;

    private float howManyEnemiesToSpawn = 1;

    private float maxEnemySpawnAmount;

    private float timesSpawned;

    private bool canSpawn = true;

    private List<GameObject> spawners;

    private GameManager _GameManager;

    #endregion
    private void Start()
    {
        _GameManager = GameManager.instance;
        maxEnemySpawnAmount = spawnerList.Count - 2f;
        spawners = new List<GameObject>();
    }
    void Update()
    {
        if (!_GameManager.gamePaused)
        {
            if (enemiesInScene == 0)
            {
                SpawnEnemies();
                cooldownTimer = 0;
            }

            else if (canSpawn) SpawnEnemies();
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
        // Every room has 2 spawners that need to be removed from the list to make enemies not spawn in the same room as the player
        spawners.Clear();
        foreach (GameObject spawner in spawnerList) spawners.Add(spawner);

        for (int i = spawners.Count - 1; i >= 0; i--)
        {
            string playerLocation = _PlayerMovement.CheckInWhichRoomIAm().ToString();
            if (spawners[i].tag == playerLocation)
            {
                spawners.Remove(spawners[i]);
            }
        }

        for (int i = 1; i <= howManyEnemiesToSpawn; i++)
        {
            int rand = Random.Range(0, spawners.Count);
            Instantiate(enemyPrefab, spawners[rand].transform.position, spawners[rand].transform.rotation);

            spawners.RemoveAt(rand);

            enemiesInScene++;
        }
    }

    private void IncreaseSpawnAmount()
    {
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
