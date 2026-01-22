using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Vampire
{
    // Manager để quản lý audio cho buttons trong scene (GameObject độc lập)
    // Kéo thả buttons từ scene vào mảng để tự động setup audio
    // Buttons trong prefab thì tự add AudioTrigger thủ công
    public class AudioTriggerManager : MonoBehaviour
    {
        [Header("Buttons to Manage")]
        [Tooltip("Kéo thả các buttons từ scene vào đây để setup audio.")]
        [SerializeField] private Button[] buttons;
        
        [Header("Audio Settings")]
        [SerializeField] private bool playHoverSound = false;
        [SerializeField] private bool playClickSound = true;

        private Dictionary<Button, AudioTrigger> buttonTriggers = new Dictionary<Button, AudioTrigger>();

        private void Start()
        {
            SetupAllButtons();
        }

        // Tự động thêm AudioTrigger cho các buttons đã gán
        private void SetupAllButtons()
        {
            if (buttons == null || buttons.Length == 0)
            {
                Debug.LogWarning("AudioTriggerManager: No buttons assigned!");
                return;
            }
            
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
                trigger.SetSettings(playHoverSound, playClickSound);
                
                buttonTriggers[button] = trigger;
            }
            
            Debug.Log($"AudioTriggerManager: Setup {buttons.Length} buttons");
        }

        // Update settings cho tất cả buttons
        public void UpdateAllSettings(bool hover, bool click)
        {
            playHoverSound = hover;
            playClickSound = click;

            foreach (var trigger in buttonTriggers.Values)
            {
                if (trigger != null)
                {
                    trigger.SetSettings(playHoverSound, playClickSound);
                }
            }
        }

        // Tự động tìm tất cả buttons trong children (Editor only)
        [ContextMenu("Refresh All Buttons")]
        private void RefreshButtons()
        {
            buttonTriggers.Clear();
            SetupAllButtons();
        }
    }
}
