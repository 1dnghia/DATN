using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Infinite Map Manager - Vampire Survivors Style
/// - Creates seamless infinite world using tile streaming
/// - Manages background tiles around player
/// - Optimizes performance with object pooling
/// - Supports different biomes and tile variations
/// </summary>
public class InfiniteMapManager : MonoBehaviour
{
    [Header("Map Settings")]
    [Tooltip("Kích thước mỗi tile (Unity units) - tất cả tiles sẽ scale theo size này")]
    public float tileSize = 20f;
    
    [Tooltip("Overlap multiplier để tránh gaps giữa tiles (1.0 = perfect fit, 1.05 = slight overlap)")]
    [Range(0.8f, 1.2f)]
    public float tileOverlap = 1.05f;
    
    [Tooltip("Số tiles được tạo xung quanh player (radius)")]
    public int renderDistance = 3; // 7x7 grid around player
    
    [Tooltip("Khoảng cách để unload tiles xa")]
    public float unloadDistance = 50f;
    
    [Header("Tile Prefabs")]
    [Tooltip("Prefab tiles cho background (each prefab should have BackgroundTile component)")]
    public GameObject[] tilePrefabs;
    
    [Header("Performance")]
    [Tooltip("Số tiles tạo mỗi frame (để tránh lag)")]
    public int tilesPerFrame = 2;
    
    [Tooltip("Enable object pooling")]
    public bool useObjectPooling = true;
    
    [Header("Debug")]
    public bool showDebugGizmos = true;
    public bool logTileOperations = false;
    
    // Components
    private Transform player;
    private Dictionary<Vector2Int, GameObject> activeTiles = new Dictionary<Vector2Int, GameObject>();
    private Queue<GameObject> tilePool = new Queue<GameObject>();
    private Queue<Vector2Int> tilesToCreate = new Queue<Vector2Int>();
    private Queue<Vector2Int> tilesToDestroy = new Queue<Vector2Int>();
    
    // Current player tile position
    private Vector2Int currentPlayerTile;
    private Vector2Int lastPlayerTile;
    
    // Stats
    public int ActiveTileCount => activeTiles.Count;
    public int PoolSize => tilePool.Count;
    public Vector2Int PlayerTilePosition => currentPlayerTile;
    
    private void Awake()
    {
        // Find player
        PlayerController playerController = FindFirstObjectByType<PlayerController>();
        if (playerController != null)
        {
            player = playerController.transform;
        }
        
        // Tile prefabs should have BackgroundTile components for weight management
    }
    
    private void Start()
    {
        if (player == null) return;
        
        // Initialize map around player
        currentPlayerTile = GetTileCoordinate(player.position);
        lastPlayerTile = currentPlayerTile;
        
        // Create initial tiles immediately
        CreateInitialMap();
    }
    
    private void Update()
    {
        if (player == null) return;
        
        UpdatePlayerTilePosition();
        ProcessTileQueue();
    }
    
    private void UpdatePlayerTilePosition()
    {
        currentPlayerTile = GetTileCoordinate(player.position);
        
        // Check if player moved to new tile
        if (currentPlayerTile != lastPlayerTile)
        {
            QueueTileUpdates();
            lastPlayerTile = currentPlayerTile;
        }
    }
    
    private void QueueTileUpdates()
    {
        // Clear previous queues
        tilesToCreate.Clear();
        tilesToDestroy.Clear();
        
        // Find tiles that should exist around player
        HashSet<Vector2Int> requiredTiles = new HashSet<Vector2Int>();
        for (int x = -renderDistance; x <= renderDistance; x++)
        {
            for (int y = -renderDistance; y <= renderDistance; y++)
            {
                Vector2Int tileCoord = currentPlayerTile + new Vector2Int(x, y);
                requiredTiles.Add(tileCoord);
            }
        }
        
        // Queue tiles to create
        foreach (Vector2Int tileCoord in requiredTiles)
        {
            if (!activeTiles.ContainsKey(tileCoord))
            {
                tilesToCreate.Enqueue(tileCoord);
            }
        }
        
        // Queue distant tiles to destroy
        List<Vector2Int> tilesToRemove = new List<Vector2Int>();
        foreach (var tile in activeTiles)
        {
            Vector2Int tileCoord = tile.Key;
            if (!requiredTiles.Contains(tileCoord))
            {
                tilesToRemove.Add(tileCoord);
            }
        }
        
        foreach (Vector2Int tileCoord in tilesToRemove)
        {
            tilesToDestroy.Enqueue(tileCoord);
        }
        
        // Debug logging for tile operations
        if (logTileOperations)
        {
            Debug.Log($"Player moved to tile {currentPlayerTile}. Queued: {tilesToCreate.Count} to create, {tilesToDestroy.Count} to destroy");
        }
    }
    
    private void ProcessTileQueue()
    {
        // Process tile creation (limited per frame)
        int tilesProcessed = 0;
        while (tilesToCreate.Count > 0 && tilesProcessed < tilesPerFrame)
        {
            Vector2Int tileCoord = tilesToCreate.Dequeue();
            CreateTile(tileCoord);
            tilesProcessed++;
        }
        
        // Process tile destruction (unlimited - faster)
        while (tilesToDestroy.Count > 0)
        {
            Vector2Int tileCoord = tilesToDestroy.Dequeue();
            DestroyTile(tileCoord);
        }
    }
    
