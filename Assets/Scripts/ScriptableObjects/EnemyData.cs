using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Data", menuName = "Game/Enemy Data")]
public class EnemyData : ScriptableObject
{
    [Header("Enemy Info")]
    public string enemyName;
    public EnemyType enemyType;
    public Sprite enemySprite;
    
    [Header("Base Stats")]
    public EnemyStatsData baseStats;
    
    [Header("Behavior")]
    public float detectionRange = 10f;
    public float attackRange = 1f;
    public float attackCooldown = 1f;
    
    [Header("Drops")]
    public GameObject[] dropItems;
    public float[] dropChances;
    
    [Header("Prefab")]
    public GameObject enemyPrefab;
}
