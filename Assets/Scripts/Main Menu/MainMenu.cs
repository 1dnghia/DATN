using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Vampire
{
    /// <summary>
    /// Main Menu UI Manager
    /// Manages all button clicks and page navigation without using OnClick events in Inspector
    /// </summary>
    public class MainMenu : MonoBehaviour
    {
        [System.Serializable]
        public struct ButtonPageMapping
        {
            [Tooltip("Button trong Main Menu")]
            public Button button;
            
            [Tooltip("Page/Panel tương ứng sẽ hiện khi click button")]
            public GameObject page;
        }

        [Header("Core References")]
        [SerializeField] private GameObject mainMenuPage;
        [SerializeField] private CharacterSelector characterSelector;
        
        [Header("Button-Page Mappings")]
        [Tooltip("Danh sách các button và page tương ứng")]
        [SerializeField] private List<ButtonPageMapping> buttonPageMappings = new List<ButtonPageMapping>();
        
        [Header("Back Buttons")]
        [Tooltip("Các button Back để quay về Main Menu")]
        [SerializeField] private List<Button> backButtons = new List<Button>();
        
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
            
            // Subscribe to back buttons
            foreach (var backButton in backButtons)
            {
                if (backButton != null)
                    backButton.onClick.AddListener(ShowMainMenu);
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
            
            // Unsubscribe from back buttons
            foreach (var backButton in backButtons)
            {
                if (backButton != null)
                    backButton.onClick.RemoveAllListeners();
            }
            
            // Unsubscribe from exit button
            if (exitButton != null)
                exitButton.onClick.RemoveAllListeners();
        }

        #region Page Navigation

        /// <summary>
        /// Show a specific page and hide all others
        /// </summary>
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

        /// <summary>
        /// Show main menu and hide all other pages
        /// </summary>
        private void ShowMainMenu()
        {
            
            // Hide all pages
            foreach (var mapping in buttonPageMappings)
            {
                if (mapping.page != null)
                    mapping.page.SetActive(false);
            }
            
            // Show main menu
            if (mainMenuPage != null)
                mainMenuPage.SetActive(true);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Start the game with selected character
        /// </summary>
        public void StartGame(CharacterBlueprint characterBlueprint)
        {
            if (characterSelector != null)
                characterSelector.StartGame(characterBlueprint);
            else
                Debug.LogError("MainMenu: CharacterSelector reference is missing!");
        }

        /// <summary>
        /// Quit the game
        /// </summary>
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
