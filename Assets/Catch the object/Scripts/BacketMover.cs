using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Gaming;

public class BacketMover : MonoBehaviour
{
    public Vector3 screenPosition;
    public Transform Backet;

    public Vector2 worldPosition;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        screenPosition = TobiiAPI.GetGazePoint().Screen;
        worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        Backet.transform.position = new Vector3(worldPosition.x, Backet.transform.position.y);
    }
}
