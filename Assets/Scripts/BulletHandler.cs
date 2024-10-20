 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHandler : MonoBehaviour
{
    public GameObject particle;
    private CanvasManager canvasManager;
    private void Start()
    {
        canvasManager = GameObject.Find("Canvas").GetComponent<CanvasManager>();
        particle.SetActive(false);
    }
    private void OnTriggerEnter(Collider collision)
    {
        // Exclude the hitboxes for the enemy movement AI
        if (collision.tag != "CenterRoom" && collision.tag != "BottomLeftRoom" && collision.tag != "BottomRightRoom" && collision.tag != "TopRightRoom" && collision.tag != "TopLeftRoom" && collision.tag != "EnemyCenterRoom")
        {
            switch (gameObject.tag)
            {
                case "EnemyBullet":
                    // Damage the player
                    if (collision.gameObject.tag == "Player")
                    {
                        canvasManager.DamagePlayer();
                    }
                    break;
                case "PlayerBullet":
                    if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "EnemyBody")
                    {
                        // Get the top parent of the enemy
                        GameObject obj = collision.gameObject;
                        if (obj.tag != "Enemy")
                        {
                            obj = obj.transform.root.gameObject;
                        }

                        // Damage the enemy
                        obj.GetComponent<EnemyHandler>().DamageEnemy();
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

                        // Destroy the enemy
                        Destroy(obj);

                        // Increase the player score
                        canvasManager.IncreaseScore();
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
        GameObject myParticle = Instantiate(particle);
        myParticle.transform.position = new Vector3(transform.position.x, myParticle.transform.position.y, transform.position.z);
        myParticle.SetActive(true);
    }
}
