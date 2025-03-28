using System;using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public enum State{
    Gameplay,
    Other,
    StopGame
}
public class StateGame : MonoBehaviour
{
    public State State { get; private set; }

    private void Start()
    {
        SetState(State.Gameplay);
    }

    public void SetState(State state)
    {
        State = state;

        if (state is State.Gameplay or State.Other)
        {
            Time.timeScale = 1f;
        }
        else
        {
            Time.timeScale = 0;
        }
    }
}
