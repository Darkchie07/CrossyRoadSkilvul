using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;

public class GameOverText : MonoBehaviour
{
    [SerializeField] private TMP_Text gameOverText;
    private const string HIGHSCORE = "Highscore";
    public static int HighScore;

    public void UpdateText(int score)
    {
        if (PlayerPrefs.HasKey(HIGHSCORE))
        {
            HighScore = PlayerPrefs.GetInt(HIGHSCORE);
            if (score > HighScore)
            {
                PlayerPrefs.SetInt(HIGHSCORE, score);
                HighScore = score;
            }
        }
        else
        {
            PlayerPrefs.SetInt(HIGHSCORE, score);
        }
        gameOverText.text = "Game Over!\nScore: " + score + "\n Highscore: " + PlayerPrefs.GetInt(HIGHSCORE);
    }
}
