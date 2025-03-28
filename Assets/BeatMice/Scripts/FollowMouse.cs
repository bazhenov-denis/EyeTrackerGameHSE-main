using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    private Vector3 _mousePosition;
    public float moveSpeed = 0.1f; // Скорость перемещения

    private void Update()
    {
        _mousePosition = Input.mousePosition;
        _mousePosition = Camera.main.ScreenToWorldPoint(_mousePosition);
        transform.position = Vector2.Lerp(transform.position, _mousePosition, moveSpeed);
    }
}