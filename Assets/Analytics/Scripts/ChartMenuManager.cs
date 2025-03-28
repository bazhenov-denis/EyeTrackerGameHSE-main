using UnityEngine;
using UnityEngine.SceneManagement;

public class ChartMenuManager : MonoBehaviour
{
    public void BackToMenuHistory()
    {
        SceneManager.LoadScene("HistoryMenu");
    }

    public void AvarageTimeChart()
    {
        SessionManager.MetricName = MetricName.AverageReactionTime;
        SceneManager.LoadScene("ChartScene");
    }

    public void TimeChart()
    {
        SessionManager.MetricName = MetricName.TimeTaken;
        SceneManager.LoadScene("ChartScene");
    }

    public void AccuracyChart()
    {
        SessionManager.MetricName = MetricName.CompletionPercentage;
        SceneManager.LoadScene("ChartScene");
    }

    public void MistakesChart()
    {
        SessionManager.MetricName = MetricName.ErrorCount;
        SceneManager.LoadScene("ChartScene");
    }

    public void RatingChart()
    {
        SessionManager.MetricName = MetricName.PerformanceRating;
        SceneManager.LoadScene("ChartScene");
    }
}
