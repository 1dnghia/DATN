using UnityEngine;

[CreateAssetMenu(fileName = "New Wave Data", menuName = "Game/Wave Data")]
public class WaveData : ScriptableObject
{
    [Header("Wave Info")]
    public string waveName;
    public float startTime;
    public float duration;
    
    [Header("Enemy Spawning")]
    public EnemySpawnData[] enemies;
    
    [Header("Difficulty Scaling")]
    public float healthMultiplier = 1f;
    public float damageMultiplier = 1f;
    public float speedMultiplier = 1f;
    public float spawnRateMultiplier = 1f;
}
