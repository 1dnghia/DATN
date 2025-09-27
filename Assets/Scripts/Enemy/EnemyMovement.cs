using UnityEngine;

/// <summary>
/// Enemy Movement - Vampire Survivors Style
/// - Simple follow player behavior
/// - Collision avoidance with other enemies
/// - Stop when in attack range
/// </summary>
public class EnemyMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 2f;
    public float stoppingDistance = 0.8f; // Stop when close to player
    
    [Header("Collision Avoidance")]
    public float avoidanceRadius = 1f;
    public float avoidanceForce = 2f;
    public LayerMask enemyLayerMask = 1 << 6; // Layer 6 for enemies
    
    [Header("Rotation")]
    public bool faceMovementDirection = true;
    public float rotationSpeed = 5f;
    
    // Components
    private Rigidbody2D rb;
    private EnemyStats enemyStats;
    private Transform player;
    
    // Movement state
    private Vector2 moveDirection;
    private Vector2 avoidanceDirection;
    private bool hasReachedPlayer;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyStats = GetComponent<EnemyStats>();
        
        // Find player
        PlayerController playerController = FindFirstObjectByType<PlayerController>();
        if (playerController != null)
        {
            player = playerController.transform;
        }
        
        #if UNITY_EDITOR
        #if UNITY_EDITOR
        // Component validation removed for cleaner console output
        #endif
        #endif
    }
    
    private void Start()
    {
        // Movement speed will be set by EnemyStats.LoadFromData()
    }
    
    private void Update()
    {
        if (player == null || enemyStats == null || enemyStats.IsDead)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }
        
        CalculateMovement();
        HandleRotation();
    }
    
    private void FixedUpdate()
    {
        if (player == null || enemyStats == null || enemyStats.IsDead) return;
        
        ApplyMovement();
    }
    
    private void CalculateMovement()
    {
        // Calculate direction to player
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        
        // Check if we should stop (in attack range)
        hasReachedPlayer = distanceToPlayer <= stoppingDistance;
        
        if (hasReachedPlayer)
        {
            moveDirection = Vector2.zero;
        }
        else
        {
            moveDirection = directionToPlayer;
        }
        
        // Calculate collision avoidance
        CalculateAvoidance();
    }
    
    private void CalculateAvoidance()
    {
        avoidanceDirection = Vector2.zero;
        
        // Find nearby enemies
        Collider2D[] nearbyEnemies = Physics2D.OverlapCircleAll(
            transform.position, 
            avoidanceRadius, 
            enemyLayerMask
        );
        
        int avoidanceCount = 0;
        
        foreach (Collider2D enemyCollider in nearbyEnemies)
        {
            if (enemyCollider.transform == transform) continue; // Skip self
            
            Vector2 awayFromEnemy = (transform.position - enemyCollider.transform.position).normalized;
            float distance = Vector2.Distance(transform.position, enemyCollider.transform.position);
            
            // Stronger avoidance when closer
            float avoidanceStrength = Mathf.Clamp01(1f - (distance / avoidanceRadius));
            avoidanceDirection += awayFromEnemy * avoidanceStrength;
            avoidanceCount++;
        }
        
        if (avoidanceCount > 0)
        {
            avoidanceDirection /= avoidanceCount; // Average the avoidance vectors
            avoidanceDirection = avoidanceDirection.normalized;
        }
    }
    
    private void ApplyMovement()
    {
        // Combine movement and avoidance
        Vector2 finalDirection = moveDirection + (avoidanceDirection * avoidanceForce);
        
        // Apply movement
        Vector2 targetVelocity = finalDirection.normalized * moveSpeed;
        
        // Don't move if we've reached the player
        if (hasReachedPlayer)
        {
            targetVelocity = avoidanceDirection * avoidanceForce * 0.5f; // Only apply avoidance
        }
        
        rb.linearVelocity = targetVelocity;
    }
    
    private void HandleRotation()
    {
        if (!faceMovementDirection || rb.linearVelocity.magnitude < 0.1f) return;
        
        // Calculate rotation based on movement direction
        Vector2 movementDirection = rb.linearVelocity.normalized;
        float targetAngle = Mathf.Atan2(movementDirection.y, movementDirection.x) * Mathf.Rad2Deg;
        
        // Smooth rotation
        float currentAngle = transform.eulerAngles.z;
        float newAngle = Mathf.LerpAngle(currentAngle, targetAngle, rotationSpeed * Time.deltaTime);
        
        transform.rotation = Quaternion.Euler(0, 0, newAngle);
    }
    
    // Public methods for external access
    public bool IsMoving() => rb.linearVelocity.magnitude > 0.1f;
    public Vector2 GetMoveDirection() => moveDirection;
    public float GetCurrentSpeed() => rb.linearVelocity.magnitude;
    public bool HasReachedPlayer() => hasReachedPlayer;
    
    public void SetTarget(Transform newTarget)
    {
        player = newTarget;
    }
    
    public void SetMoveSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
    }
    
    // Stop movement (for death, stun, etc.)
    public void StopMovement()
    {
        rb.linearVelocity = Vector2.zero;
        moveDirection = Vector2.zero;
    }
    
    // Reset method for Unity Inspector
    private void Reset()
    {
        moveSpeed = 2f;
        stoppingDistance = 0.8f;
        avoidanceRadius = 1f;
        avoidanceForce = 2f;
        faceMovementDirection = true;
        rotationSpeed = 5f;
    }
    
    
    // Debug visualization
    private void OnDrawGizmosSelected()
    {
        // Draw stopping distance
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stoppingDistance);
        
        // Draw avoidance radius
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, avoidanceRadius);
        
        // Draw movement direction
        if (Application.isPlaying && moveDirection != Vector2.zero)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, moveDirection);
        }
        
        // Draw avoidance direction
        if (Application.isPlaying && avoidanceDirection != Vector2.zero)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, avoidanceDirection);
        }
    }
}
