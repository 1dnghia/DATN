using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Vampire
{
    /// <summary>
    /// Quản lý Pause Menu trong gameplay
    /// Có 3 button: Continue, Settings, Return to Main Menu
    /// </summary>
    public class PauseMenu : MonoBehaviour
    {
        [Header("Pause Button")]
        [SerializeField] private Button pauseButton;
        [SerializeField] private Sprite pauseSprite, playSprite;
        
        [Header("Pause Menu Panel")]
        [SerializeField] private GameObject pauseMenuPanel;
        
        [Header("Menu Buttons")]
        [SerializeField] private Button continueButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button mainMenuButton;
        
        [Header("Settings Panel")]
        [SerializeField] private GameObject settingsMenuPanel;
        [SerializeField] private SettingsMenuPanel settingsMenuScript;
        
        [Header("Confirmation Dialog")]
        [SerializeField] private GameObject confirmationDialog;
        [SerializeField] private Button confirmYesButton;
        [SerializeField] private Button confirmNoButton;
        
        private bool paused = false;
        private bool timeIsFrozen = false;

        public bool TimeIsFrozen { set => timeIsFrozen = value; }
        public bool IsPaused => paused;

        private void Start()
        {
            SetupButtons();
            
            // Ẩn pause menu, settings panel và confirmation dialog ban đầu
            if (pauseMenuPanel != null)
                pauseMenuPanel.SetActive(false);
            
            if (settingsMenuPanel != null)
                settingsMenuPanel.SetActive(false);
            
            if (confirmationDialog != null)
                confirmationDialog.SetActive(false);
        }

        private void SetupButtons()
        {
            // Subscribe pause button
            if (pauseButton != null)
            {
                pauseButton.onClick.RemoveAllListeners(); // Xóa các listener cũ
                pauseButton.onClick.AddListener(PlayPause);
            }
            
            // Subscribe menu buttons
            if (continueButton != null)
            {
                continueButton.onClick.RemoveAllListeners();
                continueButton.onClick.AddListener(OnContinueClicked);
            }
            
            if (settingsButton != null)
            {
                settingsButton.onClick.RemoveAllListeners();
                settingsButton.onClick.AddListener(OnSettingsClicked);
            }
            
            if (mainMenuButton != null)
            {
                mainMenuButton.onClick.RemoveAllListeners(); // ⚠️ QUAN TRỌNG: Xóa OnClick event cũ
                mainMenuButton.onClick.AddListener(OnMainMenuClicked);
            }
            
            // Subscribe confirmation dialog buttons
            if (confirmYesButton != null)
            {
                confirmYesButton.onClick.RemoveAllListeners();
                confirmYesButton.onClick.AddListener(OnConfirmYes);
            }
            
            if (confirmNoButton != null)
            {
                confirmNoButton.onClick.RemoveAllListeners();
                confirmNoButton.onClick.AddListener(OnConfirmNo);
            }
        }

        /// <summary>
        /// Toggle pause/resume game
        /// </summary>
        public void PlayPause()
        {
            if (paused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

        /// <summary>
        /// Pause game
        /// </summary>
        public void PauseGame()
        {
            paused = true;
            
            if (!timeIsFrozen)
                Time.timeScale = 0;
            
            // Ẩn pause button khi mở pause menu
            if (pauseButton != null)
            {
                pauseButton.gameObject.SetActive(false);
            }
            
            if (pauseMenuPanel != null)
                pauseMenuPanel.SetActive(true);
            
            AudioManager.Instance.PlayMenuOpen();
            
            Debug.Log("PauseMenu: Game paused");
        }

        /// <summary>
        /// Resume game
        /// </summary>
        public void ResumeGame()
        {
            paused = false;
            
            if (!timeIsFrozen)
                Time.timeScale = 1;
            
            // Hiện lại pause button khi resume
            if (pauseButton != null)
            {
                pauseButton.gameObject.SetActive(true);
            }
            
            if (pauseMenuPanel != null)
                pauseMenuPanel.SetActive(false);
            
            // Đóng settings nếu đang mở
            if (settingsMenuPanel != null && settingsMenuPanel.activeSelf)
                CloseSettings();
            
            AudioManager.Instance.PlayMenuClose();
            
            Debug.Log("PauseMenu: Game resumed");
        }

        #region Button Callbacks

        private void OnContinueClicked()
        {
            AudioManager.Instance.PlayButtonClick();
            ResumeGame();
        }

        private void OnSettingsClicked()
        {
            AudioManager.Instance.PlayButtonClick();
            OpenSettings();
        }

        private void OnMainMenuClicked()
        {
            AudioManager.Instance.PlayButtonClick();
            ShowConfirmationDialog();
        }

        #endregion

        #region Confirmation Dialog

        /// <summary>
        /// Hiển thị hộp thoại xác nhận thoát về Main Menu
        /// </summary>
        private void ShowConfirmationDialog()
        {
            // Ẩn pause menu
            if (pauseMenuPanel != null)
                pauseMenuPanel.SetActive(false);
            
            // Hiện confirmation dialog
            if (confirmationDialog != null)
                confirmationDialog.SetActive(true);
            
            AudioManager.Instance.PlayMenuOpen();
            Debug.Log("PauseMenu: Showing confirmation dialog");
        }

        /// <summary>
        /// User click "Yes" - Xác nhận thoát
        /// </summary>
        private void OnConfirmYes()
        {
            AudioManager.Instance.PlayButtonClick();
            ReturnToMainMenu();
        }

        /// <summary>
        /// User click "No" - Hủy, quay lại Pause Menu
        /// </summary>
        private void OnConfirmNo()
        {
            AudioManager.Instance.PlayButtonClick();
            
            // Ẩn confirmation dialog
            if (confirmationDialog != null)
                confirmationDialog.SetActive(false);
            
            // Hiện lại pause menu
            if (pauseMenuPanel != null)
                pauseMenuPanel.SetActive(true);
            
            AudioManager.Instance.PlayMenuClose();
            Debug.Log("PauseMenu: Cancelled return to main menu");
        }

        #endregion

        #region Settings Management

        /// <summary>
        /// Mở Settings Menu Panel
        /// </summary>
        private void OpenSettings()
        {
            // Ẩn pause menu
            if (pauseMenuPanel != null)
                pauseMenuPanel.SetActive(false);
            
            // Hiển thị settings menu panel
            if (settingsMenuPanel != null)
            {
                settingsMenuPanel.SetActive(true);
            }
            
            Debug.Log("PauseMenu: Opened Settings");
        }

        /// <summary>
        /// Đóng Settings Menu Panel và quay lại Pause Menu
        /// </summary>
        public void CloseSettings()
        {
            // Ẩn settings menu panel
            if (settingsMenuPanel != null)
                settingsMenuPanel.SetActive(false);
            
            // Hiển thị lại pause menu nếu game đang pause
            if (paused && pauseMenuPanel != null)
                pauseMenuPanel.SetActive(true);
            
            Debug.Log("PauseMenu: Closed Settings");
        }

        #endregion

        #region Scene Management

        /// <summary>
        /// Trở về Main Menu (Scene 0)
        /// </summary>
        private void ReturnToMainMenu()
        {
            // Resume time trước khi load scene
            Time.timeScale = 1;
            paused = false;
            
            // Load Main Menu scene
            Debug.Log("PauseMenu: Returning to Main Menu");
            SceneManager.LoadScene(0);
        }

        #endregion

        private void OnDestroy()
        {
            // Clean up listeners
            if (pauseButton != null)
                pauseButton.onClick.RemoveListener(PlayPause);
            
            if (continueButton != null)
                continueButton.onClick.RemoveListener(OnContinueClicked);
            
            if (settingsButton != null)
                settingsButton.onClick.RemoveListener(OnSettingsClicked);
            
            if (mainMenuButton != null)
                mainMenuButton.onClick.RemoveListener(OnMainMenuClicked);
            
            if (confirmYesButton != null)
                confirmYesButton.onClick.RemoveListener(OnConfirmYes);
            
            if (confirmNoButton != null)
                confirmNoButton.onClick.RemoveListener(OnConfirmNo);
        }
    }
}
