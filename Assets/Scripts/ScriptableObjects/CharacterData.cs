using UnityEngine;

[CreateAssetMenu(fileName = "New Character Data", menuName = "Game/Character Data")]
public class CharacterData : ScriptableObject
{
    [Header("Character Info")]
    public string characterName;
    public string description;
    public Sprite characterSprite;
    
    [Header("Base Stats")]
    public PlayerStatsData baseStats;
    
    [Header("Starting Weapon")]
    public WeaponData startingWeapon;
}
