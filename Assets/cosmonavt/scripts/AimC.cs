using UnityEngine;
using Tobii.Gaming; // Используется для Windows

public class AimC : MonoBehaviour
{
    
    // Основная камера, используемая для преобразования координат
    public Camera mainCamera;
    
    // Скорость сглаженного перемещения
    public float lerpSpeed = 10f;
    
    // Расстояние от камеры, на котором находится объект
    public float distanceFromCamera = 5f;

    private void Awake()
    {
        // Если основная камера не задана в инспекторе, назначаем Camera.main
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    void FixedUpdate()
    {
        Vector3 targetWorldPos = Vector3.zero;
        Vector2 screenPoint;
        
        // Выбираем способ получения координат в зависимости от платформы.
        // Для Windows используем TobiiAPI, для macOS – GazeReceiver.
        if (Application.platform == RuntimePlatform.WindowsPlayer ||
            Application.platform == RuntimePlatform.WindowsEditor)
        {
            // TobiiAPI возвращает структуру с полем Screen (пиксельные координаты)
            screenPoint = TobiiAPI.GetGazePoint().Screen;
        }
        else if (Application.platform == RuntimePlatform.OSXPlayer ||
                 Application.platform == RuntimePlatform.OSXEditor)
        {
            // Предполагаем, что метод GetGazeCoordinates() возвращает нормализованные координаты.
            // Для преобразования в экранные координаты умножаем на размеры экрана.
            Vector2 normalizedCoords = GazeReceiver.Instance != null ? GazeReceiver.Instance.GetGazeCoordinates() : Vector2.zero;
            screenPoint = new Vector2(normalizedCoords.x * Screen.width, normalizedCoords.y * Screen.height);
        }
        else
        {
            // Если платформа не опознана, можно задать значение по умолчанию.
            screenPoint = Vector2.zero;
        }
        
        // Создаём экранную точку с заданным расстоянием (по оси Z)
        Vector3 sp = new Vector3(screenPoint.x, screenPoint.y, mainCamera.nearClipPlane + distanceFromCamera);
        // Преобразуем экранные координаты в мировые
        targetWorldPos = mainCamera.ScreenToWorldPoint(sp);
        
        // Плавное перемещение объекта AimC к targetWorldPos
        transform.position = Vector3.Lerp(transform.position, targetWorldPos, Time.deltaTime * lerpSpeed);
    }
}
