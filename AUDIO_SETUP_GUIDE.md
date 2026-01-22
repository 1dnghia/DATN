# Hướng Dẫn Setup Audio System

## ✅ Logic Đã Hoàn Thiện

AudioManager đã được tích hợp hoàn chỉnh vào game với các chức năng:

### 🎵 Music System
- Main Menu Music
- Gameplay Music
- Boss Music (thông qua MusicZone)
- Victory Music
- Defeat Music

### 🔊 SFX System
- **Player**: Hit, Death, Level Up
- **Enemy**: Hit, Death, Attack
- **Weapons**: Swing, Shoot, Hit, Throw, Explode
- **UI**: Button Click, Button Hover, Menu Open/Close
- **Collectables**: Coin, Gem, Health, Bomb, Magnet, Potion, Chest

---

## 📋 Các Bước Setup Trong Unity

### Bước 1: Tạo AudioData Asset
1. Trong Unity, chuột phải vào thư mục `Assets/Resources/`
   - Nếu chưa có thư mục `Resources`, tạo mới: `Assets/Resources/`
2. Chọn **Create → Vampire → Audio Data**
3. Đặt tên là `AudioData` (chính xác tên này)

### Bước 2: Gán Audio Clips vào AudioData
1. Select file `AudioData` vừa tạo
2. Trong Inspector, gán các AudioClip tương ứng:
   - **Music Section**: mainMenuMusic, gameplayMusic, bossMusic, victoryMusic, defeatMusic
   - **SFX - UI**: buttonClick, buttonHover, abilitySelect, menuOpen, menuClose
   - **SFX - Player**: playerHit, playerDeath, playerLevelUp, playerWalk
   - **SFX - Weapons**: weaponSwing, weaponShoot, weaponHit, throwableThrow, throwableExplode, abilityActivate
   - **SFX - Enemy**: enemyHit, enemyDeath, bossRoar, enemyAttack
   - **SFX - Items**: coinPickup, gemPickup, healthPickup, bombPickup, magnetPickup, potionPickup, chestOpen
   - **SFX - Ambient**: fireLoop, damageText
3. Điều chỉnh Volume Settings nếu cần:
   - Master Volume (0-1)
   - Music Volume (0-1)
   - SFX Volume (0-1)

### Bước 3: Setup AudioBootstrap
1. Mở scene **Main Menu** (scene đầu tiên của game)
2. Tạo một Empty GameObject mới, đặt tên là `AudioBootstrap`
3. Add component `AudioBootstrap` vào GameObject này
4. Đảm bảo checkbox "Initialize On Awake" được bật

### Bước 4: Test Audio System
1. Play scene Main Menu
2. Kiểm tra Console log: "AudioBootstrap: AudioManager initialized"
3. Kiểm tra các âm thanh:
   - Main Menu có nhạc nền
   - Button click có âm thanh
   - Vào gameplay có nhạc gameplay
   - Player bị hit có âm thanh
   - Enemy chết có âm thanh
   - Nhặt coin/gem có âm thanh
   - Game over/Victory có nhạc phù hợp

---

## 🔧 Optional: Thêm MusicZone cho Boss

Nếu muốn thay đổi nhạc khi Boss xuất hiện:

1. Tìm prefab hoặc GameObject của Boss
2. Add component `MusicZone`
3. Gán `zoneMusic` = boss music clip
4. Add `Collider2D` (Is Trigger = true)
5. Điều chỉnh size của collider để cover khu vực boss
6. Khi player vào zone này, nhạc sẽ tự động chuyển sang boss music

---

## 🎮 Optional: Thêm Audio cho Buttons

Các button đã tự động có audio nếu:
- Button được quản lý bởi `AudioTriggerManager`
- Hoặc có component `AudioTrigger` được add vào

Để thêm custom audio cho button riêng lẻ:
1. Select Button GameObject
2. Add component `AudioTrigger`
3. Trong Inspector, gọi method `SetSettings()`

---

## 🐛 Troubleshooting

### Không có âm thanh?
1. Kiểm tra AudioData có trong `Assets/Resources/AudioData.asset`
2. Kiểm tra Console có error "AudioData not found"?
3. Kiểm tra các AudioClip đã được gán trong AudioData chưa?
4. Kiểm tra AudioBootstrap đã được add vào scene Main Menu chưa?
5. Kiểm tra volume settings trong AudioData (không phải = 0)

### Một số âm thanh không chơi?
1. Kiểm tra AudioClip tương ứng đã được gán trong AudioData chưa?
2. Kiểm tra Console có warning nào không?

### Music không loop?
- Music sources được set loop = true tự động trong AudioManager.InitializeAudioSources()

---

## 📝 Notes

- AudioManager tự động load AudioData từ Resources folder
- AudioManager là Singleton, persist across scenes (DontDestroyOnLoad)
- Có thể gọi `AudioManager.Instance.PlayXXX()` từ bất kỳ đâu
- Volume settings được lưu vào PlayerPrefs tự động
- Hệ thống hỗ trợ 2 AudioSources: 1 cho music (loop), 1 cho SFX (one-shot)
