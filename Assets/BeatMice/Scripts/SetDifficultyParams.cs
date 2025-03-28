using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SetDifficultyParams : MonoBehaviour
{
    [SerializeField] private MouseGenerator mouseGenerator;
    [SerializeField] private List<MiceController> miceControllers;
    [SerializeField] private TimerScript timer;

    void Start()
    {
        GetSetDifficultyParams();
    }

    public void GetSetDifficultyParams()
    {
        int difficultyLevel = DifficultyManager.Instance.difficultyLevel;
        float spawnInterval = DifficultyManager.Instance.spawnInterval;
        int gameTime = DifficultyManager.Instance.gameTime;
        float hummerHitInterval = DifficultyManager.Instance.hummerHitInterval;

        SetMiceSpawnInterval(spawnInterval);
        SetGameTime(gameTime);
        SetHummerHitInterval(hummerHitInterval);
    }

    void SetMiceSpawnInterval(float spawnInterval)
    {
        mouseGenerator.spawnInterval = spawnInterval;
    }

    void SetGameTime(int gameTime)
    {
        timer.totalTime = gameTime;
    }

    void SetHummerHitInterval(float hummerHitInterval)
    {
        miceControllers.ForEach(x => x.holdTime = hummerHitInterval);
    }
}