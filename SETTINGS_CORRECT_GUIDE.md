# HƯỚNG DẪN THIẾT LẬP SETTINGS - PHIÊN BẢN ĐÚNG

## 🎯 CẤU TRÚC SETTINGS

```
Main Menu 
  ↓ Click Settings Button
Settings Menu (2 buttons: Volume và Control)
  ↓ Click Volume                    ↓ Click Control
Volume Panel                     Control Panel
(2 sliders)                      (Joystick settings)
  ↓ Back                            ↓ Back
Settings Menu
  ↓ Back
Main Menu
```

---

## 📐 CẤU TRÚC UI TRONG UNITY

```
MainMenuCanvas
├── MainMenuPage (GameObject)
│   ├── PlayButton (Button)
│   ├── SettingsButton (Button) ← MỚI THÊM
│   └── ExitButton (Button)
│
├── 📄 MainMenu (GameObject có sẵn)
│   └── MainMenu.cs - THÊM 1 reference:
│       - Settings Menu Panel (SettingsMenuPanel)
│
└── SettingsContainer (GameObject)
    │
    ├── SettingsMenuPanel (GameObject) - Ban đầu inactive
    │   └── 📄 SettingsMenuPanel.cs (6 references)
    │   ├── Background (Image)
    │   ├── Title (TextMeshPro: "CÀI ĐẶT")
    │   ├── ButtonsContainer (GameObject)
    │   │   ├── VolumeButton (Button)
    │   │   │   └── Text (TextMeshPro: "Volume")
    │   │   └── ControlButton (Button)
    │   │       └── Text (TextMeshPro: "Control")
    │   └── BackButton (Button)
    │       └── Text (TextMeshPro: "Quay lại")
    │
    ├── VolumePanel (GameObject) - Ban đầu inactive
    │   └── 📄 VolumePanel.cs (1 reference)
    │   └── 📄 AudioSettings.cs (4 references)
    │   ├── Background (Image)
    │   ├── Title (TextMeshPro: "ÂM LƯỢNG")
    │   ├── MusicSliderContainer (GameObject)
    │   │   ├── Label (TextMeshPro: "Nhạc nền")
    │   │   ├── MusicVolumeSlider (Slider)
    │   │   └── ValueLabel (TextMeshPro: "100%")
    │   ├── SFXSliderContainer (GameObject)
    │   │   ├── Label (TextMeshPro: "Âm thanh")
    │   │   ├── SFXVolumeSlider (Slider)
    │   │   └── ValueLabel (TextMeshPro: "100%")
    │   └── BackButton (Button)
    │       └── Text (TextMeshPro: "Quay lại")
    │
    └── ControlPanel (GameObject) - Ban đầu inactive
        └── 📄 ControlPanel.cs (1 reference)
        └── 📄 JoystickSettings.cs (6 references)
        ├── Background (Image)
        ├── Title (TextMeshPro: "CONTROL")
        ├── JoystickTypeContainer (GameObject)
        │   ├── Label (TextMeshPro: "Joystick Type")
        │   ├── ToggleGroup (GameObject)
        │   │   ├── FloatingToggle (Toggle)
        │   │   │   └── Label (TextMeshPro: "Floating")
        │   │   ├── FixedToggle (Toggle)
        │   │   │   └── Label (TextMeshPro: "Fixed")
        │   │   └── FixedOnTouchToggle (Toggle)
        │   │       └── Label (TextMeshPro: "Fixed On Touch")
        ├── CurrentTypeLabel (TextMeshPro: "Current: Floating")
        ├── CustomizeUIButton (Button)
        │   └── Text (TextMeshPro: "Customize UI Position")
        └── BackButton (Button)
            └── Text (TextMeshPro: "Back")
```

**Chú thích:**
- 📄 = Script cần gắn vào GameObject này
- Số trong ngoặc = số references cần gán trong Inspector
- GameObject có nhiều scripts thì gắn tất cả vào cùng 1 GameObject

---

## 🔨 BƯỚC 1: TẠO UI (15 PHÚT)

### 1.1. Tạo Settings Button

**Hierarchy**: `MainMenuPage`
- Right-click → UI → Button - TextMeshPro
- Đổi tên: `SettingsButton`
- Text: "Cài đặt" hoặc "Settings"

---

