using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CatchObjectProgressManager : MonoBehaviour
{
    public static CatchObjectProgressManager Instance { get; private set; }
    private float startTime;
    public static int errorCount = 0; // общее количество ошибок

    private static List<float> reactionTimes = new List<float>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // объект не будет уничтожаться при загрузке новой сцены
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    // Метод для удаления объекта, когда он больше не нужен
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
        int score = ScoreController.score;           // набранные очки (например, число пойманных фруктов)
        int totalFruits = Spawner.totalSpawnedObjects;
        double completionPercentage = victory ? 100 : (1 - (double)errorCount / totalFruits)*100;
        int difficulty = DifficultCatchObject.difficult;
        // Пример: оценка = 10 минус количество ошибок (минимум 1)
        int performanceRating = (int)(0.7*completionPercentage/100*5);
        if (performanceRating < 1)
            performanceRating = 1;
        else if (performanceRating > 5)
            performanceRating = 5;

        LocalDatabase.Instance.AddGameHistory(SessionManager.UserID, GameName.CatchAllFruits, score, difficulty, victory,
            timeTaken, completionPercentage, errorCount, performanceRating, GetAverageReactionTime());
        ClearInstance();
    }

    public void RegisterError()
    {
        errorCount++;
    }
}
