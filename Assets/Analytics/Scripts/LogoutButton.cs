using UnityEngine;
using UnityEngine.SceneManagement;

public class LogoutButton : MonoBehaviour
{
  public void OnLogoutButtonClick()
  {
    if (SessionManager.LogIn)
    {
      SessionManager.LogOut();

      Debug.Log("Пользователь вышел из профиля");

      SceneManager.LoadScene("ProfileScene");
    }
  }
}