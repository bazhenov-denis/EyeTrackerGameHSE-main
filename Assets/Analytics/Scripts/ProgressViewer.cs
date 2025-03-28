using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ProgressViewer : MonoBehaviour
{
    // UI-поля, куда будем выводить данные
    [SerializeField] private TextMeshProUGUI gameNameText;
    [SerializeField] private TextMeshProUGUI dateText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI difficultyText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI mistakesText;
    [SerializeField] private TextMeshProUGUI accuracyText;
    [SerializeField] private TextMeshProUGUI ratingText;
    [SerializeField] private Button userName;

    // Кнопки "предыдущая" и "следующая"
    [SerializeField] private Button prevButton;
    [SerializeField] private Button nextButton;

    private List<GameHistory> _historyList;
    private int _currentIndex = 0;

    void Start()
    {
        // Подгружаем записи из базы
        var selectedGame = SessionManager.SelectedGame;
        int userId = SessionManager.UserID;

        _historyList = LocalDatabase.Instance.GetHistoryForGame(userId, selectedGame);

        // Если нет записей - можете вывести сообщение "Нет истории" или скрыть кнопки
        if (_historyList == null || _historyList.Count == 0)
        {
            ShowEmptyState(selectedGame);
            return;
        }

        // Показываем первую запись
        _currentIndex = 0;
        ShowRecord(_historyList[_currentIndex]);

        // Подписываемся на события кнопок
        prevButton.onClick.AddListener(OnPrevClicked);
        nextButton.onClick.AddListener(OnNextClicked);

        // В конце Start() обновляем состояние кнопок
        UpdateButtonStates();
    }

    void ShowRecord(GameHistory record)
    {
        // Название игры
        switch (record.Game)
        {
            case GameName.BeatMice:
                gameNameText.text = "Бей мышей";
                break;
            case GameName.Сosmonaut:
                gameNameText.text = "Космонавт";
                break;
            case GameName.Memory:
                gameNameText.text = "Память";
                break;
            case GameName.CatchAllFruits:
                gameNameText.text = "Собери все фрукты";
                break;
        }

        // Дата (форматируем при желании)
        dateText.text = record.DatePlayed.ToString("dd.MM.yyyy HH:mm");

        // Счёт
        scoreText.text = record.Score.ToString();

        // Уровень сложности (при желании можно придумать текстовые названия)
        difficultyText.text = record.DifficultyLevel.ToString();

        timeText.text = record.TimeTaken.ToString("F1") + " sec";

        mistakesText.text = record.ErrorCount.ToString();

        accuracyText.text = record.CompletionPercentage.ToString("F1") + "%";

        // Рейтинг
        ratingText.text = record.PerformanceRating.ToString();

        TextMeshProUGUI textComponent = userName.GetComponentInChildren<TextMeshProUGUI>();
        textComponent.text = SessionManager.LoggedInUsername;
    }

    void ShowEmptyState(GameName selectedGame)
    {
        // Либо вывести заглушку, либо отключить UI
        switch (selectedGame)
        {
            case GameName.BeatMice:
                gameNameText.text = "Бей мышей";
                break;
            case GameName.Сosmonaut:
                gameNameText.text = "Космонавт";
                break;
            case GameName.Memory:
                gameNameText.text = "Память";
                break;
            case GameName.CatchAllFruits:
                gameNameText.text = "Собери все фрукты";
                break;
        }
        dateText.text = "Нет записей";
        scoreText.text = "Нет записей";
        difficultyText.text = "Нет записей";
        difficultyText.text = "Нет записей";

        timeText.text = "Нет записей";

        mistakesText.text = "Нет записей";
        accuracyText.text = "Нет записей";
        ratingText.text = "Нет записей";

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
    /// Скрывает/показывает кнопки "предыдущая" и "следующая" в зависимости от _currentIndex.
    /// </summary>
    private void UpdateButtonStates()
    {
        // Если мы на первом элементе, кнопку "предыдущая" скрываем.
        prevButton.gameObject.SetActive(_currentIndex > 0);

        // Если мы на последнем элементе, кнопку "следующая" скрываем.
        nextButton.gameObject.SetActive(_currentIndex < _historyList.Count - 1);
    }

    public void BackToHistoryMenu()
    {
        SceneManager.LoadScene("HistoryMenu");
    }
}
