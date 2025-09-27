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
    public TextMeshProUGUI levelText;
    public Image backgroundImage;
    
    [Header("Visual Settings")]
    public Color xpGlowColor = Color.white; // For level up effects only
    
    [Header("Display Options")]
    public bool showLevelText = true;
    
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
        
        // Store original colors (don't override user settings)
        if (xpFillImage != null)
        {
            originalFillColor = xpFillImage.color;
        }
        
        // Hide level text if disabled
        if (!showLevelText && levelText != null)
        {
            levelText.gameObject.SetActive(false);
        }
        
        // Initialize XP display with current player data
        InitializeXPDisplay();
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
    }
    
    /// <summary>
    /// Initialize XP display with current player data at start
    /// </summary>
    private void InitializeXPDisplay()
    {
        PlayerExperience playerExp = FindFirstObjectByType<PlayerExperience>();
        if (playerExp != null)
        {
            // Get current values from player
            float currentXP = playerExp.currentExperience;
            float requiredXP = playerExp.ExperienceRequired;
            int level = playerExp.Level;
            
            // Set values without animation
            SetXPImmediate(currentXP, requiredXP, level);
        }
        else
        {
            #if UNITY_EDITOR
            // PlayerExperience will be found later or assigned manually
            #endif
        }
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
        
        #if UNITY_EDITOR
        Debug.Log($"[ExperienceBarUI] Updating XP: {currentXP:F0}/{requiredXP:F0} ({targetProgress:P1}) Level {currentLevel}");
        #endif
        
        #if DOTWEEN_ENABLED
        // Use DOTween if available and enabled
        if (isDOTweenAvailable && useDOTween)
        {
            AnimateXPWithDOTween(targetProgress);
        }
        else
        #endif
        {
            // Fallback: Let Update() handle animation if animateXPChanges is true
            // Or update immediately if animateXPChanges is false
            if (!animateXPChanges)
            {
                currentDisplayXP = targetXP;
                if (xpFillImage != null)
                {
                    xpFillImage.fillAmount = targetProgress;
                }
            }
            // If animateXPChanges is true, Update() method will handle the smooth transition
        }
        
        UpdateLevelText();
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
    
    private void Reset()
    {
        // Auto-assign components when script is first added or Reset is pressed
        
        // Auto-find XP Fill Image (XP_Fill, Fill, etc.)
        if (xpFillImage == null)
        {
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
        
        // Auto-find Level Text (LevelText, Level, etc.)
        if (levelText == null)
        {
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
        
        // Auto-find Background Image (XP_Background, Background, etc.)
        if (backgroundImage == null)
        {
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
        
        #if UNITY_EDITOR
        if (xpFillImage != null || levelText != null || backgroundImage != null)
        {
            UnityEngine.Debug.Log($"[ExperienceBarUI] Auto-assigned components on {gameObject.name}:");
            if (xpFillImage != null) UnityEngine.Debug.Log($"  ✅ XP Fill: {xpFillImage.gameObject.name}");
            if (levelText != null) UnityEngine.Debug.Log($"  ✅ Level Text: {levelText.gameObject.name}");
            if (backgroundImage != null) UnityEngine.Debug.Log($"  ✅ Background: {backgroundImage.gameObject.name}");
        }
        #endif
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
