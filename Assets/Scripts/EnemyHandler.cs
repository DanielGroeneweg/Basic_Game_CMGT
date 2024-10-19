using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    public float health;
    public GameObject healthBar;

    private float maxHealth;
    private float healthBarMaxSize;
    private CanvasManager canvasManager;

    private void Start()
    {
        canvasManager = GameObject.Find("Canvas").GetComponent<CanvasManager>();
        maxHealth = health;
        healthBarMaxSize = healthBar.transform.localScale.x;
    }
    void Update()
    {
        if (health <= 0)
        {
            canvasManager.IncreaseScore();
            Destroy(gameObject);
        }
    }

    public void DamageEnemy()
    {
        health--;
        healthBar.transform.localScale = new Vector3(
            healthBarMaxSize * (health / maxHealth),
            healthBar.transform.localScale.y,
            healthBar.transform.localScale.z
            );
    }
}
