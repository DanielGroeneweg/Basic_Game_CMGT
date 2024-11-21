using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayScore : MonoBehaviour
{
    public IntCount score;

    public float hueIncrease;

    public TMP_Text scoreText;

    public bool isRainbow = false;

    private float hueValue;

    private void Update()
    {
        scoreText.text = score.value.ToString();
    }

    private void FixedUpdate()
    {
        if (isRainbow)
        {
            hueValue += hueIncrease;
            if (hueValue > 1) hueValue = 0;

            Color rainbowColor = Color.HSVToRGB(hueValue, 1, 1);

            scoreText.color = rainbowColor;
        }
    }
}
