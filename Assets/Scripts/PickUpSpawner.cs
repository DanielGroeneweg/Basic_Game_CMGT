using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PickUpSpawner : MonoBehaviour
{
    public float spawnCooldown = 60f;
    public bool hasSpawned = false;
    public GameObject pickUpPrefab;

    private float cooldownTimer = 0f;
    private bool canSpawn = false;
    void Update()
    {
        if (!hasSpawned)
        {
            if (canSpawn) SpawnPickUp();
            else DoCooldown();
        }
    }

    private void SpawnPickUp()
    {
        var _transform = transform.position.y - 0.5f;
        Instantiate(pickUpPrefab, new Vector3(transform.position.x, _transform, transform.position.z), Quaternion.identity, transform);
        canSpawn = false;
        hasSpawned = true;
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
