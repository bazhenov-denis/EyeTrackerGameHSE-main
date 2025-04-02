using UnityEngine;
using UnityEngine.UI;      // ��� Button
using TMPro;               // ��� TextMeshProUGUI

public class DisplayLoginInButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI userName;

    void Start()
    {
        if (SessionManager.LogIn)
            userName.text = SessionManager.LoggedInUsername;
        else
            userName.text = "���";
    }
}
