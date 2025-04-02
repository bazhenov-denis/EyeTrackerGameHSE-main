using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SocialPlatforms.Impl;
using System.Collections.Generic;
using System;
using TMPro;

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
        // Получаем компонент TextMeshProUGUI, который используется для UI-текста.
        var titleText = endLevelMenu.gameObject.GetComponentInChildren<TextMeshProUGUI>();
        if (titleText == null)
        {
            Debug.LogError("TextMeshProUGUI не найден в дочерних объектах endLevelMenu!");
            return;
        }
        int score = _miceController.GetScore();
        titleText.text += score.ToString();

        // Сохраняем историю прохождения, если вход выполнен.
        if (SessionManager.LogIn)
        {
            int currentUserId = SessionManager.UserID;
            int currentDifficulty = DifficultyManager.Instance.difficultyLevel;
            double completionPercentage = (1 - (double)_errorCount / (score / 100 + _errorCount)) * 100;
            int rating = 1;
            switch (currentDifficulty)
            {
                case 1:
                    rating = (int)(completionPercentage / 100 * 5);
                    break;
                case 2:
                    rating = (int)(1.4 * completionPercentage / 100 * 5);
                    break;
                case 3:
                    rating = (int)(1.6 * completionPercentage / 100 * 5);
                    break;
                case 4:
                    rating = (int)(1.8 * currentDifficulty * completionPercentage / 100 * 5);
                    break;
                default:
                    rating = (int)(0.7 * currentDifficulty * completionPercentage / 100 * 5);
                    break;

            }
            if (rating < 1)
                rating = 1;
            else if (rating > 5)
                rating = 5;

            float avarageReaction = GetAverageReactionTime();
            LocalDatabase.Instance.AddGameHistory(currentUserId, GameName.BeatMice, score, currentDifficulty, true,
                totalTime, completionPercentage, _errorCount, rating, avarageReaction);

            Debug.Log($"Время прохождения: {totalTime}");
            Debug.Log($"Счет: {score}");
            Debug.Log($"Количество ошибок: {_errorCount}");
            Debug.Log($"Текущий уровень: {currentDifficulty}");
            Debug.Log($"Точность выполнения: {completionPercentage}");
            Debug.Log($"Рейтинг: {rating}");
            Debug.Log($"Среднее время реакции: {avarageReaction}");
        }

        endLevelMenu.gameObject.SetActive(true);
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