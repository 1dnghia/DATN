using UnityEngine;

/// <summary>
/// Enemy Controller - Main AI and behavior controller
/// - Manages enemy states (Idle, Chasing, Attacking, Dead)
/// - Handles player detection and combat
/// - Coordinates between EnemyStats and EnemyMovement
/// </summary>
public class EnemyController : MonoBehaviour
{
    [Header("References")]
    public EnemyData enemyData;
    
    // Components
    private EnemyStats enemyStats;
    private EnemyMovement enemyMovement;
    private Collider2D enemyCollider;
    private SpriteRenderer spriteRenderer;
    
    // AI State
    public enum EnemyState
    {
        Idle,
        Chasing,
        Attacking,
        Dead
    }
    
    private EnemyState currentState = EnemyState.Idle;
    private Transform player;
    private PlayerStats playerStats;
    private float lastAttackTime;
    private bool hasInitialized = false;
    
    // Properties
    public EnemyState CurrentState => currentState;
    public bool IsAlive => enemyStats != null && !enemyStats.IsDead;
    public float DistanceToPlayer => player != null ? Vector3.Distance(transform.position, player.position) : float.MaxValue;
    
    private void Awake()
    {
        // Get components
        enemyStats = GetComponent<EnemyStats>();
        enemyMovement = GetComponent<EnemyMovement>();
        enemyCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        // Find player
        PlayerController playerController = FindFirstObjectByType<PlayerController>();
        if (playerController != null)
        {
            player = playerController.transform;
            playerStats = playerController.playerStats;
        }
        
        #if UNITY_EDITOR
        #if UNITY_EDITOR
        // Component validation removed for cleaner console output
        #endif
        #endif
    }
    
    private void Start()
    {
        Initialize();
    }
    
    private void Initialize()
    {
        if (hasInitialized) return;
        
        // Load data from ScriptableObject
        if (enemyData != null)
        {
            LoadFromData(enemyData);
        }
        
        // Subscribe to events
        if (enemyStats != null)
        {
            enemyStats.OnTakeDamage += OnTakeDamage;
            EnemyStats.OnEnemyDeath += OnEnemyDeath;
        }
        
        // Set initial state - always start chasing player (Vampire Survivors style)
        currentState = EnemyState.Chasing;
        
        hasInitialized = true;
        Debug.Log($"Enemy {gameObject.name} initialized with {enemyStats?.MaxHealth:F0} HP");
    }
    
    private void Update()
    {
        if (!IsAlive || !hasInitialized) return;
        
        UpdateAI();
        UpdateState();
    }
    
    private void UpdateAI()
    {
        if (player == null) return;
        
        float distanceToPlayer = DistanceToPlayer;
        
        // Vampire Survivors style: Always chase player, only stop to attack
        switch (currentState)
        {
            case EnemyState.Chasing:
                if (distanceToPlayer <= enemyStats.GetAttackRange())
                {
                    SetState(EnemyState.Attacking);
                }
                break;
                
            case EnemyState.Attacking:
                if (distanceToPlayer > enemyStats.GetAttackRange() * 1.2f) // Hysteresis
                {
                    SetState(EnemyState.Chasing);
                }
                else
                {
                    TryAttack();
                }
                break;
                
            // No Idle state needed - always chase or attack
        }
    }
    
    private void UpdateState()
    {
        switch (currentState)
        {
            case EnemyState.Chasing:
                // Movement is handled by EnemyMovement automatically
                break;
                
            case EnemyState.Attacking:
                // Stop movement when attacking
                if (enemyMovement != null)
                {
                    enemyMovement.StopMovement();
                }
                break;
                
            case EnemyState.Dead:
                // Stop all actions
                if (enemyMovement != null)
                {
                    enemyMovement.StopMovement();
                }
                if (enemyCollider != null)
                {
                    enemyCollider.enabled = false;
                }
                break;
        }
    }
    
