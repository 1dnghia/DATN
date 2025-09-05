# 🎨 Hướng dẫn Setup UI với Custom Assets

## 📋 Tổng quan UI cần tạo:
1. **XP Bar** - Full width ở top màn hình (dùng custom sprite)
2. **Health Bar** - Follow player trên đầu (dùng custom sprite)
3. **Level Text** - Hiển thị level hiện tại

---

## 🛠️ **Bước 1: Import và Setup Assets**

### 1.1. Import Assets
```
1. Copy asset files vào Assets/Art/UI/
2. Select tất cả sprite files
3. Inspector → Texture Type: Sprite (2D and UI)
4. Sprite Mode: Single
5. Pixels Per Unit: 100 (hoặc theo asset của bạn)
6. Filter Mode: Bilinear
7. Apply
```

### 1.2. Tạo Canvas Chính
```
1. Right-click trong Hierarchy → UI → Canvas
2. Đặt tên: "GameUI"

Canvas Component:
- Render Mode: Screen Space - Overlay
- Pixel Perfect: ✓
- Sort Order: 0

Canvas Scaler Component:
- UI Scale Mode: Scale With Screen Size
- Reference Resolution: 1920 x 1080
- Screen Match Mode: Match Width Or Height
- Match: 0.5
```

---

## 🔝 **Bước 2: Tạo XP Bar với Custom Assets & DOTween**

### 2.1. Tạo XP Bar Container
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

### 2.2. Thêm XP Bar Background
```
Right-click ExperienceBar → UI → Image
Đặt tên: "XP_Background"

Image Component:
- Source Image: [Drag XP bar background asset vào đây]
- Color: White (255, 255, 255, 255)
- Material: None
- Raycast Target: ✗
- Image Type: Simple (hoặc Sliced nếu 9-slice)

Rect Transform:
- Anchor: Stretch Stretch
- Left: 0, Right: 0, Top: 0, Bottom: 0
```

### 2.3. Thêm XP Bar Fill (Dùng Image.fillAmount)
```
Right-click ExperienceBar → UI → Image
Đặt tên: "XP_Fill"

Image Component:
- Source Image: [Drag XP bar fill asset vào đây]
- Color: White (hoặc màu bạn muốn tint)
- Image Type: Filled
- Fill Method: Horizontal
- Fill Origin: Left
- Fill Amount: 0.5 (test value, sẽ animate bằng DOTween)

Rect Transform:
- Anchor: Stretch Stretch
- Left: 0, Right: 0, Top: 0, Bottom: 0
```

### 2.4. Thêm XP Bar Frame/Border (nếu có)
```
Right-click ExperienceBar → UI → Image
Đặt tên: "XP_Frame"

Image Component:
- Source Image: [Drag XP bar frame/border asset]
- Color: White
- Raycast Target: ✗
- Image Type: Simple

Rect Transform:
- Anchor: Stretch Stretch
- Left: 0, Right: 0, Top: 0, Bottom: 0

Trong Hierarchy, drag XP_Frame xuống cuối để render trên top
```

### 2.5. Thêm Level Text
```
Right-click ExperienceBar → UI → Text - TextMeshPro
Đặt tên: "LevelText"

Rect Transform:
- Anchor: Middle Left
- Position: (10, 0, 0)
- Width: 100, Height: 40

TextMeshPro:
- Text: "Level 1"
- Font Size: 18
- Color: White (hoặc màu contrast với background)
- Alignment: Left + Middle
- Font Style: Bold
```

### 2.6. **Add Script - ExperienceBarUI (Updated)**
```
Select ExperienceBar → Add Component → Experience Bar UI

⚡ Auto-Assignment: Script sẽ tự động tìm và gán:
- XP_Fill (GameObject có tên chứa "Fill") 
- LevelText (GameObject có tên chứa "Level")
- XP_Background (GameObject có tên chứa "Background")

Manual Assignment (nếu cần):
- XP Fill Image: XP_Fill (drag từ Hierarchy)
- Level Text: LevelText (drag từ Hierarchy)
- Background Image: XP_Background (optional)

Display Options:
- Show Level Text: ✓

DOTween Settings:
- Use DOTween: ✓ (nếu có DOTween imported)
- Dotween Duration: 0.5
- Dotween Ease Type: 6 (OutQuart)
- Enable Level Up Punch: ✓
- Level Up Punch Scale: 1.2
```

