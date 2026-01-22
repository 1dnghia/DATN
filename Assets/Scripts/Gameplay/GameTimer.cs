using UnityEngine;
using TMPro;

namespace Vampire
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class GameTimer : MonoBehaviour
    {
        private TextMeshProUGUI timerText;
        private float currentTime = 0f;

        void Awake()
        {
            timerText = GetComponent<TextMeshProUGUI>();
        }

        public void SetTime(float t)
        {
            currentTime = t;
            System.TimeSpan timeSpan = System.TimeSpan.FromSeconds(t);
            timerText.text = timeSpan.ToString(@"mm\:ss");
        }
        
        public float GetCurrentTime()
        {
            return currentTime;
        }
    }
}
