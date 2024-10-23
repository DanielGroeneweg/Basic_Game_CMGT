using Unity.VisualScripting;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    // A reference to the body of the enemy tank
    public GameObject tank;

    // A reference to the GameManger script
    GameManager _GameManager;

    #region movement
    // The acceleration when the enemy is driving
    public float acceleration;

    // The maximum velocity the enemy should have
    public float maxVelocity;

    // A reference to the rigidbody on the enemy
    public Rigidbody rb;

    // An enum to check where the enemy is for the movement
    public enum rooms { BottomLeft, BottomRight, TopLeft, TopRight, Center };
    private rooms isInRoom;
    private rooms destination;
    
    // The radius in which the enemy has "Reached" the center of a room
    public float roomDestinationOffset;

    // A float to keep track of the enemy's velocity
    private float velocity;

    // The transforms of the rooms to which the enemy can move
    private Transform centerRoom;
    private Transform topLeftRoom;
    private Transform topRightRoom;
    private Transform bottomLeftRoom;
    private Transform bottomRightRoom;
    private Transform destinationTransform;

    private bool canGoToNextDestination = false;
    private bool isTraveling = false;
    #endregion

    // The speed at which the enemy tank rotates when steering
    public float rotationSpeed;

    public enum destinationTypes { Player, Room };
    private destinationTypes destinationType;

    #region wheel rotation
    // A reference for every wheel to animate them spinning when driving
    public GameObject wheelFrontLeft;
    public GameObject wheelFrontRight;
    public GameObject wheelBackLeft;
    public GameObject wheelBackRight;

    // The speed at which the wheels should spin when driving
    public float wheelRotationSpeed;
    #endregion

    private void Start()
    {
        // Get a reference to the GameManager script
        _GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        // Room references:
        centerRoom = _GameManager._CenterRoom;
        topLeftRoom = _GameManager._TopLeftRoom;
        topRightRoom = _GameManager._TopRightRoom;
        bottomLeftRoom = _GameManager._BottomLeftRoom;
        bottomRightRoom = _GameManager._BottomRightRoom;
    }
    private void FixedUpdate()
    {
        MoveEnemyForwards();
        CheckDestination();
        RotateWheels();
    }

    private void CheckDestination()
    {
        // If the enemy can see the player, rotate it towards the player
        RaycastHit hit;
        var rayDirection = _GameManager._PlayerItems.transform.position - transform.position;
        if (Physics.Raycast(tank.transform.position, rayDirection, out hit) && hit.transform == _GameManager._PlayerItems.transform)
        {
            // Set the distantion type to player to ensure the enemy rotates towards the right target
            destinationType = destinationTypes.Player;

            // Set isTraveling to false to prevent the enemy from moving towards a room
            isTraveling = false;
            canGoToNextDestination = false;
        }

        // If the enemy can not see the player AND has no destination yet, check towards which room/destination it should move
        else if (!isTraveling)
        {
            // If the enemy can not see the player, but is in the same room, move towards center of the room
            if (isInRoom.ToString() == _GameManager._PlayerMovement.isInRoom.ToString())
            {
                isTraveling = true;
                canGoToNextDestination = false;
                destination = isInRoom;
            }

            // If the player is in another room, determine to which room the enemy should move
            else
            {
                isTraveling = true;
                DetermineDestination();
                canGoToNextDestination = false;
            }

            // Rotate enemy to destination
            destinationType = destinationTypes.Room;

            // Set the destinationTransform
            switch (destination)
            {
                case rooms.BottomLeft:
                    destinationTransform = bottomLeftRoom;
                    break;
                case rooms.BottomRight:
                    destinationTransform = bottomRightRoom;
                    break;
                case rooms.TopRight:
                    destinationTransform = topRightRoom;
                    break;
                case rooms.TopLeft:
                    destinationTransform = topLeftRoom;
                    break;
                case rooms.Center:
                    destinationTransform = centerRoom;
                    break;
            }
        }

        if (destinationType == destinationTypes.Player) RotateEnemy(_GameManager._PlayerItems.transform);
        else if (destinationType == destinationTypes.Room) RotateEnemy(destinationTransform);
    }
    private void RotateEnemy(Transform target)
    {
        // Find the direction to the target in world space, but keep only the X and Z axes (ignore Y-axis)
        Vector3 toTarget = new Vector3(
            target.position.x - tank.transform.position.x,
            0,
            target.position.z - tank.transform.position.z
        );

        // Create a rotation towards the target, keeping the Y-axis rotation only
        Quaternion targetRotation = Quaternion.LookRotation(toTarget);

        // Smoothly rotate towards the target rotation, limiting to Y-axis

        tank.transform.rotation = Quaternion.Lerp(
            tank.transform.rotation,
            targetRotation,
            rotationSpeed
        );
    }

    private void MoveEnemyForwards()
    {
        // Accelerate if the enemy hasn ot reached its maximum velocity
        if (velocity < maxVelocity) velocity += acceleration;

        // Cap the enemy's velocity to the maximum in case it somehow goes over the maximum
        if (velocity > maxVelocity) velocity = maxVelocity;

        // Apply the enemy's velocity to the rigid body
        rb.velocity = tank.transform.forward * velocity;

        // Check if the enemy has reached its destination if it's traveling
        if (isTraveling)
        {
            // Check if the enemy is within range of the destination to say it has reached that destination
            Vector3 dif = transform.position - destinationTransform.position;
            if (dif.magnitude <= roomDestinationOffset)
            {
                canGoToNextDestination = true;
                isTraveling = false;
            }
        }
        
    }
    private void DetermineDestination()
    {
        /* =========================================================================================================================
         * - If the enemy is not in the same room as the player, it should move to the room the player is in.
         * - First the enemy should move towards the center of the room it is in to allow it to get out of the room into the center
         * room without bumping into a wall.
         * - After this the enemy should move to the center room.
         * - After this the enemy should move towards the room the player is in.
         * ====================================================================================================================== */
        CheckInWhichRoomIAM();

        if (canGoToNextDestination)
        {
            // Make the enemy first go to the center room to make it not run into walls
            if (isInRoom != rooms.Center) destination = rooms.Center;

            else
            {
                // Switch the destination to the room the player is in to make the enemy move towards the player from the center room
                switch (_GameManager._PlayerMovement.isInRoom)
                {
                    case PlayerMovement.rooms.BottomLeft:
                        destination = rooms.BottomLeft;
                        break;
                    case PlayerMovement.rooms.BottomRight:
                        destination = rooms.BottomRight;
                        break;
                    case PlayerMovement.rooms.TopRight:
                        destination = rooms.TopRight;
                        break;
                    case PlayerMovement.rooms.TopLeft:
                        destination = rooms.TopLeft;
                        break;
                    case PlayerMovement.rooms.Center:
                        destination = rooms.Center;
                        break;
                }
            }
        }

        // Set the destination to the room the enemy is currently in to allow it to move out of that room
        else
        {
            destination = isInRoom;
        }
    }
    private void RotateWheels()
    {
        // Left-side wheels
        RotateWheel(wheelRotationSpeed, wheelBackLeft);
        RotateWheel(wheelRotationSpeed, wheelFrontLeft);

        // Right-side wheels
        RotateWheel(wheelRotationSpeed, wheelBackRight);
        RotateWheel(wheelRotationSpeed, wheelFrontRight);
    }

    private void RotateWheel(float wheelRotation, GameObject wheel)
    {
        wheel.transform.Rotate(new Vector3(wheelRotation, 0, 0));
    }

    private void CheckInWhichRoomIAM()
    {
        // Get a list of all colliders colliding with the enemy
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1f);

        // Go through them, using their tags to check if it's the hitbox of a room. Then change isInRoom to the room of the collider hit
        foreach (Collider hitCollider in hitColliders)
        {
            switch (hitCollider.gameObject.tag)
            {
                case "CenterRoom":
                    isInRoom = rooms.Center;
                    break;
                case "BottomLeftRoom":
                    isInRoom = rooms.BottomLeft;
                    break;
                case "TopLeftRoom":
                    isInRoom = rooms.TopLeft;
                    break;
                case "BottomRightRoom":
                    isInRoom = rooms.BottomRight;
                    break;
                case "TopRightRoom":
                    isInRoom = rooms.TopRight;
                    break;
                case "EnemyCenterRoom":
                    if (isTraveling)
                    {
                        canGoToNextDestination = true;
                        isTraveling = false;
                    }
                    break;
            }
        }
    }
}