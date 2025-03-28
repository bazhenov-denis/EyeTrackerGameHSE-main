using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SessionManager
{
    // Поле для хранения имени пользователя, который вошёл
    public static string LoggedInUsername;
    public static bool LogIn;
    public static int UserID;
    public static GameName SelectedGame;
    public static bool Progress;
    public static bool Reabilitation;
    public static MetricName MetricName;

    // Можно добавить и другие поля, например:
    // public static bool IsAdmin;

    public static void LogOut()
    {
        LogIn = false;
        LoggedInUsername = string.Empty;
        UserID = 0;
    }
}
