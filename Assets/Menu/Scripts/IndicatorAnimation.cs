using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Gaming;

public class IndicatorAnimation : MonoBehaviour
{
    private SpriteRenderer _spriteRander;
    [SerializeField] private int cntIterationAnumation;
    [SerializeField] private float width;
    [SerializeField] private float height;
    private float newScaleX = 0f;
    private float newScaleY = 0f;
    // Start is called before the first frame update
    void Start()
    {
        _spriteRander = GetComponent<SpriteRenderer>();
       
    }


    public IEnumerator StartAnimation()
    {
        float deltaWidth = width / cntIterationAnumation;
        float deltaHeight = height / cntIterationAnumation;

        while (newScaleX < width && newScaleY < height)
        {
            Debug.Log($"{newScaleX} {newScaleY}");
            yield return new WaitForSeconds(0.1f);
            newScaleX += deltaWidth;
            newScaleY += deltaHeight;
            transform.localScale = new Vector3(newScaleX, newScaleY, transform.localScale.z);
            
        }

    }


    
    public void BreakAnimation()
    {
        transform.localScale = new Vector3();
        (newScaleX, newScaleY) = (0, 0);
    }
   
}
