using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Vampire
{
    // Quản lý việc hiển thị và chọn map
    public class MapSelection : MonoBehaviour
    {
        [Header("Map Data")]
        [SerializeField] private MapBlueprint[] mapBlueprints;
        
        [Header("UI References")]
        [SerializeField] private GameObject mapCardPrefab;
        [SerializeField] private Transform mapCardsContainer;
        
        private MapCard[] mapCards;
        private bool initialized = false;
        private CharacterBlueprint selectedCharacter;
        
        public MapBlueprint[] MapBlueprints => mapBlueprints;
        
        
        public void Init(CharacterBlueprint character)
        {
            selectedCharacter = character;
            InitializeMapCards();
            
            Debug.Log($"MapSelection initialized with character: {(character != null ? character.name : "null")}");
        }

        private void InitializeMapCards()
        {
            if (mapBlueprints == null || mapBlueprints.Length == 0)
            {
                Debug.LogError("MapSelector: No map blueprints assigned!");
                return;
            }
            
            
            if (mapCards != null)
            {
                foreach (var card in mapCards)
                {
                    if (card != null)
                        Destroy(card.gameObject);
                }
            }

            
            mapCards = new MapCard[mapBlueprints.Length];
            for (int i = 0; i < mapBlueprints.Length; i++)
            {
                GameObject cardObj = Instantiate(mapCardPrefab, mapCardsContainer);
                mapCards[i] = cardObj.GetComponent<MapCard>();
                mapCards[i].Init(this, mapBlueprints[i]);
            }
            
            // Force rebuild layout
            if (mapCardsContainer != null)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(mapCardsContainer.GetComponent<RectTransform>());
            }
            
            initialized = true;
        }
        
        public void StartGame(MapBlueprint mapBlueprint)
        {
            if (selectedCharacter == null)
            {
                Debug.LogError("MapSelection: No character selected!");
                return;
            }
            
            if (mapBlueprint.levelBlueprint == null)
            {
                Debug.LogError($"MapSelection: Map '{mapBlueprint.name}' does not have a level blueprint assigned!");
                return;
            }
            
            // Lưu thông tin character và map vào CrossSceneData
            CrossSceneData.CharacterBlueprint = selectedCharacter;
            CrossSceneData.CurrentMap = mapBlueprint;
            
            // Load scene của level blueprint
            Debug.Log($"Starting game with character '{selectedCharacter.name}' on map '{mapBlueprint.name}' using level '{mapBlueprint.levelBlueprint.name}'");
            SceneManager.LoadScene(mapBlueprint.levelBlueprint.sceneName);
        }

        private void OnDestroy()
        {
            if (mapCards != null)
            {
                foreach (var card in mapCards)
                {
                    if (card != null)
                        Destroy(card.gameObject);
                }
            }
        }
    }
}


