using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections.Generic;

namespace Vampire
{
    /// <summary>
    /// UI Customization Panel - All-in-one solution
    /// Cho phép kéo thả Joystick (Fixed mode only) và 4 Inventory Slots
    /// Tự động lưu/load vị trí qua PlayerPrefs
    /// </summary>
    public class UICustomizationPanel : MonoBehaviour
    {
        [Header("Panel References")]
        [SerializeField] private GameObject customizationPanel;
        
        [Header("UI Elements to Customize")]
        [SerializeField] private RectTransform joystickTransform;
        [SerializeField] private List<RectTransform> inventorySlots = new List<RectTransform>();
        
        [Header("Control Buttons")]
        [SerializeField] private Button saveButton;
        [SerializeField] private Button resetButton;
        [SerializeField] private Button closeButton;
        
        [Header("Info Text")]
        [SerializeField] private TextMeshProUGUI infoText;
        [SerializeField] private TextMeshProUGUI instructionText;
        
        [Header("Confirmation Dialog")]
        [SerializeField] private GameObject confirmationDialog;
        [SerializeField] private Button confirmYesButton;
        [SerializeField] private Button confirmNoButton;

        [Header("Drag Settings")]
        [SerializeField] private Canvas parentCanvas;
        
        // PlayerPrefs keys
        private const string JOYSTICK_POS_X = "JoystickPosX";
        private const string JOYSTICK_POS_Y = "JoystickPosY";
        private const string INVENTORY_SLOT_PREFIX = "InventorySlot";
        private const string POS_X_SUFFIX = "_PosX";
        private const string POS_Y_SUFFIX = "_PosY";

        // Drag state
        private RectTransform currentDragElement;
        private Vector2 dragOffset;
        private Vector2 originalJoystickPos;
        private List<Vector2> originalSlotPositions = new List<Vector2>();
        
        // Track unsaved changes
        private bool hasUnsavedChanges = false;

        private void Awake()
        {
            // Tự động tìm parent canvas nếu chưa assign
            if (parentCanvas == null)
            {
                parentCanvas = GetComponentInParent<Canvas>();
                if (parentCanvas == null)
                {
                    // Tìm canvas root (thường là canvas chính)
                    parentCanvas = FindObjectOfType<Canvas>();
                }
                
                if (parentCanvas != null)
                {
                    Debug.Log($"UICustomizationPanel: Auto-found parent canvas: {parentCanvas.name}");
                }
                else
                {
                    Debug.LogError("UICustomizationPanel: Cannot find any Canvas in scene!");
                }
            }
        }

        private void Start()
        {
            SetupButtons();
            ValidateAndSortInventorySlots();
            SaveOriginalPositions();
            
            // Ẩn panel và confirmation dialog ban đầu
            if (customizationPanel != null)
                customizationPanel.SetActive(false);
            
            if (confirmationDialog != null)
                confirmationDialog.SetActive(false);
            
            // Load vị trí đã lưu
            LoadAllSavedPositions();
        }
        
        /// <summary>
        /// Validate và tự động sắp xếp inventory slots theo tên/index
        /// </summary>
        private void ValidateAndSortInventorySlots()
        {
            if (inventorySlots == null || inventorySlots.Count == 0)
                return;
            
            // Remove nulls
            inventorySlots.RemoveAll(slot => slot == null);
            
            // Tự động sắp xếp theo tên nếu có pattern "Slot0", "Slot1"...
            inventorySlots.Sort((a, b) =>
            {
                // Thử extract số từ tên GameObject
                int indexA = ExtractSlotIndex(a.name);
                int indexB = ExtractSlotIndex(b.name);
                
                if (indexA >= 0 && indexB >= 0)
                    return indexA.CompareTo(indexB);
                
                // Fallback: sort theo tên alphabet
                return string.Compare(a.name, b.name, System.StringComparison.Ordinal);
            });
            
            // Log để user biết thứ tự và check visibility
            Debug.Log("UICustomizationPanel: Inventory slots order:");
            for (int i = 0; i < inventorySlots.Count; i++)
            {
                bool isActive = inventorySlots[i].gameObject.activeSelf;
                Debug.Log($"  Slot {i}: {inventorySlots[i].name} - Active: {isActive}");
            }
        }
        
