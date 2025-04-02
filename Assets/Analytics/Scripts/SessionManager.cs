using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SessionManager
{
    public static string LoggedInUsername;
    public static bool LogIn;
    public static int UserID;
    public static GameName SelectedGame;
    public static bool Progress;
    public static bool Reabilitation;
    public static MetricName MetricName;

    public static void LogOut()
    {
        LogIn = false;
        LoggedInUsername = string.Empty;
        UserID = 0;
    }
}
