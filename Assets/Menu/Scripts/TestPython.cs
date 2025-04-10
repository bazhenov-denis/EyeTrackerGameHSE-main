using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class TestPython : MonoBehaviour
{
  private Process pythonProcess;

  void Start()
  {
    StartPython();
  }

  void StartPython()
  {
    string scriptPath = "/Users/ruslanfarahov/PycharmProjects/eye_tracking/EyeGestures/examples/test.py";
    string pythonExePath = "/Users/ruslanfarahov/PycharmProjects/eye_tracking/.venv/bin/python3.11";

    ProcessStartInfo startInfo = new ProcessStartInfo();
    startInfo.FileName = pythonExePath;
    startInfo.Arguments = scriptPath;
    startInfo.UseShellExecute = false;
    startInfo.RedirectStandardOutput = true;
    startInfo.RedirectStandardError = true;
    startInfo.CreateNoWindow = true;

    // Удаляем конфликтующие переменные окружения
    if (startInfo.EnvironmentVariables.ContainsKey("PYTHONHOME"))
      startInfo.EnvironmentVariables.Remove("PYTHONHOME");
    if (startInfo.EnvironmentVariables.ContainsKey("PYTHONPATH"))
      startInfo.EnvironmentVariables.Remove("PYTHONPATH");

    pythonProcess = new Process();
    pythonProcess.StartInfo = startInfo;

    pythonProcess.OutputDataReceived += (sender, args) =>
    {
      if (string.IsNullOrEmpty(args.Data))
        return;
      Debug.Log(args.Data);
    };

    pythonProcess.ErrorDataReceived += (sender, args) =>
    {
      if (string.IsNullOrEmpty(args.Data))
        return;

      Debug.Log(args.Data);
    };

    pythonProcess.Start();
    pythonProcess.BeginOutputReadLine();
    pythonProcess.BeginErrorReadLine();
  }

  void OnApplicationQuit()
  {
    if (pythonProcess != null && !pythonProcess.HasExited)
    {
      // Завершаем процесс при закрытии приложения
      pythonProcess.Kill();
    }
  }
}