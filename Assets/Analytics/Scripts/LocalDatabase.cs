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
            DontDestroyOnLoad(gameObject); // ��������� ������ ��� �������� ����� �����.
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // 1. ���� � �����, � ������� ����� Assets (����� ������� ����, ��� Assets).
        string projectFolder = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));

        // 2. ���� � ����� "SqLiteData" (�� ��� �� ������, ��� � Assets).
        string targetFolder = Path.Combine(projectFolder, "SqLiteData");

        // �� ������ ������ �������� �����, ���� � ���.
        if (!Directory.Exists(targetFolder))
        {
            Directory.CreateDirectory(targetFolder);
        }

        // 3. ���� � �����.
        string dbPath = Path.Combine(targetFolder, "mydatabase.db");
        Debug.Log("���� � ����� � ����� ������: " + dbPath);
        // ������ (��� ���������) �� � ������� ������/������ � ��������, ���� ����� ���.
        db = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
        // ������ ������� "users", ���� ��� �� ����������.
        Debug.Log($"������� ��� ������� ��!");
        CreateDB();
    }

    /// <summary>
    /// ������ (��� ���������) ������� "users", ���� ��� �� ����������.
    /// </summary>
    private void CreateDB()
    {
        db.CreateTable<User>();
        db.CreateTable<GameHistory>();
    }

    /// <summary>
    /// ������������ ������ ������������ (��������� � �������).
    /// ���������� true, ���� ����������� �������, false � ���� ������������ ��� ����������.
    /// </summary>
    public bool RegisterUser(string username, string password)
    {
        // ���������, ���� �� ��� ������������ � ����� ������.
        if (IsUserExists(username))
        {
            Debug.LogWarning("������������ � ����� ������ ��� ����������!");
            return false;
        }

        // ���������� ���� ��� ������� ������������.
        string salt = PasswordHasher.GenerateSalt();
        // ��������� ��� ������ � �����.
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

        Debug.Log($"������ ������������ � ������: {username}");

        db.Insert(newUser);
        return true;
    }

    /// <summary>
    /// ���������, ���������� �� ������������ � ��������� ������.
    /// </summary>
    private bool IsUserExists(string username)
    {
        // �������� ���������� ������������� � ������ ������.
        var count = db.Table<User>().Where(u => u.Username == username).Count();
        return count > 0;
    }

    public bool LoginUser(string username, string password)
    {
        // ������� ������������ �� �����.
        var user = db.Table<User>().FirstOrDefault(u => u.Username == username);
        if (user == null)
        {
            Debug.Log("������������ �� ������");
            return false;
        }

        // ��������� ��� ���������� ������ � �������������� ���� �� ����.
        string enteredHash = PasswordHasher.ComputeHash(password, user.Salt);
        if (user.PasswordHash == enteredHash)
        {
            SessionManager.LoggedInUsername = username;
            SessionManager.LogIn = true;
            SessionManager.UserID = user.Id;
            Debug.Log($"���� ��������, ��� ������������: {username}");
            return true;
        }

        Debug.Log("�������� ������!");
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
        // �������� �� ������� GameHistory ��� ������ ��� ������� ������������ � ����.
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
/// ����� ������ ��� ������������. SQLite4Unity3d ���������� �������� ��� �������� �������.
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

    // ������ �� ������������, ������� ������ ����.
    public int UserId { get; set; }

    public GameName Game { get; set; }

    // ���� �����������.
    public DateTime DatePlayed { get; set; }

    // ��������� ����.
    public int Score { get; set; }

    // ������� ���������.
    public int DifficultyLevel { get; set; }

    // ��������� ����: ������ ��� ���������.
    public bool Victory { get; set; }

    // ���� ��� ������������ ���������.
    public double TimeTaken { get; set; }            // ����� ����������� � ��������.
    public double CompletionPercentage { get; set; }   // ������� ���������� (��������, �� 0 �� 100).
    public int ErrorCount { get; set; }                // ���������� ������ ��� �����������.
    public int PerformanceRating { get; set; }         // ������� ��� ������ ���������� (��������, �� 1 �� 10).
    public double AverageReactionTime { get; set; }
}
