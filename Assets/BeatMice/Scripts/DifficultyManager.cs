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

    public void BackButton()
    {
        SceneManager.LoadScene("TrainingMenu");
    }
}