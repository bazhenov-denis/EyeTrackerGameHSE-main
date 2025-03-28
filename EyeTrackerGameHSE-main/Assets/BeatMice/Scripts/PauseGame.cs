
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    private MouseGenerator _mouseGenerator;

    private void Start()
    {
        _mouseGenerator = GetComponent<MouseGenerator>();
    }

    public void StopGame()
    {
        // _mouseGenerator.Pause();
        foreach (var miceController in GetComponentsInChildren<MiceController>())
        {
            // miceController.StopGame();
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void EndGame()
    {
        // _mouseGenerator.Pause();
        foreach (var miceController in GetComponentsInChildren<MiceController>())
        {
            // miceController.EndGame();
        }
    }

    public void ResumeGame()
    {
        // _mouseGenerator.Resume();
        foreach (var miceController in GetComponentsInChildren<MiceController>())
        {
            // miceController.ResumeGame();
        }
    }
}