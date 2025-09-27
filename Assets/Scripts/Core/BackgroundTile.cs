using UnityEngine;

/// <summary>
/// Background Tile Component
/// - Automatically configures background tile sprites
/// - Supports seamless tiling and variations
/// - Performance optimized for infinite maps
/// </summary>
public class BackgroundTile : MonoBehaviour
{
    [Header("Tile Settings")]
    [Tooltip("Auto-configure sprite renderer on start")]
    public bool autoConfigureSprite = true;
    
    [Tooltip("Sprite variations for this tile type")]
    public Sprite[] spriteVariations;
    
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
        // Get or create SpriteRenderer
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
        if (spriteRenderer == null || spriteVariations == null || spriteVariations.Length == 0) 
            return;
        
        // Choose random sprite variation
        Sprite selectedSprite = spriteVariations[Random.Range(0, spriteVariations.Length)];
        spriteRenderer.sprite = selectedSprite;
        
        // Optional: Add random rotation for variety
        if (spriteVariations.Length > 1)
        {
            float randomRotation = Random.Range(0, 4) * 90f; // 0, 90, 180, 270 degrees
            transform.rotation = Quaternion.Euler(0, 0, randomRotation);
        }
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
        ConfigureSprite();
    }
    
    // Reset method for Unity Inspector
    private void Reset()
    {
        sortingOrder = -10;
        sortingLayerName = "Background";
        autoConfigureSprite = true;
    }
}