### 1.2. Tạo Settings Container

**Hierarchy**: `MainMenuCanvas`
- Right-click → Create Empty
- Đổi tên: `SettingsContainer`

---

### 1.3. Tạo Settings Menu Panel

**Trong** `SettingsContainer`:

1. **Tạo SettingsMenuPanel**:
   - Right-click `SettingsContainer` → Create Empty
   - Đổi tên: `SettingsMenuPanel`
   - **Set Active = false** ✓

2. **Tạo Background**:
   - Right-click `SettingsMenuPanel` → UI → Image
   - Color: Black, Alpha 0.8
   - Stretch to fill parent

3. **Tạo Title**:
   - Right-click `SettingsMenuPanel` → UI → Text - TextMeshPro
   - Đổi tên: `Title`
   - Text: "CÀI ĐẶT"
   - Font Size: 60
   - Anchor: Top-Center

4. **Tạo ButtonsContainer**:
   - Right-click `SettingsMenuPanel` → Create Empty
   - Đổi tên: `ButtonsContainer`
   - Add Component → **Vertical Layout Group**:
     - Child Alignment: Middle Center
     - Spacing: 30
     - Child Force Expand: Width = true, Height = false

5. **Tạo 2 Buttons trong ButtonsContainer**:
   
   **Volume Button**:
   - Right-click `ButtonsContainer` → UI → Button
   - Đổi tên: `VolumeButton`
   - Text: "Volume" hoặc "Âm lượng"
   - Preferred Height: 80
   
   **Control Button**:
   - Right-click `ButtonsContainer` → UI → Button
   - Đổi tên: `ControlButton`
   - Text: "Control" hoặc "Điều khiển"
   - Preferred Height: 80

6. **Tạo Back Button**:
   - Right-click `SettingsMenuPanel` → UI → Button
   - Đổi tên: `BackButton`
   - Text: "Quay lại"
   - Anchor: Bottom-Center

---

### 1.4. Tạo Volume Panel

**Trong** `SettingsContainer`:

1. **Tạo VolumePanel**:
   - Right-click `SettingsContainer` → Create Empty
   - Đổi tên: `VolumePanel`
   - **Set Active = false** ✓

2. **Tạo Background, Title** (giống Settings Menu Panel)

3. **Tạo Content Container**:
   - Right-click `VolumePanel` → Create Empty
   - Đổi tên: `ContentContainer`
   - Add Component → **Vertical Layout Group**:
     - Padding: 50
     - Spacing: 40

4. **Tạo Music Volume Slider**:
   
   **MusicSliderContainer** (GameObject):
   - Right-click `ContentContainer` → Create Empty
   - Add Component → **Vertical Layout Group** (Spacing: 10)
   
   Bên trong:
   - `Label` (TextMeshPro): "Nhạc nền"
   - `MusicVolumeSlider` (UI → Slider):
     - Min Value: 0
     - Max Value: 1
     - Whole Numbers: false
     - Value: 0.7
   - `ValueLabel` (TextMeshPro): "70%"

5. **Tạo SFX Volume Slider** (tương tự Music Slider)

6. **Tạo Back Button** (Anchor: Bottom-Center)

---

### 1.5. Tạo Control Panel

**Trong** `SettingsContainer`:

1. **Tạo ControlPanel**:
   - Right-click `SettingsContainer` → Create Empty
   - Đổi tên: `ControlPanel`
   - **Set Active = false** ✓

2. **Tạo Background, Title**
   - Title text: "CONTROL"

3. **Tạo Content Container** (Vertical Layout Group)

4. **Tạo Joystick Type Container**:
   
   **JoystickTypeContainer** (GameObject):
   - Add Component → **Vertical Layout Group** (Spacing: 20)
   
   Bên trong:
   - `Label` (TextMeshPro): "Joystick Type"
   - `ToggleGroup` (Empty GameObject):
     - Add Component → **Toggle Group**
     - **Allow Switch Off: false** ← QUAN TRỌNG: Bắt buộc phải chọn 1 trong 3!
   
   Trong `ToggleGroup`:
   - `FloatingToggle` (UI → Toggle)
     - Group: Gán ToggleGroup component
     - Label: "Floating"
     - **Is On: true** ← Mặc định chọn Floating
   - `FixedToggle` (UI → Toggle)
     - Group: Gán ToggleGroup component
     - Label: "Fixed"
   - `FixedOnTouchToggle` (UI → Toggle)
     - Group: Gán ToggleGroup component
     - Label: "Fixed On Touch"
   
   **⚠️ LƯU Ý**: Toggle Group đảm bảo CHỈ CHỌN 1 trong 3! Chọn cái này → 2 cái kia tự động tắt.

