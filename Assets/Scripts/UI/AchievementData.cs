using UnityEngine;
using System.Collections.Generic;

namespace Vampire
{
    // ScriptableObject chứa danh sách các achievements với điều kiện check
    [CreateAssetMenu(fileName = "AchievementData", menuName = "Vampire/Achievement Data")]
    public class AchievementData : ScriptableObject
    {
        public enum AchievementType
        {
            SurvivalTime,      // Sống sót X giây
            KillCount,         // Giết X quái
            PlayerLevel,       // Đạt level X
            CoinsCollected,    // Thu thập X coins
            DamageDealt,       // Gây X damage
            MapCompleted       // Hoàn thành map
        }
        
        public enum RewardType
        {
            None,              // Không có thưởng
            Coins,             // Thưởng vàng
            Weapon,            // Unlock vũ khí
            Character          // Unlock nhân vật
        }
        
        [System.Serializable]
        public class Achievement
        {
            [Header("Achievement Info")]
            public AchievementType type;
            public string description;
            public float requiredValue; // Giá trị cần đạt (hoặc map index cho MapCompleted)
            public MapBlueprint requiredMap; // Map bắt buộc (null = mọi map)
            
            [Header("Reward")]
            public RewardType rewardType;
            public int coinReward;                           // Số vàng thưởng (nếu rewardType = Coins)
            public GameObject weaponPrefab;                  // Vũ khí unlock (nếu rewardType = Weapon)
            public CharacterBlueprint characterBlueprint;    // Nhân vật unlock (nếu rewardType = Character)
            
            // Auto-generate ID từ description
            public string GetId()
            {
                return description.ToLower().Replace(" ", "_").Replace(".", "");
            }
            
            // Check xem achievement đã hoàn thành chưa
            public bool CheckCondition(StatsManager stats, int playerLevel, float survivalTime, string currentMap = "")
            {
                // Kiểm tra map nếu có yêu cầu map cụ thể
                if (requiredMap != null)
                {
                    if (string.IsNullOrEmpty(currentMap) || currentMap != requiredMap.name)
                    {
                        return false; // Không đúng map → fail
                    }
                }
                
                // Kiểm tra điều kiện chính
                switch (type)
                {
                    case AchievementType.SurvivalTime:
                        return survivalTime >= requiredValue;
                        
                    case AchievementType.KillCount:
                        return stats.MonstersKilled >= requiredValue;
                        
                    case AchievementType.PlayerLevel:
                        return playerLevel >= requiredValue;
                        
                    case AchievementType.CoinsCollected:
                        return stats.CoinsGained >= requiredValue;
                        
                    case AchievementType.DamageDealt:
                        return stats.DamageDealt >= requiredValue;
                        
                    case AchievementType.MapCompleted:
                        return !string.IsNullOrEmpty(currentMap);
                        
                    default:
                        return false;
                }
            }
        }
        
        [Header("Achievements List")]
        public List<Achievement> achievements = new List<Achievement>();
    }
}
