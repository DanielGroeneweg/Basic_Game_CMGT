using UnityEngine;
public class PickUpSpawner : MonoBehaviour
{
    // The time in between each pick-up spawn
    public float spawnCooldown;

    // A bool to check if the spawner has a pick-up that has spawned, but not yet been picked up
    public bool hasSpawned = false;

    // A reference to the pick-up prefab this spawner spawns
    public GameObject pickUpPrefab;

    // A float to keep track of the cooldown timer
    private float cooldownTimer = 0f;

    // A bool to check if the time for a new pick-up to be spawned has passed
    private bool canSpawn = false;
    void FixedUpdate()
    {
        if (!hasSpawned)
        {
            if (canSpawn) SpawnPickUp();
            else DoCooldown();
        }
    }

    private void SpawnPickUp()
    {
        // Get a Transform.position.y as variable, set it to this object -0.5 to prevent the pick-up from floating in the air
        var _transform = transform.position.y - 0.5f;

        // Create a copy of the pick-up
        Instantiate(pickUpPrefab, new Vector3(transform.position.x, _transform, transform.position.z), Quaternion.identity, transform);

        // Set these bools to prevent a new pick-up from being spawned & to prevent the timer from running while a pick-up has not been picked up yet
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
