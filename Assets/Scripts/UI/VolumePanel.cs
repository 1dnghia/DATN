using UnityEngine;
using UnityEngine.UI;

namespace Vampire
{
    /// <summary>
    /// Volume Panel - Chứa 2 sliders cho Music và SFX
    /// </summary>
    public class VolumePanel : MonoBehaviour
    {
        [Header("Back Button")]
        [SerializeField] private Button backButton;
        
        private SettingsMenuPanel settingsMenuPanel;

        private void Start()
        {
            SetupButtons();
            FindSettingsMenuPanel();
        }

        private void SetupButtons()
        {
            if (backButton != null)
                backButton.onClick.AddListener(OnBackButtonClicked);
        }

        private void FindSettingsMenuPanel()
        {
            // Tìm SettingsMenuPanel trong parent
            settingsMenuPanel = GetComponentInParent<SettingsMenuPanel>();
            
            if (settingsMenuPanel == null)
            {
                // Tìm trong cùng Canvas
                settingsMenuPanel = FindFirstObjectByType<SettingsMenuPanel>();
            }
        }

        private void OnBackButtonClicked()
        {
            AudioManager.Instance.PlayButtonClick();
            ReturnToSettingsMenu();
        }

        /// <summary>
        /// Quay lại Settings Menu
        /// </summary>
        private void ReturnToSettingsMenu()
        {
            if (settingsMenuPanel != null)
            {
                settingsMenuPanel.ReturnToSettingsMenu();
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        private void OnDestroy()
        {
            if (backButton != null)
                backButton.onClick.RemoveListener(OnBackButtonClicked);
        }
    }
}
