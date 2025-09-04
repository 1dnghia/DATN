using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    public PlayerMovement playerMovement;
    public PlayerStats playerStats;
    public PlayerExperience playerExperience;
    
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
        if (playerMovement == null) 
            playerMovement = GetComponent<PlayerMovement>();
        if (playerStats == null)
            playerStats = GetComponent<PlayerStats>();
        if (playerExperience == null)
            playerExperience = GetComponent<PlayerExperience>();
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
        Debug.Log("Player Controller: Player died!");
        // Disable movement
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }
        
        // Game over will be handled by GameManager
    }
    
    private void OnLevelUp(int newLevel)
    {
        Debug.Log($"Player Controller: Player reached level {newLevel}!");
        
        // Can add level up effects here (particle effects, sound, etc.)
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
