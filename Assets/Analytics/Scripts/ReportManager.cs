using System.Collections;
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
        // Устанавливаем текст кнопки равным имени пользователя из сессии
        userNameText.text = SessionManager.LoggedInUsername;
    }

    public string fileNameFormat = "AllData_{USERNAME}.csv";

    // Вызывается при нажатии кнопки "Скачать все данные"
    public void ExportAllData()
    {
        string userName = SessionManager.LoggedInUsername;

        // Чтобы избежать проблем с недопустимыми символами, уберём их из имени пользователя
        foreach (char c in System.IO.Path.GetInvalidFileNameChars())
        {
            userName = userName.Replace(c.ToString(), "_");
        }

        // Подставляем имя пользователя в шаблон (например, "AllData_{USERNAME}.csv")
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
        // 3. Путь к файлу (например, mydatabase.db)
        string path = Path.Combine(targetFolder, finalFileName);

        // Записываем CSV в файл
        File.WriteAllText(path, csv, Encoding.UTF8);

        Debug.Log("Данные сохранены в CSV-файл: " + path);
    }

    private string BuildCsv(List<GameHistory> records)
    {
        // Первая строка — заголовки колонок
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("DatePlayed,Score,DifficultyLevel,Victory,TimeTaken,CompletionPercentage,ErrorCount,PerformanceRating,AverageReactionTime");

        // Далее каждая строка — отдельная запись
        foreach (var rec in records)
        {
            // Формируем строку CSV
            // Если в значениях могут быть запятые, лучше взять другой разделитель, например `;`
            // Или экранировать строки в кавычках
            sb.AppendLine($"{rec.DatePlayed:yyyy-MM-dd HH:mm:ss},{rec.Score},{rec.DifficultyLevel},{rec.Victory},{rec.TimeTaken},{rec.CompletionPercentage},{rec.ErrorCount},{rec.PerformanceRating},{rec.AverageReactionTime}");
        }
        return sb.ToString();
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MenuAnalytics");
    }
}
