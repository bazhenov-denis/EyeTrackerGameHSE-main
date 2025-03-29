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
    public GameObject hammer;          // Ссылка на молоток
    public float holdTime = 0.7f;        // Время удержания курсора
    public GameObject stars;
    public GameObject mice;
    public Text scoreText;
    private static int _score;

    private float _holdTimer;           // Таймер удержания
    private bool _isHolding;            // Флаг, установленный когда курсор находится в зоне (OnMouseEnter)

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

    void Update()
    {
        if (stateGame.State != State.Gameplay)
            return;

        // Проверяем, есть ли валидный взгляд, или используется мышь (когда _isHolding true)
        bool useGaze = (_gazeAware != null && _gazeAware.HasGazeFocus);
        if (useGaze || _isHolding)
        {
            // Перемещаем молоток по координатам объекта (здесь можно улучшить позиционирование)
            Vector2 hammerPosition = GetComponent<Transform>().position;
            hammerPosition.y += GetComponent<BoxCollider2D>().offset.y;
            hammerPosition.x += GetComponent<BoxCollider2D>().size.x / 2;
            hammer.transform.position = hammerPosition;

            _holdTimer += Time.deltaTime;
            if (_holdTimer >= holdTime)
            {
                // Запускаем анимацию удара
                PlayHammerAnimation();

                float animationLengthHalf = _hammerAnimator.GetCurrentAnimatorStateInfo(0).length / 2;
                if (!mice.IsUnityNull())
                {
                    // Вызываем удаление мыши через Invoke
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
        // Когда курсор (мышь) входит в зону объекта (например, у норы)
        hammer.SetActive(true);
        _isHolding = true;
        _holdTimer = 0f;
    }

    void OnMouseExit()
    {
        if (stateGame.State != State.Gameplay)
            return;

        _hammerAnimator.SetBool(HitFlag, false);
        _hammerAnimator.Play("DefoltHummer");
        hammer.SetActive(false);
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
