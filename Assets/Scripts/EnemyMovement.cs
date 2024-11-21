using Unity.VisualScripting;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public GameObject tank;

    private GameManager _GameManager;

    #region movement
    public float acceleration;

    public float maxVelocity;

    public Rigidbody rb;

    private PlayerMovement.rooms isInRoom;
    private PlayerMovement.rooms destination;
    
    // The radius in which the enemy has "Reached" the center of a room
    public float roomDestinationOffset;

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

    public float rotationSpeed;

    public enum destinationTypes { Player, Room };
    private destinationTypes destinationType;

    #region wheel rotation
    public GameObject wheelFrontLeft;
    public GameObject wheelFrontRight;
    public GameObject wheelBackLeft;
    public GameObject wheelBackRight;

    public float wheelRotationSpeed;
    #endregion

    int ID;

    private void Start()
    {
        _GameManager = GameManager.instance;

        // Room references:
        centerRoom = _GameManager._CenterRoom;
        topLeftRoom = _GameManager._TopLeftRoom;
        topRightRoom = _GameManager._TopRightRoom;
        bottomLeftRoom = _GameManager._BottomLeftRoom;
        bottomRightRoom = _GameManager._BottomRightRoom;

        CheckInWhichRoomIAM();

        ID = Random.Range(0, 1000000000);
    }
    private void FixedUpdate()
    {
        if (!_GameManager.gamePaused)
        {
            MoveEnemyForwards();
            CheckDestination();
            RotateWheels();
        }

        else rb.velocity = Vector3.zero;
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
            if (isInRoom == _GameManager._PlayerMovement.CheckInWhichRoomIAm())
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

            destinationType = destinationTypes.Room;

            // Set the destinationTransform
            switch (destination)
            {
                case PlayerMovement.rooms.BottomLeftRoom:
                    destinationTransform = bottomLeftRoom;
                    break;
                case PlayerMovement.rooms.BottomRightRoom:
                    destinationTransform = bottomRightRoom;
                    break;
                case PlayerMovement.rooms.TopRightRoom:
                    destinationTransform = topRightRoom;
                    break;
                case PlayerMovement.rooms.TopLeftRoom:
                    destinationTransform = topLeftRoom;
                    break;
                case PlayerMovement.rooms.Center:
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

        Quaternion targetRotation = Quaternion.LookRotation(toTarget);

        tank.transform.rotation = Quaternion.Lerp(
            tank.transform.rotation,
            targetRotation,
            rotationSpeed
        );
    }

    private void MoveEnemyForwards()
    {
        if (velocity < maxVelocity) velocity += acceleration;

        if (velocity > maxVelocity) velocity = maxVelocity;

        rb.velocity = tank.transform.forward * velocity;

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
            if (isInRoom != PlayerMovement.rooms.Center) destination = PlayerMovement.rooms.Center;

            else
            {
                // Switch the destination to the room the player is in to make the enemy move towards the player from the center room
                switch (_GameManager._PlayerMovement.CheckInWhichRoomIAm())
                {
                    case PlayerMovement.rooms.BottomLeftRoom:
                        destination = PlayerMovement.rooms.BottomLeftRoom;
                        break;
                    case PlayerMovement.rooms.BottomRightRoom:
                        destination = PlayerMovement.rooms.BottomRightRoom;
                        break;
                    case PlayerMovement.rooms.TopRightRoom:
                        destination = PlayerMovement.rooms.TopRightRoom;
                        break;
                    case PlayerMovement.rooms.TopLeftRoom:
                        destination = PlayerMovement.rooms.TopLeftRoom;
                        break;
                    case PlayerMovement.rooms.Center:
                        destination = PlayerMovement.rooms.Center;
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

        foreach (Collider hitCollider in hitColliders)
        {
            switch (hitCollider.gameObject.tag)
            {
                case "CenterRoom":
                    isInRoom = PlayerMovement.rooms.Center;
                    break;
                case "BottomLeftRoom":
                    isInRoom = PlayerMovement.rooms.BottomLeftRoom;
                    break;
                case "TopLeftRoom":
                    isInRoom = PlayerMovement.rooms.TopLeftRoom;
                    break;
                case "BottomRightRoom":
                    isInRoom = PlayerMovement.rooms.BottomRightRoom;
                    break;
                case "TopRightRoom":
                    isInRoom = PlayerMovement.rooms.TopRightRoom;
                    break;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EnemyCenterRoom" && isTraveling)
        {
            canGoToNextDestination = true;
            isTraveling = false;
            isInRoom = PlayerMovement.rooms.Center;
        }
    }
}