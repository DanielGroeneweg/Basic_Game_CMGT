 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BulletHandler : MonoBehaviour
{
    // A reference to the explosion particle spawned upon bullet impact
    public GameObject particle;

    // A reference to the GameManager Script
    private GameManager _GameManager;
    private void Awake()
    {
        // Make sure the Particle is disabled
        particle.SetActive(false);
    }
    private void Start()
    {
        // Get a reference to the GameManager Script
        _GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    private void OnTriggerEnter(Collider collision)
    {
        // Exclude the hitboxes for the enemy movement AI
        if (collision.tag != "CenterRoom" && collision.tag != "BottomLeftRoom" && collision.tag != "BottomRightRoom" && collision.tag != "TopRightRoom" && collision.tag != "TopLeftRoom" && collision.tag != "EnemyCenterRoom")
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

                        // Damage the enemy that was hit by the player bullet
                        _GameManager.DamageEnemy(obj);
                    }
                    break;
                
                case "SuperBullet":
                    if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "EnemyBody")
                    {
                        // Get the top parent of the enemy
                        GameObject obj = collision.gameObject;
                        if (obj.tag != "Enemy")
                        {
                            obj = obj.transform.root.gameObject;
                        }

                        // Destroy the enemy hit by the player super bullet
                        Destroy(obj);

                        // Increase the player score
                        _GameManager.IncreaseScore();
                    }
                    break;
            }

            // Do not destroy the bullet if it's a super bullet hitting an enemy
            if (gameObject.tag == "SuperBullet" && (collision.tag == "Enemy" || collision.tag == "EnemyBody")) return;

            else
            {
                // Spawn an explosion particle
                SpawnParticle();

                // Destroy the bullet on impact
                Destroy(gameObject);
            }
        }
    }
    private void SpawnParticle()
    {
        // Create the explosion particle
        GameObject myParticle = Instantiate(particle);
        
        // Set the explosion particle Y position
        myParticle.transform.position = new Vector3(transform.position.x, myParticle.transform.position.y, transform.position.z);
        
        // Enable the explosion particle
        myParticle.SetActive(true);
    }
}
