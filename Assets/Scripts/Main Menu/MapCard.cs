using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Vampire
{
    public class MapCard : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private Image mapPreviewImage;
        [SerializeField] private Button selectButton;
        [SerializeField] private Image lockOverlay;
        
        [Header("Lock Overlay Settings")]
        [SerializeField] private Color lockOverlayColor = new Color(0f, 0f, 0f, 0.7f);
        
        private MapSelection mapSelection;
        private MapBlueprint mapBlueprint;
        private bool isUnlocked;

        public void Init(MapSelection mapSelection, MapBlueprint mapBlueprint)
        {
            this.mapSelection = mapSelection;
            this.mapBlueprint = mapBlueprint;
            
            CheckUnlockStatus();
            
            nameText.text = mapBlueprint.name;
            mapPreviewImage.sprite = mapBlueprint.mapPreview;
            
            selectButton.onClick.RemoveAllListeners();
            selectButton.onClick.AddListener(OnSelectMap);
            
            UpdateUI();
        }

        private void CheckUnlockStatus()
        {
            if (mapBlueprint.requiredMap == null)
            {
                isUnlocked = true;
                return;
            }
            
            string requiredMapKey = $"Map_{mapBlueprint.requiredMap.name}_Completed";
            bool requiredMapCompleted = PlayerPrefs.GetInt(requiredMapKey, 0) == 1;
            isUnlocked = requiredMapCompleted;
        }

        private void UpdateUI()
        {
            if (isUnlocked)
            {
                if (lockOverlay != null)
                {
                    lockOverlay.gameObject.SetActive(false);
                }
                selectButton.interactable = true;
            }
            else
            {
                if (lockOverlay != null)
                {
                    lockOverlay.gameObject.SetActive(true);
                    lockOverlay.color = lockOverlayColor;
                }
                selectButton.interactable = false;
            }
        }

        private void OnSelectMap()
        {
            if (isUnlocked)
            {
                mapSelection.StartGame(mapBlueprint);
            }
        }

        private void OnDestroy()
        {
            selectButton?.onClick.RemoveAllListeners();
        }
    }
}
