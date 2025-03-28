using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogInManager : MonoBehaviour
{
    public void BackToProfileScene()
    {
        SceneManager.LoadScene("ProfileScene");
    }
}
