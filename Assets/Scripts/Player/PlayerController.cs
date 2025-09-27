using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    public PlayerMovement playerMovement;
    public PlayerStats playerStats;
    public PlayerExperience playerExperience;
    public PlayerAnimationController playerAnimation;
    public LevelUpVFX levelUpVFX;
    
    // Simple pause handling
    public static System.Action OnPausePressed;
    
    private void Awake()
    {
        // Get components automatically
        AutoAssignComponents();
    }
    
    private void Reset()
    {
        // This method is called when you press "Reset" in Unity Inspector
        // It will automatically assign the required components
        AutoAssignComponents();
    }
    
    private void AutoAssignComponents()
    {
        // First try to get from same GameObject (old structure compatibility)
        if (playerMovement == null) 
        {
            playerMovement = GetComponent<PlayerMovement>();
            // If not found, try in children (new structure)
            if (playerMovement == null)
                playerMovement = GetComponentInChildren<PlayerMovement>();
        }
        
        if (playerStats == null)
        {
            playerStats = GetComponent<PlayerStats>();
            if (playerStats == null)
                playerStats = GetComponentInChildren<PlayerStats>();
        }
        
        if (playerExperience == null)
        {
            playerExperience = GetComponent<PlayerExperience>();
            if (playerExperience == null)
                playerExperience = GetComponentInChildren<PlayerExperience>();
        }
        
        if (playerAnimation == null)
        {
            playerAnimation = GetComponent<PlayerAnimationController>();
            if (playerAnimation == null)
                playerAnimation = GetComponentInChildren<PlayerAnimationController>();
        }
        
        if (levelUpVFX == null)
        {
            levelUpVFX = GetComponent<LevelUpVFX>();
            if (levelUpVFX == null)
                levelUpVFX = GetComponentInChildren<LevelUpVFX>();
        }
    }
    
    /// <summary>
    /// Public method to refresh component references after restructuring
    /// Can be called by PlayerRestructureHelper or other external scripts
    /// </summary>
    public void RefreshComponentReferences()
    {
        AutoAssignComponents();
    }
    
    private void Start()
    {
        // Subscribe to events
        PlayerStats.OnPlayerDeath += OnPlayerDeath;
        PlayerExperience.OnLevelUp += OnLevelUp;
    }
    
    private void OnDestroy()
    {
        // Unsubscribe from events
        PlayerStats.OnPlayerDeath -= OnPlayerDeath;
        PlayerExperience.OnLevelUp -= OnLevelUp;
    }
    
    private void Update()
    {
        // Simple pause - just like Vampire Survivors
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnPausePressed?.Invoke();
        }
        
        // Debug controls
        #if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.H))
        {
            playerStats.TakeDamage(10f); // Test damage
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            playerStats.Heal(10f); // Test healing
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            playerExperience.GainExperience(25f); // Test XP gain
        }
        #endif
    }
    
    private void OnPlayerDeath()
    {
        // Disable movement
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }
        
        // Game over will be handled by GameManager
    }
    
    private void OnLevelUp(int newLevel)
    {
        // Trigger animation
        if (playerAnimation != null)
        {
            playerAnimation.TriggerLevelUpAnimation();
        }
        
        // Level up VFX is handled automatically by LevelUpVFX script
    }
    
    // Public methods for external access
    public void TakeDamage(float damage)
    {
        playerStats?.TakeDamage(damage);
    }
    
    public void GainExperience(float amount)
    {
        playerExperience?.GainExperience(amount);
    }
    
    public bool IsAlive()
    {
        return playerStats != null && playerStats.IsAlive;
    }
    
    public float GetMoveSpeedMultiplier()
    {
        return playerStats != null ? playerStats.moveSpeedMultiplier : 1f;
    }
}
