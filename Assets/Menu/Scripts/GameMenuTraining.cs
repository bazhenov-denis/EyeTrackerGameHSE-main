using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenuTraining : MonoBehaviour
{
    // Start is called before the first frame update

    public void Back()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void MiceButton()
    {
        SceneManager.LoadScene("BeatMiceDifficultyManager");
    }
}