        /// <summary>
        /// Extract slot index từ tên GameObject (ví dụ: "Slot0", "InventorySlot1", "PreviewSlot2")
        /// </summary>
        private int ExtractSlotIndex(string name)
        {
            if (string.IsNullOrEmpty(name))
                return -1;
            
            // Tìm số cuối cùng trong tên
            for (int i = name.Length - 1; i >= 0; i--)
            {
                if (char.IsDigit(name[i]))
                {
                    // Tìm thấy số, parse nó
                    if (int.TryParse(name[i].ToString(), out int index))
                    {
                        return index;
                    }
                }
            }
            
            return -1; // Không tìm thấy số
        }

        private void SetupButtons()
        {
            if (saveButton != null)
                saveButton.onClick.AddListener(OnSaveClicked);
            
            if (resetButton != null)
                resetButton.onClick.AddListener(OnResetClicked);
            
            if (closeButton != null)
                closeButton.onClick.AddListener(OnCloseClicked);
            
            // Setup confirmation dialog buttons
            if (confirmYesButton != null)
                confirmYesButton.onClick.AddListener(OnConfirmExit);
            
            if (confirmNoButton != null)
                confirmNoButton.onClick.AddListener(OnCancelExit);
        }

        private void SaveOriginalPositions()
        {
            if (joystickTransform != null)
                originalJoystickPos = joystickTransform.anchoredPosition;
            
            originalSlotPositions.Clear();
            foreach (var slot in inventorySlots)
            {
                if (slot != null)
                    originalSlotPositions.Add(slot.anchoredPosition);
            }
        }

        /// <summary>
        /// Mở Customization Panel
        /// </summary>
        public void OpenCustomization()
        {
            Debug.Log("UICustomizationPanel: OpenCustomization() called");
            
            if (customizationPanel == null)
            {
                Debug.LogError("UICustomizationPanel: customizationPanel GameObject is NULL! Assign it in Inspector.");
                return;
            }
            
            customizationPanel.SetActive(true);
            Debug.Log("UICustomizationPanel: Panel set to active");
            
            UpdateJoystickVisibility();
            UpdateInstructionText();
            
            // Pause game
            Time.timeScale = 0;
            
            AudioManager.Instance.PlayMenuOpen();
            Debug.Log("UICustomizationPanel: Opened customization mode");
        }

        /// <summary>
        /// Đóng Customization Panel
        /// </summary>
        public void CloseCustomization()
        {
            if (customizationPanel != null)
                customizationPanel.SetActive(false);
            
            // Resume game
            Time.timeScale = 1;
            
            AudioManager.Instance.PlayMenuClose();
            Debug.Log("UICustomizationPanel: Closed customization mode");
        }

        private void Update()
        {
            // Chỉ xử lý drag khi panel mở
            if (customizationPanel == null || !customizationPanel.activeSelf)
                return;

            HandleDragInput();
        }

        private void HandleDragInput()
        {
            // Begin drag
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousePos;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    parentCanvas.GetComponent<RectTransform>(),
                    Input.mousePosition,
                    parentCanvas.worldCamera,
                    out mousePos
                );

                // Check joystick (nếu visible)
                if (joystickTransform != null && joystickTransform.gameObject.activeSelf)
                {
                    if (RectTransformUtility.RectangleContainsScreenPoint(joystickTransform, Input.mousePosition, parentCanvas.worldCamera))
                    {
                        currentDragElement = joystickTransform;
                        dragOffset = joystickTransform.anchoredPosition - mousePos;
                        return;
                    }
                }

                // Check inventory slots
                foreach (var slot in inventorySlots)
                {
                    if (slot != null && RectTransformUtility.RectangleContainsScreenPoint(slot, Input.mousePosition, parentCanvas.worldCamera))
                    {
                        currentDragElement = slot;
                        dragOffset = slot.anchoredPosition - mousePos;
                        return;
                    }
                }
            }

