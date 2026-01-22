using UnityEngine;
using System.Collections.Generic;

namespace Vampire
{
    // Manager để hiển thị và check achievements
    // Đặt trong Main Menu scene và gán AchievementData
    public class AchievementManager : MonoBehaviour
    {
        [Header("Achievement Data")]
        [SerializeField] private AchievementData achievementData;
        
        [Header("UI References")]
        [SerializeField] private GameObject achievementCardPrefab;
        [SerializeField] private Transform achievementContainer;
        
        private Dictionary<string, bool> achievementStatus = new Dictionary<string, bool>();
        private Dictionary<string, AchievementCard> achievementCards = new Dictionary<string, AchievementCard>();
        
        // Tự động tạo cards mỗi khi dialog được mở
        private void OnEnable()
        {
            InitializeAchievements();
        }
        
        // Spawn tất cả achievement cards
        private void InitializeAchievements()
        {
            if (achievementData == null)
            {
                Debug.LogError("AchievementManager: AchievementData is null!");
                return;
            }
            
            if (achievementCardPrefab == null || achievementContainer == null)
            {
                Debug.LogError("AchievementManager: Missing Card Prefab or Container!");
                return;
            }
            
            // Clear existing cards
            foreach (Transform child in achievementContainer)
            {
                Destroy(child.gameObject);
            }
            achievementCards.Clear();
            
            // Sắp xếp: chưa hoàn thành lên trên, đã hoàn thành xuống dưới
            var sortedAchievements = new System.Collections.Generic.List<AchievementData.Achievement>(achievementData.achievements);
            sortedAchievements.Sort((a, b) => 
            {
                bool aCompleted = IsAchievementUnlocked(a.GetId());
                bool bCompleted = IsAchievementUnlocked(b.GetId());
                return aCompleted.CompareTo(bCompleted); // false (0) trước true (1)
            });
            
            // Create achievement cards
            foreach (var achievement in sortedAchievements)
            {
                GameObject cardObj = Instantiate(achievementCardPrefab, achievementContainer);
                AchievementCard card = cardObj.GetComponent<AchievementCard>();
                
                if (card != null)
                {
                    string id = achievement.GetId();
                    card.Setup(id, achievement.description);
                    achievementCards[id] = card;
                    
                    // Kiểm tra nếu đã unlock trước đó thì mark completed luôn
                    if (IsAchievementUnlocked(id))
                    {
                        card.MarkAsCompleted();
                    }
                }
                else
                {
                    Debug.LogError("AchievementManager: Card prefab missing AchievementCard component!");
                }
            }
        }
        
        // Check tất cả achievements sau khi game over (gọi từ LevelManager)
        public void CheckAllAchievements(StatsManager stats, int playerLevel, float survivalTime, string currentMap = "")
        {
            if (achievementData == null) return;
            
            foreach (var achievement in achievementData.achievements)
            {
                string id = achievement.GetId();
                
                // Chỉ check những achievement chưa unlock
                if (!IsAchievementUnlocked(id))
                {
                    // Check điều kiện
                    if (achievement.CheckCondition(stats, playerLevel, survivalTime, currentMap))
                    {
                        UnlockAchievement(id);
                        GiveReward(achievement);
                    }
                }
            }
        }
        
        // Trao thưởng cho achievement
        private void GiveReward(AchievementData.Achievement achievement)
        {
            switch (achievement.rewardType)
            {
                case AchievementData.RewardType.Coins:
                    int currentCoins = PlayerPrefs.GetInt("Coins", 0);
                    PlayerPrefs.SetInt("Coins", currentCoins + achievement.coinReward);
                    PlayerPrefs.Save();
                    break;
                    
                case AchievementData.RewardType.Weapon:
                    if (achievement.weaponPrefab != null)
                    {
                        string weaponId = achievement.weaponPrefab.name;
                        PlayerPrefs.SetInt($"Weapon_{weaponId}_Unlocked", 1);
                        PlayerPrefs.Save();
                    }
                    break;
                    
                case AchievementData.RewardType.Character:
                    if (achievement.characterBlueprint != null)
                    {
                        string charId = achievement.characterBlueprint.name;
                        PlayerPrefs.SetInt($"Character_{charId}_Unlocked", 1);
                        PlayerPrefs.Save();
                    }
                    break;
                    
                case AchievementData.RewardType.None:
                default:
                    break;
            }
        }
        public void UnlockAchievement(string achievementId)
        {
            if (!IsAchievementUnlocked(achievementId))
            {
                PlayerPrefs.SetInt($"Achievement_{achievementId}", 1);
                PlayerPrefs.Save();
                achievementStatus[achievementId] = true;
                
                // Update UI card nếu có
                if (achievementCards.ContainsKey(achievementId))
                {
                    achievementCards[achievementId].MarkAsCompleted();
                }
                
                // TODO: Show notification popup
            }
        }
        
        // Check nếu achievement đã unlock
        public bool IsAchievementUnlocked(string achievementId)
        {
            if (achievementStatus.ContainsKey(achievementId))
                return achievementStatus[achievementId];
                
            bool unlocked = PlayerPrefs.GetInt($"Achievement_{achievementId}", 0) == 1;
            achievementStatus[achievementId] = unlocked;
            return unlocked;
        }
        
        // Reset tất cả achievements (for testing)
        [ContextMenu("Reset All Achievements")]
        public void ResetAllAchievements()
        {
            achievementStatus.Clear();
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }
    }
}
