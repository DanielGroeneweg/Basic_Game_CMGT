using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUp : MonoBehaviour
{
    private GameManager _GameManager;
    public float rotationSpeed;
    void Start()
    {
        _GameManager = GameManager.instance;
    }
    private void FixedUpdate()
    {
        transform.Rotate(Vector3.up * rotationSpeed);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            _GameManager.HealPlayer();
            Destroy(gameObject);
        }
    }
}
