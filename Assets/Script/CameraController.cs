using UnityEngine;

/// <summary>
/// Makes the camera follow the player's forward movement without moving sideways.
/// </summary>
public class CameraController : MonoBehaviour
{
    [Tooltip("The player character to follow.")]
    public Transform player;

    private Vector3 offset;

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("Player transform not assigned to CameraController. Please assign it in the Inspector.");
            this.enabled = false;
            return;
        }
        // Calculate the initial offset from the player.
        offset = transform.position - player.position;
    }

    void LateUpdate()
    {
        if (player == null) return;

        // Create a new position for the camera.
        // It uses the player's Z position but keeps its own X and Y based on the initial offset.
        Vector3 newPosition = transform.position;
        newPosition.z = player.position.z + offset.z;

        // Apply the new position to the camera.
        transform.position = newPosition;
    }
}
