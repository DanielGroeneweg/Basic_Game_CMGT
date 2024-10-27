using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelSpinAnimation : MonoBehaviour
{
    // The speed at which the wheels rotate
    public float rotationSpeed = 50;
    void Update()
    {
        // Rotate the object this script is attached to over the X axis
        transform.Rotate(new Vector3(rotationSpeed, 0, 0) * Time.deltaTime);
    }
}
