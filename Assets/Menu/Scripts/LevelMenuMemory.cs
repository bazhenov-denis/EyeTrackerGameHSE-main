using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMenuMemory : MonoBehaviour
{
    // Start is called before the first frame update

    public Button[] buttons;
    public GameObject levelButtons;
    public static int unlockedLevel;
    private void Start()
    {
        ButtonsToArray();
        unlockedLevel = PlayerPrefs.GetInt("unlockedlevel", 1);
        Debug.Log(unlockedLevel);
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = false;
        }

        for (int i = 0; i < unlockedLevel; i++)
        {
            buttons[i].interactable = true;
        }
    }

    void ButtonsToArray()
    {
        int childCount = levelButtons.transform.childCount;
        buttons = new Button[childCount];
        for (int i = 0; i < childCount; i++)
        {
            buttons[i] = levelButtons.transform.GetChild(i).gameObject.GetComponent<Button>();
        }
    }

    
    public static void OpenLevel(int levelId)
    {
        ButtonManager.Id = levelId;
        if (levelId <= 2)
        {
            AddButtons.counter = 4;
        } 
        else if (levelId <= 4)
        {
            AddButtons.counter = 6;
        } 
        else if (levelId <= 6)
        {
            AddButtons.counter = 8;
        }
        else if (levelId <= 8)
        {
            AddButtons.counter = 10;
        } 
        else if (levelId <= 10)
        {
            AddButtons.counter = 12;
        }
        else if (levelId <= 12)
        {
            AddButtons.counter = 14;
        } 
        else if (levelId <= 14)
        {
            AddButtons.counter = 16;
        }
        else if (levelId <= 17)
        {
            AddButtons.counter = 18;
        } 
        else if (levelId <= 20)
        {
            AddButtons.counter = 20;
        }

        SceneManager.LoadScene("MemoryGame");
    }
}
