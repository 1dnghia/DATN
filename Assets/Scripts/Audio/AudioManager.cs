using UnityEngine;

namespace Vampire
{
    public class AudioManager : MonoBehaviour
    {
        private static AudioManager instance;
        public static AudioManager Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject go = new GameObject("AudioManager");
                    instance = go.AddComponent<AudioManager>();
                    DontDestroyOnLoad(go);
                }
                return instance;
            }
        }

        [Header("Audio Data")]
        [SerializeField] private AudioData audioData;
        
        [Header("Audio Sources")]
        private AudioSource musicSource;
        private AudioSource sfxSource;
        
        private AudioClip currentMusic;

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }
            
            instance = this;
            DontDestroyOnLoad(gameObject);
            
            // Tự động load AudioData từ Resources nếu chưa có
            if (audioData == null)
            {
                audioData = Resources.Load<AudioData>("AudioData");
                if (audioData == null)
                {
                    Debug.LogError("AudioManager: AudioData not found in Resources folder! Please create AudioData and place it in Resources folder.");
                }
            }
            
            InitializeAudioSources();
            LoadVolumeSettings();
        }

        private void InitializeAudioSources()
        {
            // Music source
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.loop = true;
            musicSource.playOnAwake = false;
            
            // SFX source
            sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.loop = false;
            sfxSource.playOnAwake = false;
        }

        private void LoadVolumeSettings()
        {
            if (audioData != null)
            {
                SetMasterVolume(PlayerPrefs.GetFloat("MasterVolume", audioData.masterVolume));
                SetMusicVolume(PlayerPrefs.GetFloat("MusicVolume", audioData.musicVolume));
                SetSFXVolume(PlayerPrefs.GetFloat("SFXVolume", audioData.sfxVolume));
            }
        }

        // Music Control
        public void PlayMusic(AudioClip clip)
        {
            if (clip == null || clip == currentMusic) return;
            
            musicSource.Stop();
            musicSource.clip = clip;
            musicSource.Play();
            currentMusic = clip;
        }

        public void StopMusic()
        {
            musicSource.Stop();
            currentMusic = null;
        }

        public void PauseMusic()
        {
            musicSource.Pause();
        }

        public void ResumeMusic()
        {
            musicSource.UnPause();
        }

        // SFX Control
        public void PlaySFX(AudioClip clip)
        {
            if (clip == null) return;
            sfxSource.PlayOneShot(clip);
        }

        public void PlaySFXAtPosition(AudioClip clip, Vector3 position)
        {
            if (clip == null) return;
            AudioSource.PlayClipAtPoint(clip, position, sfxSource.volume);
        }

        // Volume Control
        public void SetMasterVolume(float volume)
        {
            AudioListener.volume = volume;
            PlayerPrefs.SetFloat("MasterVolume", volume);
        }

        public void SetMusicVolume(float volume)
        {
            if (musicSource != null)
                musicSource.volume = volume;
            PlayerPrefs.SetFloat("MusicVolume", volume);
        }

        public void SetSFXVolume(float volume)
        {
            if (sfxSource != null)
                sfxSource.volume = volume;
            PlayerPrefs.SetFloat("SFXVolume", volume);
        }

        // Quick access methods for common sounds
        public void PlayButtonClick() => PlaySFX(audioData?.buttonClick);
        public void PlayButtonHover() => PlaySFX(audioData?.buttonHover);
        public void PlayAbilitySelect() => PlaySFX(audioData?.abilitySelect);
        public void PlayMenuOpen() => PlaySFX(audioData?.menuOpen);
        public void PlayMenuClose() => PlaySFX(audioData?.menuClose);
        
        public void PlayPlayerLevelUp() => PlaySFX(audioData?.playerLevelUp);
        public void PlayPlayerHit() => PlaySFX(audioData?.playerHit);
        public void PlayPlayerDeath() => PlaySFX(audioData?.playerDeath);
        
        public void PlayWeaponSwing() => PlaySFX(audioData?.weaponSwing);
        public void PlayWeaponShoot() => PlaySFX(audioData?.weaponShoot);
        public void PlayWeaponHit() => PlaySFX(audioData?.weaponHit);
        public void PlayThrowableThrow() => PlaySFX(audioData?.throwableThrow);
        public void PlayThrowableExplode() => PlaySFX(audioData?.throwableExplode);
        public void PlayAbilityActivate() => PlaySFX(audioData?.abilityActivate);
        
        public void PlayEnemyHit() => PlaySFX(audioData?.enemyHit);
        public void PlayEnemyDeath() => PlaySFX(audioData?.enemyDeath);
        public void PlayEnemyAttack() => PlaySFX(audioData?.enemyAttack);
        public void PlayBossRoar() => PlaySFX(audioData?.bossRoar);
        
        public void PlayCoinPickup() => PlaySFX(audioData?.coinPickup);
        public void PlayGemPickup() => PlaySFX(audioData?.gemPickup);
        public void PlayHealthPickup() => PlaySFX(audioData?.healthPickup);
        public void PlayBombPickup() => PlaySFX(audioData?.bombPickup);
        public void PlayMagnetPickup() => PlaySFX(audioData?.magnetPickup);
        public void PlayPotionPickup() => PlaySFX(audioData?.potionPickup);
        public void PlayChestOpen() => PlaySFX(audioData?.chestOpen);
        
        public void PlayMainMenuMusic() => PlayMusic(audioData?.mainMenuMusic);
        public void PlayGameplayMusic() => PlayMusic(audioData?.gameplayMusic);
        public void PlayBossMusic() => PlayMusic(audioData?.bossMusic);
        public void PlayVictoryMusic() => PlayMusic(audioData?.victoryMusic);
        public void PlayDefeatMusic() => PlayMusic(audioData?.defeatMusic);
    }
}
