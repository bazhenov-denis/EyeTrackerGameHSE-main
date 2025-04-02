using System.Collections.Generic;
using UnityEngine;

public class CosmonautProgressManager : MonoBehaviour
{
    private float startTime;

    // Список для сохранения времени реакции.
    private static List<float> reactionTimes = new List<float>();

    void Start()
    {
        startTime = Time.time;
        reactionTimes.Clear();
    }

    public static void RecordReactionTime(float time)
    {
        reactionTimes.Add(time);
        Debug.Log("Время реакции: " + time.ToString("F2") + " sec");
    }

    public static float GetAverageReactionTime()
    {
        if (reactionTimes.Count == 0)
            return 0f;
        float sum = 0f;
        foreach (float t in reactionTimes)
            sum += t;
        return sum / reactionTimes.Count;
    }

    /// <summary>
    /// Вызывается при победе – когда счет достигает WinCount
    /// </summary>
    public void OnGameWin()
    {
        float timeTaken = Time.time - startTime;
        int score = EnemyBehaviour.counter; // Количество "считанных" врагов.
        bool victory = true;
        double completionPercentage = 100; // Игра полностью выполнена.
        int performanceRating = 5;
        int difficulty = DifficultyLevelMenuManager.difficult;

        float avarageReaction = GetAverageReactionTime();
        // Сохраняем историю для игры "Космонавт"
        LocalDatabase.Instance.AddGameHistory(SessionManager.UserID, GameName.Сosmonaut, score, difficulty, victory,
                                                timeTaken, completionPercentage, 0, performanceRating, avarageReaction);

        Debug.Log($"Время прохождения: {timeTaken}");
        Debug.Log($"Счет: {score}");
        Debug.Log($"Количество ошибок: {0}");
        Debug.Log($"Текущий уровень: {difficulty}");
        Debug.Log($"Точность выполнения: {completionPercentage}");
        Debug.Log($"Рейтинг: {performanceRating}");
        Debug.Log($"Среднее время реакции: {avarageReaction}");
    }

    /// <summary>
    /// Вызывается при поражении (например, при столкновении)
    /// </summary>
    public void OnGameLose()
    {
        float timeTaken = Time.time - startTime;
        int score = EnemyBehaviour.counter;
        bool victory = false;
        int winCount = CounterCosmonavt.WinCount;
        int difficulty = DifficultyLevelMenuManager.difficult;
        double completionPercentage = ((double)score / winCount)*100;
        int performanceRating = 1;
        switch (difficulty)
        {
            case 1:
                performanceRating = (int)(completionPercentage / 100 * 5);
                break;
            case 2:
                performanceRating = (int)(1.2 * completionPercentage / 100 * 5);
                break;
            case 3:
                performanceRating = (int)(1.4 * completionPercentage / 100 * 5);
                break;
            case 4:
                performanceRating = (int)(1.6 * completionPercentage / 100 * 5);
                break;
            default:
                performanceRating = (int)(0.6 * difficulty * completionPercentage / 100 * 5);
                break;
        }
        if (performanceRating < 1)
            performanceRating = 1;
        else if (performanceRating > 5)
            performanceRating = 5;
        float avarageReaction = GetAverageReactionTime();
        LocalDatabase.Instance.AddGameHistory(SessionManager.UserID, GameName.Сosmonaut, score, difficulty, victory,
                                                timeTaken, completionPercentage, 0, performanceRating, avarageReaction);


        Debug.Log($"Время прохождения: {timeTaken}");
        Debug.Log($"Счет: {score}");
        Debug.Log($"Количество ошибок: {0}");
        Debug.Log($"Текущий уровень: {difficulty}");
        Debug.Log($"Точность выполнения: {completionPercentage}");
        Debug.Log($"Рейтинг: {performanceRating}");
        Debug.Log($"Среднее время реакции: {avarageReaction}");
    }
}
