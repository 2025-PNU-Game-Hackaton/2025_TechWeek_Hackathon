using UnityEngine;
using System.Collections;

/// <summary>
/// Controls the player character's movement, actions, and game state.
/// </summary>
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Animator animator;

    [Header("Movement")]
    public float forwardSpeed = 12f;
    public float laneChangeSpeed = 15f;
    private Vector3 moveDirection;

    [Header("Lane System")]
    private int currentLane = 1; // 0 = Left, 1 = Middle, 2 = Right
    public float laneDistance = 3.5f;

    [Header("Jumping & Sliding")]
    public float jumpForce = 10.0f;
    public float gravity = 25.0f;
    public float slideDuration = 1.0f;
    private float verticalVelocity;
    private bool isSliding = false;
    private float originalControllerHeight;
    private Vector3 originalControllerCenter;

    [Header("Game Over")]
    private int stumbleCount = 0;
    private bool isStumbling = false; // Flag to prevent multiple stumbles at once
    public float fallThresholdY = -5f; // Y position to trigger game over
    private bool isDead = false;

    [Header("Status")]
    public bool isInvincible = false;
    public bool isMagnet = false;


    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        // Store original CharacterController dimensions for sliding
        originalControllerHeight = controller.height;
        originalControllerCenter = controller.center;
    }

    void Update()
    {
        if (isDead) return;

        // Check for game over conditions
        if (transform.position.y < fallThresholdY)
        {
            GameOver("Fell off the path");
            return;
        }

        HandleMovement();
        HandleActions();
    }

    /// <summary>
    /// Handles the core forward, lane-switching, and vertical movement.
    /// </summary>
    private void HandleMovement()
    {
        // Base movement vector for forward motion
        Vector3 moveVector = new Vector3(0, 0, forwardSpeed);

        // --- Lane Switching ---
        float targetX = (currentLane - 1) * laneDistance;
        float xDiff = targetX - transform.position.x;
        float horizontalSpeed = xDiff * laneChangeSpeed;
        moveVector.x = horizontalSpeed;

        // --- Vertical Movement (Gravity & Jump) ---
        if (controller.isGrounded)
        {
            // Check for jump input while grounded and not sliding
            if (!isSliding && (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)))
            {
                Jump();
            }
            else
            {
                // Apply a small downward force to keep the character stuck to the ground
                verticalVelocity = -gravity * Time.deltaTime;
            }
        }
        else
        {
            // Apply gravity while in the air
            verticalVelocity -= gravity * Time.deltaTime;
        }
        moveVector.y = verticalVelocity;

        // Apply the final calculated movement vector
        controller.Move(moveVector * Time.deltaTime);
    }

    /// <summary>
    /// Handles player inputs for actions like lane changes and sliding.
    /// </summary>
    private void HandleActions()
    {
        // NOTE: This uses the old Input Manager for simplicity.
        // For a more robust system, consider using the new Input System package.

        // Lane changing input: Hold to move, release to return to center.
        // Only allow lane changes while grounded.
        if (controller.isGrounded && !isSliding)
        {
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                currentLane = 0; // Target left lane
            }
            else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                currentLane = 2; // Target right lane
            }
            else
            {
                currentLane = 1; // Target center lane
            }
        }

        // Slide input
        if (controller.isGrounded && !isSliding && (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)))
        {
            StartCoroutine(Slide());
        }
    }

    /// <summary>
    /// Makes the player jump.
    /// </summary>
    private void Jump()
    {
        verticalVelocity = jumpForce;
        animator.SetTrigger("Jump");
    }

    /// <summary>
    /// Coroutine to handle the sliding action.
    /// </summary>
    private IEnumerator Slide()
    {
        isSliding = true;
        animator.SetBool("isSliding", true);

        // Shrink the CharacterController's collider
        controller.height = originalControllerHeight / 2;
        controller.center = new Vector3(originalControllerCenter.x, originalControllerCenter.y / 2, originalControllerCenter.z);

        yield return new WaitForSeconds(slideDuration);

        // Restore the collider to its original size
        controller.height = originalControllerHeight;
        controller.center = originalControllerCenter;

        isSliding = false;
        animator.SetBool("isSliding", false);
    }

    /// <summary>
    /// Called when the player stumbles on a small obstacle.
    /// </summary>
    public void Stumble()
    {
        // Prevent stumbling again if already in a stumble/recovery state.
        if (isDead || isStumbling) return;

        // Start a short cooldown to prevent the same obstacle from triggering multiple hits.
        StartCoroutine(StumbleCooldown());

        stumbleCount++;
        animator.SetTrigger("Stumble");

        if (stumbleCount >= 2)
        {
            GameOver("Stumbled twice");
        }
        else
        {
            // Apply a permanent slowdown.
            ApplyPermanentSlowdown(0.75f); // Reduce speed to 75%
        }
    }

    /// <summary>
    /// Applies a permanent speed reduction to the player.
    /// </summary>
    private void ApplyPermanentSlowdown(float speedMultiplier)
    {
        forwardSpeed *= speedMultiplier;
    }

    /// <summary>
    /// Coroutine to provide a brief period of immunity after stumbling.
    /// </summary>
    private IEnumerator StumbleCooldown()
    {
        isStumbling = true;
        yield return new WaitForSeconds(0.5f); // Cooldown duration
        isStumbling = false;
    }

    /// <summary>
    /// Triggers the game over sequence.
    /// </summary>
    /// <param name="reason">The reason for the game over.</param>
    private void GameOver(string reason)
    {
        if (isDead) return;

        isDead = true;
        Debug.Log($"Game Over: {reason}");
        animator.SetTrigger("Death");
        forwardSpeed = 0; // Stop all movement
        laneChangeSpeed = 0;
        
        // Optionally, disable the script after a delay to let the death animation play
        // Invoke(nameof(DisableScript), 2f);
    }

    private void DisableScript()
    {
        this.enabled = false;
    }

    /// <summary>
    /// Called by the CharacterController when it hits a collider while performing a Move.
    /// </summary>
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (isDead) return; // Don't process collisions if already dead

        if (isInvincible)
        {
            // 무적인 경우 그냥 지나감
            BoxCollider collider;
            if(hit.gameObject.TryGetComponent(out collider)) 
            {
                collider.isTrigger = true;
                return;
            }
        }

        // Check the tag of the object we collided with
        string tag = hit.gameObject.tag;

        if (tag == "Obstacle_Hard")
        {
            GameOver("Hit a hard obstacle");
        }
        else if (tag == "Obstacle_Soft")
        {
            Stumble();
            // Destroy the obstacle immediately to prevent hitting it multiple times
            Destroy(hit.gameObject);
        }
        else if (tag == "Chaser")
        {
            GameOver("Caught by the chaser");
        }
    }
}