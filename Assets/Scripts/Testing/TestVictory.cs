using UnityEngine;

namespace Vampire
{
    /// Test script to simulate instant victory condition
    /// Attach this to any GameObject in the scene to test the victory screen
    /// Press V key to trigger victory
    public class TestVictory : MonoBehaviour
    {
        [SerializeField] private LevelManager levelManager;
        [SerializeField] private bool autoFindLevelManager = true;

        private void Start()
        {
            if (autoFindLevelManager && levelManager == null)
            {
                levelManager = FindFirstObjectByType<LevelManager>();
            }

            if (levelManager == null)
            {
                Debug.LogError("TestVictory: LevelManager not found! Please assign it in the inspector.");
            }
        }

        private void Update()
        {
            // Press V to trigger victory
            if (Input.GetKeyDown(KeyCode.V))
            {
                TriggerVictory();
            }
        }
        /// Simulates winning the game by calling LevelPassed
        public void TriggerVictory()
        {
            if (levelManager == null)
            {
                Debug.LogError("TestVictory: Cannot trigger victory - LevelManager is null!");
                return;
            }
            
            // Call LevelPassed with null (since we don't have a real boss monster)
            // Note: LevelPassed expects a Monster parameter but doesn't actually use it
            levelManager.LevelPassed(null);
        }
        /// Alternative method using reflection to access private method if needed
        public void TriggerVictoryViaReflection()
        {
            if (levelManager == null)
            {
                Debug.LogError("TestVictory: Cannot trigger victory - LevelManager is null!");
                return;
            }
            
            var method = typeof(LevelManager).GetMethod("LevelPassed", 
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            
            if (method != null)
            {
                method.Invoke(levelManager, new object[] { null });
            }
            else
            {
                Debug.LogError("TestVictory: Could not find LevelPassed method!");
            }
        }
    }
}
