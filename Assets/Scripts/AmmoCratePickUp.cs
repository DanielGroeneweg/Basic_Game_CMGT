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
    private GameObject leftBackWheel;
    private GameObject rightBackWheel;
    private GameObject leftFrontWheel;
    private GameObject rightFrontWheel;
    public void Start()
    {
        canvasManager = GameObject.Find("Canvas").GetComponent<CanvasManager>();
        player = GameObject.Find("PlayerItems");
        playerTank = GameObject.Find("PlayerTank");
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject == playerTank || collision.gameObject == leftBackWheel || collision.gameObject == rightBackWheel || collision.gameObject == leftFrontWheel || collision.gameObject == rightFrontWheel)
        {
            canvasManager.SwitchBulletImageVisibility();
            player.GetComponent<PlayerAimingAndShooting>().hasSuperBullet = true;
            Destroy(gameObject);
        }
    }
}