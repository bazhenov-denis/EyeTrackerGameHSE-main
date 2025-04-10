using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Gaming; // для Windows
using UnityEngine.UI;

public class TobiButton : MonoBehaviour
{
    public Animator anim;
    public GameObject button;
    [SerializeField] public Button Button;
    [SerializeField] private float holdTime = 0f;
    private float holdTimer = 0f;
    private bool flag = true;
    [SerializeField] private IndicatorAnimation indicatorAnimation;

    // Для работы с Tobii (Windows)
    private GazeAware _gazeAware;
    
    // Для работы с вашим трекером (macOS)
    public GazeReceiver gazeReceiver;
    
    // Основная камера, которая будет использоваться для преобразования экранных координат
    public Camera mainCamera;
    
    // RectTransform этого UI-элемента (кнопки)
    private RectTransform rectTransform;

    private void Awake()
    {
        // Получаем ссылки на компоненты
        anim = button.GetComponent<Animator>();
        _gazeAware = GetComponent<GazeAware>();

        // Если камера не назначена в инспекторе, получаем Camera.main
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
        // Поскольку кнопка находится в Canvas, она должна иметь RectTransform.
        rectTransform = GetComponent<RectTransform>();
    }

    void Start()
    {
        gameObject.SetActive(true);
        // Если для macOS не назначен GazeReceiver через инспектор, используем синглтон
        if (gazeReceiver == null)
        {
            gazeReceiver = GazeReceiver.Instance;
        }
    }

    void FixedUpdate()
    {
        // Если платформа Windows, используем стандартный Tobii API
        if (Application.platform == RuntimePlatform.WindowsEditor ||
            Application.platform == RuntimePlatform.WindowsPlayer)
        {
            if (_gazeAware != null && _gazeAware.HasGazeFocus)
            {
                anim.SetBool("IsOpen", true);
                holdTimer += Time.deltaTime;
                if (holdTimer >= holdTime)
                {
                    Button.onClick.Invoke();
                    ResetHoldTimer();
                }
            }
            else
            {
                anim.SetBool("IsOpen", false);
                ResetHoldTimer();
            }
        }
        // Если платформа macOS, используем GazeReceiver
        else if (Application.platform == RuntimePlatform.OSXEditor ||
                 Application.platform == RuntimePlatform.OSXPlayer)
        {
            // Предполагаем, что gazeReceiver возвращает нормализованные координаты (0-1)
            Vector2 normalizedCoords = gazeReceiver.GetGazeCoordinates();
            // Преобразуем нормализованные координаты в экранные (в пикселях)
            Vector2 screenCoords = new Vector2(normalizedCoords.x * Screen.width, normalizedCoords.y * Screen.height);
            
            // Проверяем, находится ли точка взгляда внутри области кнопки
            bool hasFocus = RectTransformUtility.RectangleContainsScreenPoint(rectTransform, screenCoords, mainCamera);
            if (hasFocus)
            {
                anim.SetBool("IsOpen", true);
                holdTimer += Time.deltaTime;
                if (holdTimer >= holdTime)
                {
                    Button.onClick.Invoke();
                    ResetHoldTimer();
                }
            }
            else
            {
                anim.SetBool("IsOpen", false);
                ResetHoldTimer();
            }
        }
        else
        {
            // Если платформа не распознана, можно реализовать логику по умолчанию или ничего не делать
            ResetHoldTimer();
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    void ResetHoldTimer()
    {
        holdTimer = 0f;
    }
}
