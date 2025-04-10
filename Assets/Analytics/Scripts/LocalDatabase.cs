using UnityEngine;
using SQLite4Unity3d;
using System.IO;
using System.Linq;
using System;
using System.Collections.Generic;

public class LocalDatabase : MonoBehaviour
{
    private SQLiteConnection db;
    public static LocalDatabase Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Сохраняем объект при загрузке новой сцены.
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // 1. Путь к папке, в которой лежит Assets (одним уровнем выше, чем Assets).
        string projectFolder = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));

        // 2. Путь к папке "SqLiteData" (на том же уровне, что и Assets).
        string targetFolder = Path.Combine(projectFolder, "SqLiteData");

        // На всякий случай создадим папку, если её нет.
        if (!Directory.Exists(targetFolder))
        {
            Directory.CreateDirectory(targetFolder);
        }

        // 3. Путь к файлу.
        string dbPath = Path.Combine(targetFolder, "mydatabase.db");
        Debug.Log("Путь к файлу с базой данных: " + dbPath);
        // Создаём (или открываем) БД с флагами чтения/записи и создания, если файла нет.
        db = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
        // Создаём таблицу "users", если она не существует.
        Debug.Log($"Открыта или создана бд!");
        CreateDB();
    }

    /// <summary>
    /// Создаёт (или открывает) таблицу "users", если она не существует.
    /// </summary>
    private void CreateDB()
    {
        db.CreateTable<User>();
        db.CreateTable<GameHistory>();
    }

    /// <summary>
    /// Регистрирует нового пользователя (сохраняет в таблицу).
    /// Возвращает true, если регистрация успешна, false — если пользователь уже существует.
    /// </summary>
    public bool RegisterUser(string username, string password)
    {
        // Проверяем, есть ли уже пользователь с таким именем.
        if (IsUserExists(username))
        {
            Debug.LogWarning("Пользователь с таким именем уже существует!");
            return false;
        }

        // Генерируем соль для данного пользователя.
        string salt = PasswordHasher.GenerateSalt();
        // Вычисляем хэш пароля с солью.
        string passwordHash = PasswordHasher.ComputeHash(password, salt);

        User newUser = new User
        {
            Username = username,
            PasswordHash = passwordHash,
            Salt = salt
        };

        SessionManager.UserID = newUser.Id;
        SessionManager.LoggedInUsername = username;
        SessionManager.LogIn = true;

        Debug.Log($"Создан пользователь с именем: {username}");

        db.Insert(newUser);
        return true;
    }

    /// <summary>
    /// Проверяет, существует ли пользователь с указанным именем.
    /// </summary>
    private bool IsUserExists(string username)
    {
        // Получаем количество пользователей с данным именем.
        var count = db.Table<User>().Where(u => u.Username == username).Count();
        return count > 0;
    }

    public bool LoginUser(string username, string password)
    {
        // Находим пользователя по имени.
        var user = db.Table<User>().FirstOrDefault(u => u.Username == username);
        if (user == null)
        {
            Debug.Log("Пользователь не найден");
            return false;
        }

        // Вычисляем хэш введенного пароля с использованием соли из базы.
        string enteredHash = PasswordHasher.ComputeHash(password, user.Salt);
        if (user.PasswordHash == enteredHash)
        {
            SessionManager.LoggedInUsername = username;
            SessionManager.LogIn = true;
            SessionManager.UserID = user.Id;
            Debug.Log($"Вход выполнен, имя пользователя: {username}");
            return true;
        }

        Debug.Log("Неверный пароль!");
        return false;
    }
    
    public void AddGameHistory(int userId, GameName game, int score, int difficulty, bool victory,
        double timeTaken, double completionPercentage, int errorCount,
        int performanceRating, double averageReactionTime)
    {
        GameHistory history = new GameHistory
        {
            UserId = userId,
            Game = game,
            DatePlayed = System.DateTime.Now,
            Score = score,
            DifficultyLevel = difficulty,
            Victory = victory,
            TimeTaken = timeTaken,
            CompletionPercentage = completionPercentage,
            ErrorCount = errorCount,
            PerformanceRating = performanceRating,
            AverageReactionTime = averageReactionTime
        };
        db.Insert(history);
    }

    public List<GameHistory> GetHistoryForGame(int userId, GameName game)
    {
        // Выбираем из таблицы GameHistory все записи для данного пользователя и игры.
        return db.Table<GameHistory>()
                 .Where(h => h.UserId == userId && h.Game == game)
                 .OrderByDescending(h => h.DatePlayed)
                 .ToList();
    }

    public List<GameHistory> GetAllHistoryForUser(int userId)
    {
        return db.Table<GameHistory>()
                 .Where(h => h.UserId == userId)
                 .OrderByDescending(h => h.DatePlayed)
                 .ToList();
    }
}

/// <summary>
/// Класс модели для пользователя. SQLite4Unity3d использует атрибуты для создания таблицы.
/// </summary>
public class User
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public string Salt { get; set; }
}

public class GameHistory
{
    [PrimaryKey, AutoIncrement] public int Id { get; set; }

    // Ссылка на пользователя, который прошёл игру.
    public int UserId { get; set; }

    public GameName Game { get; set; }

    // Дата прохождения.
    public DateTime DatePlayed { get; set; }

    // Набранный счёт.
    public int Score { get; set; }

    // Уровень сложности.
    public int DifficultyLevel { get; set; }

    // Результат игры: победа или поражение.
    public bool Victory { get; set; }

    // Поля для отслеживания прогресса.
    public double TimeTaken { get; set; }            // Время прохождения в секундах.
    public double CompletionPercentage { get; set; }   // Процент выполнения (например, от 0 до 100).
    public int ErrorCount { get; set; }                // Количество ошибок или неточностей.
    public int PerformanceRating { get; set; }         // Рейтинг или оценка выполнения (например, от 1 до 10).
    public double AverageReactionTime { get; set; }
}