using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Vampire
{
    // Manager để quản lý audio cho nhiều buttons từ 1 chỗ
    public class AudioTriggerManager : MonoBehaviour
    {
        [Header("Buttons to Manage")]
        [SerializeField] private Button[] buttons;
        
        [Header("Audio Settings")]
        [SerializeField] private bool playHoverSound = false;
        [SerializeField] private bool playClickSound = true;
        [SerializeField] private AudioClip customHoverSound;
        [SerializeField] private AudioClip customClickSound;

        private Dictionary<Button, AudioTrigger> buttonTriggers = new Dictionary<Button, AudioTrigger>();

        private void Start()
        {
            SetupAllButtons();
        }

        // Tự động thêm AudioTrigger cho tất cả buttons
        private void SetupAllButtons()
        {
            if (buttons == null) return;

            foreach (Button button in buttons)
            {
                if (button == null) continue;

                // Kiểm tra xem button đã có AudioTrigger chưa
                AudioTrigger trigger = button.GetComponent<AudioTrigger>();
                if (trigger == null)
                {
                    trigger = button.gameObject.AddComponent<AudioTrigger>();
                }

                // Set các thuộc tính từ manager
                trigger.SetSettings(playHoverSound, playClickSound, customHoverSound, customClickSound);
                
                buttonTriggers[button] = trigger;
            }
        }

        // Thêm button mới vào danh sách quản lý
        public void AddButton(Button button)
        {
            if (button == null || buttonTriggers.ContainsKey(button)) return;

            AudioTrigger trigger = button.GetComponent<AudioTrigger>();
            if (trigger == null)
            {
                trigger = button.gameObject.AddComponent<AudioTrigger>();
            }

            trigger.SetSettings(playHoverSound, playClickSound, customHoverSound, customClickSound);
            buttonTriggers[button] = trigger;
        }

        // Xóa button khỏi danh sách quản lý
        public void RemoveButton(Button button)
        {
            if (button == null || !buttonTriggers.ContainsKey(button)) return;

            AudioTrigger trigger = buttonTriggers[button];
            if (trigger != null)
            {
                Destroy(trigger);
            }
            buttonTriggers.Remove(button);
        }

        // Update settings cho tất cả buttons
        public void UpdateAllSettings(bool hover, bool click, AudioClip hoverClip = null, AudioClip clickClip = null)
        {
            playHoverSound = hover;
            playClickSound = click;
            customHoverSound = hoverClip;
            customClickSound = clickClip;

            foreach (var trigger in buttonTriggers.Values)
            {
                if (trigger != null)
                {
                    trigger.SetSettings(playHoverSound, playClickSound, customHoverSound, customClickSound);
                }
            }
        }

        // Tự động tìm tất cả buttons trong children (Editor only)
        [ContextMenu("Auto Find All Buttons")]
        private void AutoFindButtons()
        {
            buttons = GetComponentsInChildren<Button>(true);
            Debug.Log($"Found {buttons.Length} buttons");
        }

        // Clear tất cả AudioTriggers (Editor only)
        [ContextMenu("Clear All Audio Triggers")]
        private void ClearAllTriggers()
        {
            foreach (var button in buttons)
            {
                if (button == null) continue;
                
                AudioTrigger trigger = button.GetComponent<AudioTrigger>();
                if (trigger != null)
                {
                    DestroyImmediate(trigger);
                }
            }
            buttonTriggers.Clear();
            Debug.Log("Cleared all AudioTriggers");
        }
    }
}
