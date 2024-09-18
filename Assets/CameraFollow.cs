using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Reference to the player’s transform
    public float smoothSpeed = 0.125f; // Smoothness of the camera movement
    public Vector3 offset; // Offset from the player's position

    private void Start()
    {
        // Initialize the offset based on the initial positions
        offset = new Vector3(transform.position.x - player.position.x,
                             transform.position.y - player.position.y,
                             -21); // Fixed Z position
    }

    private void LateUpdate()
    {
        if (player != null)
        {
            Vector3 desiredPosition = player.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, desiredPosition.z); // Ensure Z is fixed at -11
        }
    }
}