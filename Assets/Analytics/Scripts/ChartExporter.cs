using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ChartExporter : MonoBehaviour
{
    // ��������� � ���������� RectTransform �������, ����������� ������
    [SerializeField] private RectTransform chartRect;
    // ��� ����� ��� ����������
    public string fileNameFormat = "chart_{USERNAME}_{GAME}.png";

    // ���� ����� ����� ������� �� UI (��������, OnClick ������)
    public void ExportChart()
    {
        StartCoroutine(ExportCoroutine());
    }

    private IEnumerator ExportCoroutine()
    {
        // ���� ��������� �����, ����� ��������� ���������� �����������
        yield return new WaitForEndOfFrame();

        // �������� ���� RectTransform � ������� �����������
        Vector3[] corners = new Vector3[4];
        chartRect.GetWorldCorners(corners);
        // ������ ����� ���� � ������� ������
        Vector2 bottomLeft = RectTransformUtility.WorldToScreenPoint(null, corners[0]);
        Vector2 topRight = RectTransformUtility.WorldToScreenPoint(null, corners[2]);

        int width = (int)(topRight.x - bottomLeft.x);
        int height = (int)(topRight.y - bottomLeft.y);

        // ������� �������� ������� �������
        Texture2D texture = new Texture2D(width, height, TextureFormat.RGB24, false);
        // ������ ������� �� ������ � �������� ��������������
        texture.ReadPixels(new Rect(bottomLeft.x, bottomLeft.y, width, height), 0, 0);
        texture.Apply();

        // �������� �������� � PNG
        byte[] pngData = texture.EncodeToPNG();

        // ���������� ��� ����� � �������������� ����� ������������ � �������� ����
        string userName = SessionManager.LoggedInUsername;
        string gameName = SessionManager.SelectedGame.ToString();
        // ������� ������������ ������� �� ����� � ��������
        foreach (char c in Path.GetInvalidFileNameChars())
        {
            userName = userName.Replace(c.ToString(), "_");
            gameName = gameName.Replace(c.ToString(), "_");
        }
        string finalFileName = fileNameFormat.Replace("{USERNAME}", userName).Replace("{GAME}", gameName);

        // 1. ���� � �����, � ������� ����� Assets (����� ������� ����, ��� Assets)
        string projectFolder = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
        // 2. ���� � ����� "Charts" (�� ��� �� ������, ��� � Assets)
        string targetFolder = Path.Combine(projectFolder, "Charts");
        // 3. ���� � ����� (��������, mydatabase.db)
        string path = Path.Combine(targetFolder, finalFileName);
        File.WriteAllBytes(path, pngData);
        Debug.Log("Chart saved to: " + path);
    }
}
