using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ProgressViewer : MonoBehaviour
{
    // UI-����, ���� ����� �������� ������
    [SerializeField] private TextMeshProUGUI gameNameText;
    [SerializeField] private TextMeshProUGUI dateText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI difficultyText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI mistakesText;
    [SerializeField] private TextMeshProUGUI accuracyText;
    [SerializeField] private TextMeshProUGUI ratingText;
    [SerializeField] private Button userName;

    // ������ "����������" � "���������"
    [SerializeField] private Button prevButton;
    [SerializeField] private Button nextButton;

    private List<GameHistory> _historyList;
    private int _currentIndex = 0;

    void Start()
    {
        // ���������� ������ �� ����
        var selectedGame = SessionManager.SelectedGame;
        int userId = SessionManager.UserID;

        _historyList = LocalDatabase.Instance.GetHistoryForGame(userId, selectedGame);

        // ���� ��� ������� - ������ ������� ��������� "��� �������" ��� ������ ������
        if (_historyList == null || _historyList.Count == 0)
        {
            ShowEmptyState(selectedGame);
            return;
        }

        // ���������� ������ ������
        _currentIndex = 0;
        ShowRecord(_historyList[_currentIndex]);

        // ������������� �� ������� ������
        prevButton.onClick.AddListener(OnPrevClicked);
        nextButton.onClick.AddListener(OnNextClicked);

        // � ����� Start() ��������� ��������� ������
        UpdateButtonStates();
    }

    void ShowRecord(GameHistory record)
    {
        // �������� ����
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

        // ���� (����������� ��� �������)
        dateText.text = record.DatePlayed.ToString("dd.MM.yyyy HH:mm");

        // ����
        scoreText.text = record.Score.ToString();

        // ������� ��������� (��� ������� ����� ��������� ��������� ��������)
        difficultyText.text = record.DifficultyLevel.ToString();

        timeText.text = record.TimeTaken.ToString("F1") + " sec";

        mistakesText.text = record.ErrorCount.ToString();

        accuracyText.text = record.CompletionPercentage.ToString("F1") + "%";

        // �������
        ratingText.text = record.PerformanceRating.ToString();

        TextMeshProUGUI textComponent = userName.GetComponentInChildren<TextMeshProUGUI>();
        textComponent.text = SessionManager.LoggedInUsername;
    }

    void ShowEmptyState(GameName selectedGame)
    {
        // ���� ������� ��������, ���� ��������� UI
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
        difficultyText.text = "��� �������";

        timeText.text = "��� �������";

        mistakesText.text = "��� �������";
        accuracyText.text = "��� �������";
        ratingText.text = "��� �������";

        TextMeshProUGUI textComponent = userName.GetComponentInChildren<TextMeshProUGUI>();
        textComponent.text = SessionManager.LoggedInUsername;

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
