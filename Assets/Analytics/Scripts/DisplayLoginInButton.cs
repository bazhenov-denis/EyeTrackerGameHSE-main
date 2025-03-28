using UnityEngine;
using UnityEngine.UI;      // для Button
using TMPro;               // для TextMeshProUGUI

public class DisplayLoginInButton : MonoBehaviour
{
    [SerializeField] private Button targetButton;

    void Start()
    {
        if (SessionManager.LogIn)
        {
            // Получаем компонент TextMeshProUGUI внутри кнопки
            TextMeshProUGUI textComponent = targetButton.GetComponentInChildren<TextMeshProUGUI>();
            if (textComponent != null)
            {
                textComponent.text = SessionManager.LoggedInUsername;
            }
        }
        else
        {
            // Можно вывести "Гость" или "Не вошёл"
            TextMeshProUGUI textComponent = targetButton.GetComponentInChildren<TextMeshProUGUI>();
            if (textComponent != null)
            {
                textComponent.text = "Имя";
            }
        }
    }
}
