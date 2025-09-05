# 🎮 Hướng dẫn Setup Unity hoàn chỉnh - Vampire Survivors Clone

## 📋 Mục lục
1. [Chuẩn bị Project](#1-chuẩn-bị-project)
2. [Import Scripts](#2-import-scripts)
3. [Setup Player](#3-setup-player)
4. [Setup UI System](#4-setup-ui-system)
5. [Setup Input System](#5-setup-input-system)
6. [Test & Debug](#6-test--debug)

---

## 1. 🛠️ Chuẩn bị Project

### 1.1. Unity Version & Template
```
✅ Unity 2022.3 LTS hoặc mới hơn
✅ Template: 2D (URP)
✅ Target Platform: PC/Mobile
```

### 1.2. Package Manager Requirements
```
Window → Package Manager → Install:
- Universal RP (đã có sẵn trong 2D URP)
- TextMeshPro (Essential)
- Input System (New) - Optional nếu muốn dùng Input System mới
```

### 1.3. Project Settings
```
Edit → Project Settings:

Player:
- Company Name: [Tên của bạn]
- Product Name: "Vampire Survivors Clone"
- Default Icon: [Tùy chọn]

XR Settings:
- Initialize XR on Startup: ✗ (uncheck)

Graphics:
- Scriptable Render Pipeline: URP Asset (đã setup sẵn)
```

---

## 2. 📁 Import Scripts

### 2.1. Tạo cấu trúc thư mục
```
Assets/
├── Scripts/
│   ├── Core/
│   ├── Player/
│   ├── UI/
│   ├── Input/
│   └── Events/
├── Prefabs/
├── Scenes/
├── Art/
│   ├── Sprites/
│   └── UI/
└── Audio/
```

### 2.2. Copy các scripts vào đúng thư mục
```
Scripts/Core/:
- GameManager.cs (sẽ tạo sau)

Scripts/Events/:
- EventManager.cs

Scripts/Player/:
- PlayerController.cs
- PlayerStats.cs
- PlayerExperience.cs
- PlayerMovement.cs
- PlayerAnimationController.cs

Scripts/UI/:
- ExperienceBarUI.cs
- ScreenSpaceHealthBar.cs
- SimplePlayerUIManager.cs
- LevelUpVFX.cs

Scripts/Input/:
- VampireSurvivorsInputActions.cs (nếu dùng Input System cũ)
```

---

## 3. 🎮 Setup Player

### 3.1. Tạo Player GameObject

#### Bước 1: Tạo Player
```
Right-click trong Hierarchy → Create Empty
Đặt tên: "Player"
Position: (0, 0, 0)
```

#### Bước 2: Add Components cho Player
```
Select Player GameObject:

1. Add Component → Rigidbody2D
   - Body Type: Dynamic
   - Mass: 1
   - Linear Drag: 5 (để dừng mượt)
   - Angular Drag: 0.05
   - Gravity Scale: 0 (top-down game)

2. Add Component → Collider2D (Circle hoặc Capsule)
   - Circle Collider 2D:
   - Radius: 0.5

3. Add Component → Player Controller (script)
   - Script sẽ tự động assign các component khác
```

#### Bước 3: Tạo Player Visual
```
Right-click Player → 2D Object → Sprite
Đặt tên: "PlayerSprite"
Position: (0, 0, 0)

SpriteRenderer:
- Sprite: Tạm thời dùng Unity default sprite
- Color: Blue hoặc màu bạn muốn
- Sorting Layer: Player (tạo mới)
- Order in Layer: 0
```

#### Bước 4: Player Hierarchy hoàn chỉnh
```
Player
├── PlayerSprite (SpriteRenderer)
└── [Scripts sẽ tự động tạo các components]
```

### 3.2. Cài đặt Sorting Layers
```
Edit → Project Settings → Tags and Layers:

Sorting Layers (từ dưới lên):
- Default
- Background
- Ground
- Player
- Enemies
- Projectiles
- UI
```

---

## 4. 🎨 Setup UI System

### 4.1. Tạo Canvas chính

#### Bước 1: Tạo Canvas
```
Right-click Hierarchy → UI → Canvas
Đặt tên: "GameUI"
```

#### Bước 2: Cài đặt Canvas
```
Canvas Component:
- Render Mode: Screen Space - Overlay
- Pixel Perfect: ✓
- Sort Order: 0

Canvas Scaler:
- UI Scale Mode: Scale With Screen Size
- Reference Resolution: 1920 x 1080
- Screen Match Mode: Match Width Or Height
- Match: 0.5
```

#### Bước 3: Add Canvas Group (Optional)
```
Add Component → Canvas Group
- Alpha: 1
- Interactable: ✓
- Blocks Raycasts: ✓
```

### 4.2. Tạo Experience Bar (XP Bar) với Custom Assets

#### Bước 1: Tạo XP Bar Container
```
Right-click GameUI → Create Empty
Đặt tên: "ExperienceBar"

Rect Transform:
- Anchor Presets: Top Stretch (Alt+Shift+Click)
- Left: 20, Right: 20 (margin từ cạnh)
- Top: 20
- Height: 40 (hoặc theo height của asset)
- Pos Y: -30
```

#### Bước 2: Tạo XP Background
```
Right-click ExperienceBar → UI → Image
Tên: "XP_Background"
- Source Image: [Custom XP background sprite]
- Image Type: Simple/Sliced
- Anchor: Stretch Stretch
- Color: White
```

#### Bước 3: Tạo XP Fill (Animation target)
```
Right-click ExperienceBar → UI → Image  
Tên: "XP_Fill"
- Source Image: [Custom XP fill sprite]
- Image Type: Filled → Horizontal → Fill From Left
- Fill Amount: 0.5 (test value)
- Anchor: Stretch Stretch
- Color: Gold/Yellow (hoặc màu bạn muốn)
```

#### Bước 4: Tạo XP Frame (Optional - cho đẹp)
```
Right-click ExperienceBar → UI → Image
Tên: "XP_Frame" 
- Source Image: [Custom frame/border sprite]
- Image Type: Simple
- Anchor: Stretch Stretch
- Color: White
- Raycast Target: ✗ (tắt để không block input)

** Drag xuống cuối trong Hierarchy để render trên top **
```

#### Bước 5: Add Level Text
```
Right-click ExperienceBar → UI → Text - TextMeshPro
Đặt tên: "LevelText"

RectTransform:
- Anchor: Middle Center
- Width: 120, Height: 40
- Position: (0, 0, 0)

TextMeshPro:
- Text: "Level 1"
- Font Size: 20
- Color: White (hoặc màu contrast với background)
- Alignment: Center Middle
- Font Style: Bold
```

#### Bước 6: Add Script - ExperienceBarUI
```
Select ExperienceBar:
Add Component → Experience Bar UI

Script Settings:
- XP Slider: null (leave empty - chúng ta dùng custom Image)
- XP Fill Image: XP_Fill (drag từ Hierarchy)
- Level Text: LevelText (drag từ Hierarchy)
- Background Image: XP_Background (optional)

DOTween Settings (Auto-detected):
- Use DOTween: ✓ (nếu đã import DOTween)
- Duration: 0.5s
- Ease: OutQuart
- Level Up Punch: ✓
```

### 4.3. Tạo Health Bar (Follow Player) với Custom Assets

#### Bước 1: Tạo Health Bar Container  
```
Right-click GameUI → Create Empty
Đặt tên: "PlayerHealthBar"

RectTransform:
- Anchor: Middle Center
- Width: 80 (hoặc theo custom asset width)
- Height: 12 (hoặc theo custom asset height)
- Position sẽ được script điều khiển để follow player
```

#### Bước 2: Tạo Health Background
```
Right-click PlayerHealthBar → UI → Image
Tên: "Health_Background"
- Source Image: [Custom health background sprite]
- Image Type: Simple/Sliced
- Anchor: Stretch Stretch
- Color: White
```

#### Bước 3: Tạo Health Fill (Animation target)
```
Right-click PlayerHealthBar → UI → Image
Tên: "Health_Fill"
- Source Image: [Custom health fill sprite]  
- Image Type: Filled → Horizontal → Fill From Left
- Fill Amount: 1.0 (full health)
- Anchor: Stretch Stretch
- Color: Green (0, 255, 0, 255)
```

#### Bước 4: Tạo Health Frame (Optional)
```
Right-click PlayerHealthBar → UI → Image
Tên: "Health_Frame"
- Source Image: [Custom frame sprite]
- Image Type: Simple
- Anchor: Stretch Stretch
- Color: White
- Raycast Target: ✗

** Drag xuống cuối để render trên top **
```

#### Bước 5: Add Health Text (Optional)
```
Right-click PlayerHealthBar → UI → Text - TextMeshPro
Đặt tên: "HealthText"

RectTransform:
- Anchor: Middle Center
- Width: 70, Height: 15
- Position: (0, 0, 0)

TextMeshPro:
- Text: "100/100"
- Font Size: 10
- Color: White
- Alignment: Center Middle
- Font Style: Bold
```

#### Bước 6: Add Script - ScreenSpaceHealthBar
```
Select PlayerHealthBar:
Add Component → Screen Space Health Bar

Script Settings:
- Target: Player GameObject (drag từ Hierarchy)
- Health Slider: null (leave empty - chúng ta dùng custom Image)
- Health Fill Image: Health_Fill (drag từ Hierarchy)
- Health Text: HealthText (optional)
- Background Image: Health_Background (optional)

World Positioning:
- World Offset: (0, 1.2, 0) - Vị trí trên đầu player
- Screen Offset: (0, -30) - Fine-tune position

DOTween Settings (Auto-detected):
- Use DOTween: ✓
- Animation Duration: 0.3s
- Damage Shake: ✓
- Low Health Pulse: ✓
```

### 4.4. Add UI Manager
```
Right-click GameUI:
Add Component → Simple Player UI Manager
- Game UI: GameUI (auto-assigned)
- Experience Bar: ExperienceBar (auto-assigned)  
- Health Bar: PlayerHealthBar (auto-assigned)
```

### 4.5. Final UI Hierarchy
```
GameUI (Canvas)
├── ExperienceBar (Empty GameObject + ExperienceBarUI)
│   ├── XP_Background (Image) - Custom background sprite
│   ├── XP_Fill (Image) - Custom fill sprite, animated via fillAmount
│   ├── XP_Frame (Image) [Optional] - Custom border sprite
│   └── LevelText (TextMeshPro) - "Level 1"
│
├── PlayerHealthBar (Empty GameObject + ScreenSpaceHealthBar)  
│   ├── Health_Background (Image) - Custom background sprite
│   ├── Health_Fill (Image) - Custom fill sprite, animated via fillAmount
│   ├── Health_Frame (Image) [Optional] - Custom border sprite
│   └── HealthText (TextMeshPro) [Optional] - "100/100"
│
└── [SimplePlayerUIManager Component on GameUI]
```

**💡 XP Frame và Health Frame để làm gì?**
- **Visual Enhancement:** Tạo border/khung xung quanh bar để trông professional hơn
- **Depth Effect:** Tạo hiệu ứng 3D, shadow, glow cho bar
- **Branding:** Style riêng cho game (medieval, sci-fi, fantasy, etc.)
- **Polish:** Giúp UI không bị "flat", có texture và detail

**Ví dụ:**
- Background: Màu tối, texture gỗ/kim loại
- Fill: Màu sáng, gradient đẹp  
- Frame: Border có shadow, highlight, pattern decorative

### 4.6. DOTween Setup (Optional - Recommended)

```
1. Asset Store → Search "DOTween" → Import DOTween (HOTween v2)
2. Window → DOTween Utility Panel → Setup DOTween  
3. Apply Settings
4. Restart Unity để áp dụng DOTWEEN_ENABLED define symbol

✅ Benefits với DOTween:
- XP bar smooth fill animation
- Level up punch/scale effect  
- Health bar damage shake
- Low health pulse effect
- Color flash animations

⚡ Fallback mà không có DOTween:
- Instant updates (vẫn hoạt động bình thường)
- Basic lerp animations
- Không có special effects
```
```

---

## 5. ⌨️ Setup Input System

### 5.1. Traditional Input (Recommended cho bắt đầu)
```
Không cần setup gì thêm.
Script PlayerMovement đã support:
- WASD
- Arrow Keys
- Tự động detect input
```

### 5.2. Mobile Virtual Joystick (Optional)
```
Nếu muốn support mobile:

1. Asset Store → Download "Joystick Pack"
2. Hoặc tạo UI Joystick custom
3. Update PlayerMovement.cs để support touch input
```

---

## 6. 🧪 Test & Debug

### 6.1. Test Scene Setup

#### Tạo Test Scene
```
File → New Scene → 2D (URP)
Save as: "GameplayTest"
```

#### Add Camera Setup
```
Main Camera:
- Position: (0, 0, -10)
- Projection: Orthographic
- Size: 5
- Background: Dark color
```

#### Add Ground Visual (Optional)
```
Right-click Hierarchy → 2D Object → Sprite
Đặt tên: "Ground"
Position: (0, -3, 0)
Scale: (10, 1, 1)
Color: Gray
Sorting Layer: Background
```

### 6.2. Setup GameManager

#### Tạo GameManager GameObject
```
1. Right-click trong Hierarchy → Create Empty
2. Đặt tên: "GameManager"
3. Position: (0, 0, 0)
```

#### Add GameManager Script
```
1. Select GameManager GameObject
2. Add Component → Game Manager (script)
3. Cấu hình settings:
   - Start Game On Awake: ✓
   - Pause On Escape: ✓
   - Player: [Drag Player GameObject]
   - UI Manager: [Auto-assigned]
```

### 6.3. Testing Steps

#### Test 1: Player Movement
```
1. Play Scene
2. Test WASD/Arrow keys
3. Player phải di chuyển mượt mà
4. Check Console cho errors
```

#### Test 2: Health System
```
1. Play Scene
2. Press H (damage player)
3. Health bar phải update và follow player
4. Press J (heal player)
```

#### Test 3: XP System
```
1. Play Scene
2. Press K (gain XP)
3. XP bar phải update
4. Khi đầy sẽ level up và reset
```

#### Test 4: Game Flow
```
1. Press ESC → Game pause, player dừng di chuyển
2. Press ESC again → Resume
3. Press H nhiều lần → Player chết → Game Over → Auto restart
4. Press R → Manual restart
```

### 6.3. Common Issues & Solutions

#### Health Bar không hiển thị:
```
Check:
- Camera.main có tồn tại không
- Canvas RenderMode = Screen Space Overlay
- ScreenSpaceHealthBar script có Target assigned
```

#### Player không di chuyển:
```
Check:
- Rigidbody2D có attached không
- PlayerMovement script có attached không
- Input values trong Inspector (debug)
```

#### XP Bar không update:
```
Check:
- ExperienceBarUI script có attached không
- PlayerExperience events có fire không
- Console có errors không
```

#### Scripts missing references:
```
Solution:
- Select GameObject có script
- Click Reset button trong Inspector
- Hoặc manually assign references
```

---

## 🎯 Final Checklist

### ✅ Player Setup Complete:
- [ ] Player GameObject với Rigidbody2D, Collider2D
- [ ] PlayerController script attached và configured
- [ ] Player movement working với WASD
- [ ] PlayerSprite hiển thị đúng

### ✅ UI Setup Complete:
- [ ] GameUI Canvas với proper settings
- [ ] XP Bar full width ở top, có Level text
- [ ] Health Bar follow player, có Health text
- [ ] All UI scripts attached và configured

### ✅ Systems Working:
- [ ] Health system: Damage (H) và Heal (J) working
- [ ] XP system: Gain XP (K) và Level up working
- [ ] UI updates real-time
- [ ] No console errors

### ✅ Ready for Next Steps:
- [ ] Enemy System
- [ ] Weapon System
- [ ] Upgrade System
- [ ] Game Manager

---

## 🚀 Next Steps

Sau khi Player setup hoàn tất, bạn có thể tiếp tục với:

1. **Enemy System**: Spawn enemies, AI, health
2. **Weapon System**: Auto-attack, projectiles, damage
3. **Upgrade System**: Choose upgrades on level up
4. **Game Manager**: Start game, game over, score

Bạn muốn tôi hướng dẫn system nào tiếp theo?
