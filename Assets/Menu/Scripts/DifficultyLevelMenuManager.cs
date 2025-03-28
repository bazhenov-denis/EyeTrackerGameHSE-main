using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DifficultyLevelMenuManager : MonoBehaviour
{
    public static int difficult = 1 ;

    public static void ChoiseDifficult()
    {
        switch (difficult)
        {
            case 1:
                Easy();
                break;
            case 2:
                Medium();
                break;
            case 3:
                Hard();
                break;
            case 4:
                Expert();
                break;
        }
    }

    public static void Easy()
    {
        difficult = 1; 
        CounterCosmonavt.WinCount = 10;
        SpawnerEnemy.nextSpawn = 2f;
        EnemyBehaviour.Scale = 1f;
        EnemyBehaviour.step = 0.005f;
        SceneManager.LoadScene("CosmonavtGame");
    }
    
    public static void Medium()
    {
        difficult = 2;
        CounterCosmonavt.WinCount = 15;
        SpawnerEnemy.nextSpawn = 1.7f;
        EnemyBehaviour.Scale = 0.8f;
        EnemyBehaviour.step = 0.006f;
        SceneManager.LoadScene("CosmonavtGame");
    }
    public static void Hard()
    {
        difficult = 3;
        CounterCosmonavt.WinCount = 20;
        SpawnerEnemy.nextSpawn = 1.3f;
        EnemyBehaviour.Scale = 0.7f;
        EnemyBehaviour.step = 0.007f;
        SceneManager.LoadScene("CosmonavtGame");
    }
    public static void Expert()
    {
        difficult = 4;
        CounterCosmonavt.WinCount = 25;
        SpawnerEnemy.nextSpawn = 0.9f;
        EnemyBehaviour.Scale = 0.6f;
        EnemyBehaviour.step = 0.008f;
        SceneManager.LoadScene("CosmonavtGame");
    }
}
