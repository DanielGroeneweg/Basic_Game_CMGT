using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelSpinAnimation : MonoBehaviour
{
    public float rotationSpeed = 50;
    void Update()
    {
        transform.Rotate(new Vector3(rotationSpeed, 0, 0) * Time.deltaTime);
    }
}
