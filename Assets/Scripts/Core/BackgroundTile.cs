using UnityEngine;

/// <summary>
/// Background Tile Component
/// - Automatically configures background tile sprites
/// - Supports seamless tiling and variations
/// - Gets size from InfiniteMapManager for consistency
/// - Performance optimized for infinite maps
/// </summary>
public class BackgroundTile : MonoBehaviour
{
    [Header("Tile Settings")]
    [Tooltip("Auto-configure sprite renderer on start")]
    public bool autoConfigureSprite = true;
    
    [Tooltip("Prefab variations for this tile type (can be sprites or tilemap prefabs)")]
    public GameObject[] prefabVariations;
    
    [Tooltip("Weights for random selection of prefab variations")]
    public float[] chunkWeights = { 1f };
    
    [Tooltip("Auto-scale prefabs to match InfiniteMapManager tile size")]
    public bool autoScale = true;
    
    [Header("Rendering")]
    [Tooltip("Rendering layer for background")]
    public int sortingOrder = -10;
    
    [Tooltip("Sorting layer name")]
    public string sortingLayerName = "Background";
    
    // Components
    private SpriteRenderer spriteRenderer;
    
    private void Awake()
    {
        SetupComponents();
    }
    
    private void Start()
    {
        if (autoConfigureSprite)
        {
            ConfigureSprite();
        }
    }
    
    private void SetupComponents()
    {
        // Get or create SpriteRenderer (for simple sprites)
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        }
        
        // Configure for background rendering
        spriteRenderer.sortingOrder = sortingOrder;
        
