using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimerScript : MonoBehaviour
{
    public float totalTime = 60f; // Общее время в секундах
    private float _remainingTime;
    public Text timerText; // Ссылка на UI Text для отображения времени
    [SerializeField] private StateGame stateGame;
    [SerializeField] private GameObject endLevelMenu;
    [SerializeField] private MiceController _miceController;

    private void Start()
    {
        _remainingTime = totalTime;
        StartCoroutine(CountdownTimer());
    }

    private IEnumerator CountdownTimer()
    {
        while (_remainingTime > 0)
        {
            UpdateTimerDisplay(_remainingTime);
            yield return new WaitForSeconds(1f);
            _remainingTime--;
        }

        UpdateTimerDisplay(_remainingTime);
        // Время истекло, выполните необходимые действия
        ShowGameOverScreen();
    }

    private void UpdateTimerDisplay(float timeToDisplay)
    {
        int minutes = Mathf.FloorToInt(timeToDisplay / 60);
        int seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timerText.text = $"Время: {minutes:00}:{seconds:00}";
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void ShowGameOverScreen()
    {
        stateGame.SetState(State.StopGame);
        var titleText = endLevelMenu.gameObject.GetComponentInChildren<Text>();
        titleText.text += _miceController.GetScore().ToString();
        endLevelMenu.gameObject.SetActive(true);
        // Здесь можно вызвать UI окно с результатом
        // Debug.Log("Время истекло! Ваш счет: " + PlayerScore.score);
    }
}