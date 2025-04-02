using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ChartExporter : MonoBehaviour
{
    [SerializeField] private RectTransform chartRect;

    // ��� ����� ��� ����������.
    public string fileNameFormat = "�hart_{USERNAME}_{GAME}_{PARAM}.png";

    // ����� ��� ������ ����������.
    public void ExportChart()
    {
        StartCoroutine(ExportCoroutine());
    }

    private IEnumerator ExportCoroutine()
    {
        // ���� ��������� �����, ����� ��������� ���������� �����������.
        yield return new WaitForEndOfFrame();

        // �������� ���� RectTransform � ������� �����������.
        Vector3[] corners = new Vector3[4];
        chartRect.GetWorldCorners(corners);
        // ������ ����� ���� � ������� ������.
        Vector2 bottomLeft = RectTransformUtility.WorldToScreenPoint(null, corners[0]);
        Vector2 topRight = RectTransformUtility.WorldToScreenPoint(null, corners[2]);

        int width = (int)(topRight.x - bottomLeft.x);
        int height = (int)(topRight.y - bottomLeft.y);

        // ������� �������� ������� �������.
        Texture2D texture = new Texture2D(width, height, TextureFormat.RGB24, false);
        // ������ ������� �� ������ � �������� ��������������.
        texture.ReadPixels(new Rect(bottomLeft.x, bottomLeft.y, width, height), 0, 0);
        texture.Apply();

        // �������� �������� � PNG.
        byte[] pngData = texture.EncodeToPNG();

        // ���������� ��� ����� � �������������� ����� ������������ � �������� ����.
        string userName = SessionManager.LoggedInUsername;
        string gameName = SessionManager.SelectedGame.ToString();
        string metric = SessionManager.MetricName.ToString();
        Debug.Log($"{metric}");
        // ������� ������������ ������� �� ����� � ��������.
        foreach (char c in Path.GetInvalidFileNameChars())
        {
            userName = userName.Replace(c.ToString(), "_");
            gameName = gameName.Replace(c.ToString(), "_");
            metric = metric.Replace(c.ToString(), "_");
        }
        Debug.Log($"{metric}");
        Debug.Log($"{fileNameFormat}");
        string finalFileName = fileNameFormat.Replace("{USERNAME}", userName)
            .Replace("{GAME}", gameName)
            .Replace("{PARAM}", metric);

        // 1. ���� � �����, � ������� ����� Assets (����� ������� ����, ��� Assets).
        string projectFolder = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
        // 2. ���� � ����� "Charts" (�� ��� �� ������, ��� � Assets).
        string targetFolder = Path.Combine(projectFolder, "Charts");
        string path = Path.Combine(targetFolder, finalFileName);
        File.WriteAllBytes(path, pngData);
        Debug.Log("������ ��������: " + path);
    }
}
