using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Data", menuName = "Game/Weapon Data")]
public class WeaponData : ScriptableObject
{
    [Header("Weapon Info")]
    public string weaponName;
    public string description;
    public Sprite weaponIcon;
    public WeaponType weaponType;
    
    [Header("Base Stats")]
    public WeaponStatsData baseStats;
    
    [Header("Upgrade Data")]
    public float[] damagePerLevel;
    public float[] attackSpeedPerLevel;
    public int maxLevel = 8;
    
    [Header("Prefabs")]
    public GameObject weaponPrefab;
    public GameObject projectilePrefab;
}
