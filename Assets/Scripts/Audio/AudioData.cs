using UnityEngine;

namespace Vampire
{
    [CreateAssetMenu(fileName = "AudioData", menuName = "Vampire/Audio Data")]
    public class AudioData : ScriptableObject
    {
        [Header("Music")]
        public AudioClip mainMenuMusic;
        public AudioClip gameplayMusic;
        public AudioClip bossMusic;
        public AudioClip victoryMusic;
        public AudioClip defeatMusic;
        
        [Header("SFX - UI")]
        public AudioClip buttonClick;
        public AudioClip buttonHover;
        public AudioClip abilitySelect;      // Khi chọn ability trong level up dialog
        public AudioClip menuOpen;
        public AudioClip menuClose;
        
        [Header("SFX - Player")]
        public AudioClip playerHit;
        public AudioClip playerDeath;
        public AudioClip playerLevelUp;      // Âm thanh level up của player
        public AudioClip playerWalk;         // Âm thanh bước chân (optional)
        
        [Header("SFX - Weapons & Abilities")]
        public AudioClip weaponSwing;        // Vũ khí cận chiến (dao, kiếm)
        public AudioClip weaponShoot;        // Vũ khí bắn (súng, cung)
        public AudioClip weaponHit;          // Âm thanh đạn trúng
        public AudioClip throwableThrow;     // Ném vũ khí (lựu đạn, dao bay)
        public AudioClip throwableExplode;   // Nổ (bomb, molotov)
        public AudioClip abilityActivate;    // Kích hoạt ability đặc biệt
        
        [Header("SFX - Enemy")]
        public AudioClip enemyHit;           // Quái bị đánh
        public AudioClip enemyDeath;         // Quái chết
        public AudioClip bossRoar;           // Boss gầm (spawn/rage)
        public AudioClip enemyAttack;        // Quái tấn công player
        
        [Header("SFX - Items & Collectables")]
        public AudioClip coinPickup;         // Nhặt tiền
        public AudioClip gemPickup;          // Nhặt exp gem
        public AudioClip healthPickup;       // Nhặt máu
        public AudioClip bombPickup;         // Nhặt bomb item
        public AudioClip magnetPickup;       // Nhặt magnet
        public AudioClip potionPickup;       // Nhặt potion
        public AudioClip chestOpen;          // Mở rương
        
        [Header("SFX - Ambient & Effects")]
        public AudioClip fireLoop;           // Âm thanh lửa cháy (molotov fire)
        public AudioClip damageText;         // Âm thanh hiện damage text (optional)
        
        [Header("Volume Settings")]
        [Range(0f, 1f)] public float masterVolume = 1f;
        [Range(0f, 1f)] public float musicVolume = 0.7f;
        [Range(0f, 1f)] public float sfxVolume = 1f;
    }
}
