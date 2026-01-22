using UnityEngine;
using System;

namespace Vampire
{
    
    [Serializable]
    public enum MapDebuffType
    {
        None, 
        ReducedHealth,      
        IncreasedEnemySpeed, // Tăng tốc độ enemy
        IncreasedEnemyDamage, // Tăng damage enemy
        ReducedPlayerSpeed,  // Giảm tốc độ player
        ReducedExpGain,      // Giảm exp nhận được
        IncreasedEnemySpawn, // Tăng số lượng enemy spawn
        ReducedAbilityDamage // Giảm damage ability
    }
    
    [Serializable]
    public class MapDebuff
    {
        public MapDebuffType type;
        [Range(0.1f, 2.0f)]
        public float multiplier = 0.8f; // 0.8 = giảm 20%, 1.2 = tăng 20%
        
        [TextArea(2, 3)]
        public string description;
    }

    [CreateAssetMenu(fileName = "New Map", menuName = "Vampire/Map Blueprint")]
    public class MapBlueprint : ScriptableObject
    {
        [Header("Map Info")]
        [Tooltip("Tên của map")]
        public new string name;
        
        [Tooltip("Hình ảnh preview của map")]
        public Sprite mapPreview;
        
        [Header("Map Settings")]
        [Tooltip("Level Blueprint của map này")]
        public LevelBlueprint levelBlueprint;
        
        [Header("Unlock Settings")]
        [Tooltip("Map cần phải hoàn thành trước để unlock map này (nếu null thì mở sẵn)")]
        public MapBlueprint requiredMap;
        
        [Header("Map Debuffs")]
        [Tooltip("Các debuff được áp dụng cho map này")]
        public MapDebuff[] debuffs;
    }
}
