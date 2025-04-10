using System.Collections.Generic;
using System.IO;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReportManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI userNameText;

    void Start()
    {
        userNameText.text = SessionManager.LoggedInUsername;
    }

    public string fileNameFormat = "AllData_{USERNAME}.csv";

    public void ExportAllData()
    {
        string userName = SessionManager.LoggedInUsername;

        // Чтобы избежать проблем с недопустимыми символами, уберём их из имени пользователя
        foreach (char c in System.IO.Path.GetInvalidFileNameChars())
        {
            userName = userName.Replace(c.ToString(), "_");
        }

        string finalFileName = fileNameFormat.Replace("{USERNAME}", userName);

        List<GameHistory> records = LocalDatabase.Instance.GetAllHistoryForUser(SessionManager.UserID);

        if (records == null || records.Count == 0)
        {
            Debug.Log("Нет записей для экспорта!");
            return;
        }

        // Собираем CSV-строку
        string csv = BuildCsv(records);

        // 1. Путь к папке, в которой лежит Assets (одним уровнем выше, чем Assets)
        string projectFolder = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
        string targetFolder = Path.Combine(projectFolder, "CsvDataExport");
        string path = Path.Combine(targetFolder, finalFileName);

        // Записываем CSV в файл
        File.WriteAllText(path, csv, Encoding.UTF8);

        Debug.Log("Все данные сохранены в CSV-файл: " + path);
    }

    private string BuildCsv(List<GameHistory> records)
    {
        // Заголовки колонок.
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("Game;DatePlayed;Score;DifficultyLevel;Victory;TimeTaken;CompletionPercentage;ErrorCount;PerformanceRating;AverageReactionTime");

        // Формируем строку для каждой записи.
        foreach (var rec in records)
        {
            sb.AppendLine($"{rec.Game};{rec.DatePlayed:yyyy-MM-dd HH:mm:ss};{rec.Score};{rec.DifficultyLevel};{rec.Victory};{rec.TimeTaken:F2};{rec.CompletionPercentage:F2};{rec.ErrorCount};{rec.PerformanceRating:F2};{rec.AverageReactionTime:F2}");
        }
        return sb.ToString();
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MenuAnalytics");
    }
}