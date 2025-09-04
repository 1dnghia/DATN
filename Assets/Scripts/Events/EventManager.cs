using UnityEngine;

public static class EventManager
{
    // Player events
    public static System.Action<float> OnPlayerHealthChanged;
    public static System.Action<float> OnPlayerDamaged;
    public static System.Action<float> OnPlayerHealed;
    public static System.Action OnPlayerDied;
    public static System.Action<int> OnPlayerLevelUp;
    public static System.Action<float> OnExperienceGained;
    
    // Enemy events
    public static System.Action<GameObject> OnEnemyKilled;
    public static System.Action<int> OnWaveCompleted;
    
    // Game events
    public static System.Action OnGameStarted;
    public static System.Action OnGamePaused;
    public static System.Action OnGameResumed;
    public static System.Action OnGameOver;
    
    // UI events
    public static System.Action OnUpgradePanelOpen;
    public static System.Action OnUpgradePanelClose;
}
