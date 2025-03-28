using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Gaming;
using UnityEngine.UI;

public class GazeInteraction : MonoBehaviour

{
    public Color focusColor = Color.green;  // Цвет при фиксации взгляда
    public Color normalColor = Color.white; // Исходный цвет
    public float gazeTimeThreshold = 1.5f;  // Время фиксации взгляда (1.5 сек)
    public float colorLerpSpeed = 0.1f;     // Скорость плавного изменения цвета

    private GazeAware _gazeAware;
    private Image _buttonImage;
    private float _gazeTimer = 0f;
    private bool _isGazing = false;

    void Start()
    {
        _gazeAware = GetComponent<GazeAware>();
        _buttonImage = GetComponent<Image>();
        _buttonImage.color = normalColor;
    }

    void Update()
    {
        if (_gazeAware.HasGazeFocus)
        {
            _gazeTimer += Time.deltaTime;
            _buttonImage.color = Color.Lerp(_buttonImage.color, focusColor, colorLerpSpeed);

            if (_gazeTimer >= gazeTimeThreshold)
            {
                OnButtonClick();
                _gazeTimer = 0f;  // Сброс таймера после нажатия
            }
        }
        else
        {
            _gazeTimer = 0f;
            _buttonImage.color = Color.Lerp(_buttonImage.color, normalColor, colorLerpSpeed);
        }
    }

    void OnButtonClick()
    {
        Debug.Log("Кнопка нажата взглядом!");
        // Здесь можно добавить выполнение действия, например, загрузку сцены или воспроизведение анимации.
    }
}
