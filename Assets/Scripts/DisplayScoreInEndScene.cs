using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayScoreInEndScene : MonoBehaviour
{
    public IntCount score;
    public float hueIncrease;
    private TMP_Text scoreText;
    private float hueValue;

    private void Awake()
    {
        scoreText = GetComponent<TMP_Text>();
        scoreText.text = score.value.ToString();
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
