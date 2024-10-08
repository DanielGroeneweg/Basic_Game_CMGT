using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    // The player character
    public GameObject playerCharacter;
    // The distance we travel horizontally
    public float horizontalSpeed;
    // The distance we travel vertically when the left mouse button is clicked
    public float verticalSpeed;
    // The standard gravity applied to the player character
    public float gravity;
    // The maximum gravity applied to the player character
    public float gravityMax;
    // The addition to the gravity till max velocity
    public float gravityMultiplier;
    // The distance we travel horizontally down when the player did not press left mouse button
    private float downForce;
    // A boolean to check if the player has hit an object yet
    public bool isAlive = true;
    // The audio source for the sound played when flying upwards
    public AudioSource flySound;
    // The maximum Y Position we can have
    public float maxYPos;
    void Update()
    {
        if (isAlive)
        {
            MoveHorizontally();
            MoveVertically();
        }
    }
    void MoveHorizontally()
    {
        transform.Translate(horizontalSpeed * Time.deltaTime, 0, 0);
    }
    void MoveVertically()
    {
        // Move the player up if they press left mouse button
        if (Input.GetMouseButtonDown(0) && playerCharacter.transform.position.y < maxYPos)
        {
            flySound.Play();
            playerCharacter.transform.Translate(0, verticalSpeed * Time.deltaTime, 0);
            downForce = gravity;
        }
        // Move the player down if they did not press left mouse button
        else
        {
            if (downForce < gravityMax) downForce += gravityMultiplier;
            playerCharacter.transform.Translate(0, -downForce * Time.deltaTime, 0);
        }
    }
}