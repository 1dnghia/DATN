using UnityEngine;
using TMPro;

namespace Vampire
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class CoinDisplay : MonoBehaviour
    {
        private TextMeshProUGUI coinText;

        void Start()
        {
            coinText = GetComponent<TextMeshProUGUI>();
            
            // Subscribe to Singleton CoinManager
            CoinManager.Instance.OnCoinChanged.AddListener(UpdateDisplay);
            UpdateDisplay(CoinManager.Instance.GetCurrentCoins());
        }
        
        void OnDestroy()
        {
            // Unsubscribe khi destroy
            if (CoinManager.Instance != null)
                CoinManager.Instance.OnCoinChanged.RemoveListener(UpdateDisplay);
        }

        public void UpdateDisplay(int coins)
        {
            coinText.text = coins.ToString();
        }

        public void UpdateDisplay()
        {
            coinText.text = CoinManager.Instance.GetCurrentCoins().ToString();
        }
    }
}
