using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Vampire
{
    // Component cho Achievement Card với text và checkbox
    public class AchievementCard : MonoBehaviour
    {
        [Header("UI Components")]
        [SerializeField] private TextMeshProUGUI achievementText;
        [SerializeField] private Toggle checkboxToggle;
        
        private string achievementId;
        
        public void Setup(string id, string description)
        {
            achievementId = id;
            achievementText.text = description;
            
            // Load trạng thái đã hoàn thành chưa
            bool isCompleted = PlayerPrefs.GetInt($"Achievement_{achievementId}", 0) == 1;
            checkboxToggle.isOn = isCompleted;
            checkboxToggle.interactable = false; // Không cho user tự tick
            
            // Đổi màu text nếu đã hoàn thành
            UpdateCompletedState(isCompleted);
        }
        
        private void UpdateCompletedState(bool isCompleted)
        {
            // Không cần làm gì, chỉ cần checkbox
        }
        
        public void MarkAsCompleted()
        {
            if (!checkboxToggle.isOn)
            {
                checkboxToggle.isOn = true;
                PlayerPrefs.SetInt($"Achievement_{achievementId}", 1);
                PlayerPrefs.Save();
            }
        }
        
        public bool IsCompleted()
        {
            return checkboxToggle.isOn;
        }
    }
}
