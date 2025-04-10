using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    public Text scoreText;
    public static int score;
    public GameObject LosePanel;
    public GameObject Game;
    public GameObject Explosion;
    private GameObject e;
    public static int count = 10;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = score.ToString() + "/" + count;
    }

    private void OnTriggerEnter2D(Collider2D target)
    {
        if (target.tag == "fruits")
        {
            CatchObjectItem item = target.GetComponent<CatchObjectItem>();
            if (item != null)
            {
                float reactionTime = Time.time - item.spawnTime;
                CatchObjectProgressManager.RecordReactionTime(reactionTime);
            }
            score += 1;
            Destroy(target.gameObject);
            if (score == count)
            {
                LoseWinManager.mode = 1;
                SceneManager.LoadScene("LoseWinCatch");
            }
        }
        else if (target.tag == "enemy")
        {
            score -= 1;
            Destroy(target.gameObject);

            // ???????????? ?????? ??? ???????????? ? ??????
            CatchObjectProgressManager progressManager = FindObjectOfType<CatchObjectProgressManager>();
            if (progressManager != null)
            {
                progressManager.RegisterError();
            }

            if (score == -1)
            {
                LoseWinManager.mode = 0;
                SceneManager.LoadScene("LoseWinCatch");
            }
        }
        else if (target.tag == "bomb")
        {
            Destroy(target.gameObject);

            // ???????????? ?????? ??? ???????????? ? ??????
            CatchObjectProgressManager progressManager = FindObjectOfType<CatchObjectProgressManager>();
            if (progressManager != null)
            {
                progressManager.RegisterError();
            }

            LoseWinManager.mode = 0;
            SceneManager.LoadScene("LoseWinCatch");
        }
    }
}