5. **Tạo Current Type Label**:
   - TextMeshPro: "Current: Floating"
   - Hiển thị loại joystick đang chọn

6. **Tạo Customize UI Button**:
   - UI → Button
   - Text: "Customize UI Position"
   - **Chức năng**: Mở panel để điều chỉnh vị trí UI (Joystick, Health Bar, etc.) TRƯỚC KHI VÀO GAME
   - Vị trí đã chỉnh sẽ được lưu và dùng khi chơi game

7. **Tạo Back Button**
   - Text: "Back"

---

## 🎮 BƯỚC 2: GẮN SCRIPTS (10 PHÚT)

### 2.1. MainMenu.cs (CẬP NHẬT)

**GameObject**: Tìm GameObject đã có **MainMenu.cs** script (thường đặt tên là `MainMenu`)

**Gán 1 reference MỚI trong phần Settings**:
```
Settings Menu Panel: [Kéo SettingsMenuPanel GameObject]
```

**LƯU Ý**: Settings Button đã được MainMenu.cs tự động subscribe trong code, không cần gán!

---

### 2.2. SettingsMenuPanel.cs

**GameObject**: `SettingsMenuPanel`

1. Select `SettingsMenuPanel`
2. Add Component → **SettingsMenuPanel**

**Gán references**:
```
Main Menu Page:   [Kéo MainMenuPage GameObject]
Volume Button:    [Kéo VolumeButton]
Control Button:   [Kéo ControlButton]
Back Button:      [Kéo BackButton trong SettingsMenuPanel]
Volume Panel:     [Kéo VolumePanel GameObject]
Control Panel:    [Kéo ControlPanel GameObject]
```

---

### 2.3. VolumePanel.cs

**GameObject**: `VolumePanel`

1. Select `VolumePanel`
2. Add Component → **VolumePanel**

**Gán references**:
```
Back Button: [Kéo BackButton trong VolumePanel]
```

---

### 2.4. AudioSettings.cs

**GameObject**: `VolumePanel` (cùng GameObject với VolumePanel.cs)

1. Select `VolumePanel`
2. Add Component → **AudioSettings**

**Gán references**:
```
Music Volume Slider:  [Kéo MusicVolumeSlider]
SFX Volume Slider:    [Kéo SFXVolumeSlider]
Music Volume Label:   [Kéo ValueLabel của MusicSlider]
SFX Volume Label:     [Kéo ValueLabel của SFXSlider]
```

---

### 2.5. ControlPanel.cs

**GameObject**: `ControlPanel`

1. Select `ControlPanel`
2. Add Component → **ControlPanel**

**Gán references**:
```
Back Button: [Kéo BackButton trong ControlPanel]
```

---

### 2.6. JoystickSettings.cs

**GameObject**: `ControlPanel` (cùng GameObject với ControlPanel.cs)

1. Select `ControlPanel`
2. Add Component → **JoystickSettings**

**Gán references**:
```
Floating Toggle:         [Kéo FloatingToggle]
Fixed Toggle:            [Kéo FixedToggle]
Fixed On Touch Toggle:   [Kéo FixedOnTouchToggle]
Toggle Group:            [Kéo ToggleGroup GameObject]
Customize UI Button:     [Kéo CustomizeUIButton]
UI Customization Panel:  [NULL - tạo sau, xem phần "BỔ SUNG" bên dưới]
Current Type Label:      [Kéo CurrentTypeLabel]
```

**⚠️ LƯU Ý**: UI Customization Panel có thể để NULL trước, tạo sau khi Settings cơ bản đã hoạt động!

---

## 📊 BẢNG TÓM TẮT SCRIPTS

