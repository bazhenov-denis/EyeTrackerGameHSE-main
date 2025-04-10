using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
  public GameObject settingsPanel;
  public GameObject checkCalibrationPanel;
  public GameObject modeMenuPanel;
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

  public void OnCheckCalibrationButtonClicked()
  {
    // Если платформа macOS (в сборке или в редакторе)
    if (Application.platform == RuntimePlatform.OSXPlayer ||
        Application.platform == RuntimePlatform.OSXEditor)
    {
      if (GazeReceiver.Instance != null && !GazeReceiver.Instance.isCalibrate)
      {
        Panel.SetActive(false);
        checkCalibrationPanel.SetActive(true);
      }
      else
      {
        modeMenuPanel.SetActive(true);
      }
    }
    else // Для остальных (например, Windows)
    {
      modeMenuPanel.SetActive(true);
    }
  }

  public void Calibration()
  {
    SceneManager.LoadScene("Calibration");
  }

  public void ExitCheckCalibration()
  {
    checkCalibrationPanel.SetActive(false);
    Panel.SetActive(true);
  }
}