using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // The player's transform
    public Vector2 minXAndY; // Minimum x and y coordinates the camera can move to
    public Vector2 maxXAndY; // Maximum x and y coordinates the camera can move to
    public float smoothTime = 10f; // Smooth time for the camera movement

    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        // Define a target position above and behind the target transform
        Vector3 targetPosition = player.position;

        // Clamp the target position within the defined limits
        targetPosition.x = Mathf.Clamp(targetPosition.x, minXAndY.x, maxXAndY.x);
        targetPosition.y = Mathf.Clamp(targetPosition.y, minXAndY.y, maxXAndY.y);
        targetPosition.z = transform.position.z;

        // Smoothly move the camera towards the target position
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}

