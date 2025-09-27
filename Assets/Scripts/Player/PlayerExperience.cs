using UnityEngine;

public class PlayerExperience : MonoBehaviour
{
    [Header("Experience Settings")]
    public int currentLevel = 1;
    public float currentExperience = 0f;
    [SerializeField] private float baseExperienceRequired = 100f; // Base value for calculation
    public float currentLevelExperienceRequired; // Current level's actual XP requirement
    [Range(1.1f, 2.0f)]
    public float experienceMultiplier = 1.2f; // Each level requires 20% more XP
    
    [Header("Experience Gain")]
    public float experienceMultiplierBonus = 1f; // Bonus from upgrades
    
    // Events
    public static System.Action<int> OnLevelUp;
    public static System.Action<float, float> OnExperienceChanged; // current, required
    
    // Properties
    public float ExperienceRequired => currentLevelExperienceRequired;
    public float ExperienceProgress => currentExperience / ExperienceRequired;
    public int Level => currentLevel;
    
    private void Awake()
    {
        // Initialize current level XP requirement
        UpdateCurrentLevelExperienceRequired();
    }
    
    private void Start()
    {
        // Initialize XP UI
        OnExperienceChanged?.Invoke(currentExperience, ExperienceRequired);
    }
    
    // Update current level's XP requirement
    private void UpdateCurrentLevelExperienceRequired()
    {
        currentLevelExperienceRequired = GetExperienceRequiredForLevel(currentLevel + 1);
    }
    
    public void GainExperience(float amount)
    {
        float finalAmount = amount * experienceMultiplierBonus;
        currentExperience += finalAmount;
        
        // Trigger experience gained event
        EventManager.OnExperienceGained?.Invoke(finalAmount);
        OnExperienceChanged?.Invoke(currentExperience, ExperienceRequired);
        
        // Check for level up
        CheckForLevelUp();
    }
    
    private void CheckForLevelUp()
    {
        while (currentExperience >= ExperienceRequired)
        {
            LevelUp();
        }
    }
    
    private void LevelUp()
    {
        // Store values before level up for logging
        float oldXPRequired = ExperienceRequired;
        int oldLevel = currentLevel;
        
        // Remove experience used for this level
        currentExperience -= ExperienceRequired;
        currentLevel++;
        
        // Update XP requirement for new level
        UpdateCurrentLevelExperienceRequired();
        float newXPRequired = ExperienceRequired;
        
        // Trigger events
        OnLevelUp?.Invoke(currentLevel);
        EventManager.OnPlayerLevelUp?.Invoke(currentLevel);
        
        // Update XP UI for new level
        OnExperienceChanged?.Invoke(currentExperience, ExperienceRequired);
        
        // Open upgrade panel
        EventManager.OnUpgradePanelOpen?.Invoke();
    }
    
    // Calculate experience required for a specific level
    private float GetExperienceRequiredForLevel(int level)
    {
        // Formula: baseXP * (multiplier ^ (level - 1))
        // Level 1→2: 100 * 1.2^0 = 100
        // Level 2→3: 100 * 1.2^1 = 120  
        // Level 3→4: 100 * 1.2^2 = 144
        // Level 4→5: 100 * 1.2^3 = 173
        return baseExperienceRequired * Mathf.Pow(experienceMultiplier, level - 1);
    }
    
    // Get total experience required to reach a specific level from level 1
    public float GetTotalExperienceForLevel(int targetLevel)
    {
        float total = 0f;
        for (int i = 1; i < targetLevel; i++)
        {
            total += GetExperienceRequiredForLevel(i);
        }
        return total;
    }
    
    // Upgrade methods (called from upgrade system)
    public void IncreaseExperienceGain(float multiplier)
    {
        experienceMultiplierBonus += multiplier;
    }
    
    private void Reset()
    {
        // Called when Reset is pressed in Unity Inspector
        // Set default values
        currentLevel = 1;
        currentExperience = 0f;
        baseExperienceRequired = 100f;
        experienceMultiplier = 1.2f;
        experienceMultiplierBonus = 1f;
        
        // Update current level XP requirement
        UpdateCurrentLevelExperienceRequired();
    }
    
    // Debug methods
    [ContextMenu("Add 50 XP")]
    private void DebugAddExperience()
    {
        GainExperience(50f);
    }
    
    [ContextMenu("Level Up")]
    private void DebugLevelUp()
    {
        GainExperience(ExperienceRequired);
    }
    
    [ContextMenu("Show XP Table (Next 5 Levels)")]
    private void DebugShowXPTable()
    {
        for (int i = currentLevel; i <= currentLevel + 5; i++)
        {
            float xpForThisLevel = GetExperienceRequiredForLevel(i + 1); // XP needed to go from level i to i+1
            float totalXP = GetTotalExperienceForLevel(i + 1);
            string marker = (i == currentLevel) ? " ← CURRENT" : "";
        }
    }
    
    [ContextMenu("Test Level Progression")]
    private void DebugTestProgression()
    {
        // Reset to level 1 for clean test
        currentLevel = 1;
        currentExperience = 0f;
        UpdateCurrentLevelExperienceRequired();
        
        // Test first 5 level ups
        for (int i = 1; i <= 5; i++)
        {
            float xpNeeded = ExperienceRequired;
            
            // Give exact XP needed
            GainExperience(xpNeeded);
        }
    }
    
    // Get experience values for UI
    public string GetExperienceText()
    {
        return $"{currentExperience:F0}/{ExperienceRequired:F0}";
    }
    
    public string GetLevelText()
    {
        return $"Level {currentLevel}";
    }
    
    // Get progress percentage for XP bar
    public float GetExperienceProgressPercentage()
    {
        return currentExperience / ExperienceRequired;
    }
    
    // Get how much XP needed for next level
    public float GetExperienceNeededForNextLevel()
    {
        return ExperienceRequired - currentExperience;
    }
    
    // Get expected XP requirement for any future level
    public float GetExperienceRequiredForTargetLevel(int targetLevel)
    {
        if (targetLevel <= currentLevel) return 0f;
        return GetExperienceRequiredForLevel(targetLevel);
    }
}
