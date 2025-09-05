using UnityEngine;

public class SimplePlayerUIManager : MonoBehaviour
{
    [Header("UI References - Single Canvas Approach")]
    public Canvas gameUI; // Main Canvas
    public ExperienceBarUI experienceBar;
    public ScreenSpaceHealthBar healthBar;
    public LevelUpVFX levelUpVFX;
    
    [Header("Auto-Assignment")]
    public bool autoAssignComponents = true;
    
    private void Awake()
    {
        if (autoAssignComponents)
        {
            AutoAssignComponents();
        }
    }
    
    private void Start()
    {
        InitializeUI();
    }
    
    private void AutoAssignComponents()
    {
        // Auto-find main Canvas
        if (gameUI == null)
        {
            Canvas[] canvases = FindObjectsByType<Canvas>(FindObjectsSortMode.None);
            foreach (Canvas canvas in canvases)
            {
                if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
                {
                    gameUI = canvas;
                    break;
                }
            }
        }
        
        // Auto-find ExperienceBarUI
        if (experienceBar == null)
            experienceBar = FindFirstObjectByType<ExperienceBarUI>();
            
        // Auto-find ScreenSpaceHealthBar
        if (healthBar == null)
            healthBar = FindFirstObjectByType<ScreenSpaceHealthBar>();
            
        // Auto-find LevelUpVFX
        if (levelUpVFX == null)
            levelUpVFX = FindFirstObjectByType<LevelUpVFX>();
    }
    
    private void InitializeUI()
    {
        // All UI components will initialize themselves
        // This method is here for future expansion
        Debug.Log("Player UI initialized with single Canvas approach");
    }
    
    // Public methods for manual control
    public void ShowUI()
    {
        if (gameUI != null)
            gameUI.gameObject.SetActive(true);
    }
    
    public void HideUI()
    {
        if (gameUI != null)
            gameUI.gameObject.SetActive(false);
    }
    
    public void SetUIAlpha(float alpha)
    {
        if (gameUI != null)
        {
            CanvasGroup canvasGroup = gameUI.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
                canvasGroup = gameUI.gameObject.AddComponent<CanvasGroup>();
            canvasGroup.alpha = alpha;
        }
    }
    
    // Manual assignment methods
    public void SetExperienceBar(ExperienceBarUI newExperienceBar)
    {
        experienceBar = newExperienceBar;
    }
    
    public void SetHealthBar(ScreenSpaceHealthBar newHealthBar)
    {
        healthBar = newHealthBar;
    }
    
    public void SetLevelUpVFX(LevelUpVFX newLevelUpVFX)
    {
        levelUpVFX = newLevelUpVFX;
    }
    
    // Reset method for Unity Inspector
    private void Reset()
    {
        AutoAssignComponents();
    }
    
    // Debug methods
    [ContextMenu("Debug - Test UI Hide/Show")]
    private void TestUIVisibility()
    {
        if (gameUI != null)
        {
            gameUI.gameObject.SetActive(!gameUI.gameObject.activeSelf);
        }
    }
    
    [ContextMenu("Debug - Find All UI Components")]
    private void DebugFindComponents()
    {
        Debug.Log($"Canvas: {gameUI?.name ?? "Not Found"}");
        Debug.Log($"Experience Bar: {experienceBar?.name ?? "Not Found"}");
        Debug.Log($"Health Bar: {healthBar?.name ?? "Not Found"}");
        Debug.Log($"Level Up VFX: {levelUpVFX?.name ?? "Not Found"}");
    }
}
