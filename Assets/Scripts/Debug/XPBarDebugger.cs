using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class XPBarDebugger : MonoBehaviour
{
    [ContextMenu("Debug XP Bar Setup")]
    public void DebugXPBarSetup()
    {
        Debug.Log("=== XP BAR DEBUG INFORMATION ===");
        
        // Find ExperienceBarUI
        ExperienceBarUI xpBarUI = FindFirstObjectByType<ExperienceBarUI>();
        if (xpBarUI == null)
        {
            Debug.LogError("❌ ExperienceBarUI not found in scene!");
            return;
        }
        
        Debug.Log($"✅ Found ExperienceBarUI on: {xpBarUI.gameObject.name}");
        
        // Check XP Fill Image
        if (xpBarUI.xpFillImage == null)
        {
            Debug.LogError("❌ XP Fill Image is not assigned!");
        }
        else
        {
            Image fillImage = xpBarUI.xpFillImage;
            Debug.Log($"✅ XP Fill Image: {fillImage.gameObject.name}");
            Debug.Log($"   - Image Type: {fillImage.type}");
            Debug.Log($"   - Fill Amount: {fillImage.fillAmount:F2}");
            Debug.Log($"   - Fill Method: {fillImage.fillMethod}");
            Debug.Log($"   - Color: {fillImage.color}");
            Debug.Log($"   - Sprite: {(fillImage.sprite != null ? fillImage.sprite.name : "None")}");
            
            if (fillImage.type != Image.Type.Filled)
            {
                Debug.LogWarning("⚠️ XP Fill Image should be Image Type: Filled!");
            }
        }
        
        // Check Player Experience
        PlayerExperience playerExp = FindFirstObjectByType<PlayerExperience>();
        if (playerExp == null)
        {
            Debug.LogError("❌ PlayerExperience not found!");
        }
        else
        {
            Debug.Log($"✅ PlayerExperience found");
            Debug.Log($"   - Current XP: {playerExp.currentExperience:F0}");
            Debug.Log($"   - Required XP: {playerExp.ExperienceRequired:F0}");
            Debug.Log($"   - Progress: {playerExp.ExperienceProgress:P1}");
            Debug.Log($"   - Level: {playerExp.Level}");
        }
        
        // Check UI Settings
        Debug.Log("📋 ExperienceBarUI Settings:");
        Debug.Log($"   - Animate XP Changes: {xpBarUI.animateXPChanges}");
        Debug.Log($"   - Use DOTween: {xpBarUI.useDOTween}");
        Debug.Log($"   - Show Level Text: {xpBarUI.showLevelText}");
        
        Debug.Log("=== END XP BAR DEBUG ===");
    }
    
    [ContextMenu("Force Update XP Bar")]
    public void ForceUpdateXPBar()
    {
        ExperienceBarUI xpBarUI = FindFirstObjectByType<ExperienceBarUI>();
        PlayerExperience playerExp = FindFirstObjectByType<PlayerExperience>();
        
        if (xpBarUI != null && playerExp != null)
        {
            xpBarUI.SetXPImmediate(playerExp.currentExperience, playerExp.ExperienceRequired, playerExp.Level);
            Debug.Log("🔄 Forced XP Bar update");
        }
    }
    
    [ContextMenu("Test Fill Amount")]
    public void TestFillAmount()
    {
        ExperienceBarUI xpBarUI = FindFirstObjectByType<ExperienceBarUI>();
        if (xpBarUI != null && xpBarUI.xpFillImage != null)
        {
            StartCoroutine(TestFillAmountCoroutine(xpBarUI.xpFillImage));
        }
    }
    
    private System.Collections.IEnumerator TestFillAmountCoroutine(Image fillImage)
    {
        Debug.Log("🧪 Testing fill amounts: 0% → 25% → 50% → 75% → 100%");
        
        float[] testValues = { 0f, 0.25f, 0.5f, 0.75f, 1f };
        
        foreach (float value in testValues)
        {
            fillImage.fillAmount = value;
            Debug.Log($"   Set fill amount to: {value:P0}");
            yield return new UnityEngine.WaitForSeconds(1f);
        }
        
        Debug.Log("✅ Fill amount test complete");
    }
}
