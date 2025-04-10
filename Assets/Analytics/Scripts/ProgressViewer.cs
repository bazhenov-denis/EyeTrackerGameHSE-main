using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ProgressViewer : MonoBehaviour
{
  [SerializeField] private TextMeshProUGUI gameNameText;
  [SerializeField] private TextMeshProUGUI dateText;
  [SerializeField] private TextMeshProUGUI scoreText;
  [SerializeField] private TextMeshProUGUI difficultyText;
  [SerializeField] private TextMeshProUGUI timeText;
  [SerializeField] private TextMeshProUGUI mistakesText;
  [SerializeField] private TextMeshProUGUI accuracyText;
  [SerializeField] private TextMeshProUGUI ratingText;
  [SerializeField] private TextMeshProUGUI userName;

  // пїЅпїЅпїЅпїЅпїЅпїЅ "пїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅ" пїЅ "пїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅ".
  [SerializeField] private Button prevButton;
  [SerializeField] private Button nextButton;

  private List<GameHistory> _historyList;
  private int _currentIndex = 0;

  void Start()
  {
    // пїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅ пїЅпїЅпїЅпїЅпїЅпїЅ пїЅпїЅ пїЅпїЅпїЅпїЅ.
    var selectedGame = SessionManager.SelectedGame;
    int userId = SessionManager.UserID;

    _historyList = LocalDatabase.Instance.GetHistoryForGame(userId, selectedGame);

    if (_historyList == null || _historyList.Count == 0)
    {
      ShowEmptyState(selectedGame);
      return;
    }

    // пїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅ пїЅпїЅпїЅпїЅпїЅпїЅ пїЅпїЅпїЅпїЅпїЅпїЅ.
    _currentIndex = 0;
    ShowRecord(_historyList[_currentIndex]);

    // пїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅ пїЅпїЅ пїЅпїЅпїЅпїЅпїЅпїЅпїЅ пїЅпїЅпїЅпїЅпїЅпїЅ.
    prevButton.onClick.AddListener(OnPrevClicked);
    nextButton.onClick.AddListener(OnNextClicked);

    UpdateButtonStates();
  }

  void ShowRecord(GameHistory record)
  {
    switch (record.Game)
    {
      case GameName.BeatMice:
        gameNameText.text = "пїЅпїЅпїЅ пїЅпїЅпїЅпїЅпїЅ";
        break;
      case GameName.Cosmonaut:
        gameNameText.text = "пїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅ";
        break;
      case GameName.Memory:
        gameNameText.text = "пїЅпїЅпїЅпїЅпїЅпїЅ";
        break;
      case GameName.CatchAllFruits:
        gameNameText.text = "пїЅпїЅпїЅпїЅпїЅпїЅ пїЅпїЅпїЅ пїЅпїЅпїЅпїЅпїЅпїЅ";
        break;
    }

    dateText.text = record.DatePlayed.ToString("dd.MM.yyyy HH:mm");

    scoreText.text = record.Score.ToString();

    difficultyText.text = record.DifficultyLevel.ToString();

    timeText.text = record.TimeTaken.ToString("F1") + " sec";

    mistakesText.text = record.ErrorCount.ToString();

    accuracyText.text = record.CompletionPercentage.ToString("F1") + "%";

    ratingText.text = record.PerformanceRating.ToString();

    userName.text = SessionManager.LoggedInUsername;
  }

  void ShowEmptyState(GameName selectedGame)
  {
    switch (selectedGame)
    {
      case GameName.BeatMice:
        gameNameText.text = "пїЅпїЅпїЅ пїЅпїЅпїЅпїЅпїЅ";
        break;
      case GameName.Cosmonaut:
        gameNameText.text = "пїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅ";
        break;
      case GameName.Memory:
        gameNameText.text = "пїЅпїЅпїЅпїЅпїЅпїЅ";
        break;
      case GameName.CatchAllFruits:
        gameNameText.text = "пїЅпїЅпїЅпїЅпїЅпїЅ пїЅпїЅпїЅ пїЅпїЅпїЅпїЅпїЅпїЅ";
        break;
    }

    dateText.text = "пїЅпїЅпїЅ пїЅпїЅпїЅпїЅпїЅпїЅпїЅ";
    scoreText.text = "пїЅпїЅпїЅ пїЅпїЅпїЅпїЅпїЅпїЅпїЅ";
    difficultyText.text = "пїЅпїЅпїЅ пїЅпїЅпїЅпїЅпїЅпїЅпїЅ";
    difficultyText.text = "пїЅпїЅпїЅ пїЅпїЅпїЅпїЅпїЅпїЅпїЅ";

    timeText.text = "пїЅпїЅпїЅ пїЅпїЅпїЅпїЅпїЅпїЅпїЅ";

    mistakesText.text = "пїЅпїЅпїЅ пїЅпїЅпїЅпїЅпїЅпїЅпїЅ";
    accuracyText.text = "пїЅпїЅпїЅ пїЅпїЅпїЅпїЅпїЅпїЅпїЅ";
    ratingText.text = "пїЅпїЅпїЅ пїЅпїЅпїЅпїЅпїЅпїЅпїЅ";

    userName.text = SessionManager.LoggedInUsername;

    prevButton.gameObject.SetActive(false);
    nextButton.gameObject.SetActive(false);
  }

  void OnPrevClicked()
  {
    if (_currentIndex > 0)
    {
      _currentIndex--;
      ShowRecord(_historyList[_currentIndex]);
      UpdateButtonStates();
    }
  }

  void OnNextClicked()
  {
    if (_currentIndex < _historyList.Count - 1)
    {
      _currentIndex++;
      ShowRecord(_historyList[_currentIndex]);
      UpdateButtonStates();
    }
  }

  /// <summary>
  /// пїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅ/пїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅ пїЅпїЅпїЅпїЅпїЅпїЅ "пїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅ" пїЅ "пїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅ" пїЅ пїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅ пїЅпїЅ _currentIndex.
  /// </summary>
  private void UpdateButtonStates()
  {
    // пїЅпїЅпїЅпїЅ пїЅпїЅ пїЅпїЅ пїЅпїЅпїЅпїЅпїЅпїЅ пїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅ, пїЅпїЅпїЅпїЅпїЅпїЅ "пїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅ" пїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅ.
    prevButton.gameObject.SetActive(_currentIndex > 0);

    // пїЅпїЅпїЅпїЅ пїЅпїЅ пїЅпїЅ пїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅ пїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅ, пїЅпїЅпїЅпїЅпїЅпїЅ "пїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅ" пїЅпїЅпїЅпїЅпїЅпїЅпїЅпїЅ.
    nextButton.gameObject.SetActive(_currentIndex < _historyList.Count - 1);
  }

  public void BackToHistoryMenu()
  {
    SceneManager.LoadScene("HistoryMenu");
  }
}