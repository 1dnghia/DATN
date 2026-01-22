using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Vampire
{
    /// Main Menu UI Manager
    /// Manages all button clicks and page navigation without using OnClick events in Inspector
    public class MainMenu : MonoBehaviour
    {
        [System.Serializable]
        public struct ButtonPageMapping
        {
            [Tooltip("Button trong Main Menu")]
            public Button button;
            
            [Tooltip("Page/Panel tÆ°Æ¡ng á»©ng sáº½ hiá»‡n khi click button")]
            public GameObject page;
        }

        [Header("Core References")]
        [SerializeField] private GameObject mainMenuPage;
        [SerializeField] private CharacterSelector characterSelector;
        [SerializeField] private DialogBox mapSelectionDialogBox;
        [SerializeField] private MapSelection mapSelection;
        
        [Header("Settings")]
        [SerializeField] private SettingsMenuPanel settingsMenuPanel;
        
        [Header("Button-Page Mappings")]
        [Tooltip("Danh sÃ¡ch cÃ¡c button vÃ  page tÆ°Æ¡ng á»©ng")]
        [SerializeField] private List<ButtonPageMapping> buttonPageMappings = new List<ButtonPageMapping>();
        
        [Header("Exit Button")]
        [SerializeField] private Button exitButton;
        
        [Header("Settings Button")]
        [SerializeField] private Button settingsButton;

        void Start()
        {
            // Start main menu music
            AudioManager.Instance.PlayMainMenuMusic();
            
            if (characterSelector != null)
                characterSelector.Init();
            
            SubscribeToButtons();
            
            // Show main menu at start
            ShowMainMenu();
        }

        private void OnDestroy()
        {
            UnsubscribeFromButtons();
        }

        private void SubscribeToButtons()
        {
            // Subscribe to button-page mappings
            foreach (var mapping in buttonPageMappings)
            {
                if (mapping.button != null && mapping.page != null)
                {
                    // Capture the page in a local variable to avoid closure issues
                    GameObject pageToShow = mapping.page;
                    mapping.button.onClick.AddListener(() => ShowPage(pageToShow));
                }
            }
            
            // Subscribe to exit button
            if (exitButton != null)
                exitButton.onClick.AddListener(QuitGame);
            
            // Subscribe to settings button
            if (settingsButton != null)
                settingsButton.onClick.AddListener(OpenSettings);
        }

        private void UnsubscribeFromButtons()
        {
            // Unsubscribe from button-page mappings
            foreach (var mapping in buttonPageMappings)
            {
                if (mapping.button != null)
                    mapping.button.onClick.RemoveAllListeners();
            }
            
            // Unsubscribe from exit button
            if (exitButton != null)
                exitButton.onClick.RemoveAllListeners();
            
            // Unsubscribe from settings button
            if (settingsButton != null)
                settingsButton.onClick.RemoveAllListeners();
        }

        #region Page Navigation
        /// Show a specific page and hide all others
        private void ShowPage(GameObject pageToShow)
        {
            // Hide main menu
            if (mainMenuPage != null)
                mainMenuPage.SetActive(false);
            
            // Hide all other pages
            foreach (var mapping in buttonPageMappings)
            {
                if (mapping.page != null)
                    mapping.page.SetActive(false);
            }
            
            // Show the selected page
            pageToShow.SetActive(true);
        }
        /// Hide all pages including map selection
        private void HideAllPages()
        {
            // Hide main menu
            if (mainMenuPage != null)
                mainMenuPage.SetActive(false);
            
            // Hide all other pages
            foreach (var mapping in buttonPageMappings)
            {
                if (mapping.page != null)
                    mapping.page.SetActive(false);
            }
        }
        /// Show main menu and hide all other pages
        public void ShowMainMenu()
        {
            // Hide all pages
            HideAllPages();
            
            // Show main menu
            if (mainMenuPage != null)
                mainMenuPage.SetActive(true);
        }

        #endregion

        #region Public Methods
        /// Show map selection page with selected character
        public void ShowMapSelectionPage(CharacterBlueprint selectedCharacter)
        {
            // Hide all pages
            HideAllPages();
            
            // Init map selection data
            if (mapSelection != null)
            {
                mapSelection.Init(selectedCharacter);
            }
            else
            {
                Debug.LogError("MainMenu: MapSelection reference is missing!");
            }
            
            // Open dialog box
            if (mapSelectionDialogBox != null)
            {
                mapSelectionDialogBox.Open();
            }
            else
            {
                Debug.LogError("MainMenu: MapSelectionDialogBox reference is missing!");
            }
        }
        /// Show character selection page (used by back button from map selection)
        public void ShowCharacterSelectionPage()
        {
            
            foreach (var mapping in buttonPageMappings)
            {
                
                if (mapping.page != null && mapping.page.GetComponentInChildren<CharacterSelector>() != null)
                {
                    ShowPage(mapping.page);
                    return;
                }
            }
            
            Debug.LogWarning("MainMenu: Could not find character selection page!");
        }
        /// Start the game with selected character
        public void StartGame(CharacterBlueprint characterBlueprint)
        {
            if (characterSelector != null)
                characterSelector.StartGame(characterBlueprint);
            else
                Debug.LogError("MainMenu: CharacterSelector reference is missing!");
        }
        /// Quit the game
        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
        
        /// <summary>
        /// Mở Settings Panel (được gọi từ Settings Button)
        /// </summary>
        public void OpenSettings()
        {
            if (settingsMenuPanel != null)
            {
                settingsMenuPanel.OpenSettingsMenu();
                AudioManager.Instance.PlayButtonClick();
            }
            else
            {
                Debug.LogError("MainMenu: SettingsMenuPanel reference is missing!");
            }
        }

        #endregion
    }
}
