using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public static int Id;
    public void Next()
    {
        if (Id == 20)
        {
            back();
        }
        LevelMenuMemory.OpenLevel(Id + 1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
    }

    public void back()
    {
        SceneManager.LoadScene("MemoryMenu");
    }
}