    private void CreateInitialMap()
    {
        for (int x = -renderDistance; x <= renderDistance; x++)
        {
            for (int y = -renderDistance; y <= renderDistance; y++)
            {
                Vector2Int tileCoord = currentPlayerTile + new Vector2Int(x, y);
                CreateTile(tileCoord);
            }
        }
    }
    
    private void CreateTile(Vector2Int tileCoord)
    {
        if (activeTiles.ContainsKey(tileCoord)) return;
        
        Vector3 worldPosition = GetWorldPosition(tileCoord);
        GameObject tile = GetTileFromPool();
        
        if (tile != null)
        {
            tile.transform.position = worldPosition;
            tile.SetActive(true);
            activeTiles[tileCoord] = tile;
        }
    }
    
    private void DestroyTile(Vector2Int tileCoord)
    {
        if (!activeTiles.ContainsKey(tileCoord)) return;
        
        GameObject tile = activeTiles[tileCoord];
        activeTiles.Remove(tileCoord);
        
        ReturnTileToPool(tile);
    }
    
    private GameObject GetTileFromPool()
    {
        if (useObjectPooling && tilePool.Count > 0)
        {
            return tilePool.Dequeue();
        }
        else
        {
            return CreateNewTile();
        }
    }
    
    private void ReturnTileToPool(GameObject tile)
    {
        if (useObjectPooling)
        {
            tile.SetActive(false);
            tilePool.Enqueue(tile);
        }
        else
        {
            Destroy(tile);
        }
    }
    
    private GameObject CreateNewTile()
    {
        if (tilePrefabs == null || tilePrefabs.Length == 0) return null;
        
        GameObject prefab = ChooseTilePrefab();
        return Instantiate(prefab, transform);
    }
    
    private GameObject ChooseTilePrefab()
    {
        if (tilePrefabs == null || tilePrefabs.Length == 0) return null;
        
        // Simple random selection - weights are handled by BackgroundTile component
        return tilePrefabs[Random.Range(0, tilePrefabs.Length)];
    }
    
    private Vector2Int GetTileCoordinate(Vector3 worldPosition)
    {
        int x = Mathf.FloorToInt(worldPosition.x / tileSize);
        int y = Mathf.FloorToInt(worldPosition.y / tileSize);
        return new Vector2Int(x, y);
    }
    
    private Vector3 GetWorldPosition(Vector2Int tileCoordinate)
    {
        float x = tileCoordinate.x * tileSize + (tileSize * 0.5f);
        float y = tileCoordinate.y * tileSize + (tileSize * 0.5f);
        return new Vector3(x, y, 0f);
    }
    

    
    // Public control methods
    public void SetRenderDistance(int distance)
    {
        renderDistance = Mathf.Max(1, distance);
        QueueTileUpdates();
    }
    
    public void ClearAllTiles()
    {
        foreach (var tile in activeTiles.Values)
        {
            ReturnTileToPool(tile);
        }
        activeTiles.Clear();
        tilesToCreate.Clear();
        tilesToDestroy.Clear();
    }
    
    public void PrewarmPool(int poolSize)
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject tile = CreateNewTile();
            ReturnTileToPool(tile);
        }
    }
    
    // Debug methods
    [ContextMenu("Rebuild Map")]
    private void RebuildMap()
    {
        ClearAllTiles();
        if (player != null)
        {
            currentPlayerTile = GetTileCoordinate(player.position);
            CreateInitialMap();
        }
    }
    
    [ContextMenu("Prewarm Pool")]
    private void DebugPrewarmPool()
    {
        PrewarmPool(20);
    }
    
    // Reset method for Unity Inspector
    private void Reset()
    {
        tileSize = 20f;
        renderDistance = 3;
        unloadDistance = 50f;
        tilesPerFrame = 2;
        useObjectPooling = true;
        showDebugGizmos = true;
    }
    
    // Debug visualization
    private void OnDrawGizmosSelected()
    {
        if (!showDebugGizmos || player == null) return;
        
        Vector2Int playerTile = GetTileCoordinate(player.position);
        
        // Draw render area
        Gizmos.color = Color.green;
        for (int x = -renderDistance; x <= renderDistance; x++)
        {
            for (int y = -renderDistance; y <= renderDistance; y++)
            {
                Vector2Int tileCoord = playerTile + new Vector2Int(x, y);
                Vector3 tileCenter = GetWorldPosition(tileCoord);
                
                Gizmos.DrawWireCube(tileCenter, Vector3.one * tileSize);
            }
        }
        
        // Draw player tile
        Gizmos.color = Color.red;
        Vector3 playerTileCenter = GetWorldPosition(playerTile);
        Gizmos.DrawWireCube(playerTileCenter, Vector3.one * tileSize * 1.1f);
        
        // Draw active tiles
        Gizmos.color = Color.yellow;
        foreach (var tile in activeTiles)
        {
            Vector3 tileCenter = GetWorldPosition(tile.Key);
            Gizmos.DrawSphere(tileCenter, 0.5f);
        }
    }
}