        // Set sorting layer if it exists
        if (!string.IsNullOrEmpty(sortingLayerName))
        {
            spriteRenderer.sortingLayerName = sortingLayerName;
        }
    }
    
    private void ConfigureSprite()
    {
        if (prefabVariations == null || prefabVariations.Length == 0) 
            return;
        
        // Choose random prefab variation using weights
        GameObject selectedPrefab = ChoosePrefabWithWeights();
        
        if (selectedPrefab == null) return;
        
        // Check if it's a simple sprite or a complex prefab
        SpriteRenderer prefabSpriteRenderer = selectedPrefab.GetComponent<SpriteRenderer>();
        
        if (prefabSpriteRenderer != null && prefabSpriteRenderer.sprite != null)
        {
            // Handle simple sprite prefab
            ConfigureSimpleSprite(prefabSpriteRenderer.sprite);
        }
        else
        {
            // Handle complex prefab (Tilemap, etc.)
            ConfigureComplexPrefab(selectedPrefab);
        }
        
        // Optional: Add random rotation for variety
        if (prefabVariations.Length > 1)
        {
            float randomRotation = Random.Range(0, 4) * 90f; // 0, 90, 180, 270 degrees
            transform.rotation = Quaternion.Euler(0, 0, randomRotation);
        }
    }
    
    private GameObject ChoosePrefabWithWeights()
    {
        if (prefabVariations.Length == 1) return prefabVariations[0];
        
        // Initialize weights if not set properly
        if (chunkWeights == null || chunkWeights.Length != prefabVariations.Length)
        {
            InitializeDefaultWeights();
        }
        
        // Weighted random selection
        float totalWeight = 0f;
        for (int i = 0; i < chunkWeights.Length && i < prefabVariations.Length; i++)
        {
            totalWeight += chunkWeights[i];
        }
        
        if (totalWeight <= 0f) return prefabVariations[0];
        
        float randomValue = Random.Range(0f, totalWeight);
        float currentWeight = 0f;
        
        for (int i = 0; i < chunkWeights.Length && i < prefabVariations.Length; i++)
        {
            currentWeight += chunkWeights[i];
            if (randomValue <= currentWeight)
            {
                return prefabVariations[i];
            }
        }
        
        return prefabVariations[0];
    }
    
    private void InitializeDefaultWeights()
    {
        chunkWeights = new float[prefabVariations.Length];
        for (int i = 0; i < chunkWeights.Length; i++)
        {
            chunkWeights[i] = 1f;
        }
    }
    
    private void ConfigureSimpleSprite(Sprite sprite)
    {
        if (spriteRenderer == null) return;
        
        spriteRenderer.sprite = sprite;
        
        // Auto-scale to InfiniteMapManager tile size
        if (autoScale)
        {
            AutoScaleSprite(sprite);
        }
    }
    
    private void ConfigureComplexPrefab(GameObject prefab)
    {
        // For complex prefabs, instantiate as child and configure
        if (transform.childCount > 0)
        {
            // Remove existing child
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
        
        GameObject instance = Instantiate(prefab, transform);
        instance.transform.localPosition = Vector3.zero;
        
        // Auto-scale prefab
        if (autoScale)
        {
            AutoScalePrefab(instance);
        }
        
        // Configure sorting layers for all renderers in prefab
        ConfigurePrefabSortingLayers(instance);
        
        // Disable the main sprite renderer since we're using prefab
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false;
        }
    }
    
    private void AutoScalePrefab(GameObject prefab)
    {
        InfiniteMapManager mapManager = FindFirstObjectByType<InfiniteMapManager>();
        if (mapManager == null) return;
        
        float targetSize = mapManager.tileSize * mapManager.tileOverlap;
        
        // Calculate prefab bounds
        Renderer[] renderers = prefab.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0) return;
        
        Bounds bounds = renderers[0].bounds;
        foreach (Renderer renderer in renderers)
        {
            bounds.Encapsulate(renderer.bounds);
        }
        
        // Calculate scale to fit target size
        float maxSize = Mathf.Max(bounds.size.x, bounds.size.y);
        if (maxSize > 0)
        {
            float scale = targetSize / maxSize;
            prefab.transform.localScale = Vector3.one * scale;
        }
    }
    
    private void ConfigurePrefabSortingLayers(GameObject prefab)
    {
        // Configure all renderers in prefab to use background sorting
        Renderer[] renderers = prefab.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.sortingOrder = sortingOrder;
            if (!string.IsNullOrEmpty(sortingLayerName))
            {
                renderer.sortingLayerName = sortingLayerName;
            }
        }
    }
    
    private void AutoScaleSprite(Sprite sprite)
    {
        if (sprite == null) return;
        
        // Get tile size from InfiniteMapManager
        InfiniteMapManager mapManager = FindFirstObjectByType<InfiniteMapManager>();
        if (mapManager == null) return;
        
        float targetSize = mapManager.tileSize * mapManager.tileOverlap;
        
        // Calculate sprite's actual size in world units
        float spriteWidth = sprite.bounds.size.x;
        float spriteHeight = sprite.bounds.size.y;
        
        // Calculate scale for each dimension to fit target size
        float scaleX = targetSize / spriteWidth;
        float scaleY = targetSize / spriteHeight;
        
        // Apply uniform scaling (square tiles with slight overlap to prevent gaps)
        float uniformScale = Mathf.Max(scaleX, scaleY);
        transform.localScale = Vector3.one * uniformScale;
    }
    
    // Public methods for runtime configuration
    public void SetSprite(Sprite sprite)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = sprite;
        }
    }
    
    public void SetSortingOrder(int order)
    {
        sortingOrder = order;
        if (spriteRenderer != null)
        {
            spriteRenderer.sortingOrder = order;
        }
    }
    
    public void RandomizeAppearance()
    {
        // Clear existing children (if any complex prefabs)
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
        
        // Re-enable sprite renderer in case it was disabled
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = true;
        }
        
        // Reconfigure with new random selection
        ConfigureSprite();
    }
    
    // Reset method for Unity Inspector
    private void Reset()
    {
        sortingOrder = -10;
        sortingLayerName = "Background";
        autoConfigureSprite = true;
        autoScale = true;
        chunkWeights = new float[] { 1f };
    }
}