using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class GazeReceiver : MonoBehaviour
{
  public static GazeReceiver Instance { get; private set; }

  // UDP-приём координат взгляда
  private Thread receiveThread;
  private UdpClient udpClient;
  private object lockObject = new object();
  private Vector2 gazeCoordinates = Vector2.zero;
  public bool isCalibrate = false;

  // Python-процесс
  private Process pythonProcess;
  private Thread pythonOutputThread;
  private Thread pythonErrorThread;

  // Событие, вызываемое при фиксации текущей калибровочной точки
  public event Action OnCalibrationFixed;

  // Событие, которое вызывается при получении строки с "INFO:" из Python.
  public event Action OnPythonInfo;

  void Awake()
  {
    if (Application.platform == RuntimePlatform.OSXPlayer ||
        Application.platform == RuntimePlatform.OSXEditor)
    {
      if (Instance == null)
      {
        Instance = this;
        DontDestroyOnLoad(gameObject);
      }
      else
      {
        Destroy(gameObject);
      }
    }
  }

  void Start()
  {
    StartUDPReceiver();
  }

  void StartUDPReceiver()
  {
    receiveThread = new Thread(ReceiveData);
    receiveThread.IsBackground = true;
    receiveThread.Start();
  }

  void ReceiveData()
  {
    udpClient = new UdpClient(5005);
    while (true)
    {
      try
      {
        IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 5005);
        byte[] data = udpClient.Receive(ref remoteEP);
        string message = Encoding.UTF8.GetString(data);
        string[] parts = message.Split(',');
        if (parts.Length >= 2)
        {
          if (float.TryParse(parts[0], out float x) &&
              float.TryParse(parts[1], out float y))
          {
            lock (lockObject)
            {
              gazeCoordinates = new Vector2(x, y);
            }
          }
        }
      }
      catch (Exception ex)
      {
        Debug.Log("UDP receiver error: " + ex);
      }
    }
  }

  public Vector2 GetGazeCoordinates()
  {
    lock (lockObject)
    {
      return gazeCoordinates;
    }
  }

  /// <summary>
  /// Запускает Python-процесс калибровки.
  /// </summary>
  /// <param name="scriptPath">Путь к скрипту (например, calibrate.py)</param>
  /// <param name="configPath">Путь к JSON-конфигурации</param>
  /// <param name="pythonExePath">Путь к Python-исполнителю (интерпретатору)</param>
  public void StartPythonProcess(string scriptPath, string configPath, string pythonExePath)
  {
    
    // Если старый процесс существует и запущен – завершить его
    if (pythonProcess != null && !pythonProcess.HasExited)
    {
      StopPythonProcess();
    }
    
    ProcessStartInfo psi = new ProcessStartInfo
    {
      FileName = pythonExePath,
      Arguments = $"{scriptPath} \"{configPath}\"",
      UseShellExecute = false,
      CreateNoWindow = true,
      RedirectStandardOutput = true,
      RedirectStandardError = true
    };

    // Удаляем конфликтующие переменные окружения
    if (psi.EnvironmentVariables.ContainsKey("PYTHONHOME"))
      psi.EnvironmentVariables.Remove("PYTHONHOME");
    if (psi.EnvironmentVariables.ContainsKey("PYTHONPATH"))
      psi.EnvironmentVariables.Remove("PYTHONPATH");

    pythonProcess = new Process { StartInfo = psi };
    pythonProcess.Start();

    // Чтение стандартного вывода (в отдельном потоке)
    pythonOutputThread = new Thread(() =>
    {
      while (!pythonProcess.StandardOutput.EndOfStream)
      {
        string line = pythonProcess.StandardOutput.ReadLine();
        Debug.Log("Python: " + line);
        if (line == "FIXED")
        {
          // Вызываем событие фиксации
          OnCalibrationFixed?.Invoke();
        }
      }
    });
    pythonOutputThread.IsBackground = true;
    pythonOutputThread.Start();

    // Чтение ошибок
    pythonErrorThread = new Thread(() =>
    {
      while (!pythonProcess.StandardError.EndOfStream)
      {
        string line = pythonProcess.StandardError.ReadLine();
        if (!string.IsNullOrEmpty(line))
        {
          Debug.Log("Python error: " + line);
          // Если ошибка содержит "INFO:" – вызываем событие OnPythonInfo.
          if (line.Contains("INFO:"))
          {
            OnPythonInfo?.Invoke();
          }
        }
      }
    });
    pythonErrorThread.IsBackground = true;
    pythonErrorThread.Start();
  }

  public void StopPythonProcess()
  {
    if (pythonProcess != null && !pythonProcess.HasExited)
    {
      pythonProcess.Kill();
    }
  }

  void OnApplicationQuit()
  {
    if (receiveThread != null)
      receiveThread.Abort();
    if (udpClient != null)
      udpClient.Close();
    StopPythonProcess();
  }
}