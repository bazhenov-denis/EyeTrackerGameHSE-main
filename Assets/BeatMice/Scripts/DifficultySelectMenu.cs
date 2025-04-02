using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DifficultySelectMenu : MonoBehaviour
{
    public void SetEasyDifficulty()
    {
        DifficultyManager.Instance.difficultyLevel = 1;
        DifficultyManager.Instance.spawnInterval = 4f;
        DifficultyManager.Instance.gameTime = 15;
        DifficultyManager.Instance.hummerHitInterval = 0.9f;
        SceneManager.LoadScene("BeatMiceMainScene");
        // Другие параметры
    }
    
    public void SetMidleDifficulty()
    {
        DifficultyManager.Instance.difficultyLevel = 2;
        DifficultyManager.Instance.spawnInterval = 3f;
        DifficultyManager.Instance.gameTime = 70;
        DifficultyManager.Instance.hummerHitInterval = 0.8f;
        DifficultyManager.Instance.waitTime = 6f;
        SceneManager.LoadScene("BeatMiceMainScene");
        // Другие параметры
    }

    public void SetHardDifficulty()
    {
        DifficultyManager.Instance.difficultyLevel = 3;
        DifficultyManager.Instance.spawnInterval = 2f;
        DifficultyManager.Instance.gameTime = 80;
        DifficultyManager.Instance.hummerHitInterval = 0.7f;
        DifficultyManager.Instance.waitTime = 7.5f;
        SceneManager.LoadScene("BeatMiceMainScene");
        // Другие параметры
    }
    
    public void SetProDifficulty()
    {
        DifficultyManager.Instance.difficultyLevel = 4;
        DifficultyManager.Instance.spawnInterval = 1f;
        DifficultyManager.Instance.gameTime = 90;
        DifficultyManager.Instance.hummerHitInterval = 0.6f;
        DifficultyManager.Instance.waitTime = 10f;
        SceneManager.LoadScene("BeatMiceMainScene");
        // Другие параметры
    }
}
