using System.Collections.Generic;
using UnityEngine;
using XCharts;
using System.Linq;
using XCharts.Runtime;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Text;

public class ChartUpdater : MonoBehaviour
{
    [SerializeField] private LineChart lineChart;
    [SerializeField] private TextMeshProUGUI userName;

    // Список значений для выбранного параметра.
    private List<float> paramValues;
    // Метки оси X (даты).
    private List<string> dateLabels;

    void Start()
    {
        LoadDataFromDatabase();
        UpdateChart();
    }

    private void LoadDataFromDatabase()
    {
        int userId = SessionManager.UserID;
        GameName selectedGame = SessionManager.SelectedGame;

        List<GameHistory> records = LocalDatabase.Instance.GetHistoryForGame(userId, selectedGame);
        if (records == null || records.Count == 0)
        {
            paramValues = new List<float>();
            dateLabels = new List<string>();
            return;
        }

        // Сортируем записи по дате в порядке возрастания.
        records = records.OrderBy(r => r.DatePlayed).ToList();

        // Формируем список дат.
        dateLabels = records.Select(r => r.DatePlayed.ToString("dd.MM.yyyy HH:mm")).ToList();

        // Определяем, какой параметр выбрал пользователь.
        MetricName selectedParam = SessionManager.MetricName;

        // Преобразуем записи в список float, в зависимости от выбранного параметра.
        switch (selectedParam)
        {
            case MetricName.AverageReactionTime:
                paramValues = records.Select(r => (float)r.AverageReactionTime).ToList();
                break;
            case MetricName.TimeTaken:
                paramValues = records.Select(r => (float)r.TimeTaken).ToList();
                break;
            case MetricName.CompletionPercentage:
                paramValues = records.Select(r => (float)(r.CompletionPercentage)).ToList();
                break;
            case MetricName.ErrorCount:
                paramValues = records.Select(r => (float)r.ErrorCount).ToList();
                break;
            case MetricName.PerformanceRating:
                paramValues = records.Select(r => (float)r.PerformanceRating).ToList();
                break;
            default:
                paramValues = new List<float>();
                break;
        }

        userName.text = SessionManager.LoggedInUsername;
    }

    public void UpdateChart()
    {
        // Очищаем все данные графика.
        lineChart.ClearData();

        // Получаем (или создаём) XAxis.
        var xAxis = lineChart.GetChartComponent<XAxis>(0);
        if (xAxis != null)
        {
            xAxis.type = Axis.AxisType.Category;
            xAxis.data.Clear();
            xAxis.data.AddRange(dateLabels);
        }

        MetricName selectedParam = SessionManager.MetricName;
        string paramName = "Неизвестно";

        switch (selectedParam)
        {
            case MetricName.AverageReactionTime:
                paramName = "Среднее время реакции";
                break;
            case MetricName.TimeTaken:
                paramName = "Время прохождения";
                break;
            case MetricName.CompletionPercentage:
                paramName = "Точность выполнения";
                break;
            case MetricName.ErrorCount:
                paramName = "Количество ошибок";
                break;
            case MetricName.PerformanceRating:
                paramName = "Оценка выполнения";
                break;
        }

        // Получаем компонент Title графика.
        var title = lineChart.GetChartComponent<Title>();
        if (title != null)
        {
            title.text = paramName;
        }

        // Добавляем данные в серию 0.
        for (int i = 0; i < paramValues.Count; i++)
        {
            lineChart.AddData(0, paramValues[i]);
        }
    }
}