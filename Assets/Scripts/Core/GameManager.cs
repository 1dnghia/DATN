using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Game State")]
    public bool isGameStarted = false;
    public bool isGamePaused = false;
    public bool isGameOver = false;
    
    [Header("Player Reference")]
    public PlayerController player;
    
    [Header("UI References")]
    public SimplePlayerUIManager uiManager;
    
    [Header("Game Settings")]
    public bool startGameOnAwake = true;
    public bool pauseOnEscape = true;
    
    // Singleton pattern
    public static GameManager Instance { get; private set; }
    
    private void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        // Auto-find components
        if (player == null)
            player = FindFirstObjectByType<PlayerController>();
            
        if (uiManager == null)
            uiManager = FindFirstObjectByType<SimplePlayerUIManager>();
    }
    
    private void Start()
    {
        // Subscribe to events
        PlayerStats.OnPlayerDeath += OnPlayerDeath;
        
        // Start game if enabled
        if (startGameOnAwake)
        {
            StartGame();
        }
    }
    
    private void OnDestroy()
    {
        // Unsubscribe from events
        PlayerStats.OnPlayerDeath -= OnPlayerDeath;
    }
    
    private void Update()
    {
        // Handle pause with Escape key
        if (pauseOnEscape && Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused)
                ResumeGame();
            else
                PauseGame();
        }
        
        // Debug keys
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
    }
    
    // Game Flow Methods
    public void StartGame()
    {
        isGameStarted = true;
        isGamePaused = false;
        isGameOver = false;
        
        Time.timeScale = 1f;
        
        // Show UI
        if (uiManager != null)
            uiManager.ShowUI();
        
        // Fire event
        EventManager.OnGameStarted?.Invoke();
    }
    
    public void PauseGame()
    {
        if (!isGameStarted || isGameOver) return;
        
        isGamePaused = true;
        Time.timeScale = 0f;
        
        // You can add pause UI here
    }
    
    public void ResumeGame()
    {
        if (!isGameStarted || isGameOver) return;
        
        isGamePaused = false;
        Time.timeScale = 1f;
    }
    
    public void GameOver()
    {
        if (isGameOver) return;
        
        isGameOver = true;
        isGameStarted = false;
        Time.timeScale = 0f;
        
        // Fire event
        EventManager.OnGameOver?.Invoke();
        
        // Auto restart after 3 seconds for testing
        Invoke(nameof(RestartGame), 3f);
    }
    
    public void RestartGame()
    {
        // Reset time scale
        Time.timeScale = 1f;
        
        // Reload current scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }
    
    // Event Handlers
    private void OnPlayerDeath()
    {
        GameOver();
    }
    
    // Public Getters
    public bool IsGameActive()
    {
        return isGameStarted && !isGamePaused && !isGameOver;
    }
    
    public bool CanPlayerMove()
    {
        return IsGameActive();
    }
    
    // Debug Methods
    [ContextMenu("Debug - Start Game")]
    private void DebugStartGame()
    {
        StartGame();
    }
    
    [ContextMenu("Debug - Game Over")]
    private void DebugGameOver()
    {
        GameOver();
    }
    
    [ContextMenu("Debug - Restart Game")]
    private void DebugRestart()
    {
        RestartGame();
    }
}
