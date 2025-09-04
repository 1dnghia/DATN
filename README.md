# Vampire Survivors Clone - Unity Project

## 📖 Mô tả dự án
Đây là một clone của game **Vampire Survivors** được phát triển bằng Unity 2022.3+. Game là một bullet hell survival roguelike với cơ chế tự động tấn công và hệ thống nâng cấp dần tiến.

## 🎮 Gameplay Core Features
- **Tự động di chuyển**: Player chỉ điều khiển movement, không có attack button
- **Auto attack**: Weapons tự động tấn công enemies gần nhất
- **Survival gameplay**: Sống sót càng lâu càng tốt
- **Level up system**: Gain EXP từ enemies → Level up → Chọn upgrades
- **Wave-based enemies**: Enemies spawn theo waves, tăng độ khó theo thời gian
- **Power scaling**: Player và weapons mạnh dần theo level

## 🛠️ Kiến trúc dự án hiện tại

### 📁 Cấu trúc Scripts:
```
Assets/Scripts/
├── Player/              # Logic nhân vật người chơi
│   ├── PlayerController.cs      ✅ (Simple pause với ESC)
│   ├── PlayerMovement.cs        ✅ (WASD + mobile virtual joystick)
│   ├── PlayerStats.cs           📝 (Chưa implement)
│   └── PlayerExperience.cs      📝 (Chưa implement)
├── Enemy/               # Logic kẻ thù
│   ├── EnemyController.cs       📝 (Chưa implement)
│   ├── EnemyMovement.cs         📝 (Chưa implement)
│   ├── EnemyStats.cs            📝 (Chưa implement)
│   └── EnemySpawner.cs          📝 (Chưa implement)
├── Weapons/             # Hệ thống vũ khí tự động
│   ├── WeaponBase.cs            📝 (Chưa implement)
│   ├── ProjectileWeapon.cs      📝 (Chưa implement)
│   ├── MeleeWeapon.cs           📝 (Chưa implement)
│   ├── WeaponManager.cs         📝 (Chưa implement)
│   ├── Projectile.cs            📝 (Chưa implement)
│   ├── WhipWeapon.cs            📝 (Chưa implement)
│   ├── MagicMissileWeapon.cs    📝 (Chưa implement)
│   └── BibleWeapon.cs           📝 (Chưa implement)
├── Items/               # Vật phẩm pickup
│   ├── ItemPickup.cs            📝 (Chưa implement)
│   ├── ExperienceGem.cs         📝 (Chưa implement)
│   ├── HealthPotion.cs          📝 (Chưa implement)
│   └── PowerUp.cs               📝 (Chưa implement)
├── Managers/            # Quản lý hệ thống
│   ├── GameManager.cs           📝 (Chưa implement)
│   ├── WaveManager.cs           📝 (Chưa implement)
│   ├── AudioManager.cs          📝 (Chưa implement)
│   ├── ScoreManager.cs          📝 (Chưa implement)
│   └── UpgradeManager.cs        📝 (Chưa implement)
├── UI/                  # Giao diện người dùng
│   ├── UIManager.cs             📝 (Chưa implement)
│   ├── HealthBar.cs             📝 (Chưa implement)
│   ├── ExperienceBar.cs         📝 (Chưa implement)
│   ├── UpgradePanel.cs          📝 (Chưa implement)
│   ├── GameOverPanel.cs         📝 (Chưa implement)
│   └── MainMenu.cs              📝 (Chưa implement)
├── Core/                # Hệ thống cốt lõi
│   ├── GameData.cs              📝 (Chưa implement)
│   ├── SaveSystem.cs            📝 (Chưa implement)
│   ├── GameStateManager.cs      📝 (Chưa implement)
│   └── CameraController.cs      📝 (Chưa implement)
├── Input/               # Hệ thống input đơn giản
│   └── TouchInput.cs            ✅ (Virtual joystick cho mobile)
├── VFX/                 # Hiệu ứng
├── Events/              # Event system
├── Interfaces/          # Interfaces cho clean code
├── Constants/           # Constants và Enums
├── Data/                # Data structures
├── ScriptableObjects/   # Data configuration
├── Settings/            # Game settings
├── Utils/               # Utilities
└── Gameplay/           # Game mechanics
```

