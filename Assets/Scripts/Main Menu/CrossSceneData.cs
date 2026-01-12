using System.Collections.Generic;
using UnityEngine;

namespace Vampire
{
    public static class CrossSceneData
    {
        public static CharacterBlueprint CharacterBlueprint { get; set; }
        public static MapBlueprint CurrentMap { get; set; }
        /// Load all meta upgrades from PlayerPrefs and return their total values
        public static Dictionary<UpgradeStatType, float> LoadMetaUpgrades()
        {
            Dictionary<UpgradeStatType, float> upgrades = new Dictionary<UpgradeStatType, float>();
            
            // Load all upgrade types from ScriptableObject blueprints
            
            foreach (UpgradeStatType statType in System.Enum.GetValues(typeof(UpgradeStatType)))
            {
                string key = $"Upgrade_{statType}";
                int level = PlayerPrefs.GetInt(key, 0);
                
                if (level > 0)
                {
                    
                    
                    
                    
                    string valueKey = $"UpgradeValue_{statType}";
                    float valuePerLevel = PlayerPrefs.GetFloat(valueKey, 0);
                    float totalValue = valuePerLevel * level;
                    
                    upgrades[statType] = totalValue;
                }
                else
                {
                    upgrades[statType] = 0;
                }
            }
            
            return upgrades;
        }
    }
}
