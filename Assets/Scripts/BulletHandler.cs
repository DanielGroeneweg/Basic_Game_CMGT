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
                    canvasManager.IncreaseScore();
                }
                break;
            case "SuperBullet":
                if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "EnemyBody")
                {
                    GameObject obj = collision.gameObject;
                    while (obj.tag != "Enemy")
                    {
                        obj = obj.transform.parent.gameObject;
                        Debug.Log(obj.name);
                    }
                    Destroy(obj);
                    Debug.Log("Bullet Position: " + transform.position);

                    canvasManager.IncreaseScore();
                }
                break;
        }

        if (gameObject.tag == "SuperBullet" && (collision.tag == "Enemy" || collision.tag == "EnemyBody")) return;
        else Destroy(gameObject);
    }
}
