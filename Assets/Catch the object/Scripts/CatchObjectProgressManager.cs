using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CatchObjectProgressManager : MonoBehaviour
{
    public static CatchObjectProgressManager Instance { get; private set; }
    private float startTime;
    public static int errorCount = 0; // ����� ���������� ������

    private static List<float> reactionTimes = new List<float>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // ������ �� ����� ������������ ��� �������� ����� �����
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    // ����� ��� �������� �������, ����� �� ������ �� �����
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
        int score = ScoreController.score;           // ��������� ���� (��������, ����� ��������� �������)
        int totalFruits = Spawner.totalSpawnedObjects;
        double completionPercentage = victory ? 100 : (1 - (double)errorCount / totalFruits)*100;
        int difficulty = DifficultCatchObject.difficult;
        // ������: ������ = 10 ����� ���������� ������ (������� 1)
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
