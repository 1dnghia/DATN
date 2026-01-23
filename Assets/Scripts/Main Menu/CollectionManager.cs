using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Vampire
{
    public class CollectionManager : MonoBehaviour
    {
        [Header("Map Data Source")]
        [SerializeField] private MapSelection mapSelection;
        
        [Header("Character Data Source")]
        [SerializeField] private CharacterSelector characterSelector;
        
        [Header("Monster Collection")]
        [SerializeField] private GameObject monsterCardPrefab;
        [SerializeField] private Transform monsterCardsContainer;
        
        [Header("Weapon Collection")]
        [SerializeField] private GameObject weaponCardPrefab;
        [SerializeField] private Transform weaponCardsContainer;
        
        private MonsterBlueprint[] allMonsters;
        private GameObject[] allWeapons;
        private bool isInitialized = false;
        private bool isDataCollected = false;

        private void Awake()
        {
            // Không gọi CollectDataFromMaps ở đây nữa
        }
        
        private void OnEnable()
        {
            if (!isInitialized)
            {
                StartCoroutine(InitializeCollectionsDelayed());
            }
            else
            {
                // Nếu đã init rồi, chỉ cần rebuild layout
                StartCoroutine(RebuildLayoutDelayed());
            }
        }
        
        private IEnumerator InitializeCollectionsDelayed()
        {
            // Thu thập data từ maps trước (chỉ làm 1 lần)
            if (!isDataCollected)
            {
                CollectDataFromMaps();
                isDataCollected = true;
            }
            
            // Đợi 2 frames để đảm bảo GameObject đã active hoàn toàn
            yield return null;
            yield return null;
            
            InitializeMonsterCollection();
            InitializeWeaponCollection();
            
            isInitialized = true;
            
            // Đợi thêm để các card được instantiate hoàn toàn
            yield return null;
            
            // Force update canvas nhiều lần
            Canvas.ForceUpdateCanvases();
            yield return null;
            Canvas.ForceUpdateCanvases();
            
            // Rebuild layout cho content containers
            if (monsterCardsContainer != null)
            {
                RectTransform containerRect = monsterCardsContainer.GetComponent<RectTransform>();
                LayoutRebuilder.ForceRebuildLayoutImmediate(containerRect);
            }
            
            if (weaponCardsContainer != null)
            {
                RectTransform containerRect = weaponCardsContainer.GetComponent<RectTransform>();
                LayoutRebuilder.ForceRebuildLayoutImmediate(containerRect);
            }
            
            // Đợi thêm và rebuild parent (ScrollRect viewport)
            yield return null;
            
            if (monsterCardsContainer != null)
            {
                RectTransform containerRect = monsterCardsContainer.GetComponent<RectTransform>();
                RectTransform parentRect = containerRect.parent as RectTransform;
                if (parentRect != null)
                {
                    LayoutRebuilder.ForceRebuildLayoutImmediate(parentRect);
                    
                    // Rebuild cả ông nội (ScrollView root)
                    RectTransform grandParentRect = parentRect.parent as RectTransform;
                    if (grandParentRect != null)
                    {
                        LayoutRebuilder.ForceRebuildLayoutImmediate(grandParentRect);
                    }
                }
            }
            
            if (weaponCardsContainer != null)
            {
                RectTransform containerRect = weaponCardsContainer.GetComponent<RectTransform>();
                RectTransform parentRect = containerRect.parent as RectTransform;
                if (parentRect != null)
                {
                    LayoutRebuilder.ForceRebuildLayoutImmediate(parentRect);
                    
                    // Rebuild cả ông nội (ScrollView root)
                    RectTransform grandParentRect = parentRect.parent as RectTransform;
                    if (grandParentRect != null)
                    {
                        LayoutRebuilder.ForceRebuildLayoutImmediate(grandParentRect);
                    }
                }
            }
            
            // Force update lần cuối
            yield return null;
            Canvas.ForceUpdateCanvases();
        }
        
        private IEnumerator RebuildLayoutDelayed()
        {
            yield return null;
            yield return null;
            
            Canvas.ForceUpdateCanvases();
            
            if (monsterCardsContainer != null)
            {
                RectTransform containerRect = monsterCardsContainer.GetComponent<RectTransform>();
                LayoutRebuilder.ForceRebuildLayoutImmediate(containerRect);
                
                RectTransform parentRect = containerRect.parent as RectTransform;
                if (parentRect != null)
                {
                    LayoutRebuilder.ForceRebuildLayoutImmediate(parentRect);
                    
                    RectTransform grandParentRect = parentRect.parent as RectTransform;
                    if (grandParentRect != null)
                    {
                        LayoutRebuilder.ForceRebuildLayoutImmediate(grandParentRect);
                    }
                }
            }
            
            if (weaponCardsContainer != null)
            {
                RectTransform containerRect = weaponCardsContainer.GetComponent<RectTransform>();
                LayoutRebuilder.ForceRebuildLayoutImmediate(containerRect);
                
                RectTransform parentRect = containerRect.parent as RectTransform;
                if (parentRect != null)
                {
                    LayoutRebuilder.ForceRebuildLayoutImmediate(parentRect);
                    
                    RectTransform grandParentRect = parentRect.parent as RectTransform;
                    if (grandParentRect != null)
                    {
                        LayoutRebuilder.ForceRebuildLayoutImmediate(grandParentRect);
                    }
                }
            }
            
            yield return null;
            Canvas.ForceUpdateCanvases();
        }

        private void CollectDataFromMaps()
        {
            if (mapSelection == null || mapSelection.MapBlueprints == null || mapSelection.MapBlueprints.Length == 0)
            {
                Debug.LogWarning("CollectionManager: No map selector or map blueprints found!");
                return;
            }

            HashSet<MonsterBlueprint> monsterSet = new HashSet<MonsterBlueprint>();
            HashSet<GameObject> weaponSet = new HashSet<GameObject>();

            foreach (var mapBlueprint in mapSelection.MapBlueprints)
            {
                if (mapBlueprint == null || mapBlueprint.levelBlueprint == null)
                    continue;

                LevelBlueprint levelBlueprint = mapBlueprint.levelBlueprint;

                // Thu thập monsters
                if (levelBlueprint.monsters != null)
                {
                    foreach (var monsterContainer in levelBlueprint.monsters)
                    {
                        if (monsterContainer.monsterBlueprints != null)
                        {
                            foreach (var monster in monsterContainer.monsterBlueprints)
                            {
                                if (monster != null)
                                {
                                    monsterSet.Add(monster);
                                }
                            }
                        }
                    }
                }

                // Thu thập mini-bosses
                if (levelBlueprint.miniBosses != null)
                {
                    foreach (var miniBoss in levelBlueprint.miniBosses)
                    {
                        if (miniBoss != null && miniBoss.bossBlueprint != null)
                        {
                            monsterSet.Add(miniBoss.bossBlueprint);
                        }
                    }
                }

                // Thu thập final boss
                if (levelBlueprint.finalBoss != null && levelBlueprint.finalBoss.bossBlueprint != null)
                {
                    monsterSet.Add(levelBlueprint.finalBoss.bossBlueprint);
                }

                // Thu thập abilities/weapons từ level
                if (levelBlueprint.abilityPrefabs != null)
                {
                    foreach (var ability in levelBlueprint.abilityPrefabs)
                    {
                        if (ability != null)
                        {
                            weaponSet.Add(ability);
                        }
                    }
                }
            }
            
            // Thu thập starting abilities từ tất cả characters
            if (characterSelector != null)
            {
                CharacterBlueprint[] characters = GetCharacterBlueprints();
                if (characters != null)
                {
                    foreach (var character in characters)
                    {
                        if (character != null && character.startingAbilities != null)
                        {
                            foreach (var ability in character.startingAbilities)
                            {
                                if (ability != null)
                                {
                                    weaponSet.Add(ability);
                                }
                            }
                        }
                    }
                }
            }

            allMonsters = monsterSet.ToArray();
            allWeapons = weaponSet.ToArray();
        }
        
        // Lấy CharacterBlueprint[] từ CharacterSelector bằng reflection
        private CharacterBlueprint[] GetCharacterBlueprints()
        {
            if (characterSelector == null) return null;
            
            var field = typeof(CharacterSelector).GetField("characterBlueprints", 
                System.Reflection.BindingFlags.NonPublic | 
                System.Reflection.BindingFlags.Instance);
            
            if (field != null)
            {
                return field.GetValue(characterSelector) as CharacterBlueprint[];
            }
            
            return null;
        }

        private void InitializeMonsterCollection()
        {
            if (allMonsters == null || monsterCardPrefab == null || monsterCardsContainer == null)
                return;

            foreach (var monster in allMonsters)
            {
                if (monster == null) continue;
                
                GameObject cardObj = Instantiate(monsterCardPrefab, monsterCardsContainer);
                CollectionCard card = cardObj.GetComponent<CollectionCard>();
                if (card != null)
                {
                    card.InitMonster(monster);
                }
            }
            
            if (monsterCardsContainer != null)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(monsterCardsContainer.GetComponent<RectTransform>());
            }
        }

        private void InitializeWeaponCollection()
        {
            if (allWeapons == null || weaponCardPrefab == null || weaponCardsContainer == null)
                return;

            foreach (var weapon in allWeapons)
            {
                if (weapon == null) continue;
                
                GameObject cardObj = Instantiate(weaponCardPrefab, weaponCardsContainer);
                CollectionCard card = cardObj.GetComponent<CollectionCard>();
                if (card != null)
                {
                    card.InitWeapon(weapon);
                }
            }
            
            if (weaponCardsContainer != null)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(weaponCardsContainer.GetComponent<RectTransform>());
            }
        }

        public void RefreshCollection()
        {
            foreach (Transform child in monsterCardsContainer)
            {
                Destroy(child.gameObject);
            }
            foreach (Transform child in weaponCardsContainer)
            {
                Destroy(child.gameObject);
            }
            
            InitializeMonsterCollection();
            InitializeWeaponCollection();
        }
    }
}
