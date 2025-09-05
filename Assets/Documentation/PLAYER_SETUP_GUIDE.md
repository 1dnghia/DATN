# 🎮 Player Setup Guide

## ✅ Hoàn thành Player System

### **Reset Function**
Khi bạn bấm **Reset** trong Unity Inspector, tất cả components sẽ tự động:
- ✅ **PlayerController**: Auto-assign PlayerMovement, PlayerStats, PlayerExperience
- ✅ **PlayerStats**: Reset về default values (100 HP, 10 damage, etc.)
- ✅ **PlayerExperience**: Reset về Level 1, 0 XP
- ✅ **PlayerMovement**: Reset về default speed settings

### **Hệ thống Kinh nghiệm - Production Ready**
XP System đã được tối ưu cho game thực tế:

#### **Core Features:**
- ✅ **Progressive XP Requirements**: Mỗi level cần nhiều XP hơn
- ✅ **Event-driven**: Tích hợp với UI và các system khác  
- ✅ **Mobile Optimized**: Không spam debug logs
- ✅ **UI Ready**: Có các method cho progress bars và displays

#### **XP Formula (FIXED - Production Ready):**
```
Level 1→2: 100 XP (base)
Level 2→3: 120 XP (+20, 20% increase)  
Level 3→4: 144 XP (+24, 20% increase)
Level 4→5: 173 XP (+29, 20% increase)
Level 5→6: 207 XP (+34, 20% increase)
```

**Key Fix Applied:**
- ✅ **ExperienceRequired** now correctly returns XP needed for **NEXT level**
- ✅ Level progression actually increases by 20% each level
- ✅ Formula: `baseXP * (1.2 ^ level)`

#### **Settings có thể điều chỉnh:**
- `baseExperienceRequired = 100f` - XP cần cho level đầu
- `experienceMultiplier = 1.2f` - Mỗi level tăng 20% XP requirement
- Có thể thay đổi multiplier từ 1.1 (10%) đến 2.0 (100%)

---

## 🎯 Cách Setup trong Unity

### **1. Tạo Player GameObject:**
```
Player
├── Rigidbody2D (Gravity Scale = 0, Freeze Rotation Z = true)
├── CircleCollider2D hoặc BoxCollider2D
├── SpriteRenderer (gắn sprite)
├── PlayerController ← Script chính
├── PlayerMovement ← Auto-assigned khi Reset
├── PlayerStats ← Auto-assigned khi Reset  
└── PlayerExperience ← Auto-assigned khi Reset
```

### **2. Auto-Setup Steps:**
1. Tạo empty GameObject tên "Player"
2. Add component `PlayerController`
3. **Bấm Reset** trên PlayerController → Tự động gắn tất cả scripts
4. Add `Rigidbody2D` và `Collider2D`
5. Add `SpriteRenderer` và gắn sprite

---

## 🎮 Debug Controls

### **In-Game Testing:**
- **H Key**: Take 10 damage
- **J Key**: Heal 10 HP
- **K Key**: Gain 25 XP
- **ESC Key**: Pause game

### **Context Menu (Right-click trên component):**
#### PlayerExperience:
- "Add 50 XP" - Thêm 50 XP
- "Level Up" - Level up ngay lập tức  
- "Show XP Table (Next 5 Levels)" - Hiển thị bảng XP requirements
- **"Test Level Progression"** - Test tự động 5 level đầu để verify formula

---

## 📊 Console Debug Output

Khi gain XP và level up, console sẽ hiển thị:
```
Gained 25.0 XP (Base: 25.0). Current: 75.0/100.0
Gained 30.0 XP (Base: 30.0). Current: 105.0/100.0

🎉 LEVEL UP! Level 1 → 2
XP Required: 100 → 120 (+20)
Remaining XP: 5.0/120
```

---

## 🔗 Event System

### **Events được trigger:**
- `EventManager.OnPlayerDamaged` - Khi player bị damage
- `EventManager.OnPlayerHealed` - Khi player được heal
- `EventManager.OnPlayerDied` - Khi player chết
- `EventManager.OnExperienceGained` - Khi gain XP
- `EventManager.OnPlayerLevelUp` - Khi level up
- `EventManager.OnUpgradePanelOpen` - Khi level up (mở upgrade panel)

### **Local Events:**
- `PlayerStats.OnHealthChanged` - (currentHP, maxHP)
- `PlayerStats.OnPlayerDeath` - Player death
- `PlayerExperience.OnLevelUp` - (newLevel)
- `PlayerExperience.OnExperienceChanged` - (currentXP, requiredXP)

---

## 🚀 Ready for Integration

Player system đã sẵn sàng để integrate với:
- ✅ **UI System** (Health bar, XP bar, Level display)
- ✅ **Enemy System** (Take damage from enemies)
- ✅ **Weapon System** (Use damage stats)
- ✅ **Upgrade System** (Level up bonuses)
- ✅ **Mobile Support** (TouchInput integration)

**Next Step**: Implement Enemy System hoặc UI System!

---

## 📊 XP System API cho UI (Production Ready)

### **Real-time Progress Tracking:**
```csharp
// For XP Progress Bar (0.0 to 1.0)
float progress = playerExperience.GetExperienceProgressPercentage();

// For XP Display Text  
string xpText = playerExperience.GetExperienceText(); // "75/120"
string levelText = playerExperience.GetLevelText(); // "Level 3"

// For "XP to Next Level" display
float xpNeeded = playerExperience.GetExperienceNeededForNextLevel(); // 45.0
```

### **Events cho UI Updates:**
```csharp
// Subscribe trong UI script
PlayerExperience.OnExperienceChanged += UpdateXPBar; // (current, required)
PlayerExperience.OnLevelUp += ShowLevelUpEffect; // (newLevel)
EventManager.OnPlayerLevelUp += OpenUpgradePanel; // (newLevel)
```

### **System Integration:**
- ✅ **No spam logs** - Chỉ log khi cần thiết trong Editor
- ✅ **Event-driven** - UI tự động update qua events
- ✅ **Performance optimized** - Không có Update() loop không cần thiết
- ✅ **Mobile ready** - Minimal debug overhead
