using UnityEngine;

public class EnemyStats : MonoBehaviour, IDamageable
{
    [Header("Base Stats")]
    public float maxHealth = 50f;
    public float currentHealth;
    public float baseDamage = 10f;
    public float experienceValue = 10f;
    
    [Header("Combat")]
    public float attackRange = 1f;
    public float attackCooldown = 1f;
    private float lastAttackTime;
    
    [Header("Drops")]
    public GameObject[] dropItems;
    public float[] dropChances;
    
    // Events
    public static System.Action<EnemyStats> OnEnemyDeath;
    public System.Action<float> OnHealthChanged;
    public System.Action OnTakeDamage;
    
    // IDamageable implementation
    public bool IsDead => currentHealth <= 0;
    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;
    
    // Properties
    public float HealthPercentage => currentHealth / maxHealth;
    public bool CanAttack => Time.time >= lastAttackTime + attackCooldown;
    public float Damage => baseDamage;
    
    private void Awake()
    {
        currentHealth = maxHealth;
    }
    
    private void Start()
    {
        // Initialize health UI if exists
        OnHealthChanged?.Invoke(currentHealth);
    }
    
    public void TakeDamage(float damage)
    {
        if (IsDead) return;
        
        currentHealth = Mathf.Max(0, currentHealth - damage);
        OnHealthChanged?.Invoke(currentHealth);
        OnTakeDamage?.Invoke();
        
        Debug.Log($"Enemy {gameObject.name} took {damage:F1} damage. Health: {currentHealth:F1}/{maxHealth:F1}");
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    public void Heal(float amount)
    {
        if (IsDead) return;
        
        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
        OnHealthChanged?.Invoke(currentHealth);
    }
    
    public void Attack()
    {
        if (!CanAttack || IsDead) return;
        
        lastAttackTime = Time.time;
        Debug.Log($"Enemy {gameObject.name} attacks for {baseDamage:F1} damage!");
    }
    
    private void Die()
    {
        Debug.Log($"Enemy {gameObject.name} died! Dropping {experienceValue:F1} XP");
        
        // Drop items
        DropItems();
        
        // Give experience to player
        PlayerExperience playerXP = FindFirstObjectByType<PlayerExperience>();
        if (playerXP != null)
        {
            playerXP.GainExperience(experienceValue);
        }
        
        // Fire death event
        OnEnemyDeath?.Invoke(this);
        EventManager.OnEnemyKilled?.Invoke(gameObject);
        
        // Destroy enemy
        Destroy(gameObject);
    }
    
    private void DropItems()
    {
        if (dropItems == null || dropItems.Length == 0) return;
        
        for (int i = 0; i < dropItems.Length && i < dropChances.Length; i++)
        {
            if (Random.value <= dropChances[i])
            {
                Vector3 dropPosition = transform.position + Random.insideUnitSphere * 0.5f;
                dropPosition.z = 0; // Keep 2D
                
                GameObject drop = Instantiate(dropItems[i], dropPosition, Quaternion.identity);
                Debug.Log($"Enemy dropped {drop.name}");
            }
        }
    }
    
    // Load stats from ScriptableObject
    public void LoadFromData(EnemyData enemyData)
    {
        if (enemyData == null) return;
        
        EnemyStatsData stats = enemyData.baseStats;
        maxHealth = stats.maxHealth;
        currentHealth = maxHealth;
        baseDamage = stats.damage;
        experienceValue = stats.experienceValue;
        
        attackRange = enemyData.attackRange;
        attackCooldown = enemyData.attackCooldown;
        
        dropItems = enemyData.dropItems;
        dropChances = enemyData.dropChances;
        
        // Sync moveSpeed to EnemyMovement component
        EnemyMovement movement = GetComponent<EnemyMovement>();
        if (movement != null)
        {
            movement.SetMoveSpeed(stats.movementSpeed);
        }
    }
    
    // Public getters for other components
    public float GetAttackRange() => attackRange;
    public float GetAttackCooldown() => attackCooldown;
    public float GetExperienceValue() => experienceValue;
    
    // Reset method for Unity Inspector
    private void Reset()
    {
        maxHealth = 50f;
        currentHealth = maxHealth;
        baseDamage = 10f;
        experienceValue = 10f;
        attackRange = 1f;
        attackCooldown = 1f;
    }
}
