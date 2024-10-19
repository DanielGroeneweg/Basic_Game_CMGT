using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class AmmoCratePickUp : MonoBehaviour
{
    private CanvasManager canvasManager;
    private GameObject player;
    private GameObject playerTank;
    private PlayerAimingAndShooting playerScript;
    private PickUpSpawner pickUpSpawner;
    public void Start()
    {
        canvasManager = GameObject.Find("Canvas").GetComponent<CanvasManager>();
        player = GameObject.Find("PlayerItems");
        playerTank = GameObject.Find("PlayerTank");
        pickUpSpawner = GameObject.Find("PickUpSpawnPlatform").GetComponent<PickUpSpawner>();
        playerScript = player.GetComponent<PlayerAimingAndShooting>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject == playerTank && !playerScript.hasSuperBullet)
        {
            canvasManager.SwitchBulletImageVisibility();
            playerScript.hasSuperBullet = true;
            pickUpSpawner.hasSpawned = false;
            Destroy(gameObject);
        }
    }
}