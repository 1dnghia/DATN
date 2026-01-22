# 🎮 TÓM TẮT DỰ ÁN GAME

## **Tên Game:** Magic Survivors 

---

## 🔧 Công Nghệ Sử Dụng

### **Game Engine & Framework:**
- **Unity 2023+** (Unity 6)
- **C# .NET** Programming Language
- **TextMeshPro** - UI & Text rendering
- **Unity Localization** - Hỗ trợ đa ngôn ngữ
- **Unity Addressables** - Quản lý asset động
- **Unity Timeline** - Cutscenes & animations

### **Architecture & Design Patterns:**
- **ScriptableObject Architecture** - Data-driven design
- **Singleton Pattern** - Quản lý các Manager
- **Observer Pattern** - Events & callbacks
- **Object Pooling** - Tối ưu hiệu suất
- **MVC Pattern** - Quản lý UI

### **Hệ Thống Chính:**
- Audio Manager (Background music & SFX)
- Achievement System với rewards
- Save/Load System (PlayerPrefs)
- Map Debuff System
- Collection Tracker (Monster & Weapon discovery)

---

## 🎯 Gameplay

### **Thể Loại:**
Roguelike Survivor - Top-down Action (giống Vampire Survivors)

### **Cốt Lõi:**
- Điều khiển nhân vật sống sót qua các đợt quái vật tấn công liên tục
- **Thời gian sống sót:** 10 phút/map → Boss xuất hiện
- **Auto-attack weapons** với khả năng nâng cấp
- **Wave-based spawning** với difficulty curve

---

## 🎮 Hệ Thống Game

### **1. Character Selection**
- **3 nhân vật** với stats khác nhau
- Mỗi character có đặc điểm riêng:
  - HP (Health Points)
  - Speed (Tốc độ di chuyển)
  - Damage (Sát thương)
  - Armor (Giáp)
  - Recovery (Hồi phục)

### **2. Map Selection (5 Maps)**
Mỗi map có **Debuff riêng** để tăng độ khó:

| Map | Debuff |
|-----|--------|
| Map 1 | Basic map, không có debuff hoặc debuff nhẹ |
| Map 2 | Increased Enemy Speed |
| Map 3 | Reduced Player Speed + Reduced Exp Gain |
| Map 4 | Increased Enemy Spawn + Increased Enemy Damage |
| Map 5 | Multiple debuffs (Boss map) |

**Debuff Types:**
- ❤️ Reduced Health (Giảm máu tối đa)
- ⚡ Increased Enemy Speed (Tăng tốc độ enemy)
- 💥 Increased Enemy Damage (Tăng damage enemy)
- 🐌 Reduced Player Speed (Giảm tốc độ player)
- 📉 Reduced Exp Gain (Giảm exp nhận được)
- 👾 Increased Enemy Spawn (Tăng spawn rate)
- 🗡️ Reduced Ability Damage (Giảm damage skills)

**Unlock System:** Complete map trước để mở map sau

### **3. Combat System**

**8 Loại Quái Vật:**
- **Melee Monsters:** Low / Mid / High (3 tier sức mạnh)
- **Ranged Monster:** Bắn đạn từ xa
- **Boomerang Monster:** Ném boomerang quay lại
- **Gravity Monster:** Ném vật thể có trọng lực
- **Mini Boss:** Boss xuất hiện giữa game
- **Final Boss:** Boss cuối với abilities phức tạp

**Mechanics:**
- Wave-based spawning với difficulty curve theo thời gian
- Knockback & damage system
- Boss có nhiều abilities và attack patterns

### **4. Progression System**

**Level Up Flow:**
1. Kill monsters → Thu thập EXP Gems
2. Đủ EXP → Level up
3. Chọn **1 trong 3 upgrade cards** ngẫu nhiên

**Upgrade Cards:**
- 💚 **Max Health Card** - Tăng máu tối đa
- ⚔️ **Damage Card** - Tăng sát thương
- 🛡️ **Armor Card** - Tăng giáp
- 👟 **Move Speed Card** - Tăng tốc độ
- 🍀 **Luck Card** - Tăng tỷ lệ drop item
- ❤️‍🩹 **Recovery Card** - Tăng hồi phục HP
- 🧲 **Pickup Range Card** - Tăng phạm vi thu thập

### **5. Loot System**

**Collectables:**
- 💎 **Exp Gems** - Thu thập để level up
- 🪙 **Coins** - Tiền tệ trong game
- ❤️ **Health Potions** - Hồi máu
- 🧲 **Magnet** - Hút tất cả items
- 💣 **Bomb** - Gây damage AOE
- 🎁 **Chests** - Spawn định kỳ, chứa rewards

**Chest Types:**
- **Default Chest** - Chest thường
- **Boss Chest** - Drop từ boss
- **Powerup Chest** - Chứa powerups
- **Failsafe Chest** - Chest dự phòng

### **6. Achievement System**
- Unlock achievements qua gameplay
- **Rewards:** Coins, Weapons, Characters
- **Persistent tracking** - Lưu tiến trình vĩnh viễn
- Hiển thị trong Achievement menu

