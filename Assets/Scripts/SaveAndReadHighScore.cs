using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveAndReadHighScore : MonoBehaviour
{
    public IntCount highScoreIntCount;
    private void Start()
    {
        ReadHighScore();
    }
    public void ReadHighScore()
    {
        highScoreIntCount.SetValue(PlayerPrefs.GetInt("highscore"));
    }

    public void SaveHighScore()
    {
        PlayerPrefs.SetInt("highscore", highScoreIntCount.value);
    }

    private void OnApplicationQuit()
    {
        SaveHighScore();
    }
}
