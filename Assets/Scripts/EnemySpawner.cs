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

    // Spawner Objects:
    public GameObject spawner1;
    public GameObject spawner2;
    public GameObject spawner3;
    public GameObject spawner4;
    public GameObject spawner5;
    public GameObject spawner6;
    public GameObject spawner7;
    public GameObject spawner8;

    private float cooldownTimer;
    private bool canSpawn = true;

    // Update is called once per frame
    void Update()
    {
        if (canSpawn) SpawnEnemy();
        else DoCooldown();
    }
    private void SpawnEnemy()
    {
        List<GameObject> spawners = new List<GameObject> {spawner1, spawner2, spawner3, spawner4, spawner5, spawner6, spawner7, spawner8};

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

        int rand = Random.Range(0, spawners.Count);

        Instantiate(enemyPrefab, spawners[rand].transform.position, spawners[rand].transform.rotation);

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
}