## 🎯 Input System - Đơn giản như Vampire Survivors
**✅ ĐÃ HOÀN THÀNH** - Sử dụng Unity Input Manager cơ bản:

### PC Input:
```csharp
// WASD hoặc Arrow Keys
moveInput.x = Input.GetAxis("Horizontal");  // A/D, Arrow Left/Right
moveInput.y = Input.GetAxis("Vertical");    // W/S, Arrow Up/Down

// Pause game
if (Input.GetKeyDown(KeyCode.Escape))
    PauseGame();
```

### Mobile Input:
- **TouchInput.cs**: Virtual joystick với UI drag & drop
- **Cross-platform**: Tự động detect mobile và hiện joystick
- **Static access**: `TouchInput.MoveInput` - không cần references

### Đã xóa các file phức tạp:
- ❌ `InputSystem_Actions.inputactions` (không cần)
- ❌ `InputManager.cs` (không cần)
- ❌ Unity Input System phức tạp (authentic hơn khi đơn giản)

## 📊 Data Architecture

### ScriptableObjects cho configuration:
- `WeaponData.cs` - Weapon stats và upgrade paths
- `EnemyData.cs` - Enemy stats và behaviors  
- `CharacterData.cs` - Player character variations
- `WaveData.cs` - Enemy wave configurations

### Data Structures:
- `PlayerStatsData` - Player statistics
- `WeaponStatsData` - Weapon statistics  
- `EnemyStatsData` - Enemy statistics
- `UpgradeOption` - Upgrade choices
- `EnemySpawnData` - Spawn configurations

## 🎨 Asset Structure
```
Assets/
├── Art/                 # Visual assets
│   ├── Characters/      # Player sprites
│   ├── Enemies/         # Enemy sprites
│   ├── Weapons/         # Weapon sprites
│   ├── Items/           # Item sprites
│   ├── Environment/     # Background/tiles
│   ├── UI/              # UI elements
│   └── VFX/             # Visual effects
├── Audio/               # Sound effects và music
├── Prefabs/             # GameObject prefabs
│   ├── Player/
│   ├── Enemies/
│   ├── Weapons/
│   ├── Items/
│   ├── UI/
│   └── VFX/
└── Data/                # ScriptableObject instances
    ├── Weapons/
    ├── Enemies/
    ├── Characters/
    └── Waves/
```

## 🚀 Development Status

### ✅ Hoàn thành:
1. **Project structure** - Cấu trúc thư mục và files
2. **Input system** - Movement cho PC và mobile
3. **Basic player movement** - Smooth movement với acceleration
4. **Cross-platform input** - PC keyboard + mobile virtual joystick

### 📝 Cần implement tiếp:
1. **Player Stats & Experience system**
2. **Enemy AI và spawning system**  
3. **Weapons auto-attack system**
4. **Level up và upgrade system**
5. **UI system (health bar, exp bar, upgrade panel)**
6. **Game state management**
7. **Audio system**
8. **Save/Load system**

## 🎯 Development Approach
- **Authentic to original**: Giữ gameplay đơn giản như Vampire Survivors
- **Clean architecture**: Sử dụng interfaces, events, ScriptableObjects
- **Performance-focused**: Object pooling cho bullets/enemies
- **Cross-platform**: PC + Mobile support

## 🛠️ Tech Stack
- **Unity 2022.3+ LTS**
- **2D Renderer (URP)**
- **C# Scripting**
- **Unity Input Manager** (traditional, không dùng Input System)
- **ScriptableObjects** cho data
- **Event-driven architecture**
- **Single Assembly** (không dùng Assembly Definition cho đơn giản)

## 📝 Notes cho Developer:
- Game focus vào **survival và progression**, không phải skill-based combat
- **Auto-attack** là core mechanic - player chỉ di chuyển
- **Scaling** là key - enemies và player power phải scale đều
- **Simple input** - authentic với game gốc
- **Mobile-friendly** - UI và controls phải tốt trên mobile

---
**Last Updated**: August 29, 2025
**Current Focus**: Implementing player stats và experience system
