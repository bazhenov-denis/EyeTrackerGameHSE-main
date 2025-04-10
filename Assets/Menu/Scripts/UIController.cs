using UnityEngine;

public class UIController : MonoBehaviour
{
  public GameObject initialPanel; // Панель, которую нужно скрыть

  // Флаг, который устанавливается из фонового потока
  private volatile bool hidePanelFlag = false;

  void Start()
  {
    // Подписываемся на событие OnPythonInfo из GazeReceiver
    // Используем именованный метод, чтобы корректно отписываться при OnDestroy
    if (GazeReceiver.Instance != null)
    {
      GazeReceiver.Instance.OnPythonInfo += HandlePythonInfo;
    }
  }

  // Этот метод вызывается из фонового потока
  void HandlePythonInfo()
  {
    // Просто устанавливаем флаг
    hidePanelFlag = true;
  }

  void Update()
  {
    // В Update (главный поток) проверяем флаг и скрываем панель, если он установлен.
    if (hidePanelFlag)
    {
      hidePanelFlag = false;
      if (initialPanel != null)
      {
        initialPanel.SetActive(false);
      }
    }
  }

  void OnDestroy()
  {
    if (GazeReceiver.Instance != null)
    {
      GazeReceiver.Instance.OnPythonInfo -= HandlePythonInfo;
    }
  }
}