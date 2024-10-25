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

    // The max health an enemy should have
    private float maxHealth;

    // The max size of the healthbar of the enemy
    private float healthBarMaxSize;

    private void Awake()
    {
        // Save our max health and the size of the healthbar
        maxHealth = health;
        healthBarMaxSize = healthBar.transform.localScale.x;
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
