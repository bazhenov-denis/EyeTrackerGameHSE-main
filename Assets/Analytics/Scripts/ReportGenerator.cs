using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

public class ReportGenerator : MonoBehaviour
{
    public string fileNameFormat = "AnalysisReport_{USERNAME}.txt";

    public void GenerateAnalysisReport()
    {
        string userName = SessionManager.LoggedInUsername;
        // ������� ������������ ������� �� ����� ������������
        foreach (char c in Path.GetInvalidFileNameChars())
        {
            userName = userName.Replace(c.ToString(), "_");
        }
        string finalFileName = fileNameFormat.Replace("{USERNAME}", userName);

        // �������� ��� ������ ��� ������� ������������ (�� ���� �����).
        List<GameHistory> records = LocalDatabase.Instance.GetAllHistoryForUser(SessionManager.UserID);
        if (records == null || records.Count == 0)
        {
            Debug.Log("��� ������� ��� �������!");
            return;
        }

        // ���������� ������ �� ���� ����.
        var groups = records.GroupBy(r => r.Game);

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("����� �� ������������ (��������� 5 ������� ��� ������ ����):");
        sb.AppendLine();

        foreach (var group in groups)
        {
            // �������� ��������� 5 ������� �� ������ ���� (���� ������� ������ � ����� ���).
            List<GameHistory> lastFive = group.OrderBy(r => r.DatePlayed).TakeLast(5).ToList();
            if (lastFive.Count == 0)
                continue;

            // ��������� �������������� ����������.
            float avgReaction = lastFive.Average(r => (float)r.AverageReactionTime);
            float minReaction = lastFive.Min(r => (float)r.AverageReactionTime);
            float avgCompletion = lastFive.Average(r => (float)r.CompletionPercentage);
            float avgPerformance = lastFive.Average(r => (float)r.PerformanceRating);

            // ����������, ���������� �� ����� ������� ����� ��� �� 30%.
            bool reactionOk = avgReaction <= minReaction * 1.3f;

            string recommendation = "";
            if (reactionOk && avgCompletion >= 85f && avgPerformance >= 4f)
            {
                recommendation = "����� ������ ����������� � ������� ���������, ����� ����������� ���������";
            }
            else if (!reactionOk || avgPerformance < 3f || avgCompletion < 40f)
            {
                recommendation = "����� ����� �����������, ����� ��������� ���������";
            }
            else
            {
                recommendation = "�������� ��� ������ ���������, ����� �������� �� ������� ���������";
            }

            sb.AppendLine($"{group.Key}: {recommendation}");
        }

        string reportText = sb.ToString();

        // 1. ���� � �����, � ������� ����� Assets (����� ������� ����, ��� Assets).
        string projectFolder = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
        string targetFolder = Path.Combine(projectFolder, "Reports");
        string path = Path.Combine(targetFolder, finalFileName);

        File.WriteAllText(path, reportText, Encoding.UTF8);

        Debug.Log("����� ������� �������� �� ����: " + path);
    }
}
