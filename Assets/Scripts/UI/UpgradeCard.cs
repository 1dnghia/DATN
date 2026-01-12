using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Vampire
{
    public class UpgradeCard : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI upgradeNameText;
        [SerializeField] private Image iconImage;
        [SerializeField] private TextMeshProUGUI statValueText;
        [SerializeField] private Button upgradeButton;
        [SerializeField] private TextMeshProUGUI costText;
        
        [Header("Data")]
        [SerializeField] private UpgradeCardBlueprint blueprint;
        
        private int currentLevel = 0;
        private UpgradeManager upgradeManager;
        
        public void Init(UpgradeCardBlueprint blueprint, UpgradeManager manager)
        {
            this.blueprint = blueprint;
            this.upgradeManager = manager;
            
            
            currentLevel = PlayerPrefs.GetInt($"Upgrade_{blueprint.statType}", 0);
            
            // Setup button
            upgradeButton.onClick.AddListener(OnUpgradeButtonClicked);
            
            // Update UI
            UpdateUI();
        }
        
        private void UpdateUI()
        {
            if (blueprint == null) return;
            
            
            upgradeNameText.text = blueprint.upgradeName;
            
            
            if (blueprint.icon != null)
                iconImage.sprite = blueprint.icon;
            
            
            float totalValue = blueprint.GetTotalValue(currentLevel);
            statValueText.text = $"{totalValue:F1}";
            
            
            if (currentLevel >= blueprint.maxLevel)
            {

                costText.text = "MAX";
                upgradeButton.interactable = false;
            }
            else
            {
                
                int cost = blueprint.GetCostForLevel(currentLevel);
                costText.text = cost.ToString();
                
                
                bool canAfford = upgradeManager.CanAffordUpgrade(cost);
                upgradeButton.interactable = canAfford;
            }
        }
        
        private void OnUpgradeButtonClicked()
        {
            if (currentLevel >= blueprint.maxLevel)
                return;
            
            int cost = blueprint.GetCostForLevel(currentLevel);
            
            // Mua upgrade
            if (upgradeManager.TryPurchaseUpgrade(cost))
            {
                currentLevel++;
                
                
                PlayerPrefs.SetInt($"Upgrade_{blueprint.statType}", currentLevel);
                PlayerPrefs.Save();
                
                
                UpdateUI();
                
                
                upgradeManager.OnUpgradePurchased();
            }
        }
        
        
        public void RefreshUI()
        {
            UpdateUI();
        }
        
        
        public int GetCurrentLevel()
        {
            return currentLevel;
        }
        
        public UpgradeStatType GetStatType()
        {
            return blueprint.statType;
        }
    }
}
