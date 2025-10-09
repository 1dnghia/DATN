using UnityEngine;
using UnityEngine.UI;

public class HubManager : MonoBehaviour
{
    [Header("Hub UI")]
    [SerializeField] private Button backToMainMenuButton;
    [SerializeField] private Button startGameplayButton;
    
    [Header("Map Selection")]
    [SerializeField] private GameObject mapSelectionPanel;
    [SerializeField] private Button[] mapButtons;
    
    [Header("Upgrade System")]
    [SerializeField] private GameObject upgradePanel;
    [SerializeField] private Button upgradeButton;
    
    private void Start()
    {
        SetupHub();
    }
    
    private void SetupHub()
    {
        // Set game state
        if (SceneGameManager.Instance != null)
        {
            SceneGameManager.Instance.SetGameState(SceneGameState.Hub);
        }
        
        SetupButtons();
        SetupMapSelection();
    }
    
    private void SetupButtons()
    {
        // Back to Main Menu Button
        if (backToMainMenuButton != null)
        {
            backToMainMenuButton.onClick.AddListener(OnBackToMainMenuClicked);
        }
        
        // Start Gameplay Button
        if (startGameplayButton != null)
        {
            startGameplayButton.onClick.AddListener(OnStartGameplayClicked);
        }
        
        // Upgrade Button
        if (upgradeButton != null)
        {
            upgradeButton.onClick.AddListener(OnUpgradeClicked);
        }
    }
    
    private void SetupMapSelection()
    {
        if (mapButtons != null)
        {
            for (int i = 0; i < mapButtons.Length; i++)
            {
                int mapIndex = i; // Capture the index for the lambda
                if (mapButtons[i] != null)
                {
                    mapButtons[i].onClick.AddListener(() => OnMapSelected(mapIndex));
                }
            }
        }
    }
    
    public void OnBackToMainMenuClicked()
    {
        Debug.Log("Returning to main menu...");
        
        if (SceneManager.Instance != null)
        {
            SceneManager.Instance.LoadMainMenu();
        }
    }
    
    public void OnStartGameplayClicked()
    {
        Debug.Log("Starting gameplay...");
        
        // Check if a map is selected
        if (SceneGameManager.Instance != null)
        {
            string selectedMap = SceneGameManager.Instance.GetSelectedMapName();
            if (string.IsNullOrEmpty(selectedMap))
            {
                Debug.LogWarning("No map selected! Please select a map first.");
                ShowMapSelection();
                return;
            }
        }
        
        if (SceneManager.Instance != null)
        {
            SceneManager.Instance.LoadGameplay();
        }
    }
    
    public void OnUpgradeClicked()
    {
        Debug.Log("Opening upgrade panel...");
        
        if (upgradePanel != null)
        {
            upgradePanel.SetActive(!upgradePanel.activeSelf);
        }
    }
    
    public void OnMapSelected(int mapIndex)
    {
        string mapName = $"Map_{mapIndex + 1}";
        Debug.Log($"Selected map: {mapName}");
        
        if (SceneGameManager.Instance != null)
        {
            SceneGameManager.Instance.SetSelectedMap(mapName, mapIndex);
        }
        
        HideMapSelection();
    }
    
    public void ShowMapSelection()
    {
        if (mapSelectionPanel != null)
        {
            mapSelectionPanel.SetActive(true);
        }
    }
    
    public void HideMapSelection()
    {
        if (mapSelectionPanel != null)
        {
            mapSelectionPanel.SetActive(false);
        }
    }
    
    public void CloseUpgradePanel()
    {
        if (upgradePanel != null)
        {
            upgradePanel.SetActive(false);
        }
    }
}