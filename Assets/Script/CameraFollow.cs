using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;  // The player's transform
    public float smoothSpeed = 0.125f;
    public Vector3 offset;  // Offset the camera from the player's position
    public float yThreshold = 1.5f;  // Distance from player before camera moves vertically

    private void LateUpdate()
    {
        Vector3 desiredPosition;// = new Vector3(transform.position.x, player.position.y + offset.y, -transform.position.z);
        // Check if the player has moved past the Y-axis threshold
        if (Mathf.Abs(transform.position.y - player.position.y) > yThreshold)
        {
            // Target position for the camera to move towards
            desiredPosition = new Vector3(player.position.x + offset.x, player.position.y + offset.y, transform.position.z);
        }
        else
        {
            desiredPosition = new Vector3(player.position.x+ offset.x, transform.position.y, transform.position.z);
        }
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

    }
}