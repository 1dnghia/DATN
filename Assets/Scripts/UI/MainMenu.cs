using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Main Menu Buttons")]
    [SerializeField] private Button startGameButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button exitButton;
    
    [Header("Panels")]
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject creditsPanel;
    
    private void Start()
    {
        SetupButtons();
    }
    
    private void SetupButtons()
    {
        // Start Game Button
        if (startGameButton != null)
        {
            startGameButton.onClick.AddListener(OnStartGameClicked);
        }
        
        // Settings Button
        if (settingsButton != null)
        {
            settingsButton.onClick.AddListener(OnSettingsClicked);
        }
        
        // Credits Button
        if (creditsButton != null)
        {
            creditsButton.onClick.AddListener(OnCreditsClicked);
        }
        
        // Exit Button
        if (exitButton != null)
        {
            exitButton.onClick.AddListener(OnExitClicked);
        }
    }
    
    public void OnStartGameClicked()
    {
        Debug.Log("Starting game...");
        
        if (SceneManager.Instance != null)
        {
            SceneManager.Instance.LoadHub();
        }
        else
        {
            Debug.LogError("SceneManager instance not found!");
        }
    }
    
    public void OnSettingsClicked()
    {
        Debug.Log("Opening settings...");
        
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true);
        }
    }
    
    public void OnCreditsClicked()
    {
        Debug.Log("Opening credits...");
        
        if (creditsPanel != null)
        {
            creditsPanel.SetActive(true);
        }
    }
    
    public void OnExitClicked()
    {
        Debug.Log("Exiting game...");
        
        if (SceneManager.Instance != null)
        {
            SceneManager.Instance.ExitGame();
        }
        else
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }
    }
    
    public void CloseSettingsPanel()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
    }
    
    public void CloseCreditsPanel()
    {
        if (creditsPanel != null)
        {
            creditsPanel.SetActive(false);
        }
    }
}
