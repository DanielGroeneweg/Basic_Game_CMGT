 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHandler : MonoBehaviour
{
    private CanvasManager canvasManager;
    private void Start()
    {
        canvasManager = GameObject.Find("Canvas").GetComponent<CanvasManager>();
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag != "CenterRoom" && collision.tag != "BottomLeftRoom" && collision.tag != "BottomRightRoom" && collision.tag != "TopRightRoom" && collision.tag != "TopLeftRoom" && collision.tag != "EnemyCenterRoom")
        {
            switch (gameObject.tag)
            {
                case "EnemyBullet":
                    if (collision.gameObject.tag == "Player")
                    {
                        canvasManager.DamagePlayer();
                    }
                    break;
                case "PlayerBullet":
                    if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "EnemyBody")
                    {
                        GameObject obj = collision.gameObject;
                        if (obj.tag != "Enemy")
                        {
                            obj = obj.transform.root.gameObject;
                        }

                        obj.GetComponent<EnemyHandler>().DamageEnemy();
                    }
                    break;
                case "SuperBullet":
                    if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "EnemyBody")
                    {
                        GameObject obj = collision.gameObject;
                        if (obj.tag != "Enemy")
                        {
                            obj = obj.transform.root.gameObject;
                        }
                        Destroy(obj);
                        canvasManager.IncreaseScore();
                    }
                    break;
            }

            if (gameObject.tag == "SuperBullet" && (collision.tag == "Enemy" || collision.tag == "EnemyBody")) return;
            else Destroy(gameObject);
        }
    }
}
