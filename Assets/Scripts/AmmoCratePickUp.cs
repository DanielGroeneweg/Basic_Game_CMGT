using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class AmmoCratePickUp : MonoBehaviour
{
    // A reference to the GameManager Script
    private GameManager _GameManager;
    public void Start()
    {
        // Get a reference to the GameManager Script
        _GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        // If the player hit the super bullet pick up and they don't have one yet, give them a super bullet
        if (collision.gameObject.tag == "Player" && !_GameManager._PlayerAimingAndShooting.hasSuperBullet)
        {
            // Give the player a super bullet
            _GameManager.SuperBulletPickedUp();

            // Destroy the pickup
            Destroy(gameObject);
        }
    }
}