using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Gaming;
using UnityEngine.UI;

public class TobiButton : MonoBehaviour
{
    public Animator anim;
    public GameObject button;
    GazeAware _gazeAware;
    [SerializeField] public Button Button;
    [SerializeField] private float holdTime = 0f;
    private float holdTimer = 0f;
    private bool flag = true;
    [SerializeField] private IndicatorAnimation indicatorAnimation;
    // Start is called before the first frame update
    private void Awake()
    {
        anim = button.GetComponent<Animator>();
    }
    void Start()
    {
        gameObject.SetActive(true);
        _gazeAware = GetComponent<GazeAware>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_gazeAware.HasGazeFocus)
        {
            anim.SetBool("IsOpen", true);
            if (flag)
            {
                flag = false;
            }
            holdTimer += Time.deltaTime;
            if (holdTimer >= holdTime)
            {
                Button.GetComponent<Button>().onClick.Invoke();
                ResetHoldTimer();
            }

        }
        else
        {
            flag = true;
            ResetHoldTimer();
            anim.SetBool("IsOpen", false);
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
