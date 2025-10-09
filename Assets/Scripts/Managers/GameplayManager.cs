using UnityEngine;
using UnityEngine.UI;

public class GameplayManager : MonoBehaviour
{
    [Header("Gameplay UI")]
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button backToHubButton;
    [SerializeField] private GameObject pausePanel;
    
    [Header("Game Info")]
    [SerializeField] private TMPro.TextMeshProUGUI mapNameText;
    
    private bool isPaused = false;
    
    private void Start()
    {
        SetupGameplay();
    }
    
    private void Update()
    {
        // Handle pause input
        if (Input.GetKeyDown(GameConstants.PAUSE_KEY))
        {
            TogglePause();
        }
    }
    
    private void SetupGameplay()
    {
        // Set game state
        if (SceneGameManager.Instance != null)
        {
            SceneGameManager.Instance.SetGameState(SceneGameState.Gameplay);
            
            // Display selected map name
            if (mapNameText != null)
            {
                string selectedMap = SceneGameManager.Instance.GetSelectedMapName();
                mapNameText.text = !string.IsNullOrEmpty(selectedMap) ? selectedMap : "Unknown Map";
            }
        }
        
        SetupButtons();
        
        // Ensure game is not paused
        ResumeGame();
    }
    
    private void SetupButtons()
    {
        // Pause Button
        if (pauseButton != null)
        {
            pauseButton.onClick.AddListener(TogglePause);
        }
        
        // Back to Hub Button
        if (backToHubButton != null)
        {
            backToHubButton.onClick.AddListener(OnBackToHubClicked);
        }
    }
    
    public void TogglePause()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }
    
    public void PauseGame()
    {
        isPaused = true;
        
        if (SceneGameManager.Instance != null)
        {
            SceneGameManager.Instance.PauseGame();
        }
        
        if (pausePanel != null)
        {
            pausePanel.SetActive(true);
        }
        
        Debug.Log("Game paused");
    }
    
    public void ResumeGame()
    {
        isPaused = false;
        
        if (SceneGameManager.Instance != null)
        {
            SceneGameManager.Instance.ResumeGame();
        }
        
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
        
        Debug.Log("Game resumed");
    }
    
    public void OnBackToHubClicked()
    {
        Debug.Log("Returning to hub...");
        
        // Resume game before leaving
        ResumeGame();
        
        if (SceneManager.Instance != null)
        {
            SceneManager.Instance.LoadHub();
        }
    }
    
    public void RestartLevel()
    {
        Debug.Log("Restarting level...");
        
        // Resume game before restarting
        ResumeGame();
        
        if (SceneManager.Instance != null)
        {
            SceneManager.Instance.ReloadCurrentScene();
        }
    }
    
    public void OnGameOver()
    {
        Debug.Log("Game Over!");
        PauseGame();
        // Here you can show game over UI
    }
    
    public void OnLevelComplete()
    {
        Debug.Log("Level Complete!");
        // Here you can show level complete UI
        // And automatically return to hub after some time
    }
}