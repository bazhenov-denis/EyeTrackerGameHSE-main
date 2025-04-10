using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuEvent : MonoBehaviour
{
    [SerializeField] private StateGame _stateGame;
    [SerializeField] private GameObject _pauseButton;

    private void OnEnable()
    {
        _pauseButton.SetActive(false);
        _stateGame.SetState(State.StopGame);
    }

    private void OnDisable()
    {
        if (!_pauseButton.IsUnityNull())
        {
            _pauseButton.SetActive(true);
            _stateGame.SetState(State.Gameplay);
        }
    }


    public void BackDifficultyMenu()
    {
        GameObject.Find("MouseHole").GetComponent<MiceController>().ResetScore();
        SceneManager.LoadScene("BeatMiceDifficultyManager");
        _stateGame.SetState(State.Gameplay);
    }

    public static void ResatartLevel()
    {
        GameObject.Find("MouseHole").GetComponent<MiceController>().ResetScore();
        SceneManager.LoadScene("BeatMiceMainScene");
        
    }
}