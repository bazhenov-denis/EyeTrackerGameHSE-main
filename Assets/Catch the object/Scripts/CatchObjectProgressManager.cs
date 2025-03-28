using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CatchObjectProgressManager : MonoBehaviour
{
    private float startTime;
    public static int errorCount = 0; // общее количество ошибок

    private static List<float> reactionTimes = new List<float>();
    public static void RecordReactionTime(float time)
    {
        reactionTimes.Add(time);
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
    /// ¬ызываетс€ при завершении игры (победа или поражение)
    /// </summary>
    /// <param name="victory">true, если игрок выиграл, иначе false</param>
    public void OnGameEnd(bool victory)
    {
        float timeTaken = Time.time - startTime;
        int score = ScoreController.score;           // набранные очки (например, число пойманных фруктов)
        int totalFruits = ScoreController.count;       // целевое количество (например, 10 или 15)
        double completionPercentage = victory ? 1 : ((double)score / totalFruits);
        int difficulty = DifficultCatchObject.difficult;
        // ѕример: оценка = 10 минус количество ошибок (минимум 1)
        int performanceRating = (int)(0.7*completionPercentage*5);

        LocalDatabase.Instance.AddGameHistory(SessionManager.UserID, GameName.CatchAllFruits, score, difficulty, victory,
            timeTaken, completionPercentage, errorCount, performanceRating, GetAverageReactionTime());
    }

    public void RegisterError()
    {
        errorCount++;
    }
}
