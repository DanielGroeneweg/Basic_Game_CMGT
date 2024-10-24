using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class ConfettiDestroyer : MonoBehaviour
{
    public float destroyY;
    private void Update()
    {
        // Check if the particle has reached the target Y position, then destroy it
        if (transform.position.y <= destroyY) Destroy(gameObject);
    }
}
