using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text gameOverText;
    private const string HIGHSCORE = "Highscore";
    public void Play()
    {
        SceneManager.LoadScene("Main");
    }

    public void Quit()
    {
        Application.Quit();
    }
    
    

    public void Start()
    {
        gameOverText.text = "Highscore: " + PlayerPrefs.GetInt(HIGHSCORE);
    }
}
