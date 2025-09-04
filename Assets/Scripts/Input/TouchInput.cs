using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TouchInput : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [Header("Virtual Joystick")]
    public RectTransform joystickBackground;
    public RectTransform joystickHandle;
    
    [Header("Settings")]
    public float joystickRange = 50f;
    public bool showOnMobile = true;
    
    // Static output for easy access
    public static Vector2 MoveInput { get; private set; }
    
    private Vector2 centerPosition;
    private bool isDragging = false;
    
    private void Start()
    {
        // Show/hide based on platform
        bool isMobile = Application.isMobilePlatform;
        gameObject.SetActive(isMobile && showOnMobile);
        
        if (joystickBackground != null)
        {
            centerPosition = joystickBackground.anchoredPosition;
            joystickBackground.gameObject.SetActive(false);
        }
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
        
        if (joystickBackground != null)
            joystickBackground.gameObject.SetActive(true);
        
        // Move joystick to touch position
        Vector2 touchPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            transform as RectTransform, 
            eventData.position, 
            eventData.pressEventCamera, 
            out touchPos);
        
        if (joystickBackground != null)
            joystickBackground.anchoredPosition = touchPos;
        
        if (joystickHandle != null)
            joystickHandle.anchoredPosition = Vector2.zero;
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging || joystickBackground == null || joystickHandle == null) 
            return;
        
        Vector2 touchPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            joystickBackground, 
            eventData.position, 
            eventData.pressEventCamera, 
            out touchPos);
        
        // Clamp to joystick range
        touchPos = Vector2.ClampMagnitude(touchPos, joystickRange);
        
        // Update handle position
        joystickHandle.anchoredPosition = touchPos;
        
        // Calculate input value (-1 to 1)
        MoveInput = touchPos / joystickRange;
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
        
        if (joystickBackground != null)
            joystickBackground.gameObject.SetActive(false);
        
        if (joystickHandle != null)
            joystickHandle.anchoredPosition = Vector2.zero;
        
        MoveInput = Vector2.zero;
    }
    
    private void OnDisable()
    {
        MoveInput = Vector2.zero;
        isDragging = false;
    }
}
