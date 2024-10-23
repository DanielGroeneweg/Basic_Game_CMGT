using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayScoreInEndScene : MonoBehaviour
{
    // A reference to the player's score
    public IntCount score;

    // The amount with which we increase the hue to make the score display different colors
    public float hueIncrease;

    // A reference to the score text component
    public TMP_Text scoreText;

    // A float to keep track of the hue
    private float hueValue;

    private void Awake()
    {
        // Change the score text to the achieved score
        scoreText.text = score.value.ToString();

        // Allow the player to move their cursor outside the game
        Cursor.lockState = CursorLockMode.None;
    }

    private void FixedUpdate()
    {
        // Increment the hue value over time
        hueValue += hueIncrease;
        if (hueValue > 1) hueValue = 0; // Loop the hue value once it exceeds 1 (cycle complete)

        // Convert HSV to RGB
        Color rainbowColor = Color.HSVToRGB(hueValue, 1, 1); // Full saturation and brightness

        // Apply the new color to the text
        scoreText.color = rainbowColor;
    }
}
