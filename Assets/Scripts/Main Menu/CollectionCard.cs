using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Vampire
{
    public class CollectionCard : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Image itemImage;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private Image lockOverlay;
        
        [Header("Lock Overlay Settings")]
        [SerializeField] private Color lockOverlayColor = new Color(0f, 0f, 0f, 0.8f);
        
        private bool isUnlocked;

        public void InitMonster(MonsterBlueprint monster)
        {
            string unlockKey = $"Monster_{monster.name}_Discovered";
            isUnlocked = PlayerPrefs.GetInt(unlockKey, 0) == 1;
            
            if (isUnlocked)
            {
                nameText.text = monster.name;
                if (monster.walkSpriteSequence != null && monster.walkSpriteSequence.Length > 0)
                {
                    itemImage.sprite = monster.walkSpriteSequence[0];
                }
            }
            else
            {
                nameText.text = "???";
                itemImage.sprite = null;
            }
            
            UpdateUI();
        }

        public void InitWeapon(GameObject weaponPrefab)
        {
            if (weaponPrefab == null) return;
            
            Ability ability = weaponPrefab.GetComponent<Ability>();
            if (ability == null) return;
            
            // Sử dụng ability.Name thay vì weaponPrefab.name để khớp với CollectionTracker
            string abilityName = ability.Name;
            string unlockKey = $"Weapon_{abilityName}_Used";
            isUnlocked = PlayerPrefs.GetInt(unlockKey, 0) == 1;
            
            if (isUnlocked)
            {
                nameText.text = ability.Name;
                itemImage.sprite = ability.Image;
            }
            else
            {
                nameText.text = "???";
                itemImage.sprite = null;
            }
            
            UpdateUI();
        }

        private void UpdateUI()
        {
            if (lockOverlay != null)
            {
                lockOverlay.gameObject.SetActive(!isUnlocked);
                if (!isUnlocked)
                {
                    lockOverlay.color = lockOverlayColor;
                }
            }
        }
    }
}
