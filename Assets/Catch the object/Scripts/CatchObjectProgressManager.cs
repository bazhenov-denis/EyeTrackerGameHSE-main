using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CatchObjectProgressManager : MonoBehaviour
{
    public static CatchObjectProgressManager Instance { get; private set; }
    private float startTime;
    public static int errorCount = 0;

    private static List<float> reactionTimes = new List<float>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Объект не будет уничтожаться при загрузке новой сцены.
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    // Метод для удаления объекта, когда он больше не нужен.
    public static void ClearInstance()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
            Instance = null;
        }
    }


    public static void RecordReactionTime(float time)
    {
        reactionTimes.Add(time);
        Debug.Log("Реакция: " + time.ToString("F2") + " с");
    }
    public static float GetAverageReactionTime()
    {
        return reactionTimes.Count > 0 ? reactionTimes.Average() : 0f;
    }

    void Start()
    {
        startTime = Time.time;
        errorCount = 0;
    }

    /// <summary>
    /// Вызывается при завершении игры (победа или поражение)
    /// </summary>
    /// <param name="victory">true, если игрок выиграл, иначе false</param>
    public void OnGameEnd(bool victory)
    {
        float timeTaken = Time.time - startTime;
        int score = ScoreController.score;
        int totalObjects = Spawner.totalSpawnedObjects;
        double completionPercentage = (1 - (double)errorCount / totalObjects)*100;
        int difficulty = DifficultCatchObject.difficult;

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
                performanceRating = (int)(0.7 * difficulty * completionPercentage / 100 * 5);
                break;
        }
        if (performanceRating < 1)
            performanceRating = 1;
        else if (performanceRating > 5)
            performanceRating = 5;

        //float avarageReaction = GetAverageReactionTime();

        LocalDatabase.Instance.AddGameHistory(SessionManager.UserID, GameName.CatchAllFruits, score, difficulty, victory,
            timeTaken, completionPercentage, errorCount, performanceRating, 0);

        Debug.Log($"Время прохождения: {timeTaken}");
        Debug.Log($"Счет: {score}");
        Debug.Log($"Количество ошибок: {0}");
        Debug.Log($"Текущий уровень: {difficulty}");
        Debug.Log($"Точность выполнения: {completionPercentage}");
        Debug.Log($"Рейтинг: {performanceRating}");
        Debug.Log($"Среднее время реакции: {0}");

        ClearInstance();
    }

    public void RegisterError()
    {
        errorCount++;
    }
}
