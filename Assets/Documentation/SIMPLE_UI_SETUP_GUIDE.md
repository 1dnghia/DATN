# 🎮 Hướng dẫn Setup UI với 1 Canvas duy nhất

## � **Lưu ý quan trọng**
**Nếu bạn có custom assets cho XP bar và Health bar**, hãy sử dụng:
👉 **[UI_SETUP_WITH_CUSTOM_ASSETS.md](./UI_SETUP_WITH_CUSTOM_ASSETS.md)** 

File này hướng dẫn dùng Unity default UI sprites.

---

## �📋 Tổng quan
- **1 Canvas chính** (Screen Space - Overlay)
- **Health Bar** follow player trong Screen Space 
- **XP Bar** full width ở top
- **Các UI khác** (menu, pause, etc.) cùng Canvas

---

## 🛠️ Bước 1: Tạo Canvas chính

### 1.1. Tạo Canvas
```
Right-click trong Hierarchy → UI → Canvas
Đặt tên: "GameUI"
```

### 1.2. Cài đặt Canvas
```
Canvas component:
- Render Mode: Screen Space - Overlay
- Pixel Perfect: ✓ (tùy chọn)
- Sort Order: 0

Canvas Scaler component:
- UI Scale Mode: Scale With Screen Size
- Reference Resolution: 1920 x 1080
- Screen Match Mode: Match Width Or Height
- Match: 0.5
```

---

## 🛠️ Bước 2: Tạo XP Bar (Full Width Top)

### 2.1. Tạo XP Bar GameObject
```
Right-click GameUI → UI → Slider
Đặt tên: "ExperienceBar"
```

### 2.2. Cài đặt Position & Size
```
RectTransform:
- Anchor: Top Stretch (left: 0, top: 1, right: 1, bottom: 1)
- Position X: 0
- Position Y: -25
- Width: 0 (auto-stretch)
- Height: 50
```

### 2.3. Cài đặt Slider
```
Slider component:
- Min Value: 0
- Max Value: 1
- Value: 0.5 (test)
- Whole Numbers: ✗
```

### 2.4. Style XP Bar
```
Background:
- Image color: Dark (0.1, 0.1, 0.1, 0.8)

Fill:
- Image color: Gold/Yellow (1, 0.8, 0, 1)
```

### 2.5. Thêm Level Text
```
Right-click ExperienceBar → UI → Text - TextMeshPro
Đặt tên: "LevelText"

RectTransform:
- Anchor: Middle Center
- Position: (0, 0, 0)
- Size: (100, 40)

TextMeshPro:
- Text: "Level 1"
- Font Size: 24
- Color: White
- Alignment: Center
```

### 2.6. Attach Script
```
1. Add component "ExperienceBarUI" vào ExperienceBar
2. Assign references:
   - Experience Slider: ExperienceBar (Slider)
   - Level Text: LevelText (TextMeshPro)
```

---

## 🛠️ Bước 3: Tạo Health Bar (Follow Player)

### 3.1. Tạo Health Bar GameObject
```
Right-click GameUI → UI → Slider
Đặt tên: "PlayerHealthBar"
```

### 3.2. Cài đặt Size (không cần position vì script sẽ điều khiển)
```
RectTransform:
- Width: 100
- Height: 20
```

### 3.3. Cài đặt Slider
```
Slider component:
- Min Value: 0
- Max Value: 1
- Value: 1 (full health)
```

### 3.4. Style Health Bar
```
Background:
- Image color: Dark Red (0.3, 0.1, 0.1, 0.8)

Fill:
- Image color: Green (0, 1, 0, 1)
```

### 3.5. Thêm Health Text (Optional)
```
Right-click PlayerHealthBar → UI → Text - TextMeshPro
Đặt tên: "HealthText"

RectTransform:
- Anchor: Middle Center
- Position: (0, 0, 0)
- Size: (90, 18)

TextMeshPro:
- Text: "100/100"
- Font Size: 12
- Color: White
- Alignment: Center
```

### 3.6. Attach Script
```
1. Add component "ScreenSpaceHealthBar" vào PlayerHealthBar
2. Script sẽ tự động tìm Player và assign references
3. Hoặc manual assign:
   - Target: Player GameObject
   - Health Slider: PlayerHealthBar (Slider)
   - Health Fill Image: Fill (Image)
   - Health Text: HealthText (TextMeshPro)
```

---

## 🛠️ Bước 4: Cấu trúc Canvas hoàn chỉnh

```
GameUI (Canvas)
├── ExperienceBar (Slider) [Script: ExperienceBarUI]
│   ├── Background (Image)
│   ├── Fill Area
│   │   └── Fill (Image)
│   ├── Handle Slide Area
│   │   └── Handle (Image) - có thể xóa
│   └── LevelText (TextMeshPro)
│
├── PlayerHealthBar (Slider) [Script: ScreenSpaceHealthBar]
│   ├── Background (Image)
│   ├── Fill Area
│   │   └── Fill (Image)
│   ├── Handle Slide Area
│   │   └── Handle (Image) - có thể xóa
│   └── HealthText (TextMeshPro)
│
└── [Các UI khác sẽ thêm vào đây...]
```

---

## ✅ Kết quả mong đợi

1. **XP Bar**: 
   - Hiển thị full width ở top màn hình
   - Hiển thị level hiện tại
   - Auto update khi gain XP/level up

2. **Health Bar**:
   - Follow player trên màn hình
   - Hiển thị ở vị trí offset phía trên đầu player
   - Auto update khi player bị damage/heal
   - Đổi màu theo % máu (xanh → vàng → đỏ)

3. **Performance**:
   - Chỉ 1 Canvas → ít draw calls
   - Smooth animation
   - Tự động ẩn health bar khi player ở ngoài camera

---

## 🎯 Lợi ích của approach này

- ✅ **Đơn giản**: Chỉ 1 Canvas duy nhất
- ✅ **Performance**: Ít draw calls, batching tốt hơn
- ✅ **Dễ quản lý**: Tất cả UI ở 1 nơi
- ✅ **Flexible**: Dễ thêm UI mới
- ✅ **Responsive**: Scale tốt trên các resolution

---

## 🐛 Troubleshooting

### Health Bar không follow player:
```
1. Check Target assignment trong ScreenSpaceHealthBar
2. Check Camera.main có tồn tại không
3. Check Canvas settings (Screen Space - Overlay)
```

### XP Bar không full width:
```
1. Check Anchor setting: Top Stretch
2. Check Left/Right values = 0
3. Check Canvas Scaler settings
```

### UI bị blur/pixelated:
```
1. Canvas → Pixel Perfect ✓
2. Canvas Scaler → Reference Resolution phù hợp
3. Image Import Settings → Filter Mode: Point (no filter)
```
