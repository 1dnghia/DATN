using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [Header("Components")]
    private Animator animator;
    private PlayerMovement playerMovement;
    private PlayerStats playerStats;
    
    [Header("Animation Parameters")]
    private readonly int isRunningHash = Animator.StringToHash("IsRunning");
    private readonly int hurtTriggerHash = Animator.StringToHash("HurtTrigger");
    private readonly int deathTriggerHash = Animator.StringToHash("DeathTrigger");
    private readonly int speedHash = Animator.StringToHash("Speed");
    
    [Header("Settings")]
    public float hurtAnimationDuration = 0.5f;
    
    private bool isDead = false;
    
    private void Awake()
    {
        // Try to get Animator component - first on same GameObject, then parent
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            animator = GetComponentInParent<Animator>();
        }
        
        // If still not found, try to add it to same GameObject
        if (animator == null)
        {
            Debug.LogWarning($"[PlayerAnimationController] No Animator found on {gameObject.name} or parent. Adding Animator component to this GameObject...");
            animator = gameObject.AddComponent<Animator>();
        }
        
        // Try to get components from same GameObject first (old structure)
        playerMovement = GetComponent<PlayerMovement>();
        playerStats = GetComponent<PlayerStats>();
        
        // If not found, try from parent hierarchy (new structure)
        if (playerMovement == null || playerStats == null)
        {
            // Method 1: Try direct parent PlayerController
            PlayerController controller = GetComponentInParent<PlayerController>();
            if (controller != null)
            {
                if (playerMovement == null)
                    playerMovement = controller.playerMovement;
                if (playerStats == null)
                    playerStats = controller.playerStats;
            }
            
            // Method 2: If still not found, search in parent hierarchy
            if (playerMovement == null)
                playerMovement = GetComponentInParent<PlayerMovement>();
            if (playerStats == null)
                playerStats = GetComponentInParent<PlayerStats>();
                
            // Method 3: If still not found, try siblings (other children of parent)
            if (playerMovement == null && transform.parent != null)
                playerMovement = transform.parent.GetComponentInChildren<PlayerMovement>();
            if (playerStats == null && transform.parent != null)
                playerStats = transform.parent.GetComponentInChildren<PlayerStats>();
        }
        
        #if UNITY_EDITOR
        // Debug log for structure detection in editor only
        bool isNewStructure = GetComponent<PlayerMovement>() == null;
        if (playerMovement == null || playerStats == null)
        {
            Debug.LogWarning($"[PlayerAnimationController] Missing components on {gameObject.name} - PlayerMovement: {playerMovement != null}, PlayerStats: {playerStats != null}");
        }
        #endif
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
    
    /// <summary>
    /// Check if animator parameter exists to avoid errors
    /// </summary>
    private bool HasParameter(string paramName)
    {
        if (animator == null || animator.runtimeAnimatorController == null) return false;
        
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.name == paramName) return true;
        }
        return false;
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
            
            // Set running animation only if parameter exists
            if (HasParameter("IsRunning"))
            {
                animator.SetBool(isRunningHash, isMoving);
            }
            
            // Set speed for blend trees if needed
            // animator.SetFloat(speedHash, playerMovement.GetCurrentSpeed());
        }
    }
    
    private void OnPlayerDamaged(float damage)
    {
        if (isDead || animator == null || animator.runtimeAnimatorController == null) return;
        
        // Check if HurtTrigger parameter exists before triggering
        if (HasParameter("HurtTrigger"))
        {
            animator.SetTrigger(hurtTriggerHash);
        }
        
        // Optional: Add screen shake or damage effect here
    }
    
    private void OnPlayerDeath()
    {
        isDead = true;
        
        // Trigger death animation
        if (animator.runtimeAnimatorController != null && HasParameter("DeathTrigger"))
        {
            animator.SetTrigger(deathTriggerHash);
        }
    }
    
    // Public methods for external triggers
    public void TriggerLevelUpAnimation()
    {
        // Can be called from PlayerExperience when leveling up
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
            return;
        }
        OnPlayerDamaged(10f);
    }
    
    [ContextMenu("Test Death Animation")]
    private void TestDeathAnimation()
    {
        if (animator.runtimeAnimatorController == null)
        {
            return;
        }
        OnPlayerDeath();
    }
}
