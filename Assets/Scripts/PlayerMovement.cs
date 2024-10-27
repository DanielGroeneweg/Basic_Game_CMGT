using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // A reference to the body of the player tank
    public GameObject tank;

    // A reference to the gamemanager script
    public GameManager gameManager;

    #region movement
    // The acceleration when the player is driving
    public float acceleration;
    
    // The maximum velocity the player should have
    public float maxVelocity;

    // The rate at which the player deaccelerates when breaking
    public float deacceleration;

    // The maximum velocity the player should have when reversing
    public float maxReverseVelocity;

    // The rate at which the player deaccelerates when not giving input
    public float passiveSlowDown;

    // A reference to the rigidbody on the player
    public Rigidbody rb;
    
    // An enum to check where the player is for the enemy AI
    public enum rooms { Center, BottomLeft, BottomRight, TopLeft, TopRight };
    public rooms isInRoom = rooms.Center;

    // A float to keep track of the player's velocity
    private float velocity;

    // A float to be able to compare the player's current and previous velocity
    private float oldVelocity;

    // Bools for inputs
    private bool pressedForward;
    private bool pressedBackward;
    private bool pressedLeft;
    private bool pressedRight;
    #endregion

    // The speed at which the player tank rotates when steering
    public float rotationSpeed;

    #region wheel rotation
    // A reference for every wheel to animate them spinning when driving
    public GameObject wheelFrontLeft;
    public GameObject wheelFrontRight;
    public GameObject wheelBackLeft;
    public GameObject wheelBackRight;

    // The speed at which the wheels should spin when driving
    public float wheelRotationSpeed;

    // An enum to control the direction a wheel is supposed to rotate
    private enum rotateDirection { Left, Right, None};
    private rotateDirection myRotateDirection = rotateDirection.None;
    #endregion
    private void FixedUpdate()
    {
        if (gameManager.gameState == GameManager.gameStates.Playing)
        {
            RotatePlayer();
            MovePlayer();
            AnimateWheels();

            pressedLeft = false;
            pressedRight = false;
            pressedForward = false;
            pressedBackward = false;
        }
    }

    private void Update()
    {
        if (gameManager.gameState == GameManager.gameStates.Playing)
        {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) pressedLeft = true;
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) pressedRight = true;
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) pressedForward = true;
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) pressedBackward = true;
        }
    }

    private void RotatePlayer()
    {
        // Rotate the player
        Vector3 var = tank.transform.localEulerAngles;
        if (pressedRight)
        {
            tank.transform.localEulerAngles = new Vector3(
                var.x,
                var.y + rotationSpeed * Time.deltaTime,
                var.z
                );
            
            // Save our rotate direction for the wheel animations
            myRotateDirection = rotateDirection.Right;
        }

        // Rotate the player
        if (pressedLeft)
        {
            tank.transform.localEulerAngles = new Vector3(
                var.x,
                var.y - rotationSpeed * Time.deltaTime,
                var.z
                );

            // Save our rotate direction for the wheel animations
            myRotateDirection = rotateDirection.Left;
        }
    }

    private void MovePlayer()
    {
        // Give the player forwards velocty
        if (pressedForward)
        {
            if (velocity < maxVelocity) velocity += acceleration;
            if (velocity > maxVelocity) velocity = maxVelocity;
        }

        // Give the player backwards velocity
        if (pressedBackward)
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
        rb.velocity = tank.transform.forward * velocity;
        
        // Save our current velocity
        oldVelocity = velocity;
    }

    private void AnimateWheels()
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

        // Reset the rotate direction
        myRotateDirection = rotateDirection.None;
    }

    private void RotateWheel(float wheelRotation, GameObject wheel)
    {
        wheel.transform.Rotate(new Vector3(wheelRotation, 0, 0));
    }

    private void OnTriggerEnter(Collider other)
    {
        // When the player enters an area (I called them rooms) by hitting the hitbox of that room, I change the enum to that room
        // to be able to make the enemies use a movement pattern to move towards the player when they are not close to the player
        switch (other.tag)
        {
            case "CenterRoom":
                if (isInRoom != rooms.Center) isInRoom = rooms.Center;
                break;
            case "BottomLeftRoom":
                if (isInRoom != rooms.BottomLeft) isInRoom = rooms.BottomLeft;
                    break;
            case "TopLeftRoom":
                if (isInRoom != rooms.TopLeft) isInRoom = rooms.TopLeft;
                break;
            case "BottomRightRoom":
                if (isInRoom != rooms.BottomRight) isInRoom = rooms.BottomRight;
                break;
            case "TopRightRoom":
                if (isInRoom != rooms.TopRight) isInRoom = rooms.TopRight;
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Make the player stop when bumping into something
        string tag = collision.gameObject.tag;
        if (tag != "PlayerBullet" && tag != "EnemyBullet" && tag != "SuperBullet" && tag != "Untagged") velocity = 0;
    }
}