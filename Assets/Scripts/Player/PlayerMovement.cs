using UnityEngine;

/// <summary>
/// Player Movement - Vampire Survivors Style
/// - Instant acceleration/deceleration (no smoothing)
/// - Snappy, responsive controls
/// - 8-directional movement with diagonal normalization
/// - Mobile touch support
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    
    [Header("Components")]
    private Rigidbody2D rb;
    private PlayerStats playerStats;
    
    // Input
    private Vector2 moveInput;
    
    private void Awake()
    {
        // Try to find Rigidbody2D on same GameObject first (old structure)
        rb = GetComponent<Rigidbody2D>();
        
        // If not found, try to find on parent (new structure)
        if (rb == null)
        {
            rb = GetComponentInParent<Rigidbody2D>();
        }
        
        // Try to find PlayerStats on same GameObject first (old structure)
        playerStats = GetComponent<PlayerStats>();
        
        // If not found, try to find in siblings or parent hierarchy (new structure)
        if (playerStats == null)
        {
            // First try parent
            playerStats = GetComponentInParent<PlayerStats>();
            // If still not found, try in children of parent (sibling components)
            if (playerStats == null && transform.parent != null)
            {
                playerStats = transform.parent.GetComponentInChildren<PlayerStats>();
            }
        }
    }
    
    private void Reset()
    {
        // Called when Reset is pressed in Unity Inspector
        // Set default values
        moveSpeed = 5f;
    }
    
    private void Update()
    {
        GetInput();
        FlipSprite();
    }
    
    private void FixedUpdate()
    {
        Move();
    }
    
    private void GetInput()
    {
        // Check if game is active (not paused/game over)
        if (GameManager.Instance != null && !GameManager.Instance.CanPlayerMove())
        {
            moveInput = Vector2.zero;
            return;
        }
        
        // PC Input (Primary) - Simple like Vampire Survivors
        moveInput.x = Input.GetAxis("Horizontal");  // A/D, Arrow Left/Right
        moveInput.y = Input.GetAxis("Vertical");    // W/S, Arrow Up/Down
        
        // Mobile Input (Override if touching)
        #if UNITY_ANDROID || UNITY_IOS
        if (TouchInput.MoveInput != Vector2.zero)
        {
            moveInput = TouchInput.MoveInput;
        }
        #endif
        
        // Normalize diagonal movement
        if (moveInput.magnitude > 1f)
            moveInput = moveInput.normalized;
    }
    
    private void Move()
    {
        // Safety check for Rigidbody2D
        if (rb == null)
        {
            return;
        }
        
        // Get final move speed with multipliers
        float finalMoveSpeed = moveSpeed;
        if (playerStats != null)
        {
            finalMoveSpeed *= playerStats.moveSpeedMultiplier;
        }
        
        // Direct velocity assignment - instant movement like Vampire Survivors
        // No acceleration/deceleration for snappy, responsive controls
        Vector2 targetVelocity = moveInput * finalMoveSpeed;
        rb.linearVelocity = targetVelocity;
    }
    
    private void FlipSprite()
    {
        // In new structure, we need to flip the PlayerSprite, not the PlayerMovement GameObject
        // Try to find PlayerSprite in siblings (under PlayerVisual)
        Transform playerSprite = null;
        
        // First try to find PlayerSprite directly
        if (transform.parent != null)
        {
            // Look for PlayerSprite in PlayerVisual (sibling)
            Transform playerVisual = transform.parent.Find("PlayerVisual");
            if (playerVisual != null)
            {
                playerSprite = playerVisual.Find("PlayerSprite");
            }
            
            // If not found, try direct search in parent's children
            if (playerSprite == null)
            {
                playerSprite = transform.parent.Find("PlayerSprite");
            }
        }
        
        // If still not found, try old structure (PlayerSprite as child of this object)
        if (playerSprite == null)
        {
            playerSprite = transform.Find("PlayerSprite");
        }
        
        // Apply flip to the correct transform
        Transform targetTransform = playerSprite != null ? playerSprite : transform;
        
        if (moveInput.x > 0.1f)
        {
            targetTransform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (moveInput.x < -0.1f)
        {
            targetTransform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }
    
    // Public getters
    public Vector2 GetMoveInput() => moveInput;
    public Vector2 GetMoveDirection() => moveInput;
    public float GetCurrentSpeed() => rb != null ? rb.linearVelocity.magnitude : 0f;
    public bool IsMoving() => moveInput.magnitude > 0.1f;
    public float GetFinalMoveSpeed() 
    { 
        float finalSpeed = moveSpeed;
        if (playerStats != null)
            finalSpeed *= playerStats.moveSpeedMultiplier;
        return finalSpeed;
    }
}
