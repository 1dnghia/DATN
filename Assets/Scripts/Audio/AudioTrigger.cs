using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Vampire
{
    // Component để bắt sự kiện hover/click cho button
    // - Dùng độc lập: Gắn trực tiếp vào button prefab và config trong Inspector
    // - Dùng với Manager: AudioTriggerManager sẽ tự động setup cho buttons trong scene
    [RequireComponent(typeof(Button))]
    public class AudioTrigger : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
    {
        [Header("Audio Settings")]
        [SerializeField] private bool playOnHover = false;
        [SerializeField] private bool playOnClick = true;

        // AudioTriggerManager gọi method này để config
        public void SetSettings(bool hover, bool click)
        {
            playOnHover = hover;
            playOnClick = click;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!playOnHover) return;
            AudioManager.Instance.PlayButtonHover();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!playOnClick) return;
            AudioManager.Instance.PlayButtonClick();
        }
    }
}
