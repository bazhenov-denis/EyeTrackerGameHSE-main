using UnityEngine;
using TMPro;

public class DisplayLoginStatus : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statusText; // �����, ��� ������� ���������

    void Start()
    {
        if (SessionManager.LogIn)
        {
            // ���� ������������ �����, ���������� ��� ���
            statusText.text = "���� ��������!";
            statusText.color = Color.green;
        }
        else
        {
            // ���� �� �����, ����������, ��� ���� �� ��������
            statusText.text = "���� �� ��������!";
            statusText.color = Color.red;
        }
    }
}