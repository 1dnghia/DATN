using UnityEngine;

namespace Vampire
{
    /// <summary>
    /// Test script to instantly spawn the final boss
    /// Attach this to any GameObject in the scene to test boss fight
    /// Press B key to spawn boss immediately
    /// Press T key to skip to boss time (levelTime)
    /// </summary>
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
                levelManager = FindObjectOfType<LevelManager>();
            }

            if (levelManager == null)
            {
                Debug.LogError("TestBossSpawn: LevelManager not found! Please assign it in the inspector.");
            }
            else
            {
                Debug.Log("TestBossSpawn: Press B to spawn boss, Press T to skip to boss time");
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

        /// <summary>
        /// Forces the boss to spawn immediately by manipulating the level time
        /// </summary>
        public void ForceSpawnBoss()
        {
            Debug.Log("TestBossSpawn: Forcing boss spawn...");
            
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
                    Debug.Log($"TestBossSpawn: Set level time to {bossSpawnTime}. Boss should spawn in next Update!");
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

        /// <summary>
        /// Skips time to the configured skip time (default: boss spawn time)
        /// </summary>
        public void SkipToBossTime()
        {
            Debug.Log($"TestBossSpawn: Skipping to time {skipToTime}...");
            
            var levelTimeField = typeof(LevelManager).GetField("levelTime", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            if (levelTimeField != null)
            {
                levelTimeField.SetValue(levelManager, skipToTime);
                Debug.Log($"TestBossSpawn: Time set to {skipToTime} seconds");
            }
            else
            {
                Debug.LogError("TestBossSpawn: Could not access levelTime field!");
            }
        }

        /// <summary>
        /// Shows the current level time in the console
        /// </summary>
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
                    
                    Debug.Log($"TestBossSpawn: Current Time = {currentTime:F1}s | Boss Spawn Time = {bossTime:F1}s | Time Remaining = {timeRemaining:F1}s");
                }
            }
        }

        /// <summary>
        /// Add time instantly (useful for testing)
        /// </summary>
        public void AddTime(float seconds)
        {
            var levelTimeField = typeof(LevelManager).GetField("levelTime", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            if (levelTimeField != null)
            {
                float currentTime = (float)levelTimeField.GetValue(levelManager);
                float newTime = currentTime + seconds;
                levelTimeField.SetValue(levelManager, newTime);
                Debug.Log($"TestBossSpawn: Added {seconds}s. New time: {newTime:F1}s");
            }
        }
    }
}
