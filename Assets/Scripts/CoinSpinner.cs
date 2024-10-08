using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpinner : MonoBehaviour
{
    public float rotationSpeed;
    private void Update()
    {
        transform.localEulerAngles = new Vector3(
            transform.localEulerAngles.x,
            transform.localEulerAngles.y + rotationSpeed * Time.deltaTime,
            transform.localEulerAngles.z
            );
    }
}