| GameObject | Scripts gắn | Số references |
|------------|-------------|---------------|
| `MainMenu` (có sẵn) | MainMenu.cs (update) | +1 (settingsMenuPanel) |
| `SettingsMenuPanel` | SettingsMenuPanel.cs | 6 |
| `VolumePanel` | VolumePanel.cs<br>AudioSettings.cs | 1<br>4 |
| `ControlPanel` | ControlPanel.cs<br>JoystickSettings.cs | 1<br>6 |

---

## ✅ CHECKLIST

- [ ] `SettingsButton` tạo trong MainMenuPage
- [ ] `SettingsContainer` tạo trong MainMenuCanvas
- [ ] `SettingsMenuPanel` inactive, có 2 buttons (Volume, Control)
- [ ] `VolumePanel` inactive, có 2 sliders
- [ ] `ControlPanel` inactive, có 3 toggles
- [ ] Tất cả panels có Background semi-transparent
- [ ] Tất cả panels có Back button
- [ ] MainMenu.cs có +1 reference mới (Settings Menu Panel)
- [ ] SettingsMenuPanel có 6 references (mainMenuPage + 5 references cũ)
- [ ] VolumePanel có 2 scripts (VolumePanel + AudioSettings)
- [ ] ControlPanel có 2 scripts (ControlPanel + JoystickSettings)

---

## 🎮 FLOW HOẠT ĐỘNG

```
1. Click Settings Button
   → Ẩn Main Menu
   → Hiện Settings Menu Panel (2 buttons)

2. Click Volume Button
   → Ẩn Settings Menu Panel
   → Hiện Volume Panel (2 sliders)
   → User điều chỉnh volume
   → Click Back → Quay lại Settings Menu Panel

3. Click Control Button
   → Ẩn Settings Menu Panel
   → Hiện Control Panel
   
   TRONG CONTROL PANEL:
   a) Chọn Joystick Type (CHỈ 1 TRONG 3):
      - Click "Floating" → 2 cái kia TỰ ĐỘNG TẮT
      - Click "Fixed" → 2 cái kia TỰ ĐỘNG TẮT
      - Click "Fixed On Touch" → 2 cái kia TỰ ĐỘNG TẮT
      → Current Type Label cập nhật theo
   
   b) Click "Customize UI Position":
      → Mở UI Customization Panel
      → Kéo thả các UI elements (Joystick, Health Bar, etc.)
      → Save → Lưu vị trí vào PlayerPrefs
      → Khi vào game sẽ dùng vị trí đã lưu
   
   → Click Back → Quay lại Settings Menu Panel

4. Click Back trong Settings Menu Panel
   → Ẩn Settings Menu Panel
   → Hiện lại Main Menu
```

---

## 🧪 TEST

1. **Play game**
2. **Click Settings** → Thấy 2 buttons: Volume và Control
3. **Click Volume** → Thấy 2 sliders, kéo thử → Âm lượng thay đổi
4. **Click Back** → Quay lại Settings Menu
5. **Click Control** → Thấy 3 toggles, chọn thử
   - **Test Toggle Group**: Chọn "Fixed" → "Floating" và "Fixed On Touch" phải TẮT
   - Chỉ được chọn 1 toggle tại 1 thời điểm
5. **Click "Customize UI Position"** (nếu đã có UI Customization Panel)
   - Thử kéo thả các UI elements
6. **Click Back** → Quay lại Settings Menu
7. **Click Back** → Quay lại Main Menu
**⚠️ LƯU Ý QUAN TRỌNG**:
- **Toggle Group**: Chỉ 1 toggle được chọn tại 1 thời điểm
- **UI Customization**: Vị trí UI được lưu VĨN VIỄN (PlayerPrefs), dùng cho tất cả các lần chơi sau
- **Customize TRƯỚC khi vào game**: Điều chỉnh UI ở Main Menu → Lưu → Vào game sẽ thấy UI ở vị trí đã chỉnh
---

## 🐛 TROUBLESHOOTING

### Settings Menu không hiện?
- Kiểm tra SettingsMenuPanel đã inactive ban đầu
- Verify MainMenu.cs có reference đến SettingsMenuPanel
- Verify SettingsMenuPanel có reference đến MainMenuPage
- Check Console có lỗi

### Volume Panel không hiện khi click Volume?
- Verify SettingsMenuPanel có reference đến VolumePanel
- Check VolumePanel đã inactive ban đầu

