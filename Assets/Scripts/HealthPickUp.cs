using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUp : MonoBehaviour
{
    private GameManager gameManager;
    public float rotationSpeed;
    void Start()
    {
        // Get a reference to the GameManager Script
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    private void FixedUpdate()
    {
        // Spin the health pickup
        transform.Rotate(Vector3.up * rotationSpeed);
    }
    private void OnTriggerEnter(Collider other)
    {
        // Heal the player if they collided with the  pickup, then destroy this pickup
        if (other.tag == "Player")
        {
            gameManager.HealPlayer();
            Destroy(gameObject);
        }
    }
}
