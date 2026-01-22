using UnityEngine;

namespace Vampire
{
    /// <summary>
    /// Script này đảm bảo AudioManager được khởi tạo ngay khi game bắt đầu
    /// Đặt script này vào một GameObject trong scene đầu tiên (Main Menu hoặc bất kỳ scene nào)
    /// </summary>
    public class AudioBootstrap : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private bool initializeOnAwake = true;
        
        private void Awake()
        {
            if (initializeOnAwake)
            {
                // Truy cập Instance để khởi tạo AudioManager
                var audioManager = AudioManager.Instance;
            }
        }
    }
}
