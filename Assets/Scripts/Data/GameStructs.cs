using UnityEngine;

[System.Serializable]
public struct PlayerStatsData
{
    public float maxHealth;
    public float movementSpeed;
    public float damage;
    public float armor;
    public float criticalChance;
    public float lifeSteal;
    public float experienceMultiplier;
}

[System.Serializable]
public struct WeaponStatsData
{
    public float damage;
    public float attackSpeed;
    public float range;
    public int projectileCount;
    public float criticalChance;
    public float criticalMultiplier;
}

[System.Serializable]
public struct EnemyStatsData
{
    public float maxHealth;
    public float movementSpeed;
    public float damage;
    public float experienceValue;
    public float spawnWeight;
}

[System.Serializable]
public struct EnemySpawnData
{
    [Header("Enemy Type")]
    public EnemyData enemyData;
    
    [Header("Spawn Settings")]
    public int minCount;
    public int maxCount;
    public float spawnRate;
    public float spawnWeight;
    
    [Header("Timing")]
    public float startDelay;
    public float endTime; // When to stop spawning this enemy type (0 = never stop)
    
    [Header("Scaling")]
    public float healthScale;
    public float damageScale;
    public float speedScale;
}
