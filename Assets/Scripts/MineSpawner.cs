using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineSpawner : MonoBehaviour
{
    public GameObject minePrefab;

    public float spawnInterval;

    public float mineDropDistance;

    private float timer;

    private bool canSpawn = false;

    private void Update()
    {
        if (canSpawn) SpawnMine();
        else DoCooldown();
    }

    private void SpawnMine()
    {
        Vector3 pos = transform.position;
        pos.y = 0.5f;
        Instantiate(minePrefab, pos, Quaternion.identity);
        canSpawn = false;
    }

    private void DoCooldown()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            canSpawn = true;
            timer = 0;
        }
    }
}
