using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;

public class GameOverText : MonoBehaviour
{
    [SerializeField] private TMP_Text gameOverText;

    public void UpdateText(int score)
    {
        gameOverText.text = "Game Over!\nScore: " + score;
    }
}
