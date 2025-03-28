using System.Collections.Generic;
using UnityEngine;

public class CosmonautProgressManager : MonoBehaviour
{
    private float startTime;

    // ������ ��� ���������� ������� �������
    private static List<float> reactionTimes = new List<float>();

    void Start()
    {
        startTime = Time.time;
        reactionTimes.Clear();
    }

    public static void RecordReactionTime(float time)
    {
        reactionTimes.Add(time);
        Debug.Log("Reaction time recorded: " + time.ToString("F2") + " sec");
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
    /// ���������� ��� ������ � ����� ���� ��������� WinCount
    /// </summary>
    public void OnGameWin()
    {
        float timeTaken = Time.time - startTime;
        int score = EnemyBehaviour.counter; // ���������� "���������" ������
        bool victory = true;
        double completionPercentage = 1; // ���� ��������� ����������
        int performanceRating = 5;
        int difficulty = (DifficultyManager.Instance != null) ? DifficultyManager.Instance.difficultyLevel : 1;

        // ��������� ������� ��� ���� "���������"
        LocalDatabase.Instance.AddGameHistory(SessionManager.UserID, GameName.�osmonaut, score, difficulty, victory,
                                                timeTaken, completionPercentage, 0, performanceRating, GetAverageReactionTime());
    }

    /// <summary>
    /// ���������� ��� ��������� (��������, ��� ������������)
    /// </summary>
    public void OnGameLose()
    {
        float timeTaken = Time.time - startTime;
        int score = EnemyBehaviour.counter;
        bool victory = false;
        int winCount = CounterCosmonavt.WinCount;
        double completionPercentage = 1 - ((double)score / winCount);
        int performanceRating = (int)(completionPercentage * 5); // ���� ������ ��� ���������
        int difficulty = (DifficultyManager.Instance != null) ? DifficultyManager.Instance.difficultyLevel : 1;

        LocalDatabase.Instance.AddGameHistory(SessionManager.UserID, GameName.�osmonaut, score, difficulty, victory,
                                                timeTaken, completionPercentage, 0, performanceRating, GetAverageReactionTime());
    }
}
