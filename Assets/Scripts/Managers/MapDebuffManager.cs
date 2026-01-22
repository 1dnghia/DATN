using UnityEngine;

namespace Vampire
{
    
    public class MapDebuffManager : MonoBehaviour
    {
        private static MapDebuffManager instance;
        public static MapDebuffManager Instance => instance;

        private MapBlueprint currentMap;
        private bool debuffsApplied = false;

        
        public float HealthMultiplier { get; private set; } = 1f;
        public float EnemySpeedMultiplier { get; private set; } = 1f;
        public float EnemyDamageMultiplier { get; private set; } = 1f;
        public float PlayerSpeedMultiplier { get; private set; } = 1f;
        public float ExpGainMultiplier { get; private set; } = 1f;
        public float EnemySpawnMultiplier { get; private set; } = 1f;
        public float AbilityDamageMultiplier { get; private set; } = 1f;

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;
        }

        private void Start()
        {
            
            currentMap = CrossSceneData.CurrentMap;
            if (currentMap != null)
            {
                ApplyDebuffs();
            }
            else
            {
                Debug.LogWarning("MapDebuffManager: No map data found in CrossSceneData!");
            }
        }
        private void ApplyDebuffs()
        {
            if (debuffsApplied) return;
            
            if (currentMap.debuffs == null || currentMap.debuffs.Length == 0)
            {
                debuffsApplied = true;
                return;
            }

            foreach (var debuff in currentMap.debuffs)
            {
                ApplyDebuff(debuff);
            }

            debuffsApplied = true;
        }

        private void ApplyDebuff(MapDebuff debuff)
        {
            switch (debuff.type)
            {
                case MapDebuffType.ReducedHealth:
                    HealthMultiplier = debuff.multiplier;
                    break;

                case MapDebuffType.IncreasedEnemySpeed:
                    EnemySpeedMultiplier = debuff.multiplier;
                    break;

                case MapDebuffType.IncreasedEnemyDamage:
                    EnemyDamageMultiplier = debuff.multiplier;
                    break;

                case MapDebuffType.ReducedPlayerSpeed:
                    PlayerSpeedMultiplier = debuff.multiplier;
                    break;

                case MapDebuffType.ReducedExpGain:
                    ExpGainMultiplier = debuff.multiplier;
                    break;

                case MapDebuffType.IncreasedEnemySpawn:
                    EnemySpawnMultiplier = debuff.multiplier;
                    break;

                case MapDebuffType.ReducedAbilityDamage:
                    AbilityDamageMultiplier = debuff.multiplier;
                    break;

                case MapDebuffType.None:
                default:
                    break;
            }
        }
        
        public string GetDebuffsDescription()
        {
            if (currentMap == null || currentMap.debuffs == null || currentMap.debuffs.Length == 0)
            {
                return "No debuffs";
            }

            string description = "";
            foreach (var debuff in currentMap.debuffs)
            {
                if (!string.IsNullOrEmpty(description))
                    description += "\n";
                    
                description += $"â€¢ {debuff.description}";
            }

            return description;
        }

        private void OnDestroy()
        {
            if (instance == this)
            {
                instance = null;
            }
        }
    }
}
