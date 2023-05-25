using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("Main");
    }


    public void LoadCurrencyScene()
    {
        SceneManager.LoadScene("Currency");
    }

    public void LoadTimeZoneScene()
    {
        SceneManager.LoadScene("TimeZone");
    }

    public void Logout()
    {
        SceneManager.LoadScene("Login");
    }
}
