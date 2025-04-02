using UnityEngine;
using UnityEngine.SceneManagement;

public class AnalyticsMenuManager : MonoBehaviour
{
    public void OpenProfile()
    {
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
            SceneManager.LoadScene("ReportMenu");
    }
}
