using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Vampire
{
    /// <summary>
    /// UI Panel cho Audio Settings
    /// Hiển thị sliders để điều chỉnh Music và SFX volume
    /// </summary>
    public class AudioSettings : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Slider musicVolumeSlider;
        [SerializeField] private Slider sfxVolumeSlider;
        
        [Header("Volume Labels (Optional)")]
        [SerializeField] private TextMeshProUGUI musicVolumeLabel;
        [SerializeField] private TextMeshProUGUI sfxVolumeLabel;

        private void Start()
        {
            InitializeSliders();
            LoadCurrentSettings();
        }

        private void InitializeSliders()
        {
            // Setup sliders
            if (musicVolumeSlider != null)
            {
                musicVolumeSlider.minValue = 0f;
                musicVolumeSlider.maxValue = 1f;
                musicVolumeSlider.wholeNumbers = false;
                musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
            }

            if (sfxVolumeSlider != null)
            {
                sfxVolumeSlider.minValue = 0f;
                sfxVolumeSlider.maxValue = 1f;
                sfxVolumeSlider.wholeNumbers = false;
                sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
            }
        }

        private void LoadCurrentSettings()
        {
            // Load current settings from SettingsManager
            float musicVolume = SettingsManager.Instance.GetMusicVolume();
            float sfxVolume = SettingsManager.Instance.GetSFXVolume();

            // Update sliders without triggering events
            if (musicVolumeSlider != null)
            {
                musicVolumeSlider.SetValueWithoutNotify(musicVolume);
                UpdateMusicVolumeLabel(musicVolume);
            }

            if (sfxVolumeSlider != null)
            {
                sfxVolumeSlider.SetValueWithoutNotify(sfxVolume);
                UpdateSFXVolumeLabel(sfxVolume);
            }
        }

        private void OnMusicVolumeChanged(float value)
        {
            SettingsManager.Instance.SetMusicVolume(value);
            UpdateMusicVolumeLabel(value);
        }

        private void OnSFXVolumeChanged(float value)
        {
            SettingsManager.Instance.SetSFXVolume(value);
            UpdateSFXVolumeLabel(value);
            
            // Play test sound
            AudioManager.Instance.PlayButtonClick();
        }

        private void UpdateMusicVolumeLabel(float value)
        {
            if (musicVolumeLabel != null)
            {
                musicVolumeLabel.text = $"{Mathf.RoundToInt(value * 100)}%";
            }
        }

        private void UpdateSFXVolumeLabel(float value)
        {
            if (sfxVolumeLabel != null)
            {
                sfxVolumeLabel.text = $"{Mathf.RoundToInt(value * 100)}%";
            }
        }

        private void OnDestroy()
        {
            // Clean up listeners
            if (musicVolumeSlider != null)
                musicVolumeSlider.onValueChanged.RemoveListener(OnMusicVolumeChanged);
            
            if (sfxVolumeSlider != null)
                sfxVolumeSlider.onValueChanged.RemoveListener(OnSFXVolumeChanged);
        }
    }
}
