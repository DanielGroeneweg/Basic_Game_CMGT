using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class ConfettiDestroyer : MonoBehaviour
{
    public float destroyY;
    private void Update()
    {
        if (transform.position.y <= destroyY) Destroy(gameObject);
    }
}
