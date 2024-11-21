using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public GameObject tank;

    private GameManager _GameManager;
    #region movement
    public float acceleration;

    public float maxVelocity;

    public float deacceleration;

    public float maxReverseVelocity;

    public float passiveSlowDown;

    public Rigidbody rb;
    
    public enum rooms { Center, BottomLeftRoom, BottomRightRoom, TopLeftRoom, TopRightRoom };
    public rooms isInRoom = rooms.Center;

    private float velocity;

    private float oldVelocity;

    // Bools for inputs
    private bool pressedForward;
    private bool pressedBackward;
    private bool pressedLeft;
    private bool pressedRight;
    #endregion

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
    private void Start()
    {
        _GameManager = GameManager.instance;
    }
    private void FixedUpdate()
    {
        if (!_GameManager.gamePaused)
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
        if (!_GameManager.gamePaused)
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

            myRotateDirection = rotateDirection.Left;
        }
    }

    private void MovePlayer()
    {
        if (pressedForward)
        {
            if (velocity < maxVelocity) velocity += acceleration;
            if (velocity > maxVelocity) velocity = maxVelocity;
        }

        if (pressedBackward)
        {
            if (velocity > -maxReverseVelocity) velocity -= deacceleration;
        }

        // Slow the player down if the velocity hasn't changed by pressing a key
        if (velocity == oldVelocity)
        {
            if (velocity < 0) velocity += passiveSlowDown;
            else if (velocity > 0) velocity -= passiveSlowDown;
            
            //Set the player's velocity to 0 when it keeps bouncing between negative and positive values
            if (velocity >= -passiveSlowDown && velocity <= passiveSlowDown && velocity != 0) velocity = 0;
        }

        rb.velocity = tank.transform.forward * velocity;
        
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

        myRotateDirection = rotateDirection.None;
    }

    private void RotateWheel(float wheelRotation, GameObject wheel)
    {
        wheel.transform.Rotate(new Vector3(wheelRotation, 0, 0));
    }

    public rooms CheckInWhichRoomIAm()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1f);

        foreach (Collider hitCollider in hitColliders)
        {
            switch (hitCollider.gameObject.tag)
            {
                case "CenterRoom":
                    return rooms.Center;
                case "BottomLeftRoom":
                    return rooms.BottomLeftRoom;
                case "TopLeftRoom":
                    return rooms.TopLeftRoom;
                case "BottomRightRoom":
                    return rooms.BottomRightRoom;
                case "TopRightRoom":
                    return rooms.TopRightRoom;
            }
        }

        // If this part of the code is executed, something is wrong!!!
        Debug.LogWarning("Player is not in any room????");
        return rooms.Center;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Make the player stop when bumping into something
        string tag = collision.gameObject.tag;
        if (tag != "PlayerBullet" && tag != "EnemyBullet" && tag != "SuperBullet" && tag != "Untagged") velocity = 0;
    }
}