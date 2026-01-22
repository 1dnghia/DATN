using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Vampire
{
    /// <summary>
    /// Quản lý chế độ customize UI layout
    /// Cho phép drag & drop các UI elements để thay đổi vị trí
    /// </summary>
    public class UILayoutCustomizer : MonoBehaviour
    {
        [Header("UI Elements to Customize")]
        [SerializeField] private List<DraggableUI> draggableElements = new List<DraggableUI>();
        
        [Header("Control Buttons")]
        [SerializeField] private Button saveButton;
        [SerializeField] private Button cancelButton;
        [SerializeField] private Button resetButton;
        
        [Header("Preview Canvas")]
        [SerializeField] private Canvas previewCanvas;
        
        [Header("Instructions")]
        [SerializeField] private GameObject instructionsPanel;

        private Dictionary<DraggableUI, Vector2> originalPositions = new Dictionary<DraggableUI, Vector2>();

        private void Awake()
        {
            SetupButtons();
            
            // Tự động tìm các DraggableUI components nếu chưa gán
            if (draggableElements.Count == 0)
            {
                draggableElements.AddRange(GetComponentsInChildren<DraggableUI>(true));
            }
        }

        private void SetupButtons()
        {
            if (saveButton != null)
                saveButton.onClick.AddListener(OnSaveClicked);
            
            if (cancelButton != null)
                cancelButton.onClick.AddListener(OnCancelClicked);
            
            if (resetButton != null)
                resetButton.onClick.AddListener(OnResetClicked);
        }

        public void EnterCustomizationMode()
        {
            // Lưu vị trí hiện tại để có thể cancel
            SaveCurrentPositions();
            
            // Enable dragging cho tất cả elements
            foreach (var element in draggableElements)
            {
                if (element != null)
                {
                    element.EnableDragging(true);
                }
            }
            
            // Hiển thị instructions
            if (instructionsPanel != null)
                instructionsPanel.SetActive(true);
            
            Debug.Log("UILayoutCustomizer: Entered customization mode");
        }

        public void ExitCustomizationMode()
        {
            // Disable dragging
            foreach (var element in draggableElements)
            {
                if (element != null)
                {
                    element.EnableDragging(false);
                }
            }
            
            // Ẩn instructions
            if (instructionsPanel != null)
                instructionsPanel.SetActive(false);
            
            // Đóng panel
            gameObject.SetActive(false);
            
            Debug.Log("UILayoutCustomizer: Exited customization mode");
        }

        private void SaveCurrentPositions()
        {
            originalPositions.Clear();
            
            foreach (var element in draggableElements)
            {
                if (element != null)
                {
                    RectTransform rectTransform = element.GetComponent<RectTransform>();
                    if (rectTransform != null)
                    {
                        originalPositions[element] = rectTransform.anchoredPosition;
                    }
                }
            }
        }

        private void OnSaveClicked()
        {
            // Lưu vị trí của tất cả UI elements
            foreach (var element in draggableElements)
            {
                if (element != null)
                {
                    element.SavePosition();
                }
            }
            
            AudioManager.Instance.PlayButtonClick();
            ExitCustomizationMode();
            
            Debug.Log("UILayoutCustomizer: Saved UI layout");
        }

        private void OnCancelClicked()
        {
            // Restore vị trí ban đầu
            foreach (var kvp in originalPositions)
            {
                if (kvp.Key != null)
                {
                    RectTransform rectTransform = kvp.Key.GetComponent<RectTransform>();
                    if (rectTransform != null)
                    {
                        rectTransform.anchoredPosition = kvp.Value;
                    }
                }
            }
            
            AudioManager.Instance.PlayButtonClick();
            ExitCustomizationMode();
            
            Debug.Log("UILayoutCustomizer: Cancelled UI customization");
        }

        private void OnResetClicked()
        {
            // Reset tất cả về vị trí mặc định
            foreach (var element in draggableElements)
            {
                if (element != null)
                {
                    element.ResetToDefaultPosition();
                }
            }
            
            AudioManager.Instance.PlayButtonClick();
            
            Debug.Log("UILayoutCustomizer: Reset all UI elements to default");
        }

        private void OnDestroy()
        {
            // Clean up
            if (saveButton != null)
                saveButton.onClick.RemoveListener(OnSaveClicked);
            
            if (cancelButton != null)
                cancelButton.onClick.RemoveListener(OnCancelClicked);
            
            if (resetButton != null)
                resetButton.onClick.RemoveListener(OnResetClicked);
        }

        #region Public Methods

        /// <summary>
        /// Thêm UI element vào danh sách có thể customize
        /// </summary>
        public void RegisterDraggableElement(DraggableUI element)
        {
            if (element != null && !draggableElements.Contains(element))
            {
                draggableElements.Add(element);
            }
        }

        /// <summary>
        /// Xóa UI element khỏi danh sách
        /// </summary>
        public void UnregisterDraggableElement(DraggableUI element)
        {
            if (element != null && draggableElements.Contains(element))
            {
                draggableElements.Remove(element);
            }
        }

        #endregion
    }
}
