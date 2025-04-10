using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class RegisterUI : MonoBehaviour
{
  [SerializeField] private TMP_InputField usernameField;
  [SerializeField] private TMP_InputField passwordField;
  [SerializeField] private TMP_InputField confirmPasswordField;
  [SerializeField] private TextMeshProUGUI resultText;


  [SerializeField] private Color successColor = new Color(0f, 0.5f, 0f, 1f);
  [SerializeField] private Color errorColor = Color.red;

  public void OnRegisterButtonClick()
  {
    string username = usernameField.text.Trim();
    string password = passwordField.text;
    string confirmPassword = confirmPasswordField.text;

    if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
    {
      resultText.text = "Поля не должны быть пустыми!";
      resultText.color = errorColor;
      return;
    }

    if (password != confirmPassword)
    {
      resultText.text = "Пароли не совпадают!";
      resultText.color = errorColor;
      return;
    }

    bool success = LocalDatabase.Instance.RegisterUser(username, password);
    if (success)
    {
      resultText.text = "Профиль создан!";
      resultText.color = successColor;
      SceneManager.LoadScene("ProfileScene");
    }
    else
    {
      resultText.text = "Пользователь уже существует!";
      resultText.color = errorColor;
    }
  }
}