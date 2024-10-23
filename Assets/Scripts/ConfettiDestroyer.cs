using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfettiDestroyer : MonoBehaviour
{
    private float timer;
    public float targetTime;

    private void Update()
    {
        // Add time, when the target time has been reached, destroy this object to prevent it from existing forever
        timer += Time.deltaTime;
        if (timer >= targetTime) Destroy(gameObject);
    }
}
