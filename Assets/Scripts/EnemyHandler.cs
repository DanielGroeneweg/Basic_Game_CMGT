using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    // The health an enemy has
    public float health;

    // The healthbar of the enemy
    public GameObject healthBar;

    // A reference to the health pick up prefab
    public GameObject healthPickUpPrefab;

    // The max health an enemy should have
    private float maxHealth;

    // The max size of the healthbar of the enemy
    private float healthBarMaxSize;

    // A reference to the GameManager Script
    private GameManager _GameManager;

    private void Awake()
    {
        // Save our max health and the size of the healthbar
        maxHealth = health;
        healthBarMaxSize = healthBar.transform.localScale.x;
    }
    private void Start()
    {
        // Get a reference to the GameManager Script
        _GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    void Update()
    {
        // increase the player's score and destroy this enemy if it has 0 or less HP
        if (health <= 0)
        {
            // Get the position of the enemy and then float it above the ground
            var pickUpLocation = transform.position;
            pickUpLocation.y += 1;

            // Create a health pick up
            Instantiate(healthPickUpPrefab, pickUpLocation, Quaternion.identity);

            // Increase score and destroy this enemy
            _GameManager.IncreaseScore();
            Destroy(gameObject);
        }
    }

    public void DamageEnemy()
    {
        // Change enemy health
        health--;

        // Change enemy healthbar
        healthBar.transform.localScale = new Vector3(
            healthBarMaxSize * (health / maxHealth),
            healthBar.transform.localScale.y,
            healthBar.transform.localScale.z
            );
    }
}
