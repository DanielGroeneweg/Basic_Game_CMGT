using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMover : MonoBehaviour
{
    public float rotationSpeed;
    void FixedUpdate()
    {
        transform.Rotate(new Vector3(0, 1 * rotationSpeed, 0));
    }
}
