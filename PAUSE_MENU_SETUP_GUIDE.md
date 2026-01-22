# HƯỚNG DẪN THIẾT LẬP PAUSE MENU VÀ UI CUSTOMIZATION

## Tổng quan
Pause Menu trong gameplay với các tính năng:
- **Continue**: Tiếp tục game
- **Settings**: Mở Settings Panel (dùng chung với Main Menu)
- **Return to Main Menu**: Trở về Main Menu
- **UI Customization**: Cho phép drag & drop UI elements trong game

## 1. CẤU TRÚC PAUSE MENU

### Hierarchy cần tạo:
```
GameplayCanvas
├── PauseButton (Button - Image)
│   └── Icon (Image)
│
├── 📄 PauseMenuManager (GameObject)
│   └── PauseMenu.cs - Quản lý Pause Menu
│
├── PauseMenuPanel (GameObject) - Ban đầu inactive
│   ├── Background (Image - semi-transparent)
│   ├── Title (TextMeshPro - Text: "PAUSED")
│   ├── ButtonsContainer (GameObject)
│   │   ├── ContinueButton (Button)
│   │   │   └── Text (TextMeshPro: "Tiếp tục")
│   │   ├── SettingsButton (Button)
│   │   │   └── Text (TextMeshPro: "Cài đặt")
│   │   └── MainMenuButton (Button)
│   │       └── Text (TextMeshPro: "Main Menu")
│   └── ... (các decorations khác)
│
├── ConfirmationDialog (GameObject) - Ban đầu inactive
│   ├── Background (Image - semi-transparent, darker)
│   ├── DialogPanel (GameObject)
│   │   ├── PanelBackground (Image)
│   │   ├── Title (TextMeshPro: "XÁC NHẬN")
│   │   ├── Message (TextMeshPro: "Bạn có chắc muốn thoát về Main Menu?")
│   │   └── ButtonsContainer (GameObject)
│   │       ├── YesButton (Button)
│   │       │   └── Text (TextMeshPro: "Có")
│   │       └── NoButton (Button)
│   │           └── Text (TextMeshPro: "Không")
│
└── SettingsContainer (GameObject) - COPY TỪ MAIN MENU
    ├── SettingsMenuPanel (GameObject) - Ban đầu inactive
    │   └── 📄 SettingsMenuPanel.cs
    ├── VolumePanel (GameObject) - Ban đầu inactive
    │   └── 📄 VolumePanel.cs + AudioSettings.cs
    └── ControlPanel (GameObject) - Ban đầu inactive
        └── 📄 ControlPanel.cs + JoystickSettings.cs
```

**Chú thích:**
- 📄 = Script cần gắn vào GameObject
- SettingsContainer được COPY NGUYÊN từ Main Menu Scene

### Chi tiết setup:

#### BƯỚC 1: Tạo Pause Button
1. Right-click `GameplayCanvas` → UI → Button
2. Đổi tên thành `PauseButton`
3. Đặt vị trí: Top-Right hoặc Top-Left
4. Thêm Icon pause (||)
5. Component Button:
   - **KHÔNG** gán OnClick trong Inspector
   - Sẽ được gán qua code
   - Button sẽ tự động ẨN khi mở Pause Menu

#### BƯỚC 2: Tạo Pause Menu Panel
1. Right-click `GameplayCanvas` → Create Empty
2. Đổi tên: `PauseMenuPanel`
3. Thêm **Canvas Group** component
4. **Set Active = false** trong Inspector
5. Thêm Background:
   - Right-click → UI → Image
   - Color: Black với Alpha 0.8 (semi-transparent)
   - Stretch to fill parent

#### BƯỚC 3: Tạo Title
1. Right-click `PauseMenuPanel` → UI → Text - TextMeshPro
2. Text: "PAUSED" hoặc "TẠM DỪNG"
3. Font Size: 60-80
4. Alignment: Center
5. Anchor: Top-Center

#### BƯỚC 4: Tạo Buttons Container
1. Right-click `PauseMenuPanel` → Create Empty
2. Đổi tên: `ButtonsContainer`
3. Thêm **Vertical Layout Group**:
   - Child Alignment: Middle Center
   - Spacing: 20
   - Child Force Expand: Width = true, Height = false
   - Padding: 50 (tất cả các cạnh)

