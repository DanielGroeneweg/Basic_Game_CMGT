using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMover : MonoBehaviour
{
    public float rotationSpeed;
    private GameManager _GameManager;

    private void Start()
    {
        _GameManager = GameManager.instance;
    }
    void FixedUpdate()
    {
        if (!_GameManager.gamePaused) transform.Rotate(new Vector3(0, 1 * rotationSpeed, 0));
    }
}
