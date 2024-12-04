using UnityEngine;
public class PickUpSpawner : MonoBehaviour
{
    public float spawnCooldown;

    public bool hasSpawned = false;

    public GameObject pickUpPrefab;

    private float cooldownTimer = 0f;

    private bool canSpawn = false;

    private GameManager _GameManager;

    private void Start()
    {
        _GameManager = GameManager.instance;
    }
    void Update()
    {
        if (!_GameManager.gamePaused)
        {
            if (!hasSpawned)
            {
                if (canSpawn) SpawnPickUp();
                else DoCooldown();
            }
        }
    }

    private void SpawnPickUp()
    {
        float yPos = transform.position.y - 0.5f;

        Instantiate(pickUpPrefab, new Vector3(transform.position.x, yPos, transform.position.z), Quaternion.identity, transform);

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
