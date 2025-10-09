using UnityEngine;

public static class GameConstants
{
    // Layer names
    public const string PLAYER_LAYER = "Player";
    public const string ENEMY_LAYER = "Enemy";
    public const string PROJECTILE_LAYER = "Projectile";
    public const string PICKUP_LAYER = "Pickup";
    
    // Tags
    public const string PLAYER_TAG = "Player";
    public const string ENEMY_TAG = "Enemy";
    public const string WALL_TAG = "Wall";
    
    // Scene names
    public const string MAIN_MENU_SCENE = "MainMenuScene";
    public const string PERSISTENT_SCENE = "PersistentScene";
    public const string HUB_SCENE = "HubScene";
    public const string GAMEPLAY_SCENE = "GameplayScene";
    
    // Loading settings
    public const float LOADING_MIN_DURATION = 0.5f;
    public const float SCENE_TRANSITION_DELAY = 0.1f;
    
    // Input keys
    public const KeyCode PAUSE_KEY = KeyCode.Escape;
    public const KeyCode INTERACT_KEY = KeyCode.E;
    public const KeyCode INVENTORY_KEY = KeyCode.Tab;
    
    // Audio mixer groups
    public const string MASTER_VOLUME = "MasterVolume";
    public const string SFX_VOLUME = "SFXVolume";
    public const string MUSIC_VOLUME = "MusicVolume";
    
    // Player prefs keys
    public const string HIGH_SCORE_KEY = "HighScore";
    public const string VOLUME_KEY = "Volume";
    public const string CONTROLS_KEY = "Controls";
}
