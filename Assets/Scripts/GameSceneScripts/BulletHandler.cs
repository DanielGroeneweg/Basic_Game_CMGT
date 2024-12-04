 using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class BulletHandler : MonoBehaviour
{
    public GameObject particle;

    public Rigidbody rb;

    public float speed;

    private GameManager _GameManager;
    private void Start()
    {
        _GameManager = GameManager.instance;
    }
    private void Update()
    {
        if (!_GameManager.gamePaused) rb.velocity = transform.forward * speed;
        else rb.velocity = Vector3.zero;
    }
    private void OnTriggerEnter(Collider collision)
    {
        switch (gameObject.tag)
        {
            case "EnemyBullet":
                // Damage the player if they were hit by an enemy bullet
                if (collision.gameObject.tag == "Player") _GameManager.DamagePlayer();
                break;

            case "PlayerBullet":
                if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "EnemyBody")
                {
                    // Get the top parent of the enemy that was hit
                    GameObject obj = collision.gameObject;
                    if (obj.tag != "Enemy")
                    {
                        obj = obj.transform.root.gameObject;
                    }
                    _GameManager.DamageEnemy(obj);
                }
                break;

            case "SuperBullet":
                if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "EnemyBody")
                {
                    // Get the top parent of the enemy that was hit
                    GameObject obj = collision.gameObject;
                    if (obj.tag != "Enemy")
                    {
                        obj = obj.transform.root.gameObject;
                    }

                    // Kill enemies hit by the player super bullet
                    for (float i = obj.GetComponent<EnemyHandler>().health; i > 0; i--)
                    {
                        _GameManager.DamageEnemy(obj);
                    }
                }
                break;
        }

        // Do not destroy the bullet if it's a super bullet hitting an enemy
        if (gameObject.tag == "SuperBullet" && (collision.tag == "Enemy" || collision.tag == "EnemyBody")) return;

        else
        {
            SpawnParticle();
            Destroy(gameObject);
        }
    }
    private void SpawnParticle()
    {
        GameObject myParticle = Instantiate(particle);
        
        myParticle.transform.position = new Vector3(transform.position.x, myParticle.transform.position.y, transform.position.z);
    }
}
