using UnityEngine;

public class GazeFollower : MonoBehaviour
{
  // Ссылка на компонент GazeReceiver (его можно прикрепить к другому объекту или к тому же)
  public GazeReceiver gazeReceiver;

  // Основная камера, используемая для рендеринга Canvas (Screen Space - Camera)
  public Camera mainCamera;

  // RectTransform родительского контейнера (например, Canvas или Panel)
  public RectTransform canvasRectTransform;

  // Скорость сглаживания движения
  public float lerpSpeed = 10f;

  private RectTransform rectTransform;

  private void Awake()
  {
    if (mainCamera == null)
      mainCamera = Camera.main;
    if (gazeReceiver == null)
      gazeReceiver = GazeReceiver.Instance;
    // Получаем ссылку на RectTransform этого UI-элемента
    rectTransform = GetComponent<RectTransform>();
  }

  private void Start()
  {
    Debug.Log($"Размеры экрана {Screen.width}, {Screen.height}");
  }

  void Update()
  {
    if (gazeReceiver != null && mainCamera != null && canvasRectTransform != null)
    {
      // Получаем нормализованные координаты взгляда (значения от 0 до 1)
      Vector2 normalizedCoords = gazeReceiver.GetGazeCoordinates();
      // Debug.Log($@"принятые координаты {normalizedCoords.x}, {normalizedCoords.y}");

      // Преобразуем нормализованные координаты в экранные (в пикселях)
      float screenX = normalizedCoords.x * Screen.width;
      float screenY = normalizedCoords.y * Screen.height;
      // float screenY = (1f - normalizedCoords.y) * Screen.height;
      Vector2 screenPoint = new Vector2(screenX, screenY);

      // Преобразуем экранные координаты в локальные координаты для canvasRectTransform

      Vector2 localPoint;
      if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, screenPoint, mainCamera,
            out localPoint))
      {
        // Сглаженное перемещение UI-элемента к целевой локальной позиции
        // rectTransform.anchoredPosition =
        // Vector2.Lerp(rectTransform.anchoredPosition, localPoint, Time.deltaTime * lerpSpeed);
        rectTransform.anchoredPosition = localPoint;
      }
    }
  }
}