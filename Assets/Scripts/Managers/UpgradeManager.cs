using System.Collections.Generic;
using UnityEngine;

namespace Vampire
{
    public class UpgradeManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject upgradeCardPrefab;
        [SerializeField] private Transform cardContainer;  
        
        [Header("Upgrade Data")]
        [SerializeField] private List<UpgradeCardBlueprint> upgradeBlueprints = new List<UpgradeCardBlueprint>();
        
        private List<UpgradeCard> upgradeCards = new List<UpgradeCard>();
        
        void Start()
        {
            InitializeUpgradeCards();
        }
        
        private void InitializeUpgradeCards()
        {
            
            foreach (var blueprint in upgradeBlueprints)
            {
                GameObject cardObj = Instantiate(upgradeCardPrefab, cardContainer);
                UpgradeCard card = cardObj.GetComponent<UpgradeCard>();
                
                if (card != null)
                {
                    card.Init(blueprint, this);
                    upgradeCards.Add(card);
                }
            }
        }
        
        
        public bool CanAffordUpgrade(int cost)
        {
            return CoinManager.Instance.GetCurrentCoins() >= cost;
        }
        
        
        public bool TryPurchaseUpgrade(int cost)
        {
            return CoinManager.Instance.SpendCoins(cost);
        }
        
        
        public void OnUpgradePurchased()
        {
            RefreshAllCards();
        }
        
        
        private void RefreshAllCards()
        {
            foreach (var card in upgradeCards)
            {
                card.RefreshUI();
            }
        }
        
        
        public Dictionary<UpgradeStatType, int> GetAllUpgradeLevels()
        {
            Dictionary<UpgradeStatType, int> levels = new Dictionary<UpgradeStatType, int>();
            
            foreach (var card in upgradeCards)
            {
                levels[card.GetStatType()] = card.GetCurrentLevel();
            }
            
            return levels;
        }
        
        
        public void OnCurrencyChanged()
        {
            RefreshAllCards();
        }
    }
}
