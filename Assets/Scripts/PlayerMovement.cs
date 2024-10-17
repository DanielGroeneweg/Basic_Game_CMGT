using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Schema;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public GameObject tank;

    #region movement
    public float acceleration;
    public float maxVelocity;
    public float deacceleration;
    public float maxReverseVelocity;
    public float passiveSlowDown;
    public Rigidbody rb;

    private float velocity;
    private float oldVelocity;
    #endregion

    public float rotationSpeed;

    #region wheel rotation
    public GameObject wheelFrontLeft;
    public GameObject wheelFrontRight;
    public GameObject wheelBackLeft;
    public GameObject wheelBackRight;
    public float wheelRotationSpeed;
    private enum rotateDirection { Left, Right, None};
    private rotateDirection myRotateDirection = rotateDirection.None;
    #endregion
    private void Update()
    {
        RotatePlayer();
        MovePlayer();
        RotateWheels();
    }

    private void RotatePlayer()
    {
        Vector3 var = tank.transform.localEulerAngles;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            tank.transform.localEulerAngles = new Vector3(
                var.x,
                var.y + rotationSpeed * Time.deltaTime,
                var.z
                );
            myRotateDirection = rotateDirection.Right;
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            tank.transform.localEulerAngles = new Vector3(
                var.x,
                var.y - rotationSpeed * Time.deltaTime,
                var.z
                );
            myRotateDirection = rotateDirection.Left;
        }
    }

    private void MovePlayer()
    {
        // Give the player forwards velocty
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            if (velocity < maxVelocity) velocity += acceleration;
        }

        // Give the player backwards velocity
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            if (velocity > -maxReverseVelocity) velocity -= deacceleration;
        }

        // Slow the player down if the velocity hasn't changed by pressing a key
        if (velocity == oldVelocity)
        {
            if (velocity < 0) velocity += passiveSlowDown;
            else if (velocity > 0) velocity -= passiveSlowDown;
            if (velocity >= -passiveSlowDown/2f && velocity <= passiveSlowDown/2f && velocity != 0) velocity = 0;
        }

        // Apply our velocity to the rigid body
        rb.velocity = tank.transform.forward * velocity * Time.deltaTime;
        
        // Save our current velocity
        oldVelocity = velocity;
    }

    private void RotateWheels()
    {
        if (velocity == 0)
        {
            if (myRotateDirection == rotateDirection.Left)
            {
                // If rotate direction is left, the left-side wheels should rotate clockwise and the right-side counterclockwise
                // Left-side wheels
                RotateWheel(-wheelRotationSpeed, wheelBackLeft);
                RotateWheel(-wheelRotationSpeed, wheelFrontLeft);
                
                // Right-side wheels
                RotateWheel(wheelRotationSpeed, wheelBackRight);
                RotateWheel(wheelRotationSpeed, wheelFrontRight);
            }

            else if (myRotateDirection == rotateDirection.Right)
            {
                // If rotate direction is left, the left-side wheels should rotate counterclockwise and the right-side clockwise
                // Left-side wheels
                RotateWheel(wheelRotationSpeed, wheelBackLeft);
                RotateWheel(wheelRotationSpeed, wheelFrontLeft);

                // Right-side wheels
                RotateWheel(-wheelRotationSpeed, wheelBackRight);
                RotateWheel(-wheelRotationSpeed, wheelFrontRight);
            }
        }
        
        else
        {
            if (velocity > 0)
            {
                // If velocity is greater than 0 all wheels should rotate counterclockwise
                // Left-side wheels
                RotateWheel(wheelRotationSpeed, wheelBackLeft);
                RotateWheel(wheelRotationSpeed, wheelFrontLeft);

                // Right-side wheels
                RotateWheel(wheelRotationSpeed, wheelBackRight);
                RotateWheel(wheelRotationSpeed, wheelFrontRight);
            }

            else if (velocity < 0)
            {
                // If velocity is smaller than 0 all wheels should rotate clockwise
                // Left-side wheels
                RotateWheel(-wheelRotationSpeed, wheelBackLeft);
                RotateWheel(-wheelRotationSpeed, wheelFrontLeft);

                // Right-side wheels
                RotateWheel(-wheelRotationSpeed, wheelBackRight);
                RotateWheel(-wheelRotationSpeed, wheelFrontRight);
            }
        }

        myRotateDirection = rotateDirection.None;
    }

    private void RotateWheel(float wheelRotation, GameObject wheel)
    {
        wheel.transform.Rotate(new Vector3(wheelRotation * Time.deltaTime, 0, 0));
    }
}