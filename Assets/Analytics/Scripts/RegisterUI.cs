using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class RegisterUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField usernameField;
    [SerializeField] private TMP_InputField passwordField;
    [SerializeField] private TMP_InputField confirmPasswordField;
    [SerializeField] private TextMeshProUGUI resultText;


    [SerializeField] private Color successColor = Color.green;
    [SerializeField] private Color errorColor = Color.red;

    // ���������� ��� ������� �� ������ "�����������"
    public void OnRegisterButtonClick()
    {
        string username = usernameField.text.Trim();
        string password = passwordField.text;
        string confirmPassword = confirmPasswordField.text;

        // ���������� ��������
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            resultText.text = "���� �� ������ ���� �������!";
            resultText.color = errorColor;
            return;
        }

        if (password != confirmPassword)
        {
            resultText.text = "������ �� ���������!";
            resultText.color = errorColor;
            return;
        }

        // ���������� �������� LocalDatabase.Instance
        bool success = LocalDatabase.Instance.RegisterUser(username, password);
        if (success)
        {
            resultText.text = "������� ������!";
            resultText.color = successColor;
            SceneManager.LoadScene("ProfileScene");
        }
        else
        {
            resultText.text = "������������ ��� ����������!";
            resultText.color = errorColor;
        }
    }
}