---

## 💚 **Bước 3: Tạo Health Bar với Custom Assets & DOTween**

### 3.1. Tạo Health Bar Container
```
Right-click GameUI → Create Empty
Đặt tên: "PlayerHealthBar"

Rect Transform:
- Anchor: Middle Center
- Width: 80 (hoặc theo width của asset)
- Height: 12 (hoặc theo height của asset)
- Position sẽ được script điều khiển
```

### 3.2. Thêm Health Bar Background
```
Right-click PlayerHealthBar → UI → Image
Đặt tên: "Health_Background"

Image Component:
- Source Image: [Drag health bar background asset]
- Color: White
- Raycast Target: ✗
- Image Type: Simple (hoặc Sliced)

Rect Transform:
- Anchor: Stretch Stretch
- Fill parent (Left: 0, Right: 0, Top: 0, Bottom: 0)
```

### 3.3. Thêm Health Bar Fill (Dùng Image.fillAmount)
```
Right-click PlayerHealthBar → UI → Image
Đặt tên: "Health_Fill"

Image Component:
- Source Image: [Drag health bar fill asset]
- Color: Green (0, 255, 0, 255)
- Image Type: Filled
- Fill Method: Horizontal  
- Fill Origin: Left
- Fill Amount: 1.0 (full health, sẽ animate bằng DOTween)

Rect Transform:
- Anchor: Stretch Stretch
- Fill parent
```

### 3.4. Thêm Health Bar Frame (nếu có)
```
Right-click PlayerHealthBar → UI → Image
Đặt tên: "Health_Frame"

Image Component:
- Source Image: [Drag health bar frame asset]
- Color: White
- Raycast Target: ✗
- Image Type: Simple

Rect Transform:
- Anchor: Stretch Stretch
- Fill parent

Move xuống cuối Hierarchy để render trên top
```

### 3.5. **Add Script - ScreenSpaceHealthBar (Updated)**
```
Select PlayerHealthBar → Add Component → Screen Space Health Bar

⚡ Auto-Assignment: Script sẽ tự động tìm và gán:
- Health_Fill (GameObject có tên chứa "Fill")
- Health_Background (GameObject có tên chứa "Background")

Script settings:
- Target: Player GameObject (drag từ Hierarchy)
- Health Fill Image: Health_Fill (drag từ Hierarchy)  
- Background Image: Health_Background (optional)
- World Offset: (0, 1.2, 0)

DOTween Settings:
- Use DOTween: ✓
- Health Animation Duration: 0.3
- Health Animation Ease: 6 (OutQuart)
- Enable Damage Shake: ✓
- Damage Shake Strength: 5
- Enable Low Health Pulse: ✓
- Low Health Pulse Speed: 2
```

---

## 🎮 **Bước 4: Hierarchy Structure với DOTween**

```
GameUI (Canvas)
├── ExperienceBar (Empty GameObject + ExperienceBarUI)
│   ├── XP_Background (Image)
│   ├── XP_Fill (Image) - fillAmount animated by DOTween
│   ├── XP_Frame (Image) [Optional]
│   └── LevelText (TextMeshPro) - "Level 1"
│
├── PlayerHealthBar (Empty GameObject + ScreenSpaceHealthBar)
│   ├── Health_Background (Image)
│   ├── Health_Fill (Image) - fillAmount animated by DOTween
│   └── Health_Frame (Image) [Optional]
│
└── [Other UI elements...]
```

**Lưu ý:** Scripts gốc `ExperienceBarUI` và `ScreenSpaceHealthBar` đã được cập nhật để support DOTween. Không cần tạo scripts mới!

