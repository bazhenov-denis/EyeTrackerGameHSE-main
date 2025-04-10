using UnityEngine;
using Python.Runtime;

public class PythonEnvironmentSetup : MonoBehaviour
{
  void Awake()
  {
#if UNITY_STANDALONE_WIN
        string pythonHome = System.IO.Path.Combine(Application.dataPath, "PythonVenv", "venv-win");
        string pythonDll = System.IO.Path.Combine(pythonHome, "python311.dll");
#elif UNITY_STANDALONE_OSX
    var pathToVirtualEnv = @"/Users/ruslanfarahov/PycharmProjects/eye_tracking/.venv";
    string pythonHome = System.IO.Path.Combine(Application.dataPath, pathToVirtualEnv);
    string pythonDll = @"/Library/Frameworks/Python.framework/Versions/3.11/lib/libpython3.11.dylib";
#else
        string pythonHome = "";
        string pythonDll = "";
#endif
    System.Environment.SetEnvironmentVariable("PYTHONHOME", pythonHome);
    // System.Environment.SetEnvironmentVariable("PYTHONNET_PYDLL", pythonDll);

    Debug.Log("PYTHONHOME: " + pythonHome);
    Debug.Log("PYTHONNET_PYDLL: " + pythonDll);
    Runtime.PythonDLL = pythonDll;

    // Дополнительно можно установить PYTHONPATH, если у вас есть свои модули
    string pythonModulesPath = System.IO.Path.Combine(Application.dataPath, "PythonModules");
    Debug.Log(pythonModulesPath);
    System.Environment.SetEnvironmentVariable("PYTHONPATH", pythonModulesPath);
  }

  void Start()
  {
    PythonEngine.Initialize();
    Debug.Log("Иниуциализация Python прошла успешно");
    // Ваш дальнейший код
  }

  void OnApplicationQuit()
  {
    PythonEngine.Shutdown();
  }
}