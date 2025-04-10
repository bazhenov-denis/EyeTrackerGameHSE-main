using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HistoryManager : MonoBehaviour
{
    public void BackToProfileMenu()
    {
        if (SessionManager.Progress)
        {
            SceneManager.LoadScene("MenuAnalytics");
            SessionManager.Progress = false;
            return;
        }
        if (SessionManager.Reabilitation)
        {
            SceneManager.LoadScene("MenuAnalytics");
            SessionManager.Reabilitation = false;
            return;
        }
        SceneManager.LoadScene("ProfileScene");
    }

    public void OnBeatMiceButtonClick()
    {
        SessionManager.SelectedGame = GameName.BeatMice;
        if (SessionManager.Progress)
        {
            SceneManager.LoadScene("ShowProgress");
            return;
        }
        if (SessionManager.Reabilitation)
        {
            SceneManager.LoadScene("ChartMenu");    
            return;
        }
        SceneManager.LoadScene("ShowHistory");
    }

    public void OnCosmonautButtonClick()
    {
        SessionManager.SelectedGame = GameName.Cosmonaut;
        if (SessionManager.Progress)
        {
            SceneManager.LoadScene("ShowProgress");
            return;
        }
        if (SessionManager.Reabilitation)
        {
            SceneManager.LoadScene("ChartMenu");
            return;
        }
        SceneManager.LoadScene("ShowHistory");
    }

    public void OnCatchFruitsButtonClick()
    {
        SessionManager.SelectedGame = GameName.CatchAllFruits;
        if (SessionManager.Progress)
        {
            SceneManager.LoadScene("ShowProgress");  
            return;
        }
        if (SessionManager.Reabilitation)
        {
            SceneManager.LoadScene("ChartMenu");
            return;
        }
        SceneManager.LoadScene("ShowHistory");
    }

    public void OnMemoryButtonClick()
    {
        SessionManager.SelectedGame = GameName.Memory;
        if (SessionManager.Progress)
        {
            SceneManager.LoadScene("ShowProgress");
            return;
        }
        if (SessionManager.Reabilitation)
        {
            SceneManager.LoadScene("ChartMenu");
            return;
        }
        SceneManager.LoadScene("ShowHistory");
    }
}
