using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CanvasManager : MonoBehaviour
{
    public Image superBulletImage;
    public GameObject scoreText;
    public GameObject heart1;
    public GameObject heart2;
    public GameObject heart3;
    private float health = 4;
    private float score = 0;

    private Color imageColor;

    private void Start()
    {
        imageColor = superBulletImage.color;
        SwitchBulletImageVisibility();
    }
    public void SwitchBulletImageVisibility()
    {
        imageColor.a *= -1f;
        superBulletImage.color = imageColor;
    }

    public void DamagePlayer()
    {
        health--;
        switch (health)
        {
            case 3:
                heart3.SetActive(false);
                break;
            case 2:
                heart2.SetActive(false);
                break;
            case 1:
                heart1.SetActive(false);
                break;
            case 0:
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                break;
        }
    }

    public void IncreaseScore()
    {
        score++;
        TMP_Text text = scoreText.GetComponent<TMP_Text>();
        text.text = "Score: " + score;
    }
}
