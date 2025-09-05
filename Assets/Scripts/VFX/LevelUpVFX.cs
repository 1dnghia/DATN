using UnityEngine;
using System.Collections;

public class LevelUpVFX : MonoBehaviour
{
    [Header("Particle Effects")]
    public ParticleSystem levelUpBurst;
    public ParticleSystem levelUpGlow;
    public ParticleSystem confettiEffect;
    
    [Header("Screen Effects")]
    public bool enableScreenFlash = true;
    public Color flashColor = Color.white;
    public float flashDuration = 0.2f;
    
    [Header("Player Effects")]
    public bool enablePlayerGlow = true;
    public float glowDuration = 1f;
    public Color glowColor = Color.yellow;
    
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip levelUpSound;
    [Range(0f, 1f)]
    public float soundVolume = 0.8f;
    
    [Header("Screen Shake")]
    public bool enableScreenShake = true;
    public float shakeDuration = 0.3f;
    public float shakeIntensity = 0.5f;
    
    // Components
    private SpriteRenderer playerSpriteRenderer;
    private Color originalPlayerColor;
    private Camera mainCamera;
    
    // Screen flash
    private GameObject screenFlashObject;
    private SpriteRenderer screenFlashRenderer;
    
    private void Awake()
    {
        // Get player sprite renderer
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        if (playerSpriteRenderer != null)
        {
            originalPlayerColor = playerSpriteRenderer.color;
        }
        
        // Get main camera
        mainCamera = Camera.main;
        if (mainCamera == null)
            mainCamera = FindFirstObjectByType<Camera>();
    }
    
    private void Start()
    {
        // Subscribe to level up event
        PlayerExperience.OnLevelUp += TriggerLevelUpVFX;
        
        // Create screen flash object
        CreateScreenFlashObject();
        
        // Auto-assign audio source
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }
    
    private void OnDestroy()
    {
        // Unsubscribe from events
        PlayerExperience.OnLevelUp -= TriggerLevelUpVFX;
        
        // Clean up screen flash object
        if (screenFlashObject != null)
            Destroy(screenFlashObject);
    }
    
    private void TriggerLevelUpVFX(int newLevel)
    {
        // Start all level up effects
        StartCoroutine(PlayLevelUpEffects(newLevel));
    }
    
    private IEnumerator PlayLevelUpEffects(int newLevel)
    {
        Debug.Log($"🎉 Level Up VFX triggered for Level {newLevel}!");
        
        // Play particle effects
        PlayParticleEffects();
        
        // Play sound effect
        PlayLevelUpSound();
        
        // Screen flash effect
        if (enableScreenFlash)
        {
            StartCoroutine(ScreenFlashEffect());
        }
        
        // Player glow effect
        if (enablePlayerGlow && playerSpriteRenderer != null)
        {
            StartCoroutine(PlayerGlowEffect());
        }
        
        // Screen shake effect
        if (enableScreenShake && mainCamera != null)
        {
            StartCoroutine(ScreenShakeEffect());
        }
        
        yield return null;
    }
    
    private void PlayParticleEffects()
    {
        // Main level up burst
        if (levelUpBurst != null)
        {
            levelUpBurst.Play();
        }
        
        // Glow effect
        if (levelUpGlow != null)
        {
            levelUpGlow.Play();
        }
        
        // Confetti effect
        if (confettiEffect != null)
        {
            confettiEffect.Play();
        }
    }
    
    private void PlayLevelUpSound()
    {
        if (audioSource != null && levelUpSound != null)
        {
            audioSource.volume = soundVolume;
            audioSource.PlayOneShot(levelUpSound);
        }
    }
    
    private IEnumerator ScreenFlashEffect()
    {
        if (screenFlashRenderer == null) yield break;
        
        // Flash in
        screenFlashRenderer.color = flashColor;
        screenFlashObject.SetActive(true);
        
        float elapsed = 0f;
        while (elapsed < flashDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / flashDuration);
            
            Color color = flashColor;
            color.a = alpha;
            screenFlashRenderer.color = color;
            
            yield return null;
        }
        
        // Hide flash
        screenFlashObject.SetActive(false);
    }
    
    private IEnumerator PlayerGlowEffect()
    {
        float elapsed = 0f;
        
        while (elapsed < glowDuration)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / glowDuration;
            
            // Pulse glow effect
            float glow = Mathf.Sin(progress * Mathf.PI * 3f) * 0.5f + 0.5f;
            Color currentColor = Color.Lerp(originalPlayerColor, glowColor, glow * 0.5f);
            
            playerSpriteRenderer.color = currentColor;
            
            yield return null;
        }
        
        // Reset to original color
        playerSpriteRenderer.color = originalPlayerColor;
    }
    
    private IEnumerator ScreenShakeEffect()
    {
        Vector3 originalPosition = mainCamera.transform.position;
        float elapsed = 0f;
        
        while (elapsed < shakeDuration)
        {
            elapsed += Time.deltaTime;
            float intensity = Mathf.Lerp(shakeIntensity, 0f, elapsed / shakeDuration);
            
            Vector3 shakeOffset = Random.insideUnitCircle * intensity;
            mainCamera.transform.position = originalPosition + shakeOffset;
            
            yield return null;
        }
        
        // Reset camera position
        mainCamera.transform.position = originalPosition;
    }
    
    private void CreateScreenFlashObject()
    {
        if (mainCamera == null) return;
        
        // Create screen flash GameObject
        screenFlashObject = new GameObject("ScreenFlash");
        screenFlashObject.transform.SetParent(mainCamera.transform);
        screenFlashObject.transform.localPosition = Vector3.forward * 5f; // In front of camera
        
        // Add SpriteRenderer
        screenFlashRenderer = screenFlashObject.AddComponent<SpriteRenderer>();
        
        // Create a white square texture
        Texture2D flashTexture = new Texture2D(1, 1);
        flashTexture.SetPixel(0, 0, Color.white);
        flashTexture.Apply();
        
        // Create sprite from texture
        Sprite flashSprite = Sprite.Create(flashTexture, new Rect(0, 0, 1, 1), Vector2.one * 0.5f, 1f);
        screenFlashRenderer.sprite = flashSprite;
        
        // Scale to cover screen
        float camHeight = mainCamera.orthographicSize * 2f;
        float camWidth = camHeight * mainCamera.aspect;
        screenFlashObject.transform.localScale = new Vector3(camWidth, camHeight, 1f);
        
        // Set sorting order to render on top
        screenFlashRenderer.sortingOrder = 9999;
        
        // Start hidden
        screenFlashObject.SetActive(false);
    }
    
    // Public method for manual trigger
    public void TriggerLevelUpVFXManual()
    {
        TriggerLevelUpVFX(1);
    }
    
    // Reset method for Unity Inspector
    private void Reset()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
            
        if (playerSpriteRenderer == null)
            playerSpriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    // Debug methods
    [ContextMenu("Test Level Up VFX")]
    private void TestLevelUpVFX()
    {
        TriggerLevelUpVFXManual();
    }
}
