using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DifficultCatchObject : MonoBehaviour
{
    public static int difficult = 1;
    // Start is called before the first frame update

    public static void Easy()
    {
        difficult = 1;
        Spawner.nextSpawn = 2f;
        ScoreController.count = 10;
        Spawner.speed = 0.3f;
        Spawner.countBomb = 3;
        SceneManager.LoadScene("CatchObject");
    }
    public static void Medium()
    {
        difficult = 2;
        Spawner.nextSpawn = 1.7f;
        ScoreController.count = 15;
        Spawner.speed = 0.4f;
        Spawner.countBomb = 5;
        SceneManager.LoadScene("CatchObject");
    }
    public static void Hard()
    {
        difficult = 3;
        Spawner.nextSpawn = 1.5f;
        ScoreController.count = 20;
        Spawner.speed = 0.5f;
        Spawner.countBomb = 7;
        SceneManager.LoadScene("CatchObject");
    }
    public static void Expert()
    {
        difficult = 4;
        Spawner.nextSpawn = 1.2f;
        ScoreController.count = 25;
        Spawner.speed = 0.6f;
        Spawner.countBomb = 5;
        SceneManager.LoadScene("CatchObject");
    }
}
