using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;
using Random = System.Random;

[Serializable]
public class CalibrationPoint
{
  public float x;
  public float y;
}

[Serializable]
public class CalibrationConfig
{
  public int screen_width;
  public int screen_height;
  public CalibrationPoint[] calibration_points;
}

public class CalibrationManager : MonoBehaviour
{
  public CalibrationPoint[] calibrationPoints = new CalibrationPoint[]
  {
    new CalibrationPoint { x = 0.1f, y = 0.1f },
    new CalibrationPoint { x = 0.3f, y = 0.1f },
    new CalibrationPoint { x = 0.6f, y = 0.1f },
    new CalibrationPoint { x = 0.9f, y = 0.1f },

    new CalibrationPoint { x = 0.1f, y = 0.3f },
    new CalibrationPoint { x = 0.3f, y = 0.3f },
    new CalibrationPoint { x = 0.6f, y = 0.3f },
    new CalibrationPoint { x = 0.9f, y = 0.3f },

    new CalibrationPoint { x = 0.1f, y = 0.6f },
    new CalibrationPoint { x = 0.3f, y = 0.6f },
    new CalibrationPoint { x = 0.6f, y = 0.6f },
    new CalibrationPoint { x = 0.9f, y = 0.6f },

    new CalibrationPoint { x = 0.1f, y = 0.9f },
    new CalibrationPoint { x = 0.3f, y = 0.9f },
    new CalibrationPoint { x = 0.6f, y = 0.9f },
    new CalibrationPoint { x = 0.9f, y = 0.9f }
  };

  public RectTransform targetIndicator; // UI-объект мишени (например, круг, созданный как prefab)
  public Camera uiCamera;
  public RectTransform canvasRectTransform; // RectTransform Canvas или родительского контейнера

  private int currentIndex = 0;

  // Флаг для безопасного вызова NextTarget() из главного потока
  private volatile bool calibrationFixedPending = false;

  void Start()
  {
    // Подписываемся на событие фиксации из GazeReceiver
    if (GazeReceiver.Instance != null)
    {
      GazeReceiver.Instance.isCalibrate = false;
      GazeReceiver.Instance.OnCalibrationFixed += OnCalibrationFixed;
    }

    // 1. Формируем JSON-конфигурацию и сохраняем её
    string config = JsonifyConfig();
    string configPath = Path.Combine(Application.persistentDataPath, "calibration_config.json");
    File.WriteAllText(configPath, config);
    Debug.Log("Конфигурация записана в: " + Path.GetFullPath(configPath));
    Debug.Log(Application.dataPath);
      // Формируем путь до python-исполнителя внутри venv
#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
    string pythonExePath = Path.Combine(Application.dataPath, "StreamingAssets/Python/.venv/bin/python3.11");
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        string pythonExePath = Path.Combine(Application.dataPath, "PythonEnv/Scripts/python.exe");
#else
        string pythonExePath = ""; // Другие платформы
#endif
    
    string scriptPath = Path.Combine(Application.dataPath, "StreamingAssets/Python/calibrate.py");
    GazeReceiver.Instance.StartPythonProcess(scriptPath, configPath, pythonExePath);

    // 3. Показываем первую мишень
    ShowTarget(currentIndex);
  }

  // Этот метод вызывается из фонового потока, поэтому устанавливаем флаг:
  void OnCalibrationFixed()
  {
    calibrationFixedPending = true;
  }

  void Update()
  {
    // Если флаг установлен, вызываем NextTarget() на главном потоке
    if (calibrationFixedPending)
    {
      calibrationFixedPending = false;
      NextTarget();
    }
  }

  string JsonifyConfig()
  {
    CalibrationConfig configObj = new CalibrationConfig
    {
      screen_width = Screen.width,
      screen_height = Screen.height,
      calibration_points = Shuffle(calibrationPoints)
    };
    string json = JsonUtility.ToJson(configObj);
    Debug.Log("Сериализованная конфигурация: " + json);
    return json;
  }

  void ShowTarget(int index)
  {
    if (index < 0 || index >= calibrationPoints.Length)
      return;
    CalibrationPoint cp = calibrationPoints[index];
    float screenX = cp.x * Screen.width;
    float screenY = cp.y * Screen.height;
    Vector2 screenPoint = new Vector2(screenX, screenY);

    // Преобразуем экранные координаты в локальные для canvasRectTransform
    Vector2 localPoint;
    RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, screenPoint, uiCamera, out localPoint);
    targetIndicator.anchoredPosition = localPoint;
    targetIndicator.gameObject.SetActive(true);
  }

  void NextTarget()
  {
    targetIndicator.gameObject.SetActive(false);
    currentIndex++;
    if (currentIndex < calibrationPoints.Length - 1)
    {
      ShowTarget(currentIndex);
    }
    else
    {
      Debug.Log("Calibration complete!");
      if (GazeReceiver.Instance != null)
      {
        GazeReceiver.Instance.isCalibrate = true;
        SceneManager.LoadScene("MainMenu");
      }
      // Переход к следующей сцене или дальнейшая логика
    }
  }

  void OnDestroy()
  {
    if (GazeReceiver.Instance != null)
    {
      GazeReceiver.Instance.OnCalibrationFixed -= OnCalibrationFixed;
    }
  }

  private static T[] Shuffle<T>(T[] array)
  {
    T[] arrayCopy = array;
    Random rng = new Random(); // либо передавайте Random как параметр, чтобы избежать повторной инициализации
    int n = array.Length;
    while (n > 1)
    {
      n--;
      int k = rng.Next(n + 1);
      (arrayCopy[k], arrayCopy[n]) = (arrayCopy[n], arrayCopy[k]);
    }

    return arrayCopy;
  }


}