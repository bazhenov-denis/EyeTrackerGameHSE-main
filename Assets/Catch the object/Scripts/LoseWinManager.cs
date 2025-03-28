using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseWinManager : MonoBehaviour
{
    // Start is called before the first frame update
    // 0 Ц поражение, 1 Ц победа
    public static int mode;
    public GameObject winPanel;
    public GameObject losePanel;

    public void Start()
    {
        if (SessionManager.LogIn)
        {
            CatchObjectProgressManager progressManager = FindObjectOfType<CatchObjectProgressManager>();
            // —читаем, что mode == 1 означает победу, иначе поражение
            bool victory = (mode == 1);
            progressManager.OnGameEnd(victory);
        }
        MenuActivated();
    }

    void MenuActivated()
    {
        switch (mode)
        {
            case 0:
                {
                    losePanel.SetActive(true);
                    break;
                }
            case 1:
                {
                    winPanel.SetActive(true);
                    break;
                }
        }
    }

    private void DifficultManager(int difficult)
    {
        switch (difficult)
        {
            case 1:
                {
                    DifficultCatchObject.Easy();
                    break;
                }
            case 2:
                {
                    DifficultCatchObject.Medium();
                    break;
                }
            case 3:
                {
                    DifficultCatchObject.Hard();
                    break;
                }
            case 4:
                {
                    DifficultCatchObject.Expert();
                    break;
                }

        }
    }
    public void Next()
    {
        if (DifficultCatchObject.difficult == 4)
        {
            SceneManager.LoadScene("FocusMenu");
        }
        ScoreController.score = 0;
        DifficultManager(DifficultCatchObject.difficult + 1);
    }

    public void Repeat()
    {
        ScoreController.score = 0;
        DifficultManager(DifficultCatchObject.difficult);
    }

    public void Menu()
    {
        ScoreController.score = 0;
        SceneManager.LoadScene("FocusMenu");
    }
}
