using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Tobii.Gaming;

public class MiceController : MonoBehaviour
{
    public GameObject hammer; // Ссылка на молоток
    public float holdTime = 0.7f; // Время удержания курсора
    public GameObject stars;
    public GameObject mice;

    public Text scoreText;
    private static int _score;

    private float _holdTimer; // Таймер для отслеживания времени удержания

    private bool _isHolding; // Флаг для определения, удерживается ли курсор

    private Animator _hammerAnimator;

    [SerializeField] private GazeAware _gazeAware;
    private static readonly int HitFlag = Animator.StringToHash("HitFlag");
    private GameObject _starsNew;
    private SynchronizationContext _mainThreadContext;
    [SerializeField] private StateGame stateGame;


    private void Start()
    {
        _mainThreadContext = SynchronizationContext.Current;
        _hammerAnimator = hammer.GetComponent<Animator>();
        
    }

    // Start is called before the first frame update
    void Update()
    {
        if (stateGame.State != State.Gameplay)
        {
            return;
        }

        //Debug.Log(_gazeAware.HasGazeFocus);
        if (_gazeAware.HasGazeFocus)
        {
            hammer.SetActive(true); // Отобразить молоток
            Vector2 hammerPosition = GetComponent<Transform>().position;
            hammerPosition.y += GetComponent<BoxCollider2D>().offset.y;
            hammerPosition.x += GetComponent<BoxCollider2D>().size.x / 2;
            hammer.transform.position = hammerPosition;
           
            _holdTimer += Time.deltaTime;
            if (_holdTimer >= holdTime)
            {
                float reactionTime = 0f;
                if (mice != null)
                {
                    // Получаем компонент CatchObjectItem у текущей мыши
                    CatchItem item = mice.GetComponent<CatchItem>();
                    if (item != null)
                    {
                        reactionTime = Time.time - item.spawnTime;
                    }
                }
                // Регистрируем время реакции
                TimerScript.RecordReactionTime(reactionTime);

                // Воспроизвести анимацию удара
                PlayHammerAnimation();

                float animationLengthHalf = _hammerAnimator.GetCurrentAnimatorStateInfo(0).length / 2;
                if (!mice.IsUnityNull())
                {
                    // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
                    Invoke("DestroyMice", animationLengthHalf);
                }

                ResetHoldTimer();
            }
        }
        else
        {
            //_hammerAnimator.SetBool(HitFlag, false);

            //hammer.SetActive(false); // Скрыть молоток
            _isHolding = false;
            _holdTimer = 0f; // Сбрасываем таймер, если курсор не удерживается
        }
    }

    void OnMouseEnter()
    {
        //if (stateGame.State != State.Gameplay)
        //{
        //    return;
        //}

        hammer.SetActive(true); // Отобразить молоток
        Vector2 hammerPosition = GetComponent<Transform>().position;
        hammerPosition.y += GetComponent<BoxCollider2D>().offset.y;
        hammerPosition.x += GetComponent<BoxCollider2D>().size.x / 2;
        hammer.transform.position = hammerPosition;
        _isHolding = true; // Начать отсчет времени удержания
        _holdTimer = 0f; // Сбросить таймер
    }

    void OnMouseExit()
    {
        if (stateGame.State != State.Gameplay)
        {
            return;
        }

        _hammerAnimator.SetBool(HitFlag, false);
        _hammerAnimator.Play("DefoltHummer");
        hammer.SetActive(false); // Скрыть молоток
        _isHolding = false;
        ResetHoldTimer();
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
        if (_starsNew.IsUnityNull())
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