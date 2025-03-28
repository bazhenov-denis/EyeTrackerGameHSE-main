using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CatchObjectProgressManager : MonoBehaviour
{
    private float startTime;
    public static int errorCount = 0; // ����� ���������� ������

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
    /// ���������� ��� ���������� ���� (������ ��� ���������)
    /// </summary>
    /// <param name="victory">true, ���� ����� �������, ����� false</param>
    public void OnGameEnd(bool victory)
    {
        float timeTaken = Time.time - startTime;
        int score = ScoreController.score;           // ��������� ���� (��������, ����� ��������� �������)
        int totalFruits = ScoreController.count;       // ������� ���������� (��������, 10 ��� 15)
        double completionPercentage = victory ? 1 : ((double)score / totalFruits);
        int difficulty = DifficultCatchObject.difficult;
        // ������: ������ = 10 ����� ���������� ������ (������� 1)
        int performanceRating = (int)(0.7*completionPercentage*5);

        LocalDatabase.Instance.AddGameHistory(SessionManager.UserID, GameName.CatchAllFruits, score, difficulty, victory,
            timeTaken, completionPercentage, errorCount, performanceRating, GetAverageReactionTime());
    }

    public void RegisterError()
    {
        errorCount++;
    }
}
