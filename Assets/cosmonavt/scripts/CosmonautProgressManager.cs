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
        double completionPercentage = 100; // ���� ��������� ���������
        int performanceRating = 5;
        int difficulty = DifficultyLevelMenuManager.difficult;

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
        double completionPercentage = ((double)score / winCount)*100;
        int performanceRating = (int)(completionPercentage/100 * 5); // ���� ������ ��� ���������
        if (performanceRating < 1)
            performanceRating = 1;
        else if (performanceRating > 5)
            performanceRating = 5;
        int difficulty = DifficultyLevelMenuManager.difficult;

        LocalDatabase.Instance.AddGameHistory(SessionManager.UserID, GameName.�osmonaut, score, difficulty, victory,
                                                timeTaken, completionPercentage, 0, performanceRating, GetAverageReactionTime());
    }
}
