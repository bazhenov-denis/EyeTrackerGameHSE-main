using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Gaming;

public class AimC : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 cursorPos = Camera.main.ScreenToWorldPoint(TobiiAPI.GetGazePoint().Screen);
        transform.position = new Vector3(cursorPos.x, cursorPos.y, 1);
        
    }
    
}
