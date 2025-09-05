using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float acceleration = 10f;
    public float deceleration = 10f;
    
    [Header("Components")]
    private Rigidbody2D rb;
    private PlayerStats playerStats;
    
    // Input
    private Vector2 moveInput;
    private Vector2 currentVelocity;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerStats = GetComponent<PlayerStats>();
    }
    
    private void Reset()
    {
        // Called when Reset is pressed in Unity Inspector
        // Set default values
        moveSpeed = 5f;
        acceleration = 10f;
        deceleration = 10f;
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
        // Get final move speed with multipliers
        float finalMoveSpeed = moveSpeed;
        if (playerStats != null)
        {
            finalMoveSpeed *= playerStats.moveSpeedMultiplier;
        }
        
        // Target velocity
        Vector2 targetVelocity = moveInput * finalMoveSpeed;
        
        // Smooth acceleration/deceleration
        float accelRate = (moveInput.magnitude > 0) ? acceleration : deceleration;
        
        currentVelocity = Vector2.MoveTowards(
            currentVelocity, 
            targetVelocity, 
            accelRate * Time.fixedDeltaTime
        );
        
        // Apply movement
        rb.linearVelocity = currentVelocity;
    }
    
    private void FlipSprite()
    {
        // Flip sprite based on movement direction
        if (moveInput.x > 0.1f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (moveInput.x < -0.1f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }
    
    // Public getters
    public Vector2 GetMoveInput() => moveInput;
    public Vector2 GetMoveDirection() => moveInput;
    public float GetCurrentSpeed() => currentVelocity.magnitude;
    public bool IsMoving() => moveInput.magnitude > 0.1f;
    public float GetFinalMoveSpeed() 
    { 
        float finalSpeed = moveSpeed;
        if (playerStats != null)
            finalSpeed *= playerStats.moveSpeedMultiplier;
        return finalSpeed;
    }
}