            // During drag
            if (Input.GetMouseButton(0) && currentDragElement != null)
            {
                Vector2 mousePos;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    parentCanvas.GetComponent<RectTransform>(),
                    Input.mousePosition,
                    parentCanvas.worldCamera,
                    out mousePos
                );

                Vector2 newPosition = mousePos + dragOffset;
                newPosition = ClampToCanvas(newPosition, currentDragElement);
                currentDragElement.anchoredPosition = newPosition;
            }

            // End drag
            if (Input.GetMouseButtonUp(0) && currentDragElement != null)
            {
                // Đánh dấu có thay đổi chưa lưu (không lưu ngay)
                hasUnsavedChanges = true;
                UpdateSaveButtonState();
                
                if (infoText != null)
                {
                    infoText.text = "Position changed. Click Save to apply.";
                }
                
                currentDragElement = null;
            }
        }

        private Vector2 ClampToCanvas(Vector2 position, RectTransform element)
        {
            if (parentCanvas == null || element == null) return position;

            RectTransform canvasRect = parentCanvas.GetComponent<RectTransform>();
            Vector2 canvasSize = canvasRect.sizeDelta;
            Vector2 elementSize = element.sizeDelta;

            float minX = -canvasSize.x / 2 + elementSize.x / 2;
            float maxX = canvasSize.x / 2 - elementSize.x / 2;
            float minY = -canvasSize.y / 2 + elementSize.y / 2;
            float maxY = canvasSize.y / 2 - elementSize.y / 2;

            position.x = Mathf.Clamp(position.x, minX, maxX);
            position.y = Mathf.Clamp(position.y, minY, maxY);

            return position;
        }

        private void SaveElementPosition(RectTransform element)
        {
            Vector2 pos = element.anchoredPosition;

            if (element == joystickTransform)
            {
                PlayerPrefs.SetFloat(JOYSTICK_POS_X, pos.x);
                PlayerPrefs.SetFloat(JOYSTICK_POS_Y, pos.y);
                PlayerPrefs.Save();
                Debug.Log($"Saved Joystick position: {pos}");
            }
            else
            {
                int slotIndex = inventorySlots.IndexOf(element);
                if (slotIndex >= 0)
                {
                    string keyX = INVENTORY_SLOT_PREFIX + slotIndex + POS_X_SUFFIX;
                    string keyY = INVENTORY_SLOT_PREFIX + slotIndex + POS_Y_SUFFIX;
                    PlayerPrefs.SetFloat(keyX, pos.x);
                    PlayerPrefs.SetFloat(keyY, pos.y);
                    PlayerPrefs.Save();
                    Debug.Log($"Saved Slot {slotIndex} position: {pos}");
                }
            }
        }

        /// <summary>
        /// Load vị trí đã lưu cho tất cả elements
        /// </summary>
        public void LoadAllSavedPositions()
        {
            // Load joystick
            if (joystickTransform != null && PlayerPrefs.HasKey(JOYSTICK_POS_X))
            {
                float x = PlayerPrefs.GetFloat(JOYSTICK_POS_X);
                float y = PlayerPrefs.GetFloat(JOYSTICK_POS_Y);
                joystickTransform.anchoredPosition = new Vector2(x, y);
                Debug.Log($"Loaded Joystick position: ({x}, {y})");
            }

            // Load inventory slots
            for (int i = 0; i < inventorySlots.Count; i++)
            {
                if (inventorySlots[i] == null) continue;

                string keyX = INVENTORY_SLOT_PREFIX + i + POS_X_SUFFIX;
                string keyY = INVENTORY_SLOT_PREFIX + i + POS_Y_SUFFIX;

                if (PlayerPrefs.HasKey(keyX) && PlayerPrefs.HasKey(keyY))
                {
                    float x = PlayerPrefs.GetFloat(keyX);
                    float y = PlayerPrefs.GetFloat(keyY);
                    inventorySlots[i].anchoredPosition = new Vector2(x, y);
                    Debug.Log($"Loaded Slot {i} position: ({x}, {y})");
                }
            }
        }

        private void UpdateJoystickVisibility()
        {
            if (joystickTransform == null) return;

            SettingsManager.JoystickType currentType = SettingsManager.Instance.GetJoystickType();
            bool isFixedMode = (currentType == SettingsManager.JoystickType.Fixed);
            
            joystickTransform.gameObject.SetActive(isFixedMode);
            Debug.Log($"Joystick visibility = {isFixedMode} (mode: {currentType})");
        }

        private void UpdateInstructionText()
        {
            if (instructionText == null) return;

            SettingsManager.JoystickType currentType = SettingsManager.Instance.GetJoystickType();
            
            // Kiểm tra có inventory slots không
            bool hasInventorySlots = inventorySlots.Count > 0 && inventorySlots[0] != null;
            
            if (currentType == SettingsManager.JoystickType.Fixed && hasInventorySlots)
            {
                instructionText.text = "Drag to reposition Joystick and Inventory Slots\nKéo thả để di chuyển Joystick và các Inventory Slot";
            }
            else if (currentType == SettingsManager.JoystickType.Fixed && !hasInventorySlots)
            {
                instructionText.text = "Drag to reposition Joystick\nKéo thả để di chuyển Joystick";
            }
            else if (hasInventorySlots)
            {
                instructionText.text = "Drag to reposition Inventory Slots only\nKéo thả để di chuyển các Inventory Slot\n\n(Joystick only in Fixed mode)";
            }
            else
            {
                instructionText.text = "Joystick customization only available in Fixed mode\nChỉ tùy chỉnh Joystick được trong chế độ Fixed";
            }
        }

        #region Button Callbacks

        private void OnSaveClicked()
        {
            AudioManager.Instance.PlayButtonClick();
            SaveAllPositions();
        }

        private void OnResetClicked()
        {
            AudioManager.Instance.PlayButtonClick();
            ResetAllPositions();
        }

        private void OnCloseClicked()
        {
            AudioManager.Instance.PlayButtonClick();
            
            // Kiểm tra có thay đổi chưa lưu không
            if (hasUnsavedChanges)
            {
                ShowExitConfirmation();
            }
            else
            {
                // Không có thay đổi → Thoát luôn
                CloseCustomization();
            }
        }

        #endregion

        #region Confirmation Dialog

        /// <summary>
        /// Hiển thị hộp thoại xác nhận thoát khi có thay đổi chưa lưu
        /// </summary>
        private void ShowExitConfirmation()
        {
            if (confirmationDialog != null)
            {
                confirmationDialog.SetActive(true);
                AudioManager.Instance.PlayMenuOpen();
                Debug.Log("UICustomizationPanel: Showing exit confirmation (unsaved changes)");
            }
        }

        /// <summary>
        /// User click "Yes" - Thoát không lưu
        /// </summary>
        private void OnConfirmExit()
        {
            AudioManager.Instance.PlayButtonClick();
            
            // Ẩn confirmation dialog
            if (confirmationDialog != null)
                confirmationDialog.SetActive(false);
            
            // Restore về vị trí đã lưu (hoặc original)
            LoadAllSavedPositions();
            
            // Reset flag
            hasUnsavedChanges = false;
            UpdateSaveButtonState();
            
            // Đóng customization panel
            CloseCustomization();
            
            Debug.Log("UICustomizationPanel: Exit without saving");
        }

        /// <summary>
        /// User click "No" - Ở lại để lưu
        /// </summary>
        private void OnCancelExit()
        {
            AudioManager.Instance.PlayButtonClick();
            
            // Ẩn confirmation dialog, quay lại customization
            if (confirmationDialog != null)
                confirmationDialog.SetActive(false);
            
            AudioManager.Instance.PlayMenuClose();
            Debug.Log("UICustomizationPanel: Cancelled exit, staying in customization");
        }

        #endregion

        /// <summary>
        /// Lưu tất cả vị trí hiện tại vào PlayerPrefs
        /// </summary>
        private void SaveAllPositions()
        {
            // Save joystick
            if (joystickTransform != null)
            {
                Vector2 pos = joystickTransform.anchoredPosition;
                PlayerPrefs.SetFloat(JOYSTICK_POS_X, pos.x);
                PlayerPrefs.SetFloat(JOYSTICK_POS_Y, pos.y);
                Debug.Log($"Saved Joystick position: {pos}");
            }

            // Save inventory slots
            for (int i = 0; i < inventorySlots.Count; i++)
            {
                if (inventorySlots[i] == null) continue;

                Vector2 pos = inventorySlots[i].anchoredPosition;
                string keyX = INVENTORY_SLOT_PREFIX + i + POS_X_SUFFIX;
                string keyY = INVENTORY_SLOT_PREFIX + i + POS_Y_SUFFIX;
                PlayerPrefs.SetFloat(keyX, pos.x);
                PlayerPrefs.SetFloat(keyY, pos.y);
                Debug.Log($"Saved Slot {i} position: {pos}");
            }

            PlayerPrefs.Save();
            hasUnsavedChanges = false;
            UpdateSaveButtonState();

            if (infoText != null)
            {
                infoText.text = "All positions saved!";
            }

            Debug.Log("UICustomizationPanel: Saved all UI positions");
        }

        /// <summary>
        /// Update Save button visual state
        /// </summary>
        private void UpdateSaveButtonState()
        {
            if (saveButton == null) return;

            // Có thể thay đổi màu button hoặc text để highlight khi có changes
            if (hasUnsavedChanges)
            {
                // Highlight save button (màu sáng hơn hoặc có indicator)
                var colors = saveButton.colors;
                colors.normalColor = new Color(1f, 0.8f, 0f); // Màu vàng/cam để chú ý
                saveButton.colors = colors;
            }
            else
            {
                // Normal state
                var colors = saveButton.colors;
                colors.normalColor = Color.white;
                saveButton.colors = colors;
            }
        }

        /// <summary>
        /// Reset tất cả về vị trí ban đầu
        /// </summary>
        public void ResetAllPositions()
        {
            // Reset joystick
            if (joystickTransform != null)
            {
                joystickTransform.anchoredPosition = originalJoystickPos;
                PlayerPrefs.DeleteKey(JOYSTICK_POS_X);
                PlayerPrefs.DeleteKey(JOYSTICK_POS_Y);
            }

            // Reset slots
            for (int i = 0; i < inventorySlots.Count; i++)
            {
                if (inventorySlots[i] != null && i < originalSlotPositions.Count)
                {
                    inventorySlots[i].anchoredPosition = originalSlotPositions[i];
                    
                    string keyX = INVENTORY_SLOT_PREFIX + i + POS_X_SUFFIX;
                    string keyY = INVENTORY_SLOT_PREFIX + i + POS_Y_SUFFIX;
                    PlayerPrefs.DeleteKey(keyX);
                    PlayerPrefs.DeleteKey(keyY);
                }
            }

            PlayerPrefs.Save();

            if (infoText != null)
            {
                infoText.text = "All positions reset!";
            }
            
            hasUnsavedChanges = false;
            UpdateSaveButtonState();

            Debug.Log("UICustomizationPanel: Reset all positions");
        }

        private void OnDestroy()
        {
            if (saveButton != null)
                saveButton.onClick.RemoveListener(OnSaveClicked);
            
            if (resetButton != null)
                resetButton.onClick.RemoveListener(OnResetClicked);
            
            if (closeButton != null)
                closeButton.onClick.RemoveListener(OnCloseClicked);
            
            if (confirmYesButton != null)
                confirmYesButton.onClick.RemoveListener(OnConfirmExit);
            
            if (confirmNoButton != null)
                confirmNoButton.onClick.RemoveListener(OnCancelExit);
        }
    }
}

