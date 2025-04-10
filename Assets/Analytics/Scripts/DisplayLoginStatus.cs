using UnityEngine;
using TMPro;

public class DisplayLoginStatus : MonoBehaviour
{
  [SerializeField] private TextMeshProUGUI statusText;

  void Start()
  {
    if (SessionManager.LogIn)
    {
      statusText.text = "Вход выполнен!";
      statusText.color = new Color(0f, 0.5f, 0f, 1f);
    }
    else
    {
      statusText.text = "Вход не выполнен!";
      statusText.color = Color.red;
    }
  }
}