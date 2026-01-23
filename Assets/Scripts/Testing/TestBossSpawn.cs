using UnityEngine;

namespace Vampire
{
    /// Test script to instantly spawn the final boss
    /// Attach this to any GameObject in the scene to test boss fight
    /// Press B key to spawn boss immediately
    /// Press T key to skip to boss time (levelTime)
    public class TestBossSpawn : MonoBehaviour
    {
        [SerializeField] private LevelManager levelManager;
        [SerializeField] private bool autoFindLevelManager = true;
        
        [Header("Test Options")]
        [Tooltip("Set time to this value when pressing T key")]
        [SerializeField] private float skipToTime = 600f; // Default level time

        private void Start()
        {
            if (autoFindLevelManager && levelManager == null)
            {
                levelManager = FindFirstObjectByType<LevelManager>();
                
                if (levelManager != null)
                {
                    Debug.Log("TestBossSpawn: LevelManager found successfully.");
                }
                else
                {
                    Debug.LogWarning("TestBossSpawn: Searching for LevelManager in scene...");
                    // Try alternative method
                    var allManagers = FindObjectsByType<LevelManager>(FindObjectsSortMode.None);
                    Debug.Log($"Found {allManagers.Length} LevelManager(s) in scene.");
                }
            }

            if (levelManager == null)
            {
                Debug.LogError("TestBossSpawn: LevelManager not found! Please assign it in the inspector.");
                Debug.LogError("Make sure LevelManager GameObject exists and is active in the scene hierarchy.");
            }
        }

        private void Update()
        {
            if (levelManager == null) return;

            // Press B to force spawn boss
            if (Input.GetKeyDown(KeyCode.B))
            {
                ForceSpawnBoss();
            }

            // Press T to skip time to boss spawn time
            if (Input.GetKeyDown(KeyCode.T))
            {
                SkipToBossTime();
            }

            // Press N to show current time
            if (Input.GetKeyDown(KeyCode.N))
            {
                ShowCurrentTime();
            }
        }
        /// Forces the boss to spawn immediately by manipulating the level time
        public void ForceSpawnBoss()
        {
            // Use reflection to set the private levelTime field
            var levelTimeField = typeof(LevelManager).GetField("levelTime", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
            var levelBlueprintField = typeof(LevelManager).GetField("levelBlueprint", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            if (levelTimeField != null && levelBlueprintField != null)
            {
                var levelBlueprint = levelBlueprintField.GetValue(levelManager) as LevelBlueprint;
                
                if (levelBlueprint != null)
                {
                    // Set levelTime to just past the boss spawn time
                    float bossSpawnTime = levelBlueprint.levelTime + 0.1f;
                    levelTimeField.SetValue(levelManager, bossSpawnTime);
                }
                else
                {
                    Debug.LogError("TestBossSpawn: LevelBlueprint is null!");
                }
            }
            else
            {
                Debug.LogError("TestBossSpawn: Could not access levelTime or levelBlueprint field!");
            }
        }
        /// Skips time to the configured skip time (default: boss spawn time)
        public void SkipToBossTime()
        {
            var levelTimeField = typeof(LevelManager).GetField("levelTime", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            if (levelTimeField != null)
            {
                levelTimeField.SetValue(levelManager, skipToTime);
            }
            else
            {
                Debug.LogError("TestBossSpawn: Could not access levelTime field!");
            }
        }
        /// Shows the current level time in the console
        public void ShowCurrentTime()
        {
            var levelTimeField = typeof(LevelManager).GetField("levelTime", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
            var levelBlueprintField = typeof(LevelManager).GetField("levelBlueprint", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            if (levelTimeField != null && levelBlueprintField != null)
            {
                float currentTime = (float)levelTimeField.GetValue(levelManager);
                var levelBlueprint = levelBlueprintField.GetValue(levelManager) as LevelBlueprint;
                
                if (levelBlueprint != null)
                {
                    float bossTime = levelBlueprint.levelTime;
                    float timeRemaining = bossTime - currentTime;
                }
            }
        }
        /// Add time instantly (useful for testing)
        public void AddTime(float seconds)
        {
            var levelTimeField = typeof(LevelManager).GetField("levelTime", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            if (levelTimeField != null)
            {
                float currentTime = (float)levelTimeField.GetValue(levelManager);
                float newTime = currentTime + seconds;
                levelTimeField.SetValue(levelManager, newTime);
            }
        }
    }
}
