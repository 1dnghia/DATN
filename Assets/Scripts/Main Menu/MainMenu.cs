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
        
        [Header("Button-Page Mappings")]
        [Tooltip("Danh sÃ¡ch cÃ¡c button vÃ  page tÆ°Æ¡ng á»©ng")]
        [SerializeField] private List<ButtonPageMapping> buttonPageMappings = new List<ButtonPageMapping>();
        
        [Header("Exit Button")]
        [SerializeField] private Button exitButton;

        void Start()
        {
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
            Debug.Log("MainMenu: ShowMapSelectionPage called with character: " + (selectedCharacter != null ? selectedCharacter.name : "null"));
            
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
            Debug.Log("MainMenu: Quitting game...");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        #endregion
    }
}
