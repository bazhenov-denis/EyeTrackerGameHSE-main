using System;
using UnityEngine;
using UnityEngine.SceneManagement;


public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager Instance;

    public int difficultyLevel = 1; // Уровень сложности
    public float spawnInterval = 4f; // Интервал между появлением мышей
    public int gameTime = 60; // Сколько длится игра
    public float hummerHitInterval = 0.9f;
    public float waitTime = 5f;

    private void Awake()
    {
        if (Instance == null && (SceneManager.GetActiveScene().name == "BeatMiceDifficultyManager" ||
                                 SceneManager.GetActiveScene().name == "BeatMiceMainScene"))
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Time.timeScale = 1f;
    }

    public void BackButton()
    {
        SceneManager.LoadScene("TrainingMenu");
    }
    
    private void OnEnable()
    {
        // Сброс времени и установка состояния игры
        Time.timeScale = 1f;
    }
}