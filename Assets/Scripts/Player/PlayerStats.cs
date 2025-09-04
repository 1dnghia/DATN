using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth = 100f;
    public float currentHealth;
    public float healthRegenRate = 0f; // HP regen per second
    
    [Header("Damage")]
    public float baseDamage = 10f;
    public float damageMultiplier = 1f;
    
    [Header("Defense")]
    public float armor = 0f;
    public float magicResist = 0f;
    
    [Header("Speed")]
    public float moveSpeedMultiplier = 1f;
    
    [Header("Luck")]
    public float critChance = 0f;
    public float critDamage = 1.5f;
    public float pickupRange = 1f;
    
    [Header("Utility")]
    public float cooldownReduction = 0f; // Percentage (0-1)
    public int projectileCount = 0; // Additional projectiles
    public float duration = 0f; // Skill duration bonus
    
    // Events
    public static System.Action<float, float> OnHealthChanged; // current, max
    public static System.Action OnPlayerDeath;
    
    // Properties
    public float TotalDamage => baseDamage * damageMultiplier;
    public bool IsAlive => currentHealth > 0;
    public float HealthPercentage => currentHealth / maxHealth;
    
    private void Awake()
    {
        currentHealth = maxHealth;
    }
    
    private void Reset()
    {
        // Called when Reset is pressed in Unity Inspector
        // Set default values
        maxHealth = 100f;
        currentHealth = maxHealth;
        healthRegenRate = 0f;
        baseDamage = 10f;
        damageMultiplier = 1f;
        armor = 0f;
        magicResist = 0f;
        moveSpeedMultiplier = 1f;
        critChance = 0f;
        critDamage = 1.5f;
        pickupRange = 1f;
        cooldownReduction = 0f;
        projectileCount = 0;
        duration = 0f;
    }
    
    private void Start()
    {
        // Initialize health UI
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }
    
    private void Update()
    {
        // Health regeneration
        if (healthRegenRate > 0 && currentHealth < maxHealth)
        {
            Heal(healthRegenRate * Time.deltaTime);
        }
    }
    
    public void TakeDamage(float damage)
    {
        if (!IsAlive) return;
        
        // Apply armor reduction
        float finalDamage = damage * (100f / (100f + armor));
        
        currentHealth = Mathf.Max(0, currentHealth - finalDamage);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        
        // Trigger events
        EventManager.OnPlayerDamaged?.Invoke(finalDamage);
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    public void Heal(float amount)
    {
        if (!IsAlive) return;
        
        float oldHealth = currentHealth;
        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
        
        if (currentHealth != oldHealth)
        {
            OnHealthChanged?.Invoke(currentHealth, maxHealth);
            EventManager.OnPlayerHealed?.Invoke(amount);
        }
    }
    
    public void FullHeal()
    {
        Heal(maxHealth);
    }
    
    private void Die()
    {
        OnPlayerDeath?.Invoke();
        EventManager.OnPlayerDied?.Invoke();
        
        // Game over logic will be handled by GameManager
        Debug.Log("Player Died!");
    }
    
    // Stat modification methods for upgrades
    public void IncreaseMaxHealth(float amount)
    {
        maxHealth += amount;
        currentHealth += amount; // Also heal when increasing max health
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }
    
    public void IncreaseDamage(float multiplier)
    {
        damageMultiplier += multiplier;
    }
    
    public void IncreaseSpeed(float multiplier)
    {
        moveSpeedMultiplier += multiplier;
    }
    
    public void IncreaseArmor(float amount)
    {
        armor += amount;
    }
    
    public void IncreaseCritChance(float amount)
    {
        critChance = Mathf.Min(1f, critChance + amount);
    }
    
    public void IncreasePickupRange(float amount)
    {
        pickupRange += amount;
    }
    
    // Calculate if attack is critical
    public bool IsCriticalHit()
    {
        return Random.value <= critChance;
    }
    
    // Get final damage with crit calculation
    public float GetFinalDamage()
    {
        float damage = TotalDamage;
        if (IsCriticalHit())
        {
            damage *= critDamage;
        }
        return damage;
    }
}
