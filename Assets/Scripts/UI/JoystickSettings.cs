using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Vampire
{
    /// <summary>
    /// UI Panel cho Control Settings
    /// Cho phép chọn Joystick Type (Fixed/Floating/FixedOnTouch) và mở UI Customization
    /// </summary>
    public class JoystickSettings : MonoBehaviour
    {
        [Header("Joystick Type Selection")]
        [SerializeField] private Toggle floatingToggle;
        [SerializeField] private Toggle fixedToggle;
        [SerializeField] private Toggle fixedOnTouchToggle;
        [SerializeField] private ToggleGroup toggleGroup;
        
        [Header("UI Customization")]
        [SerializeField] private Button customizeUIButton;
        [SerializeField] private UICustomizationPanel uiCustomizationPanel;
        
        [Header("Labels (Optional)")]
        [SerializeField] private TextMeshProUGUI currentTypeLabel;

        private void Start()
        {
            InitializeToggles();
            LoadCurrentSettings();
            SetupButtons();
        }

        private void InitializeToggles()
        {
            // Setup toggle group
            if (toggleGroup != null)
            {
                // Bắt buộc phải chọn 1 toggle
                toggleGroup.allowSwitchOff = false;
                
                if (floatingToggle != null)
                    floatingToggle.group = toggleGroup;
                
                if (fixedToggle != null)
                    fixedToggle.group = toggleGroup;
                
                if (fixedOnTouchToggle != null)
                    fixedOnTouchToggle.group = toggleGroup;
            }

            // Add listeners
            if (floatingToggle != null)
                floatingToggle.onValueChanged.AddListener(OnFloatingToggleChanged);
            
            if (fixedToggle != null)
                fixedToggle.onValueChanged.AddListener(OnFixedToggleChanged);
            
            if (fixedOnTouchToggle != null)
                fixedOnTouchToggle.onValueChanged.AddListener(OnFixedOnTouchToggleChanged);
        }

        private void LoadCurrentSettings()
        {
            SettingsManager.JoystickType currentType = SettingsManager.Instance.GetJoystickType();

            // TẮT TẤT CẢ trước
            if (floatingToggle != null)
                floatingToggle.SetIsOnWithoutNotify(false);
            if (fixedToggle != null)
                fixedToggle.SetIsOnWithoutNotify(false);
            if (fixedOnTouchToggle != null)
                fixedOnTouchToggle.SetIsOnWithoutNotify(false);

            // BẬT toggle được chọn
            switch (currentType)
            {
                case SettingsManager.JoystickType.Floating:
                    if (floatingToggle != null)
                        floatingToggle.SetIsOnWithoutNotify(true);
                    break;
                
                case SettingsManager.JoystickType.Fixed:
                    if (fixedToggle != null)
                        fixedToggle.SetIsOnWithoutNotify(true);
                    break;
                
                case SettingsManager.JoystickType.FixedOnTouch:
                    if (fixedOnTouchToggle != null)
                        fixedOnTouchToggle.SetIsOnWithoutNotify(true);
                    break;
            }

            UpdateCurrentTypeLabel(currentType);
        }

        private void OnFloatingToggleChanged(bool isOn)
        {
            if (isOn)
            {
                // TẮT các toggle khác
                if (fixedToggle != null)
                    fixedToggle.SetIsOnWithoutNotify(false);
                if (fixedOnTouchToggle != null)
                    fixedOnTouchToggle.SetIsOnWithoutNotify(false);
                
                SettingsManager.Instance.SetJoystickType(SettingsManager.JoystickType.Floating);
                UpdateCurrentTypeLabel(SettingsManager.JoystickType.Floating);
                
                // Play feedback sound
                AudioManager.Instance.PlayButtonClick();
            }
        }

        private void OnFixedToggleChanged(bool isOn)
        {
            if (isOn)
            {
                // TẮT các toggle khác
                if (floatingToggle != null)
                    floatingToggle.SetIsOnWithoutNotify(false);
                if (fixedOnTouchToggle != null)
                    fixedOnTouchToggle.SetIsOnWithoutNotify(false);
                
                SettingsManager.Instance.SetJoystickType(SettingsManager.JoystickType.Fixed);
                UpdateCurrentTypeLabel(SettingsManager.JoystickType.Fixed);
                
                // Play feedback sound
                AudioManager.Instance.PlayButtonClick();
            }
        }

        private void OnFixedOnTouchToggleChanged(bool isOn)
        {
            if (isOn)
            {
                // TẮT các toggle khác
                if (floatingToggle != null)
                    floatingToggle.SetIsOnWithoutNotify(false);
                if (fixedToggle != null)
                    fixedToggle.SetIsOnWithoutNotify(false);
                
                SettingsManager.Instance.SetJoystickType(SettingsManager.JoystickType.FixedOnTouch);
                UpdateCurrentTypeLabel(SettingsManager.JoystickType.FixedOnTouch);
                
                // Play feedback sound
                AudioManager.Instance.PlayButtonClick();
            }
        }

        private void UpdateCurrentTypeLabel(SettingsManager.JoystickType type)
        {
            if (currentTypeLabel != null)
            {
                string typeName = "";
                switch (type)
                {
                    case SettingsManager.JoystickType.Floating:
                        typeName = "Floating";
                        break;
                    case SettingsManager.JoystickType.Fixed:
                        typeName = "Fixed";
                        break;
                    case SettingsManager.JoystickType.FixedOnTouch:
                        typeName = "Fixed On Touch";
                        break;
                }
                currentTypeLabel.text = $"Current: {typeName}";
            }
        }

        private void SetupButtons()
        {
            if (customizeUIButton != null)
            {
                customizeUIButton.onClick.AddListener(OnCustomizeUIClicked);
            }
        }

        private void OnCustomizeUIClicked()
        {
            Debug.Log("JoystickSettings: Customize UI button clicked!");
            
            // Mở UI Customization Panel
            if (uiCustomizationPanel != null)
            {
                Debug.Log("JoystickSettings: Opening UICustomizationPanel...");
                uiCustomizationPanel.OpenCustomization();
            }
            else
            {
                Debug.LogError("JoystickSettings: UICustomizationPanel is NULL! Please assign it in Inspector.");
            }

            AudioManager.Instance.PlayButtonClick();
        }

        private void OnDestroy()
        {
            // Clean up listeners
            if (floatingToggle != null)
                floatingToggle.onValueChanged.RemoveListener(OnFloatingToggleChanged);
            
            if (fixedToggle != null)
                fixedToggle.onValueChanged.RemoveListener(OnFixedToggleChanged);
            
            if (fixedOnTouchToggle != null)
                fixedOnTouchToggle.onValueChanged.RemoveListener(OnFixedOnTouchToggleChanged);
            
            if (customizeUIButton != null)
                customizeUIButton.onClick.RemoveListener(OnCustomizeUIClicked);
        }
    }
}
