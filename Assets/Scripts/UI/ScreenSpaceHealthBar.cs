using UnityEngine;
using UnityEngine.UI;
using TMPro;

#if DOTWEEN_ENABLED
using DG.Tweening;
#endif

public class ScreenSpaceHealthBar : MonoBehaviour
{
    [Header("Health Bar Components")]
    public Transform target; // Player to follow
    public Image healthFillImage; // Primary - for custom assets with fillAmount
    public TextMeshProUGUI healthText;
    public Image backgroundImage;
    
    [Header("Positioning")]
    public Vector3 worldOffset = new Vector3(0, 1.2f, 0);
    public Vector2 screenOffset = new Vector2(0, -30);
    public bool followTarget = true;
    
    [Header("Visual Settings")]
    public Color healthColor = Color.green;
    public Color lowHealthColor = Color.red;
    public Color criticalHealthColor = new Color(0.8f, 0f, 0f);
    public float lowHealthThreshold = 0.3f;
    public float criticalHealthThreshold = 0.1f;
    
    [Header("DOTween Animation")]
    public bool useDOTween = true;
    public float healthAnimationDuration = 0.3f;
    #pragma warning disable 0414
    [SerializeField] private int healthAnimationEase = 6; // OutQuart
    #pragma warning restore 0414
    public float damageShakeDuration = 0.2f;
    public float damageShakeStrength = 5f;
    public bool enableDamageShake = true;
    public bool enableLowHealthPulse = true;
    public float lowHealthPulseSpeed = 2f;
    
    [Header("Legacy Animation")]
    public bool animateHealthChanges = true;
    public float animationSpeed = 5f;
    
    private Camera mainCamera;
    private Canvas parentCanvas;
    private RectTransform rectTransform;
    private float targetHealth;
    private float currentDisplayHealth;
    private float maxHealth;
    private float previousHealth;
    
    #if DOTWEEN_ENABLED
    private Tween healthTween;
    private Tween colorTween;
    private Tween shakeTween;
    private Tween pulseTween;
    #pragma warning disable 0414
    private bool isDOTweenAvailable = true;
    #pragma warning restore 0414
    #else
    #pragma warning disable 0414
    private bool isDOTweenAvailable = false;
    #pragma warning restore 0414
    #endif
    
    private void Awake()
    {
        // Get components
        rectTransform = GetComponent<RectTransform>();
        parentCanvas = GetComponentInParent<Canvas>();
        
        // Find main camera
        mainCamera = Camera.main;
        if (mainCamera == null)
            mainCamera = FindFirstObjectByType<Camera>();
            
        // Auto-assign target if not set
        if (target == null)
        {
            PlayerController playerController = FindFirstObjectByType<PlayerController>();
            if (playerController != null)
                target = playerController.transform;
        }
    }
    
    private void Start()
    {
        // Subscribe to player health events
        PlayerStats.OnHealthChanged += UpdateHealthDisplay;
        
        // Initialize health display
        PlayerStats playerStats = FindFirstObjectByType<PlayerStats>();
        if (playerStats != null)
        {
            maxHealth = playerStats.maxHealth;
            targetHealth = playerStats.currentHealth;
            currentDisplayHealth = targetHealth;
            previousHealth = targetHealth;
            
            // Set initial health bar
            UpdateHealthBar();
            UpdateHealthText(targetHealth, maxHealth);
        }
    }
    
    private void OnDestroy()
    {
        // Unsubscribe from events
        PlayerStats.OnHealthChanged -= UpdateHealthDisplay;
        
        #if DOTWEEN_ENABLED
        // Kill any ongoing tweens
        healthTween?.Kill();
        colorTween?.Kill();
        shakeTween?.Kill();
        pulseTween?.Kill();
        #endif
    }
    
    private void Update()
    {
        // Update position to follow target
        if (followTarget && target != null)
        {
            UpdateScreenPosition();
        }
        
        bool useLegacyAnimation = !isDOTweenAvailable || !useDOTween;
        
        // Handle smooth health bar animation (legacy mode)
        if (useLegacyAnimation && animateHealthChanges)
        {
            if (Mathf.Abs(currentDisplayHealth - targetHealth) > 0.01f)
            {
                currentDisplayHealth = Mathf.Lerp(currentDisplayHealth, targetHealth, 
                    Time.unscaledDeltaTime * animationSpeed);
                UpdateHealthBar();
                UpdateHealthText(currentDisplayHealth, maxHealth);
            }
        }
    }
    
    private void UpdateScreenPosition()
    {
        if (mainCamera == null || target == null || parentCanvas == null)
            return;
            
        Vector3 worldPosition = target.position + worldOffset;
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(mainCamera, worldPosition);
        
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentCanvas.transform as RectTransform,
            screenPoint + screenOffset,
            parentCanvas.worldCamera,
            out localPoint);
            