#### BƯỚC 5: Tạo 3 Buttons
Trong `ButtonsContainer`, tạo 3 buttons:

**Continue Button**:
- Text: "Tiếp tục" hoặc "Continue"
- Preferred Height: 60
- Font Size: 32

**Settings Button**:
- Text: "Cài đặt" hoặc "Settings"  
- Preferred Height: 60
- Font Size: 32

**Main Menu Button**:
- Text: "Main Menu" hoặc "Thoát"
- Preferred Height: 60
- Font Size: 32

#### BƯỚC 6: Tạo Confirmation Dialog

1. **Tạo ConfirmationDialog**:
   - Right-click `GameplayCanvas` → Create Empty
   - Đổi tên: `ConfirmationDialog`
   - **Set Active = false** ✓

2. **Tạo Background**:
   - Right-click `ConfirmationDialog` → UI → Image
   - Color: Black, Alpha 0.9 (darker than pause menu)
   - Stretch to fill parent (full screen)

3. **Tạo Dialog Panel**:
   - Right-click `ConfirmationDialog` → Create Empty
   - Đổi tên: `DialogPanel`
   - Add **Vertical Layout Group**:
     - Child Alignment: Middle Center
     - Spacing: 30
     - Padding: 40

4. **Tạo Panel Background**:
   - Right-click `DialogPanel` → UI → Image
   - Color: Dark gray/blue
   - Anchor: Center
   - Width: 500, Height: 300

5. **Tạo Title**:
   - Right-click `DialogPanel` → UI → Text - TextMeshPro
   - Text: "XÁC NHẬN" hoặc "CONFIRMATION"
   - Font Size: 48
   - Alignment: Center
   - Color: Warning (Yellow/Orange)

6. **Tạo Message**:
   - Right-click `DialogPanel` → UI → Text - TextMeshPro
   - Text: "Bạn có chắc muốn thoát về Main Menu?\nTiến trình chơi sẽ bị mất!"
   - Or: "Are you sure you want to return to Main Menu?\nYour progress will be lost!"
   - Font Size: 28
   - Alignment: Center
   - Word Wrapping: Enabled

7. **Tạo Buttons Container**:
   - Right-click `DialogPanel` → Create Empty
   - Add **Horizontal Layout Group**:
     - Spacing: 40
     - Child Force Expand: Width = true, Height = false

8. **Tạo 2 Buttons**:
   
   **Yes Button** (Confirm):
   - Text: "Có" / "Yes"
   - Preferred Height: 60
   - Width: 150
   - Color: Red (warning color)
   
   **No Button** (Cancel):
   - Text: "Không" / "No"
   - Preferred Height: 60
   - Width: 150
   - Color: Green/Blue (safe color)

#### BƯỚC 7: Copy Settings Container từ Main Menu

**⚠️ VẤN ĐỀ CROSS-SCENE**: Main Menu và Gameplay là 2 scene khác nhau → KHÔNG THỂ reference GameObject qua scene!

**GIẢI PHÁP: Copy toàn bộ SettingsContainer**

**Cách 1: Copy trực tiếp (Khuyến nghị)**

1. **Mở Main Menu Scene**:
   - File → Open Scene → Scenes/MainMenu

2. **Tìm SettingsContainer**:
   - Trong Hierarchy → MainMenuCanvas → SettingsContainer

3. **Copy SettingsContainer**:
   - Right-click `SettingsContainer` → Copy
   - Hoặc Ctrl+C

4. **Mở Gameplay Scene**:
   - File → Open Scene → Scenes/Gameplay (hoặc scene nào bạn đang làm)

5. **Paste vào GameplayCanvas**:
   - Click vào `GameplayCanvas`
   - Right-click → Paste
   - Hoặc Ctrl+V

6. **Kiểm tra**:
   - ✅ SettingsMenuPanel có script SettingsMenuPanel.cs
   - ✅ VolumePanel có VolumePanel.cs + AudioSettings.cs
   - ✅ ControlPanel có ControlPanel.cs + JoystickSettings.cs
   - ✅ Tất cả references trong Inspector đã được gán
   - ✅ Set Active = false cho tất cả panels

**Cách 2: Tạo Prefab (Nếu cần dùng lại nhiều lần)**

1. **Trong Main Menu Scene**:
   - Kéo `SettingsContainer` từ Hierarchy vào folder `Assets/Prefabs/UI/`
   - Unity tự tạo Prefab

2. **Trong Gameplay Scene**:
   - Kéo Prefab `SettingsContainer` từ Project vào `GameplayCanvas`
   - Unity tự instantiate với tất cả references

**⚠️ LƯU Ý QUAN TRỌNG:**
- Settings ở Main Menu và Gameplay **HOÀN TOÀN GIỐNG NHAU**
- Khi sửa 1 bên → phải sửa bên kia (hoặc dùng Prefab để sync)
- MainMenuPage reference trong SettingsMenuPanel **CHỈ dùng ở Main Menu**, trong Gameplay để NULL

#### BƯỚC 8: Setup PauseMenu Script

1. **Tạo PauseMenuManager GameObject**:
   - Right-click `GameplayCanvas` → Create Empty
   - Đổi tên: `PauseMenuManager`
   - Add Component → **PauseMenu**

2. **Gán references trong PauseMenu.cs**:

**Pause Button:**
```
Pause Button: [Kéo PauseButton GameObject]
```
**LƯU Ý**: Không cần gán Pause/Play Sprite nữa - Button sẽ bị ẩn khi mở Pause Menu

**Pause Menu Panel:**
```
Pause Menu Panel: [Kéo PauseMenuPanel GameObject]
```

**Menu Buttons:**
```
Continue Button:  [Kéo ContinueButton]
Settings Button:  [Kéo SettingsButton]
Main Menu Button: [Kéo MainMenuButton]
```

**Settings Menu Panel (QUAN TRỌNG - Liên kết đến Settings):**
```
Settings Menu Panel: [Kéo SettingsContainer/SettingsMenuPanel GameObject]
```

**Confirmation Dialog:**
```
Confirmation Dialog: [Kéo ConfirmationDialog GameObject]
Confirm Yes Button:  [Kéo YesButton]
Confirm No Button:   [Kéo NoButton]
```

**⚠️ LƯU Ý QUAN TRỌNG:**
- **Settings Button** click sẽ gọi `OpenSettings()` → Hiện `settingsMenuPanel`
- `settingsMenuPanel` chính là **SettingsMenuPanel** GameObject đã copy từ Main Menu
- SettingsMenuPanel tự quản lý VolumePanel và ControlPanel
- Back button trong Settings sẽ gọi `PauseMenu.CloseSettings()` → Quay lại Pause Menu

**Flow:**
1. Click Settings Button → Ẩn PauseMenuPanel → Hiện SettingsMenuPanel
2. User điều chỉnh settings (Volume/Control)
3. Click Back trong Settings → Ẩn SettingsMenuPanel → Hiện PauseMenuPanel

---

## 2. UI CUSTOMIZATION SYSTEM

### Tổng quan
Cho phép người chơi drag & drop các UI elements trong game để tùy chỉnh vị trí.

### UI Elements có thể customize:
- Item/Weapon pickup notifications
- Skill/Ability buttons
- Health/XP bars (optional)
- Minimap (nếu có)

### Cấu trúc:

```
GameplayCanvas
├── CustomizableUIElements (GameObject)
│   ├── PickupNotification (GameObject + DraggableUI)
│   ├── SkillButton1 (GameObject + DraggableUI)
│   ├── SkillButton2 (GameObject + DraggableUI)
│   └── ... (các UI khác)
│
└── UICustomizationPanel (GameObject) - Ban đầu inactive
    ├── Background (Image)
    ├── Title (TextMeshPro: "Tùy chỉnh giao diện")
    ├── Instructions (TextMeshPro: "Kéo thả các UI để đổi vị trí")
    └── ButtonsContainer (GameObject)
        ├── SaveButton (Button: "Lưu")
        ├── CancelButton (Button: "Hủy")
        └── ResetButton (Button: "Đặt lại")
```

### Setup chi tiết:

#### BƯỚC 1: Đánh dấu UI Elements có thể drag
1. Chọn UI element muốn cho phép drag (VD: PickupNotification)
2. Add Component → **DraggableUI**
3. Cấu hình:
   - **Element Name**: Tên unique (VD: "PickupNotification")
   - **Load Position On Start**: true
   - **Constrain To Screen**: true
   - **Background Image**: Gán Image component của element

Lặp lại cho tất cả UI elements muốn customize.

#### BƯỚC 2: Tạo UI Customization Panel
1. Right-click `GameplayCanvas` → Create Empty
2. Đổi tên: `UICustomizationPanel`
3. Set Active = false
4. Add Component → **UILayoutCustomizer**

5. Tạo Background:
   - Right-click → UI → Image
   - Color: Semi-transparent
   - Stretch to fill

6. Tạo Title:
   - Text: "Tùy chỉnh giao diện"
   - Font Size: 48
   - Anchor: Top-Center

7. Tạo Instructions Panel:
   ```
   InstructionsPanel (GameObject)
   ├── Background (Image)
   └── Text (TextMeshPro: "Kéo thả các UI để thay đổi vị trí\nNhấn Lưu để áp dụng")
   ```

8. Tạo Buttons Container:
   - Add Horizontal Layout Group
   - Spacing: 20

9. Tạo 3 buttons:
   - **SaveButton**: "Lưu" / "Save"
   - **CancelButton**: "Hủy" / "Cancel"
   - **ResetButton**: "Đặt lại" / "Reset"

#### BƯỚC 3: Setup UILayoutCustomizer Script
Chọn `UICustomizationPanel`:

**UI Elements to Customize:**
- Size: Số lượng UI elements
- Gán tất cả DraggableUI components

**Control Buttons:**
- Save Button: Gán SaveButton
- Cancel Button: Gán CancelButton
- Reset Button: Gán ResetButton

**Instructions:**
- Gán InstructionsPanel GameObject

#### BƯỚC 4: Tích hợp vào JoystickSettings
Trong Control tab của Settings Panel:
- **Customize UI Button**: Button "Tùy chỉnh vị trí UI"
- **UI Customization Panel**: Gán UICustomizationPanel

---

## 3. FLOW HOẠT ĐỘNG

### Pause Game Flow:
```
1. Click Pause Button
   → Game paused (Time.timeScale = 0)
   → Show Pause Menu Panel
   
2. Trong Pause Menu:
   
   Option A: Click "Continue"
   → Resume game (Time.timeScale = 1)
   → Hide Pause Menu
   
   Option B: Click "Settings"
   → Hide Pause Menu
   → Show Settings Panel
   → User thay đổi settings
   → Click Close/Back
   → Show Pause Menu again
   
   Option C: Click "Main Menu"
   → Resume time (Time.timeScale = 1)
   → Load Scene 0 (Main Menu)
```

### UI Customization Flow:
```
1. Trong Pause Menu → Settings → Control Tab
   → Click "Tùy chỉnh vị trí UI"

2. Enter Customization Mode:
   → Tất cả DraggableUI được enable
   → Show Instructions
   → Show Save/Cancel/Reset buttons

3. User drag & drop UI elements

4. Click "Save":
   → Lưu vị trí vào PlayerPrefs
   → Exit customization mode
   → Disable dragging

5. Click "Cancel":
   → Restore vị trí ban đầu
   → Exit customization mode

6. Click "Reset":
   → Đặt lại tất cả về vị trí mặc định
   → Lưu vào PlayerPrefs
```

---

## 4. TESTING

### Test Pause Menu:
1. Chạy game (Play Mode)
2. Click Pause Button → Game dừng
3. Click Continue → Game resume
4. Pause lại → Click Settings → Settings mở
5. Click Close → Quay lại Pause Menu
6. Click Main Menu → Về Main Menu scene

### Test UI Customization:
1. Vào game
2. Pause → Settings → Control → "Tùy chỉnh vị trí UI"
3. Drag một UI element sang vị trí khác
4. Click Save
5. Restart game
6. Kiểm tra UI có ở vị trí đã lưu không

