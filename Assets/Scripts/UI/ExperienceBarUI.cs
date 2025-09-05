using UnityEngine;
using UnityEngine.UI;
using TMPro;

#if DOTWEEN_ENABLED
using DG.Tweening;
#endif

public class ExperienceBarUI : MonoBehaviour
{
    [Header("Experience Bar Components")]
    public Image xpFillImage; // Primary - for custom assets with fillAmount
    public TextMeshProUGUI xpText;
    public TextMeshProUGUI levelText;
    public Image backgroundImage;
    
    [Header("Visual Settings")]
    public Color xpColor = new Color(0f, 0.8f, 1f); // Cyan
    public Color xpGlowColor = Color.white;
    public Color backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.8f);
    
    [Header("Display Options")]
    public bool showXPNumbers = true;
    public bool showLevelText = true;
    public bool fullWidthBar = true;
    
    [Header("Animation")]
    public bool animateXPChanges = true;
    public float animationSpeed = 3f;
    
    [Header("DOTween Settings")]
    public bool useDOTween = true;
    public float dotweenDuration = 0.5f;
    #pragma warning disable 0414
    [SerializeField] private int dotweenEaseType = 6; // OutQuart
    #pragma warning restore 0414
    public bool enableLevelUpPunch = true;
    public float levelUpPunchScale = 1.2f;
    
    [Header("Level Up Effects")]
    public ParticleSystem levelUpParticles;
    public AudioSource levelUpAudioSource;
    public AudioClip levelUpSound;
    
    // Private variables
    private float targetXP;
    private float currentDisplayXP;
    private float maxXP = 100f;
    private int currentLevel = 1;
    private Color originalFillColor;
    
    #if DOTWEEN_ENABLED
    private Tween fillTween;
    private Tween colorTween;
    private Tween scaleTween;
    #pragma warning disable 0414
    private bool isDOTweenAvailable = true;
    #pragma warning restore 0414
    #else
    #pragma warning disable 0414
    private bool isDOTweenAvailable = false;
    #pragma warning restore 0414
    #endif
    
    private void Start()
    {
        // Subscribe to player XP events
        PlayerExperience.OnExperienceChanged += UpdateXPDisplay;
        PlayerExperience.OnLevelUp += OnLevelUp;
        
        // Store original colors
        if (xpFillImage != null)
        {
            originalFillColor = xpFillImage.color;
        }
        
        // Set initial XP color
        if (xpFillImage != null)
        {
            xpFillImage.color = xpColor;
            originalFillColor = xpColor;
        }
        
        // Set background color
        if (backgroundImage != null)
        {
            backgroundImage.color = backgroundColor;
        }
        
        // Hide XP numbers if disabled
        if (!showXPNumbers && xpText != null)
        {
            xpText.gameObject.SetActive(false);
        }
        
        // Hide level text if disabled
        if (!showLevelText && levelText != null)
        {
            levelText.gameObject.SetActive(false);
        }
        
        // Setup full width layout if enabled
        if (fullWidthBar)
        {
            SetupFullWidthLayout();
        }
    }
    
    /// <summary>
    /// Public method for immediate XP display initialization
    /// </summary>
    public void SetXPImmediate(float currentXP, float maxXP, int level)
    {
        targetXP = currentXP;
        this.maxXP = maxXP;
        currentLevel = level;
        
        float progress = maxXP > 0 ? currentXP / maxXP : 0f;
        progress = Mathf.Clamp01(progress);
        
        // Set immediately without animation
        if (xpFillImage != null)
        {
            xpFillImage.fillAmount = progress;
        }
        
        UpdateLevelText();
        UpdateXPText();
    }
    
    private void OnDestroy()
    {
        // Unsubscribe from events
        PlayerExperience.OnExperienceChanged -= UpdateXPDisplay;
        PlayerExperience.OnLevelUp -= OnLevelUp;
        
        #if DOTWEEN_ENABLED
        // Kill any ongoing tweens
        fillTween?.Kill();
        colorTween?.Kill();
        scaleTween?.Kill();
        #endif
    }
    
    private void Update()
    {
        #if DOTWEEN_ENABLED
        // Skip lerp update if DOTween is handling animations
        if (isDOTweenAvailable && useDOTween) return;
        #endif
        
        // Fallback smooth animation without DOTween
        if (animateXPChanges && Mathf.Abs(currentDisplayXP - targetXP) > 0.01f)
        {
            currentDisplayXP = Mathf.Lerp(currentDisplayXP, targetXP, Time.unscaledDeltaTime * animationSpeed);
            
            float progress = maxXP > 0 ? currentDisplayXP / maxXP : 0f;
            progress = Mathf.Clamp01(progress);
            
            if (xpFillImage != null)
            {
                xpFillImage.fillAmount = progress;
            }
            
            UpdateXPText();
        }
        else if (!animateXPChanges)
        {
            // Immediate update without animation
            currentDisplayXP = targetXP;
            float progress = maxXP > 0 ? targetXP / maxXP : 0f;
            progress = Mathf.Clamp01(progress);
            
            if (xpFillImage != null)
            {
                xpFillImage.fillAmount = progress;
            }
        }
    }
    
    private void UpdateXPDisplay(float currentXP, float requiredXP)
    {
        // Get experience data
        PlayerExperience playerExp = FindFirstObjectByType<PlayerExperience>();
        if (playerExp == null) return;
        
        targetXP = currentXP;
        maxXP = requiredXP;
        currentLevel = playerExp.Level;
        
        // Calculate progress (0 to 1)
        float targetProgress = maxXP > 0 ? targetXP / maxXP : 0f;
        targetProgress = Mathf.Clamp01(targetProgress);
        
        #if DOTWEEN_ENABLED
        // Use DOTween if available and enabled
        if (isDOTweenAvailable && useDOTween)
        {
            AnimateXPWithDOTween(targetProgress);
        }
        else
        #endif
        {
            // Fallback to immediate update or legacy lerp
            if (!animateXPChanges)
            {
                currentDisplayXP = targetXP;
                if (xpFillImage != null)
                {
                    xpFillImage.fillAmount = targetProgress;
                }
            }
        }
        
        UpdateLevelText();
        UpdateXPText();
    }
    
    #if DOTWEEN_ENABLED
    private void AnimateXPWithDOTween(float targetProgress)
    {
        // Kill existing animation
        fillTween?.Kill();
        
        if (xpFillImage != null)
        {
            // Animate fillAmount for custom assets
            fillTween = xpFillImage.DOFillAmount(targetProgress, dotweenDuration)
                .SetEase((Ease)dotweenEaseType)
                .SetUpdate(true);
        }
        
        // Update display value immediately for text updates
        currentDisplayXP = targetXP;
    }
    #endif
    
    private void UpdateLevelText()
    {
        if (levelText != null && showLevelText)
        {
            levelText.text = $"Level {currentLevel}";
        }
    }
    
    private void UpdateXPText()
    {
        if (xpText != null && showXPNumbers)
        {
            xpText.text = $"{Mathf.RoundToInt(currentDisplayXP)}/{Mathf.RoundToInt(maxXP)} XP";
        }
    }
    
    private void OnLevelUp(int newLevel)
    {
        currentLevel = newLevel;
        UpdateLevelText();
        
        // Play level up effects
        #if DOTWEEN_ENABLED
        if (isDOTweenAvailable && useDOTween && enableLevelUpPunch)
        {
            PlayLevelUpEffectsDOTween();
        }
        else
        #endif
        {
            PlayLevelUpEffectsLegacy();
        }
        
        // Play particle effect
        if (levelUpParticles != null)
        {
            levelUpParticles.Play();
        }
        
        // Play sound effect
        if (levelUpAudioSource != null && levelUpSound != null)
        {
            levelUpAudioSource.PlayOneShot(levelUpSound);
        }
    }
    
    #if DOTWEEN_ENABLED
    private void PlayLevelUpEffectsDOTween()
    {
        if (levelText != null)
        {
            // Scale punch effect
            scaleTween?.Kill();
            scaleTween = levelText.transform.DOPunchScale(Vector3.one * (levelUpPunchScale - 1f), 0.3f, 1, 0.5f)
                .SetUpdate(true);
            
            // Color flash effect
            colorTween?.Kill();
            if (xpFillImage != null)
            {
                colorTween = xpFillImage.DOColor(xpGlowColor, 0.1f)
                    .SetLoops(2, LoopType.Yoyo)
                    .OnComplete(() => xpFillImage.color = originalFillColor)
                    .SetUpdate(true);
            }
        }
    }
    #endif
    
    private void PlayLevelUpEffectsLegacy()
    {
        // Simple flash effect without DOTween
        if (xpFillImage != null)
        {
            xpFillImage.color = xpGlowColor;
            Invoke(nameof(RestoreOriginalColor), 0.2f);
        }
    }
    
    private void RestoreOriginalColor()
    {
        if (xpFillImage != null)
        {
            xpFillImage.color = originalFillColor;
        }
    }
    
    private void SetupFullWidthLayout()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            // Set to stretch full width
            rectTransform.anchorMin = new Vector2(0, 1);
            rectTransform.anchorMax = new Vector2(1, 1);
            rectTransform.anchoredPosition = new Vector2(0, -30);
            rectTransform.sizeDelta = new Vector2(0, 40);
        }
    }
    
    private void Reset()
    {
        // Auto-assign components when script is first added
        if (xpFillImage == null)
        {
            // Look for child Image with "Fill" in name
            Image[] images = GetComponentsInChildren<Image>();
            foreach (Image img in images)
            {
                if (img.gameObject.name.Contains("Fill"))
                {
                    xpFillImage = img;
                    break;
                }
            }
        }
        
        if (levelText == null)
        {
            // Look for child TextMeshPro with "Level" in name
            TextMeshProUGUI[] texts = GetComponentsInChildren<TextMeshProUGUI>();
            foreach (TextMeshProUGUI text in texts)
            {
                if (text.gameObject.name.Contains("Level"))
                {
                    levelText = text;
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
    
    [ContextMenu("Test Level Up")]
    private void TestLevelUp()
    {
        OnLevelUp(currentLevel + 1);
    }
    
    [ContextMenu("Test XP Gain")]
    private void TestXPGain()
    {
        UpdateXPDisplay(targetXP + 25f, maxXP);
    }
}
