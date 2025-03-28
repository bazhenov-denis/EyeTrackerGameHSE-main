using UnityEngine;
using UnityEngine.SceneManagement;

public class AnalyticsMenuManager : MonoBehaviour
{
    // Этот метод будет вызываться при нажатии на кнопку "Профиль"
    public void OpenProfile()
    {
        // Замените "ProfileScene" на точное имя вашей сцены профиля, указанное в Build Settings
        SceneManager.LoadScene("ProfileScene");
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OpenProgress()
    {
        if (SessionManager.LogIn)
        {
            SessionManager.Progress = true;
            SceneManager.LoadScene("HistoryMenu");
        }
    }

    public void OpenReabilitation()
    {
        if (SessionManager.LogIn)
        {
            SessionManager.Reabilitation = true;
            SceneManager.LoadScene("HistoryMenu");
        }
    }

    public void OpenGeneration()
    {
        if (SessionManager.LogIn)
        {
            SceneManager.LoadScene("ReportMenu");
        }
    }
}
