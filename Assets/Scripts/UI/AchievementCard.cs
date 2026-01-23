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
        [SerializeField] private Image checkmarkImage; // Thay đổi: Dùng Image thay vì GameObject
        
        private string achievementId;
        
        public void Setup(string id, string description)
        {
            achievementId = id;
            achievementText.text = description;
            
            // QUAN TRỌNG: Tách checkmarkImage khỏi Toggle control trước khi set bất cứ thứ gì
            if (checkboxToggle != null && checkmarkImage != null)
            {
                // Xóa checkmarkImage khỏi Toggle's graphic control
                checkboxToggle.targetGraphic = null;
                checkboxToggle.graphic = null;
                
                // Đảm bảo Toggle không ảnh hưởng đến checkmark
                if (checkboxToggle.graphic == checkmarkImage)
                {
                    checkboxToggle.graphic = null;
                }
            }
            
            // Load trạng thái đã hoàn thành chưa từ PlayerPrefs
            bool isCompleted = PlayerPrefs.GetInt($"Achievement_{achievementId}", 0) == 1;
            
            // Update checkmark TRƯỚC khi set toggle state
            UpdateCheckmarkVisibility(isCompleted);
            
            // Set toggle state SAU khi đã update checkmark
            if (checkboxToggle != null)
            {
                checkboxToggle.isOn = isCompleted;
                checkboxToggle.interactable = false; // Không cho user tự tick
            }
        }
        
        private void UpdateCheckmarkVisibility(bool isCompleted)
        {
            if (checkmarkImage != null)
            {
                // Luôn giữ GameObject active
                if (!checkmarkImage.gameObject.activeSelf)
                {
                    checkmarkImage.gameObject.SetActive(true);
                }
                
                // Luôn giữ Image component enabled
                if (!checkmarkImage.enabled)
                {
                    checkmarkImage.enabled = true;
                }
                
                // Set alpha dựa vào completion: 1 = hiện, 0 = ẩn
                Color color = checkmarkImage.color;
                color.a = isCompleted ? 1f : 0f;
                checkmarkImage.color = color;
            }
        }
        
        public void MarkAsCompleted()
        {
            if (checkboxToggle != null && !checkboxToggle.isOn)
            {
                checkboxToggle.isOn = true;
                PlayerPrefs.SetInt($"Achievement_{achievementId}", 1);
                PlayerPrefs.Save();
                
                // Update checkmark visibility
                UpdateCheckmarkVisibility(true);
            }
        }
        
        public bool IsCompleted()
        {
            return checkboxToggle.isOn;
        }
    }
}
