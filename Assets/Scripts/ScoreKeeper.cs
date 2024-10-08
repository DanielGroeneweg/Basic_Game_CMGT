using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreKeeper : MonoBehaviour
{
    private TMP_Text scoreText;
    private void Start()
    {
        scoreText = GameObject.Find("ScoreText").GetComponent<TMP_Text>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == GameObject.Find("Player"))
        {
            scoreText.GetComponent<ScoreIncreaser>().IncreaseScore();
            Destroy(this.gameObject);
        }
    }
}