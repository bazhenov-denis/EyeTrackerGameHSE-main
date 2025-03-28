using UnityEngine.UI;
using Tobii.Gaming;
using UnityEngine;

public class GazeUIRaycast : MonoBehaviour
{
    public Button targetButton;  // Ссылка на кнопку
    public Color focusColor = Color.green;
    public Color normalColor = Color.white;
    public float gazeTimeThreshold = 1.5f;

    private float gazeTimer = 0f;
    private bool isGazing = false;
    private Image buttonImage;

    void Start()
    {
        buttonImage = targetButton.GetComponent<Image>();
        buttonImage.color = normalColor;
    }

    void Update()
    {
        Vector2 gazePos = TobiiAPI.GetGazePoint().Screen;  // Получаем координаты взгляда на экране
        Ray ray = Camera.main.ScreenPointToRay(gazePos);   // Преобразуем в луч

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject == targetButton.gameObject)
            {
                gazeTimer += Time.deltaTime;
                buttonImage.color = Color.Lerp(buttonImage.color, focusColor, 0.1f);

                if (gazeTimer >= gazeTimeThreshold)
                {
                    targetButton.onClick.Invoke();  // Симуляция нажатия на кнопку
                    Debug.Log("Кнопка нажата взглядом!");
                    gazeTimer = 0f;  // Сброс таймера
                }
            }
        }
        else
        {
            gazeTimer = 0f;
            buttonImage.color = Color.Lerp(buttonImage.color, normalColor, 0.1f);
        }
    }
}
