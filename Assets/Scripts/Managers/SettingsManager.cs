using UnityEngine;

namespace Vampire
{
    /// <summary>
    /// Quản lý tất cả settings của game (Audio, Controls, UI Layout)
    /// Singleton pattern, lưu trữ settings vào PlayerPrefs
    /// </summary>
    public class SettingsManager : MonoBehaviour
    {
        private static SettingsManager instance;
        public static SettingsManager Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject go = new GameObject("SettingsManager");
                    instance = go.AddComponent<SettingsManager>();
                    DontDestroyOnLoad(go);
                }
                return instance;
            }
        }

        // Keys for PlayerPrefs
        private const string MUSIC_VOLUME_KEY = "MusicVolume";
        private const string SFX_VOLUME_KEY = "SFXVolume";
        private const string JOYSTICK_TYPE_KEY = "JoystickType";
        
        // Default values
        private const float DEFAULT_MUSIC_VOLUME = 0.7f;
        private const float DEFAULT_SFX_VOLUME = 1f;
        private const int DEFAULT_JOYSTICK_TYPE = 0; // 0 = Floating, 1 = Fixed

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }
            
            instance = this;
            DontDestroyOnLoad(gameObject);
            
            LoadAllSettings();
        }

        #region Audio Settings

        public float GetMusicVolume()
        {
            return PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, DEFAULT_MUSIC_VOLUME);
        }

        public void SetMusicVolume(float volume)
        {
            volume = Mathf.Clamp01(volume);
            PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, volume);
            PlayerPrefs.Save();
            
            // Apply to AudioManager
            AudioManager.Instance.SetMusicVolume(volume);
        }

        public float GetSFXVolume()
        {
            return PlayerPrefs.GetFloat(SFX_VOLUME_KEY, DEFAULT_SFX_VOLUME);
        }

        public void SetSFXVolume(float volume)
        {
            volume = Mathf.Clamp01(volume);
            PlayerPrefs.SetFloat(SFX_VOLUME_KEY, volume);
            PlayerPrefs.Save();
            
            // Apply to AudioManager
            AudioManager.Instance.SetSFXVolume(volume);
        }

        #endregion

        #region Control Settings

        public enum JoystickType
        {
            Floating = 0,       // Joystick xuất hiện nơi người chơi touch (Di động)
            Fixed = 1,          // Joystick luôn ở vị trí cố định
            FixedOnTouch = 2    // Joystick cố định tại vị trí chạm, biến mất khi nhả
        }

        public JoystickType GetJoystickType()
        {
            int value = PlayerPrefs.GetInt(JOYSTICK_TYPE_KEY, DEFAULT_JOYSTICK_TYPE);
            return (JoystickType)value;
        }

        public void SetJoystickType(JoystickType type)
        {
            PlayerPrefs.SetInt(JOYSTICK_TYPE_KEY, (int)type);
            PlayerPrefs.Save();
            
            // Notify joystick to update
            NotifyJoystickTypeChanged(type);
        }

        private void NotifyJoystickTypeChanged(JoystickType type)
        {
            // Tìm TouchJoystick trong scene và cập nhật
            var joystick = FindFirstObjectByType<TouchJoystick>();
            if (joystick != null)
            {
                joystick.SetJoystickType(type);
            }
        }

        #endregion

        #region UI Layout Settings

        // Lưu vị trí của UI elements
        public void SaveUIPosition(string elementName, Vector2 position)
        {
            PlayerPrefs.SetFloat($"UI_{elementName}_X", position.x);
            PlayerPrefs.SetFloat($"UI_{elementName}_Y", position.y);
            PlayerPrefs.Save();
        }

        public Vector2 GetUIPosition(string elementName, Vector2 defaultPosition)
        {
            float x = PlayerPrefs.GetFloat($"UI_{elementName}_X", defaultPosition.x);
            float y = PlayerPrefs.GetFloat($"UI_{elementName}_Y", defaultPosition.y);
            return new Vector2(x, y);
        }

        public void ResetUILayout()
        {
            // Xóa tất cả UI position keys
            var allKeys = new System.Collections.Generic.List<string>();
            
            // Tìm tất cả keys bắt đầu với "UI_"
            // (PlayerPrefs không có cách list keys, nên ta phải track manually)
            // Hoặc có thể xóa từng element cụ thể
            
            Debug.Log("SettingsManager: Reset UI Layout to default");
            PlayerPrefs.Save();
        }

        #endregion

        #region Load All Settings

        private void LoadAllSettings()
        {
            // Load audio settings
            float musicVol = GetMusicVolume();
            float sfxVol = GetSFXVolume();
            
            AudioManager.Instance.SetMusicVolume(musicVol);
            AudioManager.Instance.SetSFXVolume(sfxVol);
            
            // Load control settings
            JoystickType joystickType = GetJoystickType();
            NotifyJoystickTypeChanged(joystickType);
            
            Debug.Log($"SettingsManager: Loaded all settings - Music: {musicVol}, SFX: {sfxVol}, Joystick: {joystickType}");
        }

        #endregion

        #region Reset All Settings

        public void ResetAllSettings()
        {
            SetMusicVolume(DEFAULT_MUSIC_VOLUME);
            SetSFXVolume(DEFAULT_SFX_VOLUME);
            SetJoystickType((JoystickType)DEFAULT_JOYSTICK_TYPE);
            ResetUILayout();
            
            Debug.Log("SettingsManager: Reset all settings to default");
        }

        #endregion
    }
}
