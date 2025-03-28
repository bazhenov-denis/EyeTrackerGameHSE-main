using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine;
using Tobii.Gaming;
using System;

public class ButtonColorChange : MonoBehaviour
{

    public Color selectionColor;

    private GazeAware _gazeAwareComponent;
    private MeshRenderer _meshRenderer;

    private Color _deselectionColor;
    private Color _lerpColor;
    private float _fadeSpeed = 0.1f;

    void Start()
    {
        _gazeAwareComponent = GetComponent<GazeAware>();
        _meshRenderer = GetComponent<MeshRenderer>();
        //_lerpColor = _meshRenderer.material.color;
        _lerpColor = GetComponent<Image>().color;
        _deselectionColor = GetComponent<Image>().color;
    }

    /// <summary>
    /// Lerping the color
    /// </summary>
    void Update()
    {

        if (GetComponent<Image>().color != _lerpColor)
        {
            GetComponent<Image>().color = Color.Lerp(_meshRenderer.material.color, _lerpColor, _fadeSpeed);
        }

        // Change the color of the cube
        if (_gazeAwareComponent.HasGazeFocus)
        {
            Debug.Log("1");
            SetLerpColor(selectionColor);
        }
        else
        {
            SetLerpColor(_deselectionColor);
        }
    }

    /// <summary>
    /// Update the color, which should used for the lerping
    /// </summary>
    /// <param name="lerpColor"></param>
    public void SetLerpColor(Color lerpColor)
    {
        this._lerpColor = lerpColor;
    }

}
