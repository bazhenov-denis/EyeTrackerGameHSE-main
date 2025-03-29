using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddButtons : MonoBehaviour
{
    [SerializeField] private Transform puzzleField;
    [SerializeField] private GameObject button;
    [SerializeField] public GridLayoutGroup field;
    public static int counter = 6;

    private void Start()
    {
        Level();
    }

    public void Level()
    {
        if (counter <= 4)
        {
            field.cellSize = new Vector2(300, 300);
            field.constraintCount = 2;
        } 
        else if (counter <= 6)
        {
            field.cellSize = new Vector2(300, 300);
            field.constraintCount = 3;
        } 
        else if (counter <= 8)
        {
            field.cellSize = new Vector2(300, 300); 
            field.constraintCount = 4;
        }
        else if (counter <= 10)
        {
            field.cellSize = new Vector2(300, 300);
            field.constraintCount = 5;
        } 
        else if (counter <= 12)
        {
            field.cellSize = new Vector2(300, 300);
            field.constraintCount = 4;
        }
        else if (counter <= 16)
        {
            field.cellSize = new Vector2(200, 200);
            field.constraintCount = 4;
        } 
        else if (counter <= 18)
        {
            field.cellSize = new Vector2(200, 200);
            field.constraintCount = 6;
        }
        else if (counter <= 20)
        {
            field.cellSize = new Vector2(200, 200);
            field.constraintCount = 5;
        } 
        
    }
    private void Awake()
    {
        for (int i = 0; i < counter; i++)
        {
            GameObject _button = Instantiate(this.button);
            _button.name = "" + i;
            _button.transform.SetParent(puzzleField, false);
        }
    }
}
