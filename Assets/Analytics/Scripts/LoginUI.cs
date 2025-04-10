using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LoginUI : MonoBehaviour
{
  [SerializeField] private TMP_InputField usernameField;
  [SerializeField] private TMP_InputField passwordField;
  [SerializeField] private TextMeshProUGUI resultText;

  public void OnLoginButtonClick()
  {
    string username = usernameField.text.Trim();
    string password = passwordField.text;

    if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
    {
      resultText.text = "Введите имя и пароль!";
      resultText.color = Color.red;
      return;
    }

    bool success = LocalDatabase.Instance.LoginUser(username, password);
    if (success)
    {
      resultText.text = "Вход выполнен!";
      resultText.color = new Color(0f, 0.5f, 0f, 1f);

      SceneManager.LoadScene("ProfileScene");
    }
    else
    {
      resultText.text = "Неверный логин или пароль!";
      resultText.color = Color.red;
    }
  }
}