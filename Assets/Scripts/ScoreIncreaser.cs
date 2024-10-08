using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreIncreaser : MonoBehaviour
{
    public AudioSource coinPickupSound;
    private int score = 0;
    public TMP_Text scoreText;
    public void IncreaseScore()
    {
        coinPickupSound.Play();
        score++;
        scoreText.text = "Score: " + score;
    }
}
