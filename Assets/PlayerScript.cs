using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    void Update()
    {
        // Lấy input từ bàn phím
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        // Tạo vector di chuyển
        Vector2 move = new Vector2(moveX, moveY) * moveSpeed * Time.deltaTime;

        // Cập nhật vị trí của nhân vật
        transform.Translate(move);
    }
}