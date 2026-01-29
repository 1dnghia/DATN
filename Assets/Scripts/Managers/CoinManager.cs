using UnityEngine;
using UnityEngine.Events;

namespace Vampire
{
    public class CoinManager : MonoBehaviour
    {
        private static CoinManager instance;
        public static CoinManager Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject go = new GameObject("CoinManager");
                    instance = go.AddComponent<CoinManager>();
                }
                return instance;
            }
        }
        
        private const string COIN_SAVE_KEY = "Coins";  
        
        [SerializeField] private int currentCoins = 0;
        
        public UnityEvent<int> OnCoinChanged = new UnityEvent<int>();
        
        void Awake()
        {
            
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }
            
            instance = this;
            DontDestroyOnLoad(gameObject);
            
            LoadCoins();
        }
    
    void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
    
    
    private void LoadCoins()
    {
        currentCoins = PlayerPrefs.GetInt(COIN_SAVE_KEY, 0);
        OnCoinChanged?.Invoke(currentCoins);
    }
    
    
    public void AddCoins(int amount)
        {
            if (amount <= 0) return;
            
            currentCoins += amount;
            Debug.Log($"[CoinManager] Nhặt được {amount} coins. Tổng coins hiện tại: {currentCoins}");
            SaveCoins();
            OnCoinChanged?.Invoke(currentCoins);
        }
        
        
        public bool SpendCoins(int amount)
        {
            if (amount <= 0 || currentCoins < amount)
                return false;
            
            currentCoins -= amount;
            SaveCoins();
            OnCoinChanged?.Invoke(currentCoins);
            return true;
        }
        
        
        public int GetCurrentCoins()
        {
            return currentCoins;
        }
        
        
        private void SaveCoins()
        {
            PlayerPrefs.SetInt(COIN_SAVE_KEY, currentCoins);
            PlayerPrefs.Save();
        }
        
        
        public void ResetCoins()
        {
            currentCoins = 0;
            SaveCoins();
            OnCoinChanged?.Invoke(currentCoins);
        }
    }
}
