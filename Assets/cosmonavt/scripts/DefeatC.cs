using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DefeatC : MonoBehaviour
{
    public GameObject DefeatPanel;

    public GameObject Game;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name != "Aim")
        {
            DefeatPanel.SetActive(true);
            Game.SetActive(false);

            CosmonautProgressManager progressManager = FindObjectOfType<CosmonautProgressManager>();
            if (progressManager != null)
            {
                progressManager.OnGameLose();
            }
        }
    }
}
