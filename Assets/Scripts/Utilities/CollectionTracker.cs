using UnityEngine;

namespace Vampire
{
    public static class CollectionTracker
    {
        public static void UnlockMonster(MonsterBlueprint monster)
        {
            if (monster == null) return;
            
            string unlockKey = $"Monster_{monster.name}_Discovered";
            if (PlayerPrefs.GetInt(unlockKey, 0) == 0)
            {
                PlayerPrefs.SetInt(unlockKey, 1);
                PlayerPrefs.Save();
            }
        }

        public static void UnlockWeapon(string abilityName)
        {
            if (string.IsNullOrEmpty(abilityName)) return;
            
            string unlockKey = $"Weapon_{abilityName}_Used";
            if (PlayerPrefs.GetInt(unlockKey, 0) == 0)
            {
                PlayerPrefs.SetInt(unlockKey, 1);
                PlayerPrefs.Save();
            }
        }

        public static bool IsMonsterUnlocked(MonsterBlueprint monster)
        {
            if (monster == null) return false;
            string unlockKey = $"Monster_{monster.name}_Discovered";
            return PlayerPrefs.GetInt(unlockKey, 0) == 1;
        }

        public static bool IsWeaponUnlocked(string abilityName)
        {
            if (string.IsNullOrEmpty(abilityName)) return false;
            string unlockKey = $"Weapon_{abilityName}_Used";
            return PlayerPrefs.GetInt(unlockKey, 0) == 1;
        }

        public static void ResetAllCollection()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }
    }
}
