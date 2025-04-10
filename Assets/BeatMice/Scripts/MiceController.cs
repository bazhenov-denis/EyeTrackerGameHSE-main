using System;
using System.Collections;
using System.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using Tobii.Gaming; // Для Windows

public class MiceController : MonoBehaviour
{
    public GameObject hammer;          // Ссылка на молоток
    public float holdTime = 0.7f;        // Время удержания курсора
    public GameObject stars;
    public GameObject mice;
    public Text scoreText;
    private static int _score;

    private float _holdTimer;           // Таймер удержания
    private bool _isHolding;            // Флаг, установленный когда курсор находится в зоне (OnMouseEnter)

    private Animator _hammerAnimator;
    
    [SerializeField] private GazeAware _gazeAware; // Используется для Windows
    private static readonly int HitFlag = Animator.StringToHash("HitFlag");

    private GameObject _starsNew;
    private SynchronizationContext _mainThreadContext;
    
    [SerializeField] private StateGame stateGame;

    // Добавляем ссылку на основную камеру, необходимую для преобразования координат
    public Camera mainCamera;
    
    void Start()
    {
        _mainThreadContext = SynchronizationContext.Current;
        _hammerAnimator = hammer.GetComponent<Animator>();
    }

    void Update()
    {
        if (stateGame.State != State.Gameplay)
            return;

        bool useGaze = false;
        
        #if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
            // Для Windows используем TobiiAPI через _gazeAware
            useGaze = (_gazeAware != null && _gazeAware.HasGazeFocus);
        #elif UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
            // Для macOS используем данные из GazeReceiver
            if (GazeReceiver.Instance != null)
            {
                // Получаем нормализованные координаты (0..1) от GazeReceiver
                Vector2 normCoords = GazeReceiver.Instance.GetGazeCoordinates();
                // Переводим их в экранные координаты (в пикселях)
                Vector3 screenGaze = new Vector3(normCoords.x * Screen.width, normCoords.y * Screen.height, mainCamera.nearClipPlane + 5f);
                // Переводим экранную точку в мировую (мы задаём Z как nearClipPlane + distance, здесь 5f используется как пример)
                Vector3 worldGaze = mainCamera.ScreenToWorldPoint(screenGaze);
                // Проверяем, попадает ли мировая точка взгляда в область данного объекта (предполагается, что он имеет Collider2D)
                Collider2D col = GetComponent<Collider2D>();
                useGaze = (col != null && col.OverlapPoint(worldGaze));
            }
        #else
            // Для остальных платформ можно задать дефолтное поведение
            useGaze = false;
        #endif

        // Если действует мышь (OnMouseEnter) или зафиксирован взгляд
        if (useGaze || _isHolding)
        {
            // Перемещаем молоток по координатам объекта; здесь можно улучшить позиционирование
            // Например, получаем позицию объекта плюс смещение из BoxCollider2D
            Vector2 hammerPosition = transform.position;
            BoxCollider2D boxCol = GetComponent<BoxCollider2D>();
            if (boxCol != null)
            {
                hammerPosition.y += boxCol.offset.y;
                hammerPosition.x += boxCol.size.x / 2;
            }
            hammer.transform.position = hammerPosition;

            _holdTimer += Time.deltaTime;
            if (_holdTimer >= holdTime)
            {
                float reactionTime = 0f;
                if (mice != null)
                {
                    // Получаем компонент CatchItem у текущей мыши
                    CatchItem item = mice.GetComponent<CatchItem>();
                    if (item != null)
                    {
                        reactionTime = Time.time - item.spawnTime;
                    }
                }
                // Регистрируем время реакции
                TimerScript.RecordReactionTime(reactionTime);

                // Запускаем анимацию удара
                PlayHammerAnimation();

                float animationLengthHalf = _hammerAnimator.GetCurrentAnimatorStateInfo(0).length / 2;
                if (mice != null)
                {
                    Invoke("DestroyMice", animationLengthHalf);
                }

                ResetHoldTimer();
            }
        }
        else
        {
            _holdTimer = 0f;
        }
    }

    void OnMouseEnter()
    {
        // // Когда курсор (мышь) входит в зону объекта (например, у норы)
        // hammer.SetActive(true);
        // _isHolding = true;
        // _holdTimer = 0f;
    }

    void OnMouseExit()
    {
        // if (stateGame.State != State.Gameplay)
        //     return;
        //
        // _hammerAnimator.SetBool(HitFlag, false);
        // _hammerAnimator.Play("DefoltHummer");
        // hammer.SetActive(false);
        // _isHolding = false;
        // ResetHoldTimer();
    }

    void ResetHoldTimer()
    {
        _holdTimer = 0f;
    }

    void PlayHammerAnimation()
    {
        _hammerAnimator.SetBool(HitFlag, true);
        StartCoroutine(ResetAnimAfterDelay());
    }

    private IEnumerator ResetAnimAfterDelay()
    {
        float animationLength = _hammerAnimator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animationLength);
        _hammerAnimator.SetBool(HitFlag, false);
    }

    private void UpdateScore()
    {
        _score += 100;
        scoreText.text = "Счёт: " + _score;
    }

    public void ResetScore()
    {
        _score = 0;
    }

    public int GetScore()
    {
        return _score;
    }

    private async Task DestroyMice()
    {
        var starsPos = mice.transform.position;
        starsPos.y += 1.3f;
        if (_starsNew == null)
        {
            _starsNew = Instantiate(stars, starsPos, Quaternion.identity);
            UpdateScore();
        }

        await Task.Delay(TimeSpan.FromSeconds(0.5f));
        Destroy(mice);
        mice = null;
        Destroy(_starsNew);
    }
}
