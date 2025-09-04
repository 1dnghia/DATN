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
