using UnityEngine;
using UnityEngine.SceneManagement;

public class LogoutButton : MonoBehaviour
{
    // Вызывается при нажатии на кнопку "Выйти"
    public void OnLogoutButtonClick()
    {
        if (SessionManager.LogIn)
        {
            // Сбрасываем состояние
            SessionManager.LogOut();

            Debug.Log("Пользователь вышел из профиля");

            SceneManager.LoadScene("ProfileScene");
        }
    }
}