using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Vampire
{
    public class CharacterSelector : MonoBehaviour
    {
        [SerializeField] protected CharacterBlueprint[] characterBlueprints;
        [SerializeField] protected GameObject characterCardPrefab;
        [SerializeField] protected CoinDisplay coinDisplay;

        private CharacterCard[] characterCards;
        private bool initialized = false;

        private void OnEnable()
        {
            // Kiểm tra xem các card có bị destroy không
            bool cardsValid = true;
            if (characterCards != null)
            {
                foreach (var card in characterCards)
                {
                    if (card == null)
                    {
                        cardsValid = false;
                        break;
                    }
                }
            }
            
            // Khởi tạo lại nếu chưa init hoặc cards bị destroy
            if (!initialized || characterCards == null || characterCards.Length == 0 || !cardsValid)
            {
                Init();
            }
        }
        
        public void Init()
        {
            // Xóa các card cũ nếu có
            if (characterCards != null)
            {
                foreach (var card in characterCards)
                {
                    if (card != null)
                        Destroy(card.gameObject);
                }
            }

            characterCards = new CharacterCard[characterBlueprints.Length];
            for (int i = 0; i < characterBlueprints.Length; i++)
            {
                characterCards[i] = Instantiate(characterCardPrefab, this.transform).GetComponent<CharacterCard>();
                characterCards[i].Init(this, characterBlueprints[i], coinDisplay);
            }
            
            LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
            
            for (int i = 0; i < characterBlueprints.Length; i++)
            {
                characterCards[i].UpdateLayout();
            }
            
            initialized = true;
        }
        
        public void StartGame(CharacterBlueprint characterBlueprint)
        {
            CrossSceneData.CharacterBlueprint = characterBlueprint;
            SceneManager.LoadScene(1);
        }
    }
}
