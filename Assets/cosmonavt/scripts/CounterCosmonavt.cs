using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class CounterCosmonavt : MonoBehaviour
{
    public static int WinCount = 10;
    public GameObject WinnerPanel;
    public GameObject Game;
    public Text textScore;
    void Update()
    {
        textScore.text = EnemyBehaviour.counter + "/" + WinCount.ToString();
        if (EnemyBehaviour.counter == WinCount)
        {
            WinnerPanel.SetActive(true);
            Game.SetActive(false);

            if (SessionManager.LogIn)
            {
                CosmonautProgressManager progressManager = FindObjectOfType<CosmonautProgressManager>();
                if (progressManager != null)
                {
                    progressManager.OnGameWin();
                }
            }
        }
    }
}
