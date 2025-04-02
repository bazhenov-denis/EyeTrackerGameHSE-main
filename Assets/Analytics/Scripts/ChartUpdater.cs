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

    // ������ �������� ��� ���������� ���������.
    private List<float> paramValues;
    // ����� ��� X (����).
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

        // ��������� ������ �� ���� � ������� �����������.
        records = records.OrderBy(r => r.DatePlayed).ToList();

        // ��������� ������ ���.
        dateLabels = records.Select(r => r.DatePlayed.ToString("dd.MM.yyyy HH:mm")).ToList();

        // ����������, ����� �������� ������ ������������.
        MetricName selectedParam = SessionManager.MetricName;

        // ����������� ������ � ������ float, � ����������� �� ���������� ���������.
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
        // ������� ��� ������ �������.
        lineChart.ClearData();

        // �������� (��� ������) XAxis.
        var xAxis = lineChart.GetChartComponent<XAxis>(0);
        if (xAxis != null)
        {
            xAxis.type = Axis.AxisType.Category;
            xAxis.data.Clear();
            xAxis.data.AddRange(dateLabels);
        }

        MetricName selectedParam = SessionManager.MetricName;
        string paramName = "����������";

        switch (selectedParam)
        {
            case MetricName.AverageReactionTime:
                paramName = "������� ����� �������";
                break;
            case MetricName.TimeTaken:
                paramName = "����� �����������";
                break;
            case MetricName.CompletionPercentage:
                paramName = "�������� ����������";
                break;
            case MetricName.ErrorCount:
                paramName = "���������� ������";
                break;
            case MetricName.PerformanceRating:
                paramName = "������ ����������";
                break;
        }

        // �������� ��������� Title �������.
        var title = lineChart.GetChartComponent<Title>();
        if (title != null)
        {
            title.text = paramName;
        }

        // ��������� ������ � ����� 0.
        for (int i = 0; i < paramValues.Count; i++)
        {
            lineChart.AddData(0, paramValues[i]);
        }
    }
}
