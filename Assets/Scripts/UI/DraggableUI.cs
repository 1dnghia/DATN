using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Vampire
{
    /// <summary>
    /// Component cho phép UI element có thể drag & drop
    /// Lưu và load vị trí từ PlayerPrefs thông qua SettingsManager
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class DraggableUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [Header("Settings")]
        [SerializeField] private string elementName = "UIElement"; // Unique name cho mỗi element
        [SerializeField] private bool loadPositionOnStart = true;
        
        [Header("Visual Feedback")]
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Color normalColor = Color.white;
        [SerializeField] private Color draggingColor = new Color(1f, 1f, 1f, 0.7f);
        
        [Header("Constraints")]
        [SerializeField] private bool constrainToScreen = true;
        [SerializeField] private RectTransform boundaryRect; // Giới hạn vùng drag (nếu có)

        private RectTransform rectTransform;
        private Canvas canvas;
        private Vector2 defaultPosition;
        private bool isDraggingEnabled = false;
        private bool isDragging = false;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            canvas = GetComponentInParent<Canvas>();
            
            // Lưu vị trí mặc định
            defaultPosition = rectTransform.anchoredPosition;
            
            // Tìm background image nếu chưa gán
            if (backgroundImage == null)
            {
                backgroundImage = GetComponent<Image>();
            }
        }

        private void Start()
        {
            if (loadPositionOnStart)
            {
                LoadPosition();
            }
        }

        #region Drag Events

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!isDraggingEnabled) return;
            
            isDragging = true;
            
            // Visual feedback
            if (backgroundImage != null)
            {
                backgroundImage.color = draggingColor;
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!isDraggingEnabled || !isDragging) return;
            
            // Convert screen point to local point in canvas
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                eventData.position,
                canvas.worldCamera,
                out localPoint
            );
            
            // Apply position
            rectTransform.anchoredPosition = localPoint;
            
            // Apply constraints
            if (constrainToScreen)
            {
                ClampToScreen();
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!isDraggingEnabled) return;
            
            isDragging = false;
            
            // Visual feedback
            if (backgroundImage != null)
            {
                backgroundImage.color = normalColor;
            }
            
            // Optional: Auto-save khi kết thúc drag
            // SavePosition();
        }

        #endregion

        #region Position Management

        public void SavePosition()
        {
            Vector2 currentPosition = rectTransform.anchoredPosition;
            SettingsManager.Instance.SaveUIPosition(elementName, currentPosition);
            Debug.Log($"DraggableUI [{elementName}]: Saved position {currentPosition}");
        }

        public void LoadPosition()
        {
            Vector2 savedPosition = SettingsManager.Instance.GetUIPosition(elementName, defaultPosition);
            rectTransform.anchoredPosition = savedPosition;
            Debug.Log($"DraggableUI [{elementName}]: Loaded position {savedPosition}");
        }

        public void ResetToDefaultPosition()
        {
            rectTransform.anchoredPosition = defaultPosition;
            
            // Xóa vị trí đã lưu
            SettingsManager.Instance.SaveUIPosition(elementName, defaultPosition);
            
            Debug.Log($"DraggableUI [{elementName}]: Reset to default position {defaultPosition}");
        }

        #endregion

        #region Enable/Disable Dragging

        public void EnableDragging(bool enable)
        {
            isDraggingEnabled = enable;
            
            // Visual feedback
            if (backgroundImage != null)
            {
                backgroundImage.color = enable ? draggingColor : normalColor;
            }
        }

        #endregion

        #region Constraints

        private void ClampToScreen()
        {
            if (canvas == null) return;
            
            RectTransform canvasRect = canvas.transform as RectTransform;
            if (canvasRect == null) return;
            
            // Lấy kích thước của element và canvas
            Vector2 elementSize = rectTransform.sizeDelta;
            Vector2 canvasSize = canvasRect.sizeDelta;
            
            // Tính toán giới hạn
            float minX = -canvasSize.x / 2 + elementSize.x / 2;
            float maxX = canvasSize.x / 2 - elementSize.x / 2;
            float minY = -canvasSize.y / 2 + elementSize.y / 2;
            float maxY = canvasSize.y / 2 - elementSize.y / 2;
            
            // Clamp position
            Vector2 clampedPosition = rectTransform.anchoredPosition;
            clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX);
            clampedPosition.y = Mathf.Clamp(clampedPosition.y, minY, maxY);
            
            rectTransform.anchoredPosition = clampedPosition;
        }

        #endregion

        #region Editor Helper

#if UNITY_EDITOR
        [ContextMenu("Set as Default Position")]
        private void SetAsDefaultPosition()
        {
            defaultPosition = rectTransform.anchoredPosition;
            Debug.Log($"DraggableUI [{elementName}]: Set default position to {defaultPosition}");
        }

        [ContextMenu("Test Save Position")]
        private void TestSavePosition()
        {
            SavePosition();
        }

        [ContextMenu("Test Load Position")]
        private void TestLoadPosition()
        {
            LoadPosition();
        }
#endif

        #endregion
    }
}