### **7. Collection System**
- 📖 **Monster Collection** - Track các monster đã gặp
- ⚔️ **Weapon Collection** - Track các weapon đã dùng
- Gallery view trong Main Menu
- Unlock progress được lưu

---

## 🏆 Win/Lose Conditions

### **Win Condition:**
✅ Survive đủ 10 phút và đánh bại Final Boss

### **Lose Condition:**
❌ HP = 0 (Game Over)

---

## 📂 Cấu Trúc Dự Án

```
Assets/
├── Scripts/              # C# game logic
│   ├── Character/        # Player controller, Abilities
│   ├── Monsters/         # AI behavior, Boss patterns
│   ├── Managers/         # Audio, Level, Debuff managers
│   ├── UI/               # Menus, Dialogs, HUD
│   ├── Gameplay/         # Game systems (Timer, Spawning)
│   ├── Utilities/        # Helper classes
│   ├── ScriptableObjects/ # Blueprint definitions
│   └── Testing/          # Development test scripts
│
├── Blueprints/           # ScriptableObjects (data files)
│   ├── Characters/       # Character stats
│   ├── Levels/           # Level configurations
│   ├── Map/              # Map data with debuffs
│   ├── Monsters/         # Monster stats
│   ├── Chests/           # Chest configurations
│   ├── UpgradeCard/      # Upgrade card data
│   └── Audio/            # Audio data
│
├── Scenes/               # Unity scenes
│   └── Game/             # Main Menu, Gameplay
│
├── Prefabs/              # Reusable GameObjects
│   ├── Characters/       # Player prefabs
│   ├── Monsters/         # Enemy prefabs
│   ├── Abilities/        # Weapon/skill prefabs
│   └── UI/               # UI components
│
├── Sprites/              # 2D artwork & textures
├── Audio/                # Music & Sound effects
├── Localization/         # Multi-language files
├── Materials/            # Unity materials
└── Shaders/              # Custom shaders

```

---

## 🎨 Đặc Điểm Nổi Bật

### ✅ **Modular Design**
- Dễ dàng thêm character/map/monster/weapon mới
- Không cần code, chỉ cần tạo ScriptableObject

### ✅ **Data-Driven Architecture**
- Tất cả game data trong ScriptableObject
- Designer-friendly, không cần lập trình viên

### ✅ **Localization Ready**
- Hỗ trợ đa ngôn ngữ (EN, VI, JP...)
- Unity Localization System

### ✅ **Achievement & Collection**
- Tăng replayability
- Unlock system khuyến khích chơi lại

### ✅ **Map Debuff System**
- Độc đáo, khác biệt với Vampire Survivors gốc
- Mỗi map có thách thức riêng

### ✅ **Complete Audio System**
- Background music cho từng scene
- Sound effects cho mọi action
- Volume control & mute options

### ✅ **Development Tools**
- Test scripts để test boss/victory nhanh
- Cheat commands cho debugging

---

## 🔄 Game Flow

```
Main Menu
    ↓
Character Selection (3 choices)
    ↓
Map Selection (5 maps)
    ↓
Gameplay Start
    ↓
[Loop: Survive → Kill Monsters → Gain EXP → Level Up → Choose Upgrades]
    ↓
10 minutes → Boss Spawn
    ↓
Defeat Boss → Victory Screen
OR
HP = 0 → Game Over Screen
    ↓
Stats Display + Achievements Check
    ↓
Return to Main Menu
```

---

## 🛠️ Testing Tools

### **TestVictory.cs**
- Nhấn **V** để trigger chiến thắng ngay
- Test victory screen & achievements

### **TestBossSpawn.cs**
- Nhấn **B** để spawn boss ngay lập tức
- Nhấn **T** để nhảy tới thời điểm boss spawn
- Nhấn **N** để hiển thị thời gian hiện tại

### **MiscTesting.cs**
- **Space** - Thu thập tất cả coins & gems
- **G** - Damage tất cả enemies (500 dmg)
- **E** - Gain 1000 exp
- **A** - Play audio test

---

## 📊 Technical Stats

- **Unity Version:** 2023+ (Unity 6)
- **Target Platform:** PC (Windows/Mac/Linux)
- **Scripting Backend:** IL2CPP / Mono
- **API Level:** .NET Standard 2.1
- **Total Scripts:** 50+ C# files
- **Total Assets:** 40+ ScriptableObjects
- **Scenes:** 2 (Main Menu + Game)

---

## 🎯 Roadmap (Future Features)

- [ ] More characters (5+)
- [ ] More maps (10+)
- [ ] More weapons & abilities
- [ ] Procedural map generation
- [ ] Online leaderboards
- [ ] Daily challenges
- [ ] Meta progression system
- [ ] Mobile platform support

---

**Developed with Unity 2023+**  
**Architecture: ScriptableObject-based Data-Driven Design**  
**Genre: Roguelike Survivor / Bullet Heaven**
