using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToBeatMice : MonoBehaviour
{
    public void GoToBeatMiceDifficulty()
    {
        SceneManager.LoadScene("BeatMiceDifficultyManager");
    }
}
