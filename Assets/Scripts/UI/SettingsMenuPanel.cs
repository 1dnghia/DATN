using UnityEngine;
using UnityEngine.UI;

namespace Vampire
{
    /// <summary>
    /// Settings Menu Panel - Hiển thị 2 buttons: Volume và Control
    /// Tự quản lý việc show/hide với Main Menu
    /// </summary>
    public class SettingsMenuPanel : MonoBehaviour
    {
        [Header("Main Menu Reference")]
        [SerializeField] private GameObject mainMenuPage;
        
        [Header("Menu Buttons")]
        [SerializeField] private Button volumeButton;
        [SerializeField] private Button controlButton;
        [SerializeField] private Button backButton;
        
        [Header("Sub Panels")]
        [SerializeField] private GameObject volumePanel;
        [SerializeField] private GameObject controlPanel;

        private void Start()
        {
            SetupButtons();
        }

        private void SetupButtons()
        {
            if (volumeButton != null)
                volumeButton.onClick.AddListener(OnVolumeButtonClicked);
            
            if (controlButton != null)
                controlButton.onClick.AddListener(OnControlButtonClicked);
            
            if (backButton != null)
                backButton.onClick.AddListener(OnBackButtonClicked);
        }

        private void OnVolumeButtonClicked()
        {
            AudioManager.Instance.PlayButtonClick();
            OpenVolumePanel();
        }

        private void OnControlButtonClicked()
        {
            AudioManager.Instance.PlayButtonClick();
            OpenControlPanel();
        }

        private void OnBackButtonClicked()
        {
            AudioManager.Instance.PlayButtonClick();
            CloseSettingsMenu();
        }

        /// <summary>
        /// Mở Volume Panel
        /// </summary>
        private void OpenVolumePanel()
        {
            // Ẩn settings menu
            gameObject.SetActive(false);
            
            // Hiển thị volume panel
            if (volumePanel != null)
                volumePanel.SetActive(true);
        }

        /// <summary>
        /// Mở Control Panel
        /// </summary>
        private void OpenControlPanel()
        {
            // Ẩn settings menu
            gameObject.SetActive(false);
            
            // Hiển thị control panel
            if (controlPanel != null)
                controlPanel.SetActive(true);
        }

        /// <summary>
        /// Đóng Settings Menu và quay về Main Menu
        /// </summary>
        private void CloseSettingsMenu()
        {
            // Ẩn settings menu
            gameObject.SetActive(false);
            
            // Hiển thị main menu
            if (mainMenuPage != null)
            {
                mainMenuPage.SetActive(true);
            }
            else
            {
                Debug.LogWarning("SettingsMenuPanel: mainMenuPage reference is missing!");
            }
        }
        
        /// <summary>
        /// Mở Settings Menu từ Main Menu (được gọi từ Settings Button)
        /// </summary>
        public void OpenSettingsMenu()
        {
            // Ẩn main menu
            if (mainMenuPage != null)
            {
                mainMenuPage.SetActive(false);
            }
            
            // Hiển thị settings menu
            gameObject.SetActive(true);
        }

        /// <summary>
        /// Quay lại Settings Menu từ sub-panel
        /// </summary>
        public void ReturnToSettingsMenu()
        {
            // Ẩn tất cả sub-panels
            if (volumePanel != null)
                volumePanel.SetActive(false);
            
            if (controlPanel != null)
                controlPanel.SetActive(false);
            
            // Hiển thị lại settings menu
            gameObject.SetActive(true);
        }

        private void OnDestroy()
        {
            if (volumeButton != null)
                volumeButton.onClick.RemoveListener(OnVolumeButtonClicked);
            
            if (controlButton != null)
                controlButton.onClick.RemoveListener(OnControlButtonClicked);
            
            if (backButton != null)
                backButton.onClick.RemoveListener(OnBackButtonClicked);
        }
    }
}
