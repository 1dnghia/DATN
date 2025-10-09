using UnityEngine;

public class SceneGameManager : MonoBehaviour
{
    public static SceneGameManager Instance;
    
    [Header("Game State")]
    [SerializeField] private bool isGamePaused = false;
    [SerializeField] private bool isInHub = false;
    [SerializeField] private bool isInGameplay = false;
    
    [Header("Selected Map")]
    [SerializeField] private string selectedMapName = "";
    [SerializeField] private int selectedMapIndex = 0;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void Start()
    {
        // Initialize game state
        Time.timeScale = 1f;
        isGamePaused = false;
    }
    
    public void SetGameState(SceneGameState state)
    {
        switch (state)
        {
            case SceneGameState.MainMenu:
                isInHub = false;
                isInGameplay = false;
                break;
            case SceneGameState.Hub:
                isInHub = true;
                isInGameplay = false;
                break;
            case SceneGameState.Gameplay:
                isInHub = false;
                isInGameplay = true;
                break;
        }
    }
    
    public void PauseGame()
    {
        isGamePaused = true;
        Time.timeScale = 0f;
    }
    
    public void ResumeGame()
    {
        isGamePaused = false;
        Time.timeScale = 1f;
    }
    
    public void SetSelectedMap(string mapName, int mapIndex)
    {
        selectedMapName = mapName;
        selectedMapIndex = mapIndex;
        Debug.Log($"Selected map: {mapName} (Index: {mapIndex})");
    }
    
    public string GetSelectedMapName()
    {
        return selectedMapName;
    }
    
    public int GetSelectedMapIndex()
    {
        return selectedMapIndex;
    }
    
    public bool IsInHub()
    {
        return isInHub;
    }
    
    public bool IsInGameplay()
    {
        return isInGameplay;
    }
    
    public bool IsGamePaused()
    {
        return isGamePaused;
    }
}

public enum SceneGameState
{
    MainMenu,
    Hub,
    Gameplay
}