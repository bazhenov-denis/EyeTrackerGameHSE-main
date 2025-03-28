using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManagerCosmonavt : MonoBehaviour
{
    [SerializeField] public GameObject Game;
    public GameObject DefeatMenu;
    public GameObject WinnerMenu;
    public void PlayGame()
    {
        EnemyBehaviour.counter = 0;
        WinnerMenu.SetActive(false);
        if (DifficultyLevelMenuManager.difficult == 4)
        {
            SceneManager.LoadScene("TrainingMenu");
        }
        DifficultyLevelMenuManager.difficult += 1;
        DifficultyLevelMenuManager.ChoiseDifficult();
        Game.SetActive(true);
        
    }
    public void RestartGame()
    {
        EnemyBehaviour.counter = 0;
        DefeatMenu.SetActive(false);
        DifficultyLevelMenuManager.ChoiseDifficult();
        Game.SetActive(true);
        
    }
    
    public void GameMenu()
    {
        SceneManager.LoadScene("TrainingMenu");
    }

}
