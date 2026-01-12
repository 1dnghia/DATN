using UnityEngine;
using UnityEngine.EventSystems;

namespace Vampire
{
    // Component nhẹ để bắt sự kiện hover/click cho button
    // Được quản lý bởi AudioTriggerManager
    public class AudioTrigger : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
    {
        private bool playOnHover = false;
        private bool playOnClick = true;
        private AudioClip hoverSound;
        private AudioClip clickSound;

        // AudioTriggerManager gọi method này để config
        public void SetSettings(bool hover, bool click, AudioClip hoverClip = null, AudioClip clickClip = null)
        {
            playOnHover = hover;
            playOnClick = click;
            hoverSound = hoverClip;
            clickSound = clickClip;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!playOnHover) return;

            if (hoverSound != null)
                AudioManager.Instance.PlaySFX(hoverSound);
            else
                AudioManager.Instance.PlayButtonHover();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!playOnClick) return;

            if (clickSound != null)
                AudioManager.Instance.PlaySFX(clickSound);
            else
                AudioManager.Instance.PlayButtonClick();
        }
    }
}
