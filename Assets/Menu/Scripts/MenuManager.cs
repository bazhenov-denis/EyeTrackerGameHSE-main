using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject settingsPanel;
    public GameObject Panel;


    public void Training()
    {
        SceneManager.LoadScene("TrainingMenu");
    }
    
    public void Attention()
    {
        SceneManager.LoadScene("FocusMenu");
    }
    public void Logic()
    {
        SceneManager.LoadScene("LogicMenu");
    }
    public void Memory()
    {
        SceneManager.LoadScene("MemoryMenu");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void SettingsPanel()
    {
        settingsPanel.SetActive(true);
    }

    public void ExitSettings()
    {
        settingsPanel.SetActive(false);
        Panel.SetActive(true);

    }
}
