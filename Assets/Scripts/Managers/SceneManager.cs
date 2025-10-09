using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    public static SceneManager Instance;
    
    [Header("Scene Names")]
    [SerializeField] private string mainMenuSceneName = GameConstants.MAIN_MENU_SCENE;
    [SerializeField] private string persistentSceneName = GameConstants.PERSISTENT_SCENE;
    [SerializeField] private string hubSceneName = GameConstants.HUB_SCENE;
    [SerializeField] private string gameplaySceneName = GameConstants.GAMEPLAY_SCENE;
    
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
        // Validate PersistentScene is running
        if (!IsPersistentSceneLoaded())
        {
            return;
        }
        
        // Auto load MainMenu after PersistentScene is initialized
        if (GetCurrentSceneName() == GameConstants.PERSISTENT_SCENE)
        {
            LoadMainMenu();
        }
    }
    
    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }
    
    public void LoadMainMenu()
    {
        LoadScene(mainMenuSceneName);
    }
    
    public void LoadPersistent()
    {
        LoadScene(persistentSceneName);
    }
    
    public void LoadHub()
    {
        LoadScene(hubSceneName);
    }
    
    public void LoadGameplay()
    {
        LoadScene(gameplaySceneName);
    }
    
    public void ReloadCurrentScene()
    {
        string currentScene = GetCurrentSceneName();
        LoadScene(currentScene);
    }
    
    private IEnumerator LoadSceneAsync(string sceneName)
    {
        // Don't reload PersistentScene if it's already loaded
        if (sceneName == persistentSceneName)
        {
            yield break;
        }
        
        // Show loading UI
        if (LoadingManager.Instance != null)
        {
            LoadingManager.Instance.ShowLoading();
        }
        
        // Small delay to ensure loading UI is visible
        yield return new WaitForSeconds(GameConstants.SCENE_TRANSITION_DELAY);
        
        // Unload current scene if it's not PersistentScene
        string currentScene = GetNonPersistentSceneName();
        if (!string.IsNullOrEmpty(currentScene))
        {
            yield return UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(currentScene);
        }
        
        // Always use Additive mode to keep PersistentScene running
        LoadSceneMode loadMode = LoadSceneMode.Additive;

        // Start loading the scene
        AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, loadMode);
        
        // Don't let the scene activate automatically
        asyncLoad.allowSceneActivation = false;
        
        // Wait until the scene is almost loaded (90%)
        while (asyncLoad.progress < 0.9f)
        {
            if (LoadingManager.Instance != null)
            {
                LoadingManager.Instance.UpdateLoadingProgress(asyncLoad.progress);
            }
            yield return null;
        }
        
        // Update loading to 100%
        if (LoadingManager.Instance != null)
        {
            LoadingManager.Instance.UpdateLoadingProgress(1f);
        }
        
        // Wait a bit more for smooth transition
        yield return new WaitForSeconds(GameConstants.LOADING_MIN_DURATION);
        
        // Hide loading UI
        if (LoadingManager.Instance != null)
        {
            LoadingManager.Instance.HideLoading();
        }
        
        // Activate the scene
        asyncLoad.allowSceneActivation = true;
        
        // Wait for scene to be fully loaded
        yield return asyncLoad;
        
        // Set the newly loaded scene as active scene
        Scene newScene = UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneName);
        if (newScene.isLoaded)
        {
            UnityEngine.SceneManagement.SceneManager.SetActiveScene(newScene);
            
            // Fix Audio Listener issues after scene load
            FixAudioListeners();
        }
    }
    
    private void FixAudioListeners()
    {
        AudioListener[] listeners = FindObjectsByType<AudioListener>(FindObjectsSortMode.None);
        
        if (listeners.Length > 1)
        {
   
            // Disable all listeners first
            foreach (var listener in listeners)
            {
                listener.enabled = false;
            }
            
            // Enable only the first one found
            if (listeners.Length > 0)
            {
                listeners[0].enabled = true;
            }
        }
    }
    
    public void ExitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
    
    // Utility methods for getting scene names
    public string GetCurrentSceneName()
    {
        // Return active scene name (should not be PersistentScene after initial load)
        string activeSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        return activeSceneName;
    }
    
    public string GetNonPersistentSceneName()
    {
        // Get the currently active non-persistent scene
        for (int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCount; i++)
        {
            Scene scene = UnityEngine.SceneManagement.SceneManager.GetSceneAt(i);
            if (scene.name != persistentSceneName && scene.isLoaded)
            {
                return scene.name;
            }
        }
        return "";
    }
    
    public string GetMainMenuSceneName()
    {
        return mainMenuSceneName;
    }
    
    public string GetPersistentSceneName()
    {
        return persistentSceneName;
    }
    
    public string GetHubSceneName()
    {
        return hubSceneName;
    }
    
    public string GetGameplaySceneName()
    {
        return gameplaySceneName;
    }
    
    // Check current scene type
    public bool IsInMainMenu()
    {
        return GetCurrentSceneName() == mainMenuSceneName;
    }
    
    public bool IsInHub()
    {
        return GetCurrentSceneName() == hubSceneName;
    }
    
    public bool IsInGameplay()
    {
        return GetCurrentSceneName() == gameplaySceneName;
    }
    
    public bool IsPersistentSceneLoaded()
    {
        // Check if PersistentScene is loaded (should always be true after game start)
        for (int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCount; i++)
        {
            Scene scene = UnityEngine.SceneManagement.SceneManager.GetSceneAt(i);
            if (scene.name == persistentSceneName)
            {
                return true;
            }
        }
        return false;
    }
}