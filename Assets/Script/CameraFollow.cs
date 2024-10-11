using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;        // Transform của nhân vật
    public float smoothSpeed = 0.125f;
    public Vector3 offset;          // Offset cho camera
    public float yThreshold = 1.5f; // Khoảng cách Y trước khi camera di chuyển theo

    // Giới hạn cho camera di chuyển
    public Vector2 minBoundary;     // Giới hạn tối thiểu (minX, minY)
    public Vector2 maxBoundary;     // Giới hạn tối đa (maxX, maxY)

    private void LateUpdate()
    {
        Vector3 desiredPosition;

        // Kiểm tra xem nhân vật có di chuyển qua ngưỡng Y không
        if (Mathf.Abs(transform.position.y - player.position.y) > yThreshold)
        {
            // Vị trí mục tiêu của camera
            desiredPosition = new Vector3(player.position.x + offset.x, player.position.y + offset.y, transform.position.z);
        }
        else
        {
            desiredPosition = new Vector3(player.position.x + offset.x, transform.position.y, transform.position.z);
        }

        // Giới hạn vị trí của camera trong các giá trị min/max
        float limitedX = Mathf.Clamp(desiredPosition.x, minBoundary.x, maxBoundary.x);
        float limitedY = Mathf.Clamp(desiredPosition.y, minBoundary.y, maxBoundary.y);

        // Vị trí mới của camera sau khi áp dụng giới hạn
        Vector3 limitedPosition = new Vector3(limitedX, limitedY, desiredPosition.z);

        // Áp dụng smooth để di chuyển camera một cách mượt mà
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, limitedPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
