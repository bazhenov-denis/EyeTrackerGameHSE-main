using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

public class GameHistoryViewer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI gameNameText;
    [SerializeField] private TextMeshProUGUI dateText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI difficultyText;
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private TextMeshProUGUI userName;

    // ������ "����������" � "���������".
    [SerializeField] private Button prevButton;
    [SerializeField] private Button nextButton;

    private List<GameHistory> _historyList;
    private int _currentIndex = 0;

    void Start()
    {
        // ���������� ������ �� ����.
        var selectedGame = SessionManager.SelectedGame;
        int userId = SessionManager.UserID;

        _historyList = LocalDatabase.Instance.GetHistoryForGame(userId, selectedGame);

        if (_historyList == null || _historyList.Count == 0)
        {
            ShowEmptyState(selectedGame);
            return;
        }

        // ���������� ������ ������.
        _currentIndex = 0;
        ShowRecord(_historyList[_currentIndex]);

        // ������������� �� ������� ������.
        prevButton.onClick.AddListener(OnPrevClicked);
        nextButton.onClick.AddListener(OnNextClicked);

        UpdateButtonStates();
    }

    void ShowRecord(GameHistory record)
    {
        switch (record.Game)
        {
            case GameName.BeatMice:
                gameNameText.text = "��� �����";
                break;
            case GameName.�osmonaut:
                gameNameText.text = "���������";
                break;
            case GameName.Memory:
                gameNameText.text = "������";
                break;
            case GameName.CatchAllFruits:
                gameNameText.text = "������ ��� ������";
                break;
        }

        dateText.text = record.DatePlayed.ToString("dd.MM.yyyy HH:mm");

        scoreText.text = record.Score.ToString();

        difficultyText.text = record.DifficultyLevel.ToString();

        resultText.text = record.Victory ? "������" : "���������";

        userName.text = SessionManager.LoggedInUsername;
    }

    void ShowEmptyState(GameName selectedGame)
    {
        switch (selectedGame)
        {
            case GameName.BeatMice:
                gameNameText.text = "��� �����";
                break;
            case GameName.�osmonaut:
                gameNameText.text = "���������";
                break;
            case GameName.Memory:
                gameNameText.text = "������";
                break;
            case GameName.CatchAllFruits:
                gameNameText.text = "������ ��� ������";
                break;
        }
        dateText.text = "��� �������";
        scoreText.text = "��� �������";
        difficultyText.text = "��� �������";
        resultText.text = "��� �������";

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
    /// ��������/���������� ������ "����������" � "���������" � ����������� �� _currentIndex.
    /// </summary>
    private void UpdateButtonStates()
    {
        // ���� �� �� ������ ��������, ������ "����������" ��������.
        prevButton.gameObject.SetActive(_currentIndex > 0);

        // ���� �� �� ��������� ��������, ������ "���������" ��������.
        nextButton.gameObject.SetActive(_currentIndex < _historyList.Count - 1);
    }

    public void BackToHistoryMenu()
    {
        SceneManager.LoadScene("HistoryMenu");
    }
}
