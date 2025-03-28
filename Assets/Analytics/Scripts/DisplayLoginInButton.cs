using UnityEngine;
using UnityEngine.UI;      // ��� Button
using TMPro;               // ��� TextMeshProUGUI

public class DisplayLoginInButton : MonoBehaviour
{
    [SerializeField] private Button targetButton;

    void Start()
    {
        if (SessionManager.LogIn)
        {
            // �������� ��������� TextMeshProUGUI ������ ������
            TextMeshProUGUI textComponent = targetButton.GetComponentInChildren<TextMeshProUGUI>();
            if (textComponent != null)
            {
                textComponent.text = SessionManager.LoggedInUsername;
            }
        }
        else
        {
            // ����� ������� "�����" ��� "�� �����"
            TextMeshProUGUI textComponent = targetButton.GetComponentInChildren<TextMeshProUGUI>();
            if (textComponent != null)
            {
                textComponent.text = "���";
            }
        }
    }
}
