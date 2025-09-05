# ✅ Unity Setup Checklist - Vampire Survivors Clone

## 🎯 Phase 1: Project Setup (10 phút)

### Project Creation
- [ ] Tạo Unity project mới với template **2D (URP)**
- [ ] Import **TextMeshPro** từ Package Manager
- [ ] Setup **Sorting Layers**: Background, Ground, Player, Enemies, Projectiles, UI

### Folder Structure
```
Assets/
├── Scripts/Events/     ← EventManager.cs
├── Scripts/Player/     ← Player scripts (5 files)
├── Scripts/UI/         ← UI scripts (4 files)
├── Scripts/Input/      ← Input scripts
├── Prefabs/           ← Sẽ lưu Player, Enemy prefabs
├── Scenes/            ← Game scenes
└── Art/Sprites/       ← Sprite assets
```

---

## 🎮 Phase 2: Player Setup (15 phút)

### Player GameObject
- [ ] Tạo **Player** GameObject (Empty)
- [ ] Add **Rigidbody2D**: Gravity Scale = 0, Linear Drag = 5
- [ ] Add **CircleCollider2D**: Radius = 0.5
- [ ] Add **PlayerController** script (sẽ auto-assign các script khác)

### Player Visual
- [ ] Tạo child **PlayerSprite** với SpriteRenderer
- [ ] Set Sorting Layer = **Player**
- [ ] Tạm dùng Unity default sprite, màu xanh

### Test Player Movement
- [ ] Play scene
- [ ] Test **WASD** keys → Player phải di chuyển được
- [ ] Test **H** (damage), **J** (heal), **K** (gain XP)

---

## 🎨 Phase 3: UI Setup (20 phút)

### Main Canvas
- [ ] Tạo **GameUI** Canvas
- [ ] Set **Screen Space - Overlay**
- [ ] Canvas Scaler: **Scale With Screen Size**, Resolution **1920x1080**

### XP Bar (Top of screen)
- [ ] Tạo **ExperienceBar** Slider trong GameUI
- [ ] Anchor: **Top Stretch**, Height = 50
- [ ] Style: Background màu đen, Fill màu vàng
- [ ] Add child **LevelText** (TextMeshPro)
- [ ] Add **ExperienceBarUI** script

### Health Bar (Follow Player)
- [ ] Tạo **PlayerHealthBar** Slider trong GameUI
- [ ] Size: Width = 100, Height = 20
- [ ] Style: Background màu đỏ đậm, Fill màu xanh
- [ ] Add child **HealthText** (TextMeshPro)
- [ ] Add **ScreenSpaceHealthBar** script
- [ ] Assign **Target = Player**

### UI Manager
- [ ] Add **SimplePlayerUIManager** script vào GameUI
- [ ] Check auto-assignment working

---

## 🧪 Phase 4: Testing (10 phút)

### Movement Test
- [ ] Play scene
- [ ] **WASD** keys → Player di chuyển mượt mà
- [ ] Player không bị "trượt" khi thả phím

### Health System Test  
- [ ] Press **H** → Health bar giảm và đổi màu
- [ ] Press **J** → Health bar tăng
- [ ] Health bar **follow player** khi di chuyển

### XP System Test
- [ ] Press **K** → XP bar tăng
- [ ] Press **K** nhiều lần → Level up, XP bar reset
- [ ] Level text update đúng

### Game Flow Test
- [ ] Press **ESC** → Game pause, player dừng
- [ ] Press **ESC** again → Resume
- [ ] Press **H** nhiều lần → Player chết → Game over → Auto restart
- [ ] Press **R** → Manual restart

### No Errors
- [ ] Console **không có error** màu đỏ
- [ ] Chỉ có debug logs màu trắng

---

## 🎯 Expected Results

### ✅ Player:
- Di chuyển mượt mà với WASD
- Có visual sprite hiển thị
- Debug keys (H/J/K) working

### ✅ Health Bar:
- Hiển thị trên đầu player
- Follow player khi di chuyển  
- Đổi màu: Xanh → Vàng → Đỏ
- Hiển thị số "100/100"

### ✅ XP Bar:
- Full width ở top màn hình
- Màu vàng, background đen
- Hiển thị "Level 1"
- Level up khi đầy

---

## 🚨 Common Issues

### Player không di chuyển:
```
Fix: Check Rigidbody2D attached và Gravity Scale = 0
```

### Health bar không follow:
```
Fix: Assign Player vào Target field của ScreenSpaceHealthBar
```

### UI không hiển thị:
```
Fix: Check Canvas RenderMode = Screen Space Overlay
```

### Scripts báo lỗi:
```
Fix: Copy đúng code, check namespace và using statements
```

---

## 🚀 Next Phase Options

Sau khi hoàn thành checklist này, bạn có thể chọn:

### Option A: Enemy System
- Enemy spawning
- Enemy AI (follow player)
- Enemy health & damage

### Option B: Weapon System  
- Auto-attack enemies
- Projectile shooting
- Damage calculation

### Option C: Game Manager
- Start game flow
- Game over screen
- Score system

**Bạn muốn làm system nào tiếp theo?**