        rectTransform.localPosition = localPoint;
    }
    
    private void UpdateHealthDisplay(float currentHealth, float maximumHealth)
    {
        previousHealth = targetHealth;
        targetHealth = currentHealth;
        maxHealth = maximumHealth;
        
        #if DOTWEEN_ENABLED
        if (isDOTweenAvailable && useDOTween)
        {
            AnimateHealthWithDOTween(currentHealth, maximumHealth);
        }
        else
        #endif
        {
            // Immediate update for legacy mode
            if (!animateHealthChanges)
            {
                currentDisplayHealth = targetHealth;
                UpdateHealthBar();
            }
        }
        
        UpdateHealthText(currentHealth, maximumHealth);
        
        // Trigger damage shake effect
        if (currentHealth < previousHealth && enableDamageShake)
        {
            #if DOTWEEN_ENABLED
            if (isDOTweenAvailable && useDOTween)
            {
                TriggerDamageShake();
            }
            #endif
        }
    }
    
    #if DOTWEEN_ENABLED
    private void AnimateHealthWithDOTween(float currentHealth, float maximumHealth)
    {
        float targetPercentage = maximumHealth > 0 ? currentHealth / maximumHealth : 0f;
        
        // Kill existing health animation
        healthTween?.Kill();
        
        if (healthFillImage != null)
        {
            // Animate fillAmount for custom assets
            healthTween = healthFillImage.DOFillAmount(targetPercentage, healthAnimationDuration)
                .SetEase((Ease)healthAnimationEase)
                .SetUpdate(true);
        }
        
        // Update display value immediately
        currentDisplayHealth = targetHealth;
        
        // Handle low health pulse
        HandleLowHealthEffects(targetPercentage);
    }
    
    private void HandleLowHealthEffects(float healthPercentage)
    {
        if (!enableLowHealthPulse || healthFillImage == null) return;
        
        pulseTween?.Kill();
        
        if (healthPercentage <= criticalHealthThreshold)
        {
            // Critical health - fast pulse
            pulseTween = healthFillImage.DOFade(0.3f, 0.2f)
                .SetLoops(-1, LoopType.Yoyo)
                .SetUpdate(true);
        }
        else if (healthPercentage <= lowHealthThreshold)
        {
            // Low health - slow pulse
            pulseTween = healthFillImage.DOFade(0.6f, 0.5f)
                .SetLoops(-1, LoopType.Yoyo)
                .SetUpdate(true);
        }
        else
        {
            // Normal health - no pulse
            if (healthFillImage.color.a != 1f)
            {
                healthFillImage.DOFade(1f, 0.2f).SetUpdate(true);
            }
        }
    }
    
    private void TriggerDamageShake()
    {
        if (!enableDamageShake || rectTransform == null) return;
        
        shakeTween?.Kill();
        shakeTween = rectTransform.DOShakePosition(damageShakeDuration, damageShakeStrength, 20, 90, false, true)
            .SetUpdate(true);
    }
    #endif
    
    private void UpdateHealthBar()
    {
        if (healthFillImage == null) return;
        
        float healthPercentage = maxHealth > 0 ? currentDisplayHealth / maxHealth : 0f;
        healthPercentage = Mathf.Clamp01(healthPercentage);
        
        // Update fill amount
        healthFillImage.fillAmount = healthPercentage;
        
        // Update color based on health percentage
        Color targetColor = healthColor;
        
        if (healthPercentage <= criticalHealthThreshold)
        {
            targetColor = criticalHealthColor;
        }
        else if (healthPercentage <= lowHealthThreshold)
        {
            targetColor = Color.Lerp(lowHealthColor, healthColor, 
                (healthPercentage - criticalHealthThreshold) / (lowHealthThreshold - criticalHealthThreshold));
        }
        
        if (healthFillImage.color != targetColor)
        {
            healthFillImage.color = targetColor;
        }
    }
    
    private void UpdateHealthText(float currentHealth, float maxHealth)
    {
        if (healthText != null)
        {
            healthText.text = $"{Mathf.RoundToInt(currentHealth)}/{Mathf.RoundToInt(maxHealth)}";
        }
    }
    
    private void Reset()
    {
        // Auto-assign components when script is first added
        if (healthFillImage == null)
        {
            // Look for child Image with "Fill" in name
            Image[] images = GetComponentsInChildren<Image>();
            foreach (Image img in images)
            {
                if (img.gameObject.name.Contains("Fill"))
                {
                    healthFillImage = img;
                    break;
                }
            }
        }
        
        if (healthText == null)
        {
            // Look for child TextMeshPro with "Health" in name
            TextMeshProUGUI[] texts = GetComponentsInChildren<TextMeshProUGUI>();
            foreach (TextMeshProUGUI text in texts)
            {
                if (text.gameObject.name.Contains("Health"))
                {
                    healthText = text;
                    break;
                }
            }
        }
        
        if (backgroundImage == null)
        {
            // Look for child Image with "Background" in name
            Image[] images = GetComponentsInChildren<Image>();
            foreach (Image img in images)
            {
                if (img.gameObject.name.Contains("Background"))
                {
                    backgroundImage = img;
                    break;
                }
            }
        }
    }
    
    [ContextMenu("Test Damage")]
    private void TestDamage()
    {
        UpdateHealthDisplay(targetHealth - 25f, maxHealth);
    }
    
    [ContextMenu("Test Full Health")]
    private void TestFullHealth()
    {
        UpdateHealthDisplay(100f, 100f);
    }
}
