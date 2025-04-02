using UnityEngine;
using UnityEngine.UI;      // для Button
using TMPro;               // для TextMeshProUGUI

public class DisplayLoginInButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI userName;

    void Start()
    {
        if (SessionManager.LogIn)
            userName.text = SessionManager.LoggedInUsername;
        else
            userName.text = "Имя";
    }
}
