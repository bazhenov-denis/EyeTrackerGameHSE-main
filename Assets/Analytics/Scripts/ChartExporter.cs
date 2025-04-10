using System.Collections;
using System.IO;
using UnityEngine;


public class ChartExporter : MonoBehaviour
{
    [SerializeField] private RectTransform chartRect;

    // Имя файла для сохранения.
    public string fileNameFormat = "Сhart_{USERNAME}_{GAME}_{PARAM}.png";

    // Метод для кнопки скачивания.
    public void ExportChart()
    {
        StartCoroutine(ExportCoroutine());
    }

    private IEnumerator ExportCoroutine()
    {
        // Ждем окончания кадра, чтобы захватить актуальное изображение.
        yield return new WaitForEndOfFrame();

        // Получаем углы RectTransform в мировых координатах.
        Vector3[] corners = new Vector3[4];
        chartRect.GetWorldCorners(corners);
        // Нижний левый угол и верхний правый.
        Vector2 bottomLeft = RectTransformUtility.WorldToScreenPoint(null, corners[0]);
        Vector2 topRight = RectTransformUtility.WorldToScreenPoint(null, corners[2]);

        int width = (int)(topRight.x - bottomLeft.x);
        int height = (int)(topRight.y - bottomLeft.y);

        // Создаем текстуру нужного размера.
        Texture2D texture = new Texture2D(width, height, TextureFormat.RGB24, false);
        // Читаем пиксели из экрана в заданном прямоугольнике.
        texture.ReadPixels(new Rect(bottomLeft.x, bottomLeft.y, width, height), 0, 0);
        texture.Apply();

        // Кодируем текстуру в PNG.
        byte[] pngData = texture.EncodeToPNG();

        // Генерируем имя файла с использованием имени пользователя и названия игры.
        string userName = SessionManager.LoggedInUsername;
        string gameName = SessionManager.SelectedGame.ToString();
        string metric = SessionManager.MetricName.ToString();
        Debug.Log($"{metric}");
        // Удаляем недопустимые символы из имени и названия.
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

        // 1. Путь к папке, в которой лежит Assets (одним уровнем выше, чем Assets).
        string projectFolder = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
        // 2. Путь к папке "Charts" (на том же уровне, что и Assets).
        string targetFolder = Path.Combine(projectFolder, "Charts");
        string path = Path.Combine(targetFolder, finalFileName);
        File.WriteAllBytes(path, pngData);
        Debug.Log("График сохранен: " + path);
    }
}