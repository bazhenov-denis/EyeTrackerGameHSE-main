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
            DontDestroyOnLoad(gameObject); // ������ �� ����� ������������ ��� �������� ����� �����.
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    // ����� ��� �������� �������, ����� �� ������ �� �����.
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
        Debug.Log("�������: " + time.ToString("F2") + " �");
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

        Debug.Log($"����� �����������: {timeTaken}");
        Debug.Log($"����: {score}");
        Debug.Log($"���������� ������: {0}");
        Debug.Log($"������� �������: {difficulty}");
        Debug.Log($"�������� ����������: {completionPercentage}");
        Debug.Log($"�������: {performanceRating}");
        Debug.Log($"������� ����� �������: {0}");

        ClearInstance();
    }

    public void RegisterError()
    {
        errorCount++;
    }
}
