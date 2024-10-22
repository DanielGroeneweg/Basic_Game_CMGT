using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class CanvasManager : MonoBehaviour
{
    public UnityEvent playerDied;
    public IntCount score;
    public IntCount playerHealth;
    public Image superBulletImage;
    public TMP_Text scoreText;
    public List<GameObject> heartImages;
    public SceneLoader sceneLoader;

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
        // Decrease health by 1, then remove a heart from the HUD
        playerHealth.ChangeValue(-1);
        if (playerHealth.value > 0) heartImages[(int)playerHealth.value - 1].SetActive(false);

        // Load end screen if player died
        if (playerHealth.value <= 0) sceneLoader.LoadScene("EndScene");
    }

    public void IncreaseScore()
    {
        score.ChangeValue(1);
        scoreText.text = "Score: " + score.value;
        Debug.Log(score.value);
    }
}
