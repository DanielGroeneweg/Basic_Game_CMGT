using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUp : MonoBehaviour
{
    private GameManager _GameManager;
    public float rotationSpeed;
    void Start()
    {
        // Get a reference to the GameManager Script
        _GameManager = GameManager.instance;
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
            _GameManager.HealPlayer();
            Destroy(gameObject);
        }
    }
}
