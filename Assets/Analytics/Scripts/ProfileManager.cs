using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProfileManager : MonoBehaviour
{
    public void BackToMenu()
    {
        SceneManager.LoadScene("MenuAnalytics");
    }

    public void GoToCreateProfile()
    {
        SceneManager.LoadScene("CreateProfile");
    }

    public void GoToLogIn()
    {
        SceneManager.LoadScene("LogIn");
    }

    public void GoToHistoryMenu()
    {
        if (SessionManager.LogIn)
            SceneManager.LoadScene("HistoryMenu");
    }
}


