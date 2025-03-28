using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToAnalyticsButton : MonoBehaviour
{
    public void GoToAnalytics()
    {
        SceneManager.LoadScene("MenuAnalytics");
    }
}
