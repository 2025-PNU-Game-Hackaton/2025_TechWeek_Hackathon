using UnityEngine;

/// <summary>
/// Controls the chaser character that follows the player.
/// </summary>
public class ChaserController : MonoBehaviour
{
    [Tooltip("The player character to follow.")]
    public Transform player;

    [Tooltip("How fast the chaser follows the player's horizontal and forward movement.")]
    public float followSpeed = 5f;

    [Tooltip("The distance the chaser tries to maintain behind the player.")]
    public float followDistance = 7f;

    void Update()
    {
        if (player == null)
        {
            // If the player is gone (e.g., game over), do nothing.
            return;
        }

        // Calculate the chaser's target position.
        // It should be a certain distance behind the player on the Z-axis,
        // and at the same X-position as the player.
        Vector3 targetPosition = new Vector3(player.position.x, transform.position.y, player.position.z - followDistance);

        // Smoothly move towards the target position using Lerp.
        // This creates a natural-looking follow movement.
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * followSpeed);
    }
}
