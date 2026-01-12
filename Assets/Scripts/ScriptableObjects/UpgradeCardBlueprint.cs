using UnityEngine;

namespace Vampire
{
    [CreateAssetMenu(fileName = "UpgradeCard", menuName = "Blueprints/Upgrade Card", order = 2)]
    public class UpgradeCardBlueprint : ScriptableObject
    {
        [Header("Display Info")]
        public string upgradeName;  
        public Sprite icon;  
        
        [Header("Upgrade Stats")]
        public UpgradeStatType statType;  
        public float valuePerLevel;  
        public int maxLevel;  
        
        [Header("Cost")]
        public int baseCost;  
        public float costMultiplier = 1.5f;  
        
        
        public int GetCostForLevel(int currentLevel)
        {
            if (currentLevel >= maxLevel)
                return 0;
            
            return Mathf.RoundToInt(baseCost * Mathf.Pow(costMultiplier, currentLevel));
        }
        
        
        public float GetTotalValue(int currentLevel)
        {
            return valuePerLevel * currentLevel;
        }
    }
    
    public enum UpgradeStatType
    {
        MaxHealth,      
        Recovery,       
        Armor,          
        MoveSpeed,      
        Damage,         
        Luck,           
        PickupRange,    
        
    }
}
