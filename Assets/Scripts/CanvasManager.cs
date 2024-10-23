using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System.ComponentModel;

public class CanvasManager : MonoBehaviour
{
    // A reference to the Super Bullet Image
    public GameObject superBulletImage;

    // A reference to the score text
    public TMP_Text scoreText;

    // A list of all heart images that display the player's health
    public List<GameObject> heartImages;

    private void Start()
    {
        // Turn the super bullet image off at the start of the game (the player doesn't have one yet)
        SwitchBulletImageVisibility();
    }
    public void SwitchBulletImageVisibility()
    {
        // Switch the GameObject.active state to the other possibility (it's a bool so the only options are true or false)
        superBulletImage.SetActive(!superBulletImage.activeSelf);
    }

    public void ChangeHealthDisplay(int playerHealth)
    {
        // Disable the most right of the hearts that display the player's health
        if (playerHealth > 0) heartImages[(int)playerHealth - 1].SetActive(false);
    }

    public void ChangeScoreText(int score)
    {
        // Change the score text to display a new score
        scoreText.text = "Score: " + score;
    }
}
