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
        // Get our highscore saved in the player prefs and set the scriptable object to that value
        highScoreIntCount.SetValue(PlayerPrefs.GetInt("highscore"));
    }

    public void SaveHighScore()
    {
        // Save our highscore saved in the scriptable object into the player prefs
        PlayerPrefs.SetInt("highscore", highScoreIntCount.value);
    }

    private void OnApplicationQuit()
    {
        SaveHighScore();
    }
}
