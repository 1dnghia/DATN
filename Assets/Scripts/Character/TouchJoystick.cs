using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.InputSystem.Layouts;

namespace Vampire
{
    /// Touch interactable joystick. Utilizes IPointer interfaces to ensure
    /// make touches interacting with other UI elements easier to handle.
    /// Supports Fixed (permanent), Floating, and FixedOnTouch modes
    public class TouchJoystick : OnScreenControl, IPointerDownHandler, IPointerUpHandler
    {
        [Header("Joystick Type")]
        [SerializeField] private SettingsManager.JoystickType joystickType = SettingsManager.JoystickType.Floating;
        
        [Header("Joystick Settings")]
        [SerializeField] private float joystickRadius;
        [SerializeField] private RectTransform joystick, joystickBounds;
        
        [Header("Events")]
        [SerializeField] private UnityEvent<Vector2> onJoystickMoved;
        [SerializeField] private UnityEvent onStartTouch, onEndTouch;
        
        [InputControl(layout = "Vector2")]
        [SerializeField]
        private string m_ControlPath;
        protected override string controlPathInternal
        {
            get => m_ControlPath;
            set => m_ControlPath = value;
        }

        private RectTransform controlRect;
        private bool beingTouched = false;
        private Vector2 initialTouchPosition;
        private Vector2 fixedJoystickPosition; // Lưu vị trí cố định cho Fixed mode

        public bool BeingTouched { get => beingTouched; }

        void Awake()
        {
            controlRect = GetComponent<RectTransform>();
            
            // Lưu vị trí cố định cho Fixed mode
            fixedJoystickPosition = joystick.localPosition;
            
            // Load joystick type từ settings
            LoadJoystickType();
        }

        void Update()
        {
            if (beingTouched)
            {
                if (Time.timeScale > 0)
                {
                    Vector2 touchPosition;
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(controlRect, Input.mousePosition, null, out touchPosition);
                    UpdateTouch(touchPosition);
                }
                else
                {
                    EndTouch();
                }
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Vector2 touchPosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(controlRect, eventData.position, null, out touchPosition);
            
            // Xác định vị trí joystick dựa trên mode
            Vector2 joystickPosition;
            switch (joystickType)
            {
                case SettingsManager.JoystickType.Fixed:
                    joystickPosition = fixedJoystickPosition;
                    break;
                
                case SettingsManager.JoystickType.Floating:
                case SettingsManager.JoystickType.FixedOnTouch:
                    joystickPosition = touchPosition;
                    break;
                
                default:
                    joystickPosition = touchPosition;
                    break;
            }
            
            StartTouch(joystickPosition);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            EndTouch();
        }

        public void StartTouch(Vector2 touchPosition)
        {
            if (Time.timeScale > 0)
            {
                beingTouched = true;
                // Save the initial touch position
                initialTouchPosition = touchPosition;
                // Position the joystick
                joystick.localPosition = initialTouchPosition;
                joystickBounds.localPosition = initialTouchPosition;
                // Size the joystick
                joystickBounds.sizeDelta = Vector2.one * joystickRadius * 2;
                // Enable the joystick
                joystick.gameObject.SetActive(true);
                joystickBounds.gameObject.SetActive(true);
                // Invoke touch started callback
                onStartTouch.Invoke();
            }
        }

        public void UpdateTouch(Vector2 touchPosition)
        {
            Vector2 joystickDelta = (touchPosition - initialTouchPosition);
            Vector2 moveDirection = joystickDelta.normalized;
            // Update the joystick position, locking it within the joystick bounds
            joystick.localPosition = joystickDelta.magnitude > joystickRadius ? initialTouchPosition + moveDirection * joystickRadius : touchPosition;
            // Invoke on move callback
            onJoystickMoved.Invoke(moveDirection);
            //SendValueToControl<Vector2>(moveDirection);
        }

        public void EndTouch()
        {
            joystick.localPosition = joystickBounds.localPosition;
            
            // Xác định có ẩn joystick hay không dựa trên mode
            bool shouldHideJoystick = joystickType != SettingsManager.JoystickType.Fixed;
            
            // Disable the joystick nếu không phải Fixed mode
            joystick.gameObject.SetActive(!shouldHideJoystick);
            joystickBounds.gameObject.SetActive(!shouldHideJoystick);
            
            // Invoke touch ended callback
            onJoystickMoved.Invoke(Vector2.zero);
            //SendValueToControl<Vector2>(Vector2.zero);
            onEndTouch.Invoke();
            beingTouched = false;
        }

        #region Settings Integration

        /// <summary>
        /// Load joystick type từ SettingsManager
        /// </summary>
        private void LoadJoystickType()
        {
            if (SettingsManager.Instance != null)
            {
                joystickType = SettingsManager.Instance.GetJoystickType();
                ApplyJoystickType();
            }
        }

        /// <summary>
        /// Thay đổi joystick type (được gọi từ SettingsManager)
        /// </summary>
        public void SetJoystickType(SettingsManager.JoystickType type)
        {
            joystickType = type;
            ApplyJoystickType();
            Debug.Log($"TouchJoystick: Set to {type} mode");
        }

        /// <summary>
        /// Áp dụng cài đặt joystick type
        /// </summary>
        private void ApplyJoystickType()
        {
            if (!beingTouched)
            {
                // Chỉ hiển thị joystick khi ở Fixed mode
                bool shouldShowJoystick = joystickType == SettingsManager.JoystickType.Fixed;
                joystick.gameObject.SetActive(shouldShowJoystick);
                joystickBounds.gameObject.SetActive(shouldShowJoystick);
                
                // Đặt lại vị trí nếu là Fixed mode
                if (shouldShowJoystick)
                {
                    joystick.localPosition = fixedJoystickPosition;
                    joystickBounds.localPosition = fixedJoystickPosition;
                }
            }
        }

        #endregion
    }
}
