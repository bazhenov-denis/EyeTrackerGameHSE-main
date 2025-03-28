using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Destroyer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D target)
    {
        if (target.tag == "fruits")
        {
            ScoreController.score -= 1;
            Destroy(target.gameObject);
            if (ScoreController.score == -1)
            {
                LoseWinManager.mode = 1;
                SceneManager.LoadScene("LoseWinCatch");
            }
        }
        else if (target.tag == "enemy")
        {
            Destroy(target.gameObject);
            if (ScoreController.score == ScoreController.count)
            {
                LoseWinManager.mode = 0;
                SceneManager.LoadScene("LoseWinCatch");
            }
        }
        else if (target.tag == "bomb")
        {
            Destroy(target.gameObject);

        }
    }
}
