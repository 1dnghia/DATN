using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Enemy Spawner - Vampire Survivors Style
/// - Spawns enemies around player at safe distances
/// - Manages spawn rates and enemy limits
/// - Supports multiple enemy types and waves
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    [Header("Spawning Settings")]
    public float spawnRate = 2f; // Enemies per second
    public int maxEnemies = 50;
    public float despawnDistance = 20f; // Distance to despawn enemies
    
    [Header("Spawn Positioning")]
    public float minSpawnRadius = 10f; // Minimum distance from player to spawn
    public float maxSpawnRadius = 15f; // Maximum distance from player to spawn
    
    [Header("Enemy Types")]
    public EnemyData[] enemyTypes;
    public float[] spawnWeights; // Relative spawn chance for each type
    
    [Header("Difficulty Scaling")]
    public bool enableDifficultyScaling = true;
    public float difficultyIncreaseRate = 0.1f; // Per second
    public float maxDifficultyMultiplier = 5f;
    
    [Header("Debug")]
    public bool showSpawnGizmos = true;
    public bool logSpawning = true;
    
    // Components
    private Transform player;
    private List<GameObject> activeEnemies = new List<GameObject>();
    
    // Spawning state
    private float spawnTimer;
    private float currentDifficulty = 1f;
    private float gameStartTime;
    private bool isSpawning = false;
    
    // Properties
    public int ActiveEnemyCount => activeEnemies.Count;
    public float CurrentDifficulty => currentDifficulty;
    public bool IsSpawning => isSpawning;
    
    private void Awake()
    {
        // Find player
        PlayerController playerController = FindFirstObjectByType<PlayerController>();
        if (playerController != null)
        {
            player = playerController.transform;
        }
        
        #if UNITY_EDITOR
        if (player == null) { /* Player not found */ }
        if (enemyTypes == null || enemyTypes.Length == 0) { /* No enemy types configured */ }
        #endif
    }
    
    private void Start()
    {
        // Initialize spawn weights if not set
        if (spawnWeights == null || spawnWeights.Length != enemyTypes.Length)
        {
            InitializeDefaultWeights();
        }
        
        // Subscribe to events
        EnemyStats.OnEnemyDeath += OnEnemyDeath;
        EventManager.OnGameStarted += StartSpawning;
        EventManager.OnGameOver += StopSpawning;
        
        gameStartTime = Time.time;
        
        // Auto-start spawning (change this to event-based later)
        StartSpawning();
    }
    
    private void Update()
    {
        if (!isSpawning || player == null) return;
        
        UpdateDifficulty();
        UpdateSpawning();
        CleanupEnemies();
    }
    
    private void UpdateDifficulty()
    {
        if (!enableDifficultyScaling) return;
        
        float gameTime = Time.time - gameStartTime;
        currentDifficulty = Mathf.Clamp(
            1f + (gameTime * difficultyIncreaseRate),
            1f,
            maxDifficultyMultiplier
        );
    }
    
    private void UpdateSpawning()
    {
        // Check if we should spawn more enemies
        if (activeEnemies.Count >= maxEnemies) return;
        
        spawnTimer += Time.deltaTime;
        float currentSpawnRate = spawnRate * currentDifficulty;
        float spawnInterval = 1f / currentSpawnRate;
        
        if (spawnTimer >= spawnInterval)
        {
            SpawnEnemy();
            spawnTimer = 0f;
        }
    }
    
    private void SpawnEnemy()
    {
        if (enemyTypes == null || enemyTypes.Length == 0) return;
        
        // Choose enemy type based on weights
        EnemyData enemyData = ChooseEnemyType();
        if (enemyData == null || enemyData.enemyPrefab == null) return;
        
        // Find spawn position
        Vector3 spawnPosition = GetSpawnPosition();
        if (spawnPosition == Vector3.zero) return; // Failed to find valid position
        
        // Spawn enemy
        GameObject enemy = Instantiate(enemyData.enemyPrefab, spawnPosition, Quaternion.identity);
        
        // Configure enemy with data
        EnemyController enemyController = enemy.GetComponent<EnemyController>();
        if (enemyController != null)
        {
            enemyController.LoadFromData(enemyData);
        }
        
        // Apply difficulty scaling to stats
        ApplyDifficultyScaling(enemy);
        
        // Add to active enemies list
        activeEnemies.Add(enemy);
        
        // Optional: Log spawning if enabled in inspector
        if (logSpawning)
        {
            // Spawned enemy at position with difficulty scaling
        }
    }
    
    private EnemyData ChooseEnemyType()
    {
        if (enemyTypes.Length == 1) return enemyTypes[0];
        
        // Calculate total weight
        float totalWeight = 0f;
        for (int i = 0; i < spawnWeights.Length && i < enemyTypes.Length; i++)
        {
            totalWeight += spawnWeights[i];
        }
        
        // Choose random value
        float randomValue = Random.Range(0f, totalWeight);
        float currentWeight = 0f;
        
        // Find selected enemy type
        for (int i = 0; i < spawnWeights.Length && i < enemyTypes.Length; i++)
        {
            currentWeight += spawnWeights[i];
            if (randomValue <= currentWeight)
            {
                return enemyTypes[i];
            }
        }
        
        // Fallback to first enemy type
        return enemyTypes[0];
    }
    
    private Vector3 GetSpawnPosition()
    {
        if (player == null) return Vector3.zero;
        
        int attempts = 0;
        int maxAttempts = 10;
        
        while (attempts < maxAttempts)
        {
            attempts++;
            
            // Random angle
            float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
            
            // Random distance within spawn range
            float distance = Random.Range(minSpawnRadius, maxSpawnRadius);
            
            // Calculate position
            Vector3 spawnPos = player.position + new Vector3(
                Mathf.Cos(angle) * distance,
                Mathf.Sin(angle) * distance,
                0f
            );
            
            // Check if position is valid (not too close to other enemies)
            if (IsValidSpawnPosition(spawnPos))
            {
                return spawnPos;
            }
        }
        
        // Failed to find valid spawn position after max attempts
        return Vector3.zero;
    }
    
    private bool IsValidSpawnPosition(Vector3 position)
    {
        // Check minimum distance from player
        float distanceToPlayer = Vector3.Distance(position, player.position);
        if (distanceToPlayer < minSpawnRadius) return false;
        
        // Check for nearby enemies (avoid spawning too close together)
        float minEnemyDistance = 2f;
        foreach (GameObject enemy in activeEnemies)
        {
            if (enemy == null) continue;
            
            float distanceToEnemy = Vector3.Distance(position, enemy.transform.position);
            if (distanceToEnemy < minEnemyDistance) return false;
        }
        
        return true;
    }
    
    private void ApplyDifficultyScaling(GameObject enemy)
    {
        EnemyStats enemyStats = enemy.GetComponent<EnemyStats>();
        if (enemyStats == null) return;
        
        // Scale health and damage with difficulty
        float healthMultiplier = Mathf.Sqrt(currentDifficulty); // Square root scaling for health
        float damageMultiplier = currentDifficulty * 0.5f; // Linear scaling for damage
        
        // Apply scaling (this would require modifications to EnemyStats to support runtime scaling)
        // For now, we'll just note the intended scaling
    }
    
    private void CleanupEnemies()
    {
        // Remove null references and despawn distant enemies
        for (int i = activeEnemies.Count - 1; i >= 0; i--)
        {
            GameObject enemy = activeEnemies[i];
            
            // Remove destroyed enemies
            if (enemy == null)
            {
                activeEnemies.RemoveAt(i);
                continue;
            }
            
            // Check if enemy is too far from player
            float distanceToPlayer = Vector3.Distance(enemy.transform.position, player.position);
            if (distanceToPlayer > despawnDistance)
            {
                // Despawn distant enemy
                
                Destroy(enemy);
                activeEnemies.RemoveAt(i);
            }
        }
    }
    
    private void OnEnemyDeath(EnemyStats deadEnemy)
    {
        if (deadEnemy != null && deadEnemy.gameObject != null)
        {
            activeEnemies.Remove(deadEnemy.gameObject);
        }
    }
    
    private void InitializeDefaultWeights()
    {
        spawnWeights = new float[enemyTypes.Length];
        for (int i = 0; i < spawnWeights.Length; i++)
        {
            spawnWeights[i] = 1f; // Equal weight by default
        }
    }
    
    // Public control methods
    public void StartSpawning()
    {
        isSpawning = true;
        gameStartTime = Time.time;
    }
    
    public void StopSpawning()
    {
        isSpawning = false;
    }
    
    public void ClearAllEnemies()
    {
        foreach (GameObject enemy in activeEnemies)
        {
            if (enemy != null)
            {
                Destroy(enemy);
            }
        }
        activeEnemies.Clear();
    }
    
    public void SetSpawnRate(float newRate)
    {
        spawnRate = Mathf.Max(0.1f, newRate);
    }
    
    public void SetMaxEnemies(int newMax)
    {
        maxEnemies = Mathf.Max(1, newMax);
    }
    
    private void OnDestroy()
    {
        // Unsubscribe from events
        EnemyStats.OnEnemyDeath -= OnEnemyDeath;
        EventManager.OnGameStarted -= StartSpawning;
        EventManager.OnGameOver -= StopSpawning;
    }
    
    // Reset method for Unity Inspector
    private void Reset()
    {
        spawnRate = 2f;
        maxEnemies = 50;
        despawnDistance = 20f;
        minSpawnRadius = 10f;
        maxSpawnRadius = 15f;
        enableDifficultyScaling = true;
        difficultyIncreaseRate = 0.1f;
        maxDifficultyMultiplier = 5f;
    }
    
    // Debug methods
    [ContextMenu("Spawn Test Enemy")]
    private void SpawnTestEnemy()
    {
        SpawnEnemy();
    }
    
    [ContextMenu("Clear All Enemies")]
    private void DebugClearEnemies()
    {
        ClearAllEnemies();
    }
    
    // Debug visualization
    private void OnDrawGizmosSelected()
    {
        if (!showSpawnGizmos || player == null) return;
        
        Vector3 playerPos = Application.isPlaying ? player.position : transform.position;
        
        // Draw spawn radius
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(playerPos, minSpawnRadius);
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(playerPos, maxSpawnRadius);
        
        // Draw despawn radius
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(playerPos, despawnDistance);
    }
}
