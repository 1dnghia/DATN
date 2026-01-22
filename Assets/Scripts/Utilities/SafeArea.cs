using UnityEngine;

namespace Vampire
{
    public class SafeArea : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private bool applySafeArea = true;
        
        void Start()
        {
            if (applySafeArea)
                ResetSafeArea();
        }

        public void ResetSafeArea()
        {
            RectTransform rectTransform = GetComponent<RectTransform>();
            Rect safeArea = Screen.safeArea;
            
            // Nếu safe area là full screen (không có notch), không làm gì
            if (safeArea.position == Vector2.zero && 
                safeArea.size.x >= Screen.width && 
                safeArea.size.y >= Screen.height)
            {
                return;
            }
            
            Vector2 minAnchor = safeArea.position;
            Vector2 maxAnchor = minAnchor + safeArea.size;
            
            minAnchor.x /= Screen.width;
            minAnchor.y /= Screen.height;
            maxAnchor.x /= Screen.width;
            maxAnchor.y /= Screen.height;

            rectTransform.anchorMin = minAnchor;
            rectTransform.anchorMax = maxAnchor;
        }
    }
}
