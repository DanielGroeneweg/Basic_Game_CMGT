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
    public GameObject superBulletImage;

    public TMP_Text scoreText;

    public List<GameObject> heartImages;

    private void Start()
    {
        SwitchBulletImageVisibility();
    }
    public void SwitchBulletImageVisibility()
    {
        superBulletImage.SetActive(!superBulletImage.activeSelf);
    }

    public void ChangeHealthDisplay(int playerHealth)
    {
        heartImages[playerHealth - 1].gameObject.SetActive(!heartImages[playerHealth - 1].gameObject.activeSelf);
    }

    public void ChangeScoreText(int score)
    {
        scoreText.text = "Score: " + score;
    }
}
