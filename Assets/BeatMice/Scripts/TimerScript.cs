using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SocialPlatforms.Impl;
using System.Collections.Generic;
using System;

public class TimerScript : MonoBehaviour
{
    public float totalTime = 60f; // Общее время в секундах
    private float _remainingTime;
    private int _errorCount = 0;
    public Text timerText; // Ссылка на UI Text для отображения времени
    [SerializeField] private StateGame stateGame;
    [SerializeField] private GameObject endLevelMenu;
    [SerializeField] private MiceController _miceController;

    private static List<float> reactionTimes = new List<float>();

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
        int score = _miceController.GetScore();
        titleText.text += _miceController.GetScore().ToString();

        // Сохраняем историю прохождения, если вход выполнен
        if (SessionManager.LogIn)
        {
            int currentUserId = SessionManager.UserID;
            int currentDifficulty = DifficultyManager.Instance.difficultyLevel;
            double completionPercentage = (1 - (double)_errorCount / (score / 100 + _errorCount))*100;
            Debug.Log("процент: " + completionPercentage + "  "+_errorCount / (score / 100 + _errorCount)
                + "  " + _errorCount + "  " + (score / 100 + _errorCount));
            int rating = Math.Min(5, (int)(0.6*currentDifficulty*completionPercentage/100 * 5));
            // Если игра завершена по таймеру – считаем это поражением (false)
            LocalDatabase.Instance.AddGameHistory(currentUserId, GameName.BeatMice, score, currentDifficulty, true,
                totalTime, completionPercentage ,_errorCount, rating, GetAverageReactionTime());
        }

        endLevelMenu.gameObject.SetActive(true);
        // Здесь можно вызвать UI окно с результатом
        // Debug.Log("Время истекло! Ваш счет: " + PlayerScore.score);
    }

    public void RegisterError()
    {
        _errorCount++;
    }

    public static void RecordReactionTime(float time)
    {
        reactionTimes.Add(time);
        Debug.Log("Реакция: " + time.ToString("F2") + " с");
    }

    public static float GetAverageReactionTime()
    {
        if (reactionTimes.Count == 0) return 0f;
        float sum = 0f;
        foreach (float t in reactionTimes)
        {
            sum += t;
        }
        return sum / reactionTimes.Count;
    }
}