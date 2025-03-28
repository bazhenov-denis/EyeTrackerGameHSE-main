using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChartManager : MonoBehaviour
{
    public void BackToChartMenu()
    {
        SceneManager.LoadScene("ChartMenu");
    }
}
