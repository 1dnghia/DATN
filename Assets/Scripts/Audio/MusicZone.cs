using UnityEngine;

namespace Vampire
{
    // Component để tự động đổi nhạc khi vào vùng nào đó (vd: boss zone)
    public class MusicZone : MonoBehaviour
    {
        [Header("Music Settings")]
        [SerializeField] private AudioClip zoneMusic;
        [SerializeField] private bool playOnStart = false;
        
        private AudioClip previousMusic;

        private void Start()
        {
            if (playOnStart && zoneMusic != null)
            {
                AudioManager.Instance.PlayMusic(zoneMusic);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player") && zoneMusic != null)
            {
                AudioManager.Instance.PlayMusic(zoneMusic);
            }
        }
    }
}
