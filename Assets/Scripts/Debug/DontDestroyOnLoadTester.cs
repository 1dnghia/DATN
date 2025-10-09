using UnityEngine;

public class DontDestroyOnLoadTester : MonoBehaviour
{
    [Header("Test DontDestroyOnLoad")]
    [SerializeField] private bool testOnStart = true;
    
    private void Start()
    {
        if (testOnStart)
        {
            TestDontDestroyOnLoad();
        }
    }
    
    [ContextMenu("Test DontDestroyOnLoad")]
    public void TestDontDestroyOnLoad()
    {
        Debug.Log("=== TESTING DONTDESTROYONLOAD ===");
        Debug.Log("Step 1: Press Play");
        Debug.Log("Step 2: Look at Hierarchy panel");
        Debug.Log("Step 3: You should see a scene called 'DontDestroyOnLoad'");
        Debug.Log("Step 4: Expand it to see persistent GameObjects");
        Debug.Log("===============================");
        
        // Find all DontDestroyOnLoad objects
        GameObject[] rootObjects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
        
        Debug.Log($"Current active scene: {UnityEngine.SceneManagement.SceneManager.GetActiveScene().name}");
        Debug.Log($"Total scenes loaded: {UnityEngine.SceneManagement.SceneManager.sceneCount}");
        
        for (int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCount; i++)
        {
            var scene = UnityEngine.SceneManagement.SceneManager.GetSceneAt(i);
            Debug.Log($"Scene {i}: {scene.name} (Root objects: {scene.rootCount})");
            
            if (scene.name == "DontDestroyOnLoad")
            {
                Debug.Log("✓ Found DontDestroyOnLoad scene!");
                var dontDestroyRoots = scene.GetRootGameObjects();
                foreach (var obj in dontDestroyRoots)
                {
                    Debug.Log($"  - {obj.name}");
                }
            }
        }
    }
    
    private void Update()
    {
        // Press Space to test
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TestDontDestroyOnLoad();
        }
    }
}