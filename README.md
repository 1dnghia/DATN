# Vampire Survivors Clone - Unity Project

## 📖 Mô tả dự án
Đây là một clone của game **Vampire Survivors** được phát triển bằng Unity 2022.3+. Game là một bullet hell survival roguelike với cơ chế tự động tấn công và hệ thống nâng cấp dần tiến.

## 📚 Documentation
**→ Tất cả hướng dẫn chi tiết có trong: [`Assets/Documentation/`](./Assets/Documentation/README.md)**

### 🚀 Quick Start:
1. **[Setup Unity](./Assets/Documentation/UNITY_SETUP_COMPLETE_GUIDE.md)** - Hướng dẫn setup từ đầu
2. **[Checklist](./Assets/Documentation/UNITY_SETUP_CHECKLIST.md)** - Theo dõi tiến độ nhanh
3. **[Player System](./Assets/Documentation/PLAYER_SETUP_GUIDE.md)** - Setup Player & UI

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
│   ├── PlayerController.cs      ✅ (Main coordinator với auto-assignment)
│   ├── PlayerMovement.cs        ✅ (Physics-based movement với WASD + mobile)
│   ├── PlayerStats.cs           ✅ (Health system với events)
│   ├── PlayerExperience.cs      ✅ (XP system với progressive scaling)
│   └── PlayerAnimationController.cs ✅ (Animation control)
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
│   ├── GameManager.cs           ✅ (Game state, pause/resume, restart)
│   ├── WaveManager.cs           📝 (Enemy wave progression)
│   ├── AudioManager.cs          📝 (SFX và music)
│   ├── ScoreManager.cs          📝 (Scoring system)
│   └── UpgradeManager.cs        📝 (Level up upgrade choices)
├── UI/                  # Giao diện người dùng
│   ├── ExperienceBarUI.cs       ✅ (XP bar với custom assets + DOTween)
│   ├── ScreenSpaceHealthBar.cs  ✅ (Health bar follow player + effects)
│   ├── SimplePlayerUIManager.cs ✅ (Single canvas UI coordinator)
│   ├── GameOverPanel.cs         📝 (Game over UI)
│   ├── MainMenu.cs              📝 (Menu system)
│   └── UpgradePanel.cs          📝 (Level up upgrade choices)
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
1. **Project structure** - Cấu trúc thư mục và files ✅
2. **Input system** - Movement cho PC và mobile ✅
3. **Player system hoàn chỉnh** - PlayerController, PlayerStats, PlayerExperience ✅
4. **Player movement** - Smooth movement với physics ✅
5. **Health & XP systems** - Complete với events và progression ✅
6. **UI system** - Custom asset support với DOTween animations ✅
   - XP Bar với level up effects
   - Health Bar follow player
   - Single Canvas architecture
   - Custom sprite support
7. **Event-driven architecture** - PlayerStats, PlayerExperience events ✅
8. **GameManager** - Game state, pause/resume, restart ✅
9. **Code optimization** - Clean legacy code, no duplicates ✅

### 📝 Đang tiếp tục:
1. **Enemy System** - AI, spawning, health/damage
2. **Weapon System** - Auto-attack, projectiles, upgrades
3. **Item/Pickup System** - XP gems, health potions, power-ups
4. **Upgrade System** - Level up choices, weapon evolution
5. **Wave Management** - Progressive enemy difficulty
6. **Audio System** - SFX và background music
7. **Game Polish** - Particle effects, screen shake, juice

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
**Last Updated**: September 4, 2025
**Current Focus**: Enemy System implementation - AI, spawning, combat mechanics

**Player System**: ✅ **HOÀN THÀNH 100%** - Ready for production!
