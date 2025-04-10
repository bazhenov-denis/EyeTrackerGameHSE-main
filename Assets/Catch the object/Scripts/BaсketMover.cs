using UnityEngine;
using Tobii.Gaming; // Для Windows

public class BaсketMover : MonoBehaviour
{
    // Ссылка на объект корзины, который будет перемещаться (его Transform)
    public Transform basket;
    
    // Камера, через которую производится преобразование координат
    public Camera mainCamera;
    
    // Расстояние от камеры (по оси Z) для ScreenToWorldPoint (при необходимости)
    public float distanceFromCamera = 5f;
    
    private Vector3 screenPosition;
    private Vector2 worldPosition;

    private void Awake()
    {
        // Если камера не задана через инспектор, используем Camera.main
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    void FixedUpdate()
    {
        Vector2 screenPoint;

        // Определяем источник координат в зависимости от платформы
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        // Для Windows используем TobiiAPI, который возвращает экранные координаты (в пикселях)
        screenPoint = TobiiAPI.GetGazePoint().Screen;
#elif UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
        // Для macOS используем GazeReceiver (предполагается, что GetGazeCoordinates() возвращает нормализованные координаты)
        Vector2 normalizedCoords = (GazeReceiver.Instance != null) ? GazeReceiver.Instance.GetGazeCoordinates() : Vector2.zero;
        // Переводим нормализованные координаты (0..1) в экранные (в пикселях)
        screenPoint = new Vector2(normalizedCoords.x * Screen.width, normalizedCoords.y * Screen.height);
#else
        screenPoint = Vector2.zero;
#endif

        // Формируем экранную точку – задаем Z как расстояние от камеры
        screenPosition = new Vector3(screenPoint.x, screenPoint.y, mainCamera.nearClipPlane + distanceFromCamera);
        // Преобразуем экранную точку в мировую позицию
        Vector3 targetWorldPos = mainCamera.ScreenToWorldPoint(screenPosition);

        // Обновляем только горизонтальную позицию (X),
        // оставляя Y и Z текущими для объекта корзины.
        basket.position = new Vector3(targetWorldPos.x, basket.position.y, basket.position.z);
    }
}
