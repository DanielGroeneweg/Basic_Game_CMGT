using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHandler : MonoBehaviour
{
    private GameObject player;
    private GameObject playerTank;
    private GameObject playerTankBarrel;
    private GameObject enemy;
    private GameObject enemyTank;
    private GameObject enemyTankBarrel;
    private CanvasManager canvasManager;
    private void Start()
    {
        player = GameObject.Find("PlayerItems");
        playerTank = GameObject.Find("PlayerTankTop");
        playerTankBarrel = GameObject.Find("PlayerTankBarrel");
        enemy = GameObject.Find("EnemyTank");
        enemyTank = GameObject.Find("EnemyTankTop");
        enemyTankBarrel = GameObject.Find("EnemyTankBarrel");
        canvasManager = GameObject.Find("Canvas").GetComponent<CanvasManager>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        switch (gameObject.tag)
        {
            case "EnemyBullet":
                if (collision.gameObject == player || collision.gameObject == playerTank || collision.gameObject == playerTankBarrel)
                {
                    Debug.Log("Player Hit By Enemy Missile!");
                    canvasManager.DamagePlayer();
                }
                break;
            case "PlayerBullet":
                if (collision.gameObject == enemy || collision.gameObject == enemyTank || collision.gameObject == enemyTankBarrel)
                {
                    Debug.Log("Enemy Hit By Player Missile!");
                    canvasManager.IncreaseScore();
                }
                break;
            case "SuperBullet":
                break;
        }

        Destroy(gameObject);
    }
}
