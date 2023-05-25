using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void LoadSceneHighScore()
    {
        SceneManager.LoadScene("HighScore");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
