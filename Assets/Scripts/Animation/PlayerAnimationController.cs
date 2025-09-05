using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [Header("Components")]
    private Animator animator;
    private PlayerMovement playerMovement;
    private PlayerStats playerStats;
    
    [Header("Animation Parameters")]
    private readonly int isWalkingHash = Animator.StringToHash("IsWalking");
    private readonly int hurtTriggerHash = Animator.StringToHash("HurtTrigger");
    private readonly int deathTriggerHash = Animator.StringToHash("DeathTrigger");
    private readonly int speedHash = Animator.StringToHash("Speed");
    
    [Header("Settings")]
    public float hurtAnimationDuration = 0.5f;
    
    private bool isDead = false;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        playerStats = GetComponent<PlayerStats>();
    }
    
    private void Start()
    {
        // Subscribe to player events
        if (playerStats != null)
        {
            PlayerStats.OnPlayerDeath += OnPlayerDeath;
            // Subscribe to damage event from EventManager
            EventManager.OnPlayerDamaged += OnPlayerDamaged;
        }
        
        // Check if AnimatorController is assigned
        CheckAnimatorController();
    }
    
    private void CheckAnimatorController()
    {
        if (animator != null && animator.runtimeAnimatorController == null)
        {
            // Only show warning in editor or development builds
            #if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.LogWarning($"[{gameObject.name}] PlayerAnimationController: No AnimatorController assigned to Animator component. Animation features will be disabled until an AnimatorController is assigned.", this);
            #endif
        }
    }
    
    private void OnDestroy()
    {
        // Unsubscribe from events
        PlayerStats.OnPlayerDeath -= OnPlayerDeath;
        EventManager.OnPlayerDamaged -= OnPlayerDamaged;
    }
    
    private void Update()
    {
        if (isDead || animator == null || animator.runtimeAnimatorController == null) return;
        
        UpdateMovementAnimation();
    }
    
    private void UpdateMovementAnimation()
    {
        if (playerMovement != null && animator.runtimeAnimatorController != null)
        {
            bool isMoving = playerMovement.IsMoving();
            float speed = playerMovement.GetCurrentSpeed();
            
            // Set walking animation
            animator.SetBool(isWalkingHash, isMoving);
            
            // Set speed for blend trees (if using)
            animator.SetFloat(speedHash, speed);
        }
    }
    
    private void OnPlayerDamaged(float damage)
    {
        if (isDead || animator.runtimeAnimatorController == null) return;
        
        // Trigger hurt animation
        animator.SetTrigger(hurtTriggerHash);
        
        // Optional: Add screen shake or damage effect here
        Debug.Log($"Player hurt animation triggered! Damage: {damage:F1}");
    }
    
    private void OnPlayerDeath()
    {
        isDead = true;
        
        // Trigger death animation
        if (animator.runtimeAnimatorController != null)
        {
            animator.SetTrigger(deathTriggerHash);
        }
        
        Debug.Log("Player death animation triggered!");
    }
    
    // Public methods for external triggers
    public void TriggerLevelUpAnimation()
    {
        // Can be called from PlayerExperience when leveling up
        Debug.Log("Level up animation triggered!");
        // Add level up animation logic here
    }
    
    // Reset method for Unity Inspector
    private void Reset()
    {
        // Auto-assign animator if not set
        if (animator == null)
            animator = GetComponent<Animator>();
    }
    
    // Debug methods
    [ContextMenu("Test Hurt Animation")]
    private void TestHurtAnimation()
    {
        if (animator.runtimeAnimatorController == null)
        {
            Debug.LogWarning("Cannot test hurt animation: No AnimatorController assigned!");
            return;
        }
        OnPlayerDamaged(10f);
    }
    
    [ContextMenu("Test Death Animation")]
    private void TestDeathAnimation()
    {
        if (animator.runtimeAnimatorController == null)
        {
            Debug.LogWarning("Cannot test death animation: No AnimatorController assigned!");
            return;
        }
        OnPlayerDeath();
    }
}