---

## 🎨 **Bước 5: DOTween Setup & Import**

### 5.1. Import DOTween:
```
1. Asset Store → Search "DOTween" → Import DOTween (HOTween v2)
2. Window → DOTween Utility Panel → Setup DOTween
3. Apply Settings
```

### 5.2. Script Assignment:
```
ExperienceBar:
- Add Component: Experience Bar UI
- XP Fill Image: XP_Fill (drag from Hierarchy)
- Level Text: LevelText (drag from Hierarchy)
- Background Image: XP_Background (optional)

Display Options:
- Show Level Text: ✓ (để hiển thị level như "Level 2")

PlayerHealthBar:
- Add Component: Screen Space Health Bar
- Target: Player GameObject (drag from Hierarchy)
- Health Fill Image: Health_Fill (drag from Hierarchy)  
- Background Image: Health_Background (optional)

```

---

## ⚡ **Bước 6: DOTween Animation Settings**

### 6.1. XP Bar Animation Settings:
```
ExperienceBarUI (Updated with DOTween support):
- Fill Animation Duration: 0.5s
- Fill Animation Ease: OutQuart
- Level Up Flash Duration: 0.3s
- Level Up Flash Color: Yellow
- Level Up Scale Punch: 1.2
```

### 6.2. Health Bar Animation Settings:
```
ScreenSpaceHealthBar (Updated with DOTween support):
- Health Animation Duration: 0.3s
- Health Animation Ease: OutQuart
- Damage Shake Duration: 0.2s
- Damage Shake Strength: 5
- Enable Low Health Pulse: ✓
- Low Health Pulse Speed: 2
```

---

## ✅ **Bước 7: Testing với DOTween**

### 7.1. Expected DOTween Effects:
```
XP Bar:
✅ Smooth fillAmount animation (không jerky)
✅ Color gradient theo progress
✅ Level up flash effect + scale punch
✅ Frame glow effect khi level up

Health Bar:
✅ Smooth fillAmount animation
✅ Color transition: Green → Yellow → Red
✅ Shake effect khi bị damage
✅ Low health pulse effect (<25% HP)
✅ Follow player smoothly
```

### 7.2. Test Commands:
```
1. Press K → XP bar smooth fill với DOTween
2. Press H → Health bar smooth decrease với shake
3. Press J → Health bar smooth increase
4. Health < 25% → Pulse effect
5. XP full → Level up flash + scale punch
```

---

## 🎨 **Bước 8: Custom Asset Advantages**

### 8.1. Asset-Driven Animation:
```
✅ fillAmount controls your custom sprite fill
✅ DOTween provides smooth easing curves
✅ Color tinting cho different states
✅ Layered approach: Background + Fill + Frame
✅ Custom effects: shake, pulse, flash, scale
```

### 8.2. Performance Benefits:
```
✅ No Slider overhead
✅ Direct Image.fillAmount manipulation
✅ DOTween optimized tweening
✅ GPU-friendly fill effects
✅ Batching-friendly setup
```

### 8.3. Art Integration:
```
✅ Your custom sprites used directly
✅ 9-slice support cho stretchable bars
✅ Pixel-perfect rendering
✅ Color variations through tinting
✅ Frame/border effects
```

---

## 🐛 **Troubleshooting DOTween Issues**

### DOTween not found:
```
Fix: Import DOTween từ Asset Store
Window → DOTween Utility Panel → Setup DOTween
```

### fillAmount không animate:
```
Fix: 
- Check Image Type = Filled
- Fill Method = Horizontal
- Script reference đúng Image component
```

### Animation jerky/laggy:
```
Fix:
- Reduce animation duration
- Change Ease type (OutQuart recommended)
- Check SetUpdate(true) cho pause-safe animation
```

### Colors không smooth transition:
```
Fix:
- Use DOColor() thay vì set color directly
- Check color transition duration
- Ensure gradient setup đúng
```

Với setup này, UI sẽ có animation mượt mà với custom assets của bạn! 🎨✨
