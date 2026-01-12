using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

namespace Vampire
{
    public class CollectionManager : MonoBehaviour
    {
        [Header("Map Data Source")]
        [SerializeField] private MapSelection mapSelection;
        
        [Header("Monster Collection")]
        [SerializeField] private GameObject monsterCardPrefab;
        [SerializeField] private Transform monsterCardsContainer;
        
        [Header("Weapon Collection")]
        [SerializeField] private GameObject weaponCardPrefab;
        [SerializeField] private Transform weaponCardsContainer;
        
        private MonsterBlueprint[] allMonsters;
        private GameObject[] allWeapons;

        private void Start()
        {
            CollectDataFromMaps();
            InitializeMonsterCollection();
            InitializeWeaponCollection();
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

                // Thu thập abilities/weapons
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

            allMonsters = monsterSet.ToArray();
            allWeapons = weaponSet.ToArray();

            Debug.Log($"CollectionManager: Collected {allMonsters.Length} unique monsters and {allWeapons.Length} unique weapons");
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
