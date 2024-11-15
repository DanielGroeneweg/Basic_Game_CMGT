using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class AmmoCratePickUp : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        // If the player hit the super bullet pick up and they don't have one yet, give them a super bullet
        if (collision.gameObject.tag == "Player" && !GameManager.instance._PlayerAimingAndShooting.hasSuperBullet)
        {
            GameManager.instance.SuperBulletPickedUp();
            Destroy(gameObject);
        }
    }
}