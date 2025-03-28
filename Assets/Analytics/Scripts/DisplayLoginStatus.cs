using UnityEngine;
using TMPro;

public class DisplayLoginStatus : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statusText; // Текст, где покажем результат

    void Start()
    {
        if (SessionManager.LogIn)
        {
            // Если пользователь вошёл, показываем его имя
            statusText.text = "Вход выполнен!";
            statusText.color = Color.green;
        }
        else
        {
            // Если не вошёл, показываем, что вход не выполнен
            statusText.text = "Вход не выполнен!";
            statusText.color = Color.red;
        }
    }
}