### Test Settings Apply in Game:
1. Pause → Settings → Volume
2. Thay đổi Music volume → Nghe nhạc nền thay đổi
3. Thay đổi SFX volume → Click button để test

4. Settings → Control → Chọn Joystick type khác
5. Resume game → Test joystick hoạt động theo mode mới

---

## 5. LƯU Ý QUAN TRỌNG

### Time Scale:
- Khi pause: `Time.timeScale = 0`
- Khi resume: `Time.timeScale = 1`
- Nhớ reset về 1 trước khi load scene khác

### UI Blocking:
- Pause Menu phải block tất cả input vào game
- Dùng Canvas Group với Blocks Raycasts = true
- Interactable = true

### Settings Panel:
- Settings Panel **KHÔNG thể dùng chung qua scene**
- Phải **COPY SettingsContainer** từ Main Menu sang Gameplay
- MainMenuPage reference để NULL trong Gameplay (không cần)
- Settings trong Pause Menu chỉ cần Back button về Pause Menu

### DraggableUI:
- Mỗi UI element phải có unique `elementName`
- Vị trí được lưu vào PlayerPrefs tự động
- Load vị trí khi Start() nếu loadPositionOnStart = true

### Audio:
- PlayMenuOpen() khi mở panel
- PlayMenuClose() khi đóng panel
- PlayButtonClick() khi click button

---

## 6. TROUBLESHOOTING

### Pause không hoạt động?
- Kiểm tra EventSystem có trong scene
- Verify Time.timeScale được set đúng
- Check Pause Button có Canvas Group blocking input không

### Settings không mở được?
- Đảm bảo SettingsPanel GameObject được gán
- Kiểm tra SettingsPanelScript reference
- Verify Settings Panel có SettingsPanel component

### UI không drag được?
- Kiểm tra DraggableUI component đã được add
- Verify UILayoutCustomizer đã enter customization mode
- Check Canvas có đúng Render Mode

### Vị trí UI không được lưu?
- Verify Element Name là unique
- Check SettingsManager có hoạt động
- Xem Debug.Log khi Save/Load

### Main Menu button không hoạt động?
- Kiểm tra Scene 0 là Main Menu trong Build Settings
- Verify SceneManager được import
- Check Time.timeScale được reset về 1

---

## 7. FILES LIÊN QUAN

### Scripts:
- `/Assets/Scripts/UI/PauseMenu.cs` - Quản lý Pause Menu
- `/Assets/Scripts/UI/SettingsPanel.cs` - Settings Panel
- `/Assets/Scripts/UI/UILayoutCustomizer.cs` - UI Customization system
- `/Assets/Scripts/UI/DraggableUI.cs` - Draggable UI component
- `/Assets/Scripts/UI/AudioSettings.cs` - Volume settings
- `/Assets/Scripts/UI/JoystickSettings.cs` - Joystick settings
- `/Assets/Scripts/Managers/SettingsManager.cs` - Settings manager

### Documentation:
- [SETTINGS_UI_SETUP_GUIDE.md](SETTINGS_UI_SETUP_GUIDE.md) - Setup Settings Panel
- [SETTINGS_SETUP_GUIDE.md](SETTINGS_SETUP_GUIDE.md) - Settings system guide (nếu có)

---

## 8. BEST PRACTICES

1. **Organize Hierarchy**:
   - Group UI elements logically
   - Use empty GameObjects as containers
   - Name consistently

2. **Performance**:
   - Disable panels khi không dùng (SetActive false)
   - Use Canvas Groups cho fade effects
   - Pool UI elements nếu có nhiều

3. **User Experience**:
   - Smooth transitions giữa các panels
   - Clear instructions cho UI customization
   - Save settings tự động
   - Provide Reset option

4. **Testing**:
   - Test trên nhiều resolutions
   - Verify UI constraints work properly
   - Test save/load functionality
   - Check scene transitions

5. **Accessibility**:
   - Large enough buttons
   - Clear text labels
   - Good color contrast
   - Responsive touch targets