    private void TryAttack()
    {
        if (Time.time < lastAttackTime + enemyStats.GetAttackCooldown()) return;
        if (playerStats == null) return;
        
        lastAttackTime = Time.time;
        
        // Perform attack
        float damage = enemyStats != null ? enemyStats.Damage : 10f;
        playerStats.TakeDamage(damage);
        
        // Attack animation/effects would go here
        Debug.Log($"Enemy {gameObject.name} attacks player for {damage:F1} damage!");
    }
    
    private void SetState(EnemyState newState)
    {
        if (currentState == newState) return;
        
        EnemyState previousState = currentState;
        currentState = newState;
        
        Debug.Log($"Enemy {gameObject.name} state: {previousState} → {newState}");
        
        // State entry actions
        switch (newState)
        {
            case EnemyState.Chasing:
                // Start chasing player
                break;
                
            case EnemyState.Attacking:
                // Prepare for attack
                break;
                
            case EnemyState.Dead:
                OnDeath();
                break;
        }
    }
    
    private void OnTakeDamage()
    {
        // Visual feedback for taking damage
        if (spriteRenderer != null)
        {
            StartCoroutine(FlashRed());
        }
    }
    
    private void OnEnemyDeath(EnemyStats deadEnemy)
    {
        if (deadEnemy == enemyStats)
        {
            SetState(EnemyState.Dead);
        }
    }
    
    private void OnDeath()
    {
        Debug.Log($"Enemy {gameObject.name} is dead!");
        
        // Death effects would go here
        // Object will be destroyed by EnemyStats
    }
    
    private System.Collections.IEnumerator FlashRed()
    {
        if (spriteRenderer == null) yield break;
        
        Color originalColor = spriteRenderer.color;
        spriteRenderer.color = Color.red;
        
        yield return new WaitForSeconds(0.1f);
        
        spriteRenderer.color = originalColor;
    }
    
    public void LoadFromData(EnemyData data)
    {
        if (data == null) return;
        
        enemyData = data;
        
        // Load stats to EnemyStats component (it will handle combat data)
        if (enemyStats != null)
        {
            enemyStats.LoadFromData(data);
        }
    }
    
    // Public methods for external control
    public void SetTarget(Transform newTarget)
    {
        player = newTarget;
        if (enemyMovement != null)
        {
            enemyMovement.SetTarget(newTarget);
        }
    }
    
    public void ForceAttack()
    {
        if (IsAlive)
        {
            TryAttack();
        }
    }
    
    public void ForceState(EnemyState state)
    {
        SetState(state);
    }
    
    // Collision detection for player contact
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsAlive) return;
        
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            // Damage player on contact
            TryAttack();
        }
    }
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (!IsAlive) return;
        
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            // Continuous damage while touching
            TryAttack();
        }
    }
    
    private void OnDestroy()
    {
        // Unsubscribe from events
        if (enemyStats != null)
        {
            enemyStats.OnTakeDamage -= OnTakeDamage;
            EnemyStats.OnEnemyDeath -= OnEnemyDeath;
        }
    }
    
    // Reset method for Unity Inspector
    private void Reset()
    {
        // Combat values are handled by EnemyStats component
    }
    
    // Debug methods
    [ContextMenu("Test Attack")]
    private void TestAttack()
    {
        ForceAttack();
    }
    
    [ContextMenu("Test Death")]
    private void TestDeath()
    {
        if (enemyStats != null)
        {
            enemyStats.TakeDamage(enemyStats.CurrentHealth + 100f);
        }
    }
    
    // Debug visualization
    private void OnDrawGizmosSelected()
    {
        // Draw attack range
        Gizmos.color = Color.red;
        float attackRange = enemyStats != null ? enemyStats.GetAttackRange() : 1f;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        
        // Draw line to player
        if (player != null && Application.isPlaying)
        {
            Gizmos.color = currentState == EnemyState.Chasing ? Color.green : Color.gray;
            Gizmos.DrawLine(transform.position, player.position);
        }
    }
}
