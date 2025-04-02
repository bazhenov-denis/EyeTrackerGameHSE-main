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
        // Удаляем недопустимые символы из имени пользователя
        foreach (char c in Path.GetInvalidFileNameChars())
        {
            userName = userName.Replace(c.ToString(), "_");
        }
        string finalFileName = fileNameFormat.Replace("{USERNAME}", userName);

        // Получаем все записи для данного пользователя (по всем играм).
        List<GameHistory> records = LocalDatabase.Instance.GetAllHistoryForUser(SessionManager.UserID);
        if (records == null || records.Count == 0)
        {
            Debug.Log("Нет записей для анализа!");
            return;
        }

        // Группируем записи по типу игры.
        var groups = records.GroupBy(r => r.Game);

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("Отчет по прохождениям (последние 5 записей для каждой игры):");
        sb.AppendLine();

        foreach (var group in groups)
        {
            // Получаем последние 5 записей по данной игре (если записей меньше — берем все).
            List<GameHistory> lastFive = group.OrderBy(r => r.DatePlayed).TakeLast(5).ToList();
            if (lastFive.Count == 0)
                continue;

            // Вычисляем агрегированные показатели.
            float avgReaction = lastFive.Average(r => (float)r.AverageReactionTime);
            float minReaction = lastFive.Min(r => (float)r.AverageReactionTime);
            float avgCompletion = lastFive.Average(r => (float)r.CompletionPercentage);
            float avgPerformance = lastFive.Average(r => (float)r.PerformanceRating);

            // Определяем, ухудшилось ли время реакции более чем на 30%.
            bool reactionOk = avgReaction <= minReaction * 1.3f;

            string recommendation = "";
            if (reactionOk && avgCompletion >= 85f && avgPerformance >= 4f)
            {
                recommendation = "Игрок хорошо справляется с текущей нагрузкой, можно увеличивать сложность";
            }
            else if (!reactionOk || avgPerformance < 3f || avgCompletion < 40f)
            {
                recommendation = "Игрок плохо справляется, стоит уменьшить сложность";
            }
            else
            {
                recommendation = "Нагрузка для игрока умеренная, стоит остаться на текущей сложности";
            }

            sb.AppendLine($"{group.Key}: {recommendation}");
        }

        string reportText = sb.ToString();

        // 1. Путь к папке, в которой лежит Assets (одним уровнем выше, чем Assets).
        string projectFolder = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
        string targetFolder = Path.Combine(projectFolder, "Reports");
        string path = Path.Combine(targetFolder, finalFileName);

        File.WriteAllText(path, reportText, Encoding.UTF8);

        Debug.Log("Отчет анализа сохранен по пути: " + path);
    }
}