### Back button không hoạt động?
- Kiểm tra VolumePanel.cs và ControlPanel.cs đã gắn đúng
- Verify Back button references đã gán

### Sliders không thay đổi volume?
- Check AudioSettings.cs đã gắn vào VolumePanel
- Verify 4 references (2 sliders + 2 labels) đã gán

---

## 📁 FILES MỚI TẠO

**Scripts (ĐÃ CÓ)**:
- `Assets/Scripts/UI/SettingsMenuPanel.cs` ✓
- `Assets/Scripts/UI/VolumePanel.cs` ✓
- `Assets/Scripts/UI/ControlPanel.cs` ✓
- `Assets/Scripts/Main Menu/MainMenu.cs` ✓ (updated - gộp MainMenuSettings)

**Scripts (CÓ SẴN - không cần tạo mới)**:
- `Assets/Scripts/UI/UILayoutCustomizer.cs` ✓ (đã có trong project)
- `Assets/Scripts/UI/DraggableUI.cs` ✓ (đã có trong project)

**UI Panel (TÙY CHỌN - tạo sau)**:
- UI Customization Panel - Xem hướng dẫn bên dưới ⬇️

---

## 🎨 BỔ SUNG: TẠO UI CUSTOMIZATION PANEL (TÙY CHỌN)

**LƯU Ý**: Phần này **KHÔNG BẮT BUỘC** ngay bây giờ. Bạn có thể làm sau khi Settings cơ bản đã hoạt động.

### Cách hoạt động:
1. Click "Customize UI Position" trong Control Panel
2. Hiện UI Customization Panel với:
   - Preview canvas hiển thị các UI elements (Joystick, Health Bar, etc.)
   - Các UI có thể kéo thả (đã gắn DraggableUI.cs)
   - 3 buttons: Save, Cancel, Reset

### Hướng dẫn tạo (TÓM TẮT):

```
MainMenuCanvas/SettingsContainer
└── UICustomizationPanel (GameObject) - Ban đầu inactive
    └── 📄 UILayoutCustomizer.cs
    ├── Background (Image - semi-transparent)
    ├── Title (TextMeshPro: "Customize UI Layout")
    ├── PreviewCanvas (Canvas - hiển thị UI preview)
    │   ├── Joystick Preview (với DraggableUI.cs)
    │   ├── HealthBar Preview (với DraggableUI.cs)
    │   └── ... (các UI elements khác)
    ├── InstructionsPanel (TextMeshPro: "Drag to move")
    └── ButtonsContainer (Horizontal Layout)
        ├── SaveButton (Button: "Save")
        ├── CancelButton (Button: "Cancel")
        └── ResetButton (Button: "Reset")
```

**Gán references trong UILayoutCustomizer**:
- Draggable Elements: Auto-detect (không cần gán thủ công)
- Save Button: [Kéo SaveButton]
- Cancel Button: [Kéo CancelButton]
- Reset Button: [Kéo ResetButton]
- Preview Canvas: [Kéo PreviewCanvas]
- Instructions Panel: [Kéo InstructionsPanel]

**Trong JoystickSettings**:
```
UI Customization Panel: [Kéo UICustomizationPanel GameObject]
```

**Flow**:
1. Click "Customize UI Position" → Hiện UICustomizationPanel
2. Kéo thả UI elements trên PreviewCanvas
3. Click "Save" → Lưu vị trí → Đóng panel
4. Click "Cancel" → Hủy thay đổi → Đóng panel
5. Click "Reset" → Về vị trí mặc định

---

## ⚡ NHANH CHÓNG: HOÀN THIỆN SETTINGS TRƯỚC

**Ưu tiên làm trước**:
1. ✅ Settings Menu Panel (Volume + Control buttons)
2. ✅ Volume Panel (2 sliders)
3. ✅ Control Panel (3 toggle + Customize button)
4. ✅ Test Settings flow hoạt động

**Làm sau khi Settings đã OK**:
5. ⏳ UI Customization Panel (kéo thả UI)

**Lý do**: UI Customization là tính năng phụ, Settings cơ bản quan trọng hơn!

---

**KHÔNG CẦN MainMenuSettings.cs** - Logic đã chuyển vào SettingsMenuPanel.cs!
**SettingsPanel.cs KHÔNG dùng** - Đó là tab system!

Chúc thành công! 🚀
