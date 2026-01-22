# 📚 HƯỚNG DẪN THIẾT LẬP SETTINGS TỔNG HỢP

> **Magic Survivors - Complete Settings Setup Guide**  
> **Tổng hợp từ:** AUDIO_SETUP_GUIDE.md, PAUSE_MENU_SETUP_GUIDE.md, SETTINGS_SETUP_GUIDE.md, SETTINGS_UI_SETUP_GUIDE.md  
> **Ngày tạo:** 22/01/2026

---

## 📋 MỤC LỤC

1. [Tổng quan hệ thống Settings](#1-tổng-quan-hệ-thống-settings)
2. [Audio System Setup](#2-audio-system-setup)
3. [Settings UI trong Main Menu](#3-settings-ui-trong-main-menu)
4. [Pause Menu và Settings trong Gameplay](#4-pause-menu-và-settings-trong-gameplay)
5. [UI Customization System](#5-ui-customization-system)
6. [Testing và Troubleshooting](#6-testing-và-troubleshooting)
7. [API Reference](#7-api-reference)

---

## 1. TỔNG QUAN HỆ THỐNG SETTINGS

### Các tính năng chính

Settings System bao gồm:
- **Audio Settings**: Điều chỉnh Music Volume và SFX Volume
- **Control Settings**: 
  - Chọn Joystick Type (Fixed/Floating/Fixed On Touch)
  - Customize UI Layout (Drag & Drop)
- **Pause Menu**: Tạm dừng game trong gameplay
- **Tự động lưu/load** qua PlayerPrefs

### Các Script đã tạo

#### Core Managers
- `SettingsManager.cs` - Singleton quản lý tất cả settings và PlayerPrefs
- `AudioManager.cs` - Quản lý audio system (Music + SFX)

#### UI Components
- `SettingsPanel.cs` - Quản lý tabs (Audio, Controls)
- `AudioSettings.cs` - UI sliders cho volume
- `JoystickSettings.cs` - Chọn joystick type và mở UI customizer
- `UILayoutCustomizer.cs` - Chế độ drag & drop UI elements
- `DraggableUI.cs` - Component cho UI có thể drag
- `PauseMenu.cs` - Quản lý pause menu trong gameplay
- `MainMenuSettings.cs` - Kết nối settings với main menu

#### Updated Scripts
- `TouchJoystick.cs` - Đã thêm support Fixed/Floating mode
- `AudioBootstrap.cs` - Khởi tạo AudioManager tự động

---

## 2. AUDIO SYSTEM SETUP

### ✅ Logic đã hoàn thiện

AudioManager đã được tích hợp hoàn chỉnh vào game với các chức năng:

#### 🎵 Music System
- Main Menu Music
- Gameplay Music
- Boss Music (thông qua MusicZone)
- Victory Music
- Defeat Music

#### 🔊 SFX System
- **Player**: Hit, Death, Level Up, Walk
- **Enemy**: Hit, Death, Attack, Boss Roar
- **Weapons**: Swing, Shoot, Hit, Throw, Explode, Ability Activate
- **UI**: Button Click, Button Hover, Menu Open/Close, Ability Select
- **Collectables**: Coin, Gem, Health, Bomb, Magnet, Potion, Chest
- **Ambient**: Fire Loop, Damage Text

### 📋 Các bước Setup trong Unity

#### Bước 1: Tạo AudioData Asset
1. Trong Unity, chuột phải vào thư mục `Assets/Resources/`
   - Nếu chưa có thư mục `Resources`, tạo mới: `Assets/Resources/`
2. Chọn **Create → Vampire → Audio Data**
3. Đặt tên là `AudioData` (chính xác tên này)

#### Bước 2: Gán Audio Clips vào AudioData
1. Select file `AudioData` vừa tạo
2. Trong Inspector, gán các AudioClip tương ứng:
   - **Music Section**: mainMenuMusic, gameplayMusic, bossMusic, victoryMusic, defeatMusic
   - **SFX - UI**: buttonClick, buttonHover, abilitySelect, menuOpen, menuClose
   - **SFX - Player**: playerHit, playerDeath, playerLevelUp, playerWalk
   - **SFX - Weapons**: weaponSwing, weaponShoot, weaponHit, throwableThrow, throwableExplode, abilityActivate
   - **SFX - Enemy**: enemyHit, enemyDeath, bossRoar, enemyAttack
   - **SFX - Items**: coinPickup, gemPickup, healthPickup, bombPickup, magnetPickup, potionPickup, chestOpen
   - **SFX - Ambient**: fireLoop, damageText
3. Điều chỉnh Volume Settings nếu cần:
   - Master Volume (0-1)
   - Music Volume (0-1)
   - SFX Volume (0-1)

#### Bước 3: Setup AudioBootstrap
1. Mở scene **Main Menu** (scene đầu tiên của game)
2. Tạo một Empty GameObject mới, đặt tên là `AudioBootstrap`
3. Add component `AudioBootstrap` vào GameObject này
4. Đảm bảo checkbox "Initialize On Awake" được bật

#### Bước 4: Test Audio System
1. Play scene Main Menu
2. Kiểm tra Console log: "AudioBootstrap: AudioManager initialized"
3. Kiểm tra các âm thanh:
   - Main Menu có nhạc nền
   - Button click có âm thanh
   - Vào gameplay có nhạc gameplay
   - Player bị hit có âm thanh
   - Enemy chết có âm thanh
   - Nhặt coin/gem có âm thanh
   - Game over/Victory có nhạc phù hợp

### 🔧 Optional: Thêm MusicZone cho Boss

Nếu muốn thay đổi nhạc khi Boss xuất hiện:

1. Tìm prefab hoặc GameObject của Boss
2. Add component `MusicZone`
3. Gán `zoneMusic` = boss music clip
4. Add `Collider2D` (Is Trigger = true)
5. Điều chỉnh size của collider để cover khu vực boss
6. Khi player vào zone này, nhạc sẽ tự động chuyển sang boss music

---

## 3. SETTINGS UI TRONG MAIN MENU

### Cấu trúc UI cần tạo

```
MainMenuCanvas
├── MainMenuPage (GameObject)
│   ├── PlayButton (Button)
│   ├── SettingsButton (Button) ← MỚI THÊM
│   ├── ExitButton (Button)
│   └── ... (các button khác)
│
└── SettingsPanel (GameObject) - Ban đầu inactive
    ├── Background (Image)
    ├── Title (TextMeshPro - Text: "Settings")
    ├── TabsContainer (GameObject)
    │   ├── VolumeTabButton (Button)
    │   └── ControlTabButton (Button)
    │
    ├── TabContents (GameObject)
    │   ├── VolumeContent (GameObject)
    │   │   ├── MusicVolumeSlider (Slider)
    │   │   │   └── ValueLabel (TextMeshPro - Text)
    │   │   └── SFXVolumeSlider (Slider)
    │   │       └── ValueLabel (TextMeshPro - Text)
    │   │
    │   └── ControlContent (GameObject)
    │       ├── JoystickTypePanel (GameObject)
    │       │   ├── Title (TextMeshPro - Text)
    │       │   ├── FloatingToggle (Toggle)
    │       │   │   └── Label (TextMeshPro - Text: "Di động")
    │       │   ├── FixedToggle (Toggle)
    │       │   │   └── Label (TextMeshPro - Text: "Cố định")
    │       │   └── FixedOnTouchToggle (Toggle)
    │       │       └── Label (TextMeshPro - Text: "Cố định khi chạm")
    │       │
    │       ├── CurrentTypeLabel (TextMeshPro - Text)
    │       └── CustomizeUIButton (Button)
    │           └── Label (TextMeshPro - Text: "Tùy chỉnh vị trí UI")
    │
    ├── CloseButton (Button)
    └── BackButton (Button)
```

### Chi tiết Setup từng phần

#### BƯỚC 1: Tạo Settings Button trong Main Menu

1. Tìm GameObject `MainMenuPage` trong Hierarchy
2. Right-click → UI → Button - TextMeshPro
3. Đổi tên thành `SettingsButton`
4. Trong component Button:
   - Để **Interactable** = true
   - **KHÔNG** thiết lập OnClick trong Inspector (sẽ được gán qua code)

#### BƯỚC 2: Tạo Settings Panel

1. Right-click trên `MainMenuCanvas` → Create Empty
2. Đổi tên thành `SettingsPanel`
3. Thêm component **Canvas Group** (để fade in/out nếu cần)
4. **Đặt Active = false** trong Inspector
5. Thêm Background:
   - Right-click `SettingsPanel` → UI → Image
   - Đặt Color với Alpha thấp (semi-transparent)
   - Stretch to fill parent

#### BƯỚC 3: Tạo Tab Buttons

1. Trong `SettingsPanel`, tạo `TabsContainer` (Empty GameObject)
2. Thêm component **Horizontal Layout Group**:
   - Child Alignment: Middle Center
   - Spacing: 20
   - Child Force Expand: Width = false, Height = false

3. Tạo 2 buttons trong `TabsContainer`:
   - `VolumeTabButton` (Text: "Volume")
   - `ControlTabButton` (Text: "Control")

#### BƯỚC 4: Tạo Volume Content

1. Tạo GameObject `VolumeContent` trong `SettingsPanel`
2. Thêm component **Vertical Layout Group**:
   - Padding: 20
   - Spacing: 30
   - Child Force Expand: Width = true, Height = false

3. Tạo **Music Volume Slider**:
   ```
   MusicSliderContainer (GameObject)
   ├── Title (TextMeshPro: "Âm lượng nhạc")
   ├── MusicVolumeSlider (Slider)
   └── ValueLabel (TextMeshPro: "100%")
   ```
   
   Cấu hình Slider:
   - Min Value: 0
   - Max Value: 1
   - Whole Numbers: false
   - Value: 0.7

4. Tạo **SFX Volume Slider** tương tự

#### BƯỚC 5: Tạo Control Content

1. Tạo GameObject `ControlContent` trong `SettingsPanel`
2. Thêm **Vertical Layout Group**

3. Tạo **Joystick Type Panel**:
   ```
   JoystickTypePanel (GameObject)
   ├── Title (TextMeshPro: "Loại Joystick")
   ├── ToggleGroup (Empty GameObject với Toggle Group component)
   │   ├── FloatingToggle (Toggle)
   │   ├── FixedToggle (Toggle)
   │   └── FixedOnTouchToggle (Toggle)
   └── CurrentTypeLabel (TextMeshPro: "Hiện tại: Di động")
   ```

4. Cấu hình Toggle Group:
   - Trong GameObject `ToggleGroup`, thêm component **Toggle Group**
   - Allow Switch Off: false
   - Trong mỗi Toggle component, gán **Group** = Toggle Group này

5. Tạo **Customize UI Button**:
   - Button với text "Tùy chỉnh vị trí UI"

#### BƯỚC 6: Setup Scripts trong Unity Inspector

##### Cho SettingsPanel.cs:
1. Chọn `SettingsPanel` GameObject
2. Trong **Tabs** list, thêm 2 elements:
   
   **Element 0 (Volume Tab)**:
   - Tab Button: VolumeTabButton
   - Tab Content: VolumeContent GameObject
   
   **Element 1 (Control Tab)**:
   - Tab Button: ControlTabButton
   - Tab Content: ControlContent GameObject

3. Gán:
   - **Close Button**: CloseButton
   - **Active Tab Color**: White (1, 1, 1, 1)
   - **Inactive Tab Color**: Gray (0.7, 0.7, 0.7, 1)

##### Cho AudioSettings.cs:
1. Chọn `VolumeContent` GameObject, thêm component **AudioSettings**
2. Gán:
   - **Music Volume Slider**: MusicVolumeSlider
   - **SFX Volume Slider**: SFXVolumeSlider
   - **Music Volume Label**: Label của MusicSlider
   - **SFX Volume Label**: Label của SFXSlider

##### Cho JoystickSettings.cs:
1. Chọn `ControlContent` GameObject, thêm component **JoystickSettings**
2. Gán:
   - **Floating Toggle**: FloatingToggle
   - **Fixed Toggle**: FixedToggle
   - **Fixed On Touch Toggle**: FixedOnTouchToggle
   - **Toggle Group**: ToggleGroup component
   - **Customize UI Button**: CustomizeUIButton
   - **UI Customization Panel**: (Tạo riêng ở phần sau)
   - **Current Type Label**: CurrentTypeLabel

---

## 4. PAUSE MENU VÀ SETTINGS TRONG GAMEPLAY

### Tổng quan
Pause Menu trong gameplay với các tính năng:
- **Continue**: Tiếp tục game
- **Settings**: Mở Settings Panel (dùng chung với Main Menu)
- **Return to Main Menu**: Trở về Main Menu
- **UI Customization**: Cho phép drag & drop UI elements trong game

### Cấu trúc Pause Menu

```
GameplayCanvas
├── PauseButton (Button - Image)
│   └── Icon (Image)
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
└── SettingsPanel (GameObject) - Ban đầu inactive
    └── (Cấu trúc giống Settings Panel ở Main Menu)
```

### Chi tiết setup

#### BƯỚC 1: Tạo Pause Button
1. Right-click `GameplayCanvas` → UI → Button
2. Đổi tên thành `PauseButton`
3. Đặt vị trí: Top-Right hoặc Top-Left
4. Thêm Icon pause (||) hoặc play (▶)
5. Component Button:
   - **KHÔNG** gán OnClick trong Inspector
   - Sẽ được gán qua code

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

#### BƯỚC 6: Tạo Settings Panel trong Gameplay
1. Có 2 cách:
   
   **Cách 1: Copy từ Main Menu** (Khuyến nghị)
   - Copy toàn bộ `SettingsPanel` từ Main Menu Scene
   - Paste vào `GameplayCanvas`
   - Set Active = false
   
   **Cách 2: Tạo mới**
   - Tạo giống hệt như trong Main Menu
   - Xem phần [Settings UI trong Main Menu](#3-settings-ui-trong-main-menu)

#### BƯỚC 7: Setup Scripts

##### PauseMenu.cs:
Chọn GameObject có PauseMenu component (có thể là GameplayCanvas hoặc GameObject riêng):

**Pause Button:**
- Gán `PauseButton` Image component

**Pause Sprites:**
- Pause Sprite: Icon pause (||)
- Play Sprite: Icon play (▶)

**Pause Menu Panel:**
- Gán `PauseMenuPanel` GameObject

**Menu Buttons:**
- Continue Button: Gán ContinueButton
- Settings Button: Gán SettingsButton
- Main Menu Button: Gán MainMenuButton

**Settings Panel:**
- Settings Panel: Gán SettingsPanel GameObject
- Settings Panel Script: Gán SettingsPanel component

---

## 5. UI CUSTOMIZATION SYSTEM

### Tổng quan
Cho phép người chơi drag & drop các UI elements trong game để tùy chỉnh vị trí.

### UI Elements có thể customize:
- Item/Weapon pickup notifications
- Skill/Ability buttons
- Health/XP bars (optional)
- Minimap (nếu có)
- Joystick

### Cấu trúc

```
GameplayCanvas
├── CustomizableUIElements (GameObject)
│   ├── PickupNotification (GameObject + DraggableUI)
│   ├── SkillButton1 (GameObject + DraggableUI)
│   ├── SkillButton2 (GameObject + DraggableUI)
│   ├── Joystick (GameObject + DraggableUI)
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

### Setup chi tiết

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

### Các loại Joystick

#### 1. Di động (Floating)
- Joystick xuất hiện tại vị trí người chơi chạm
- Biến mất khi nhả tay
- Thích hợp cho người chơi thích linh hoạt

#### 2. Cố định (Fixed)
- Joystick luôn hiển thị ở vị trí cố định
- Không biến mất
- Thích hợp cho người chơi quen thuộc với vị trí

#### 3. Cố định khi chạm (Fixed On Touch)
- Joystick xuất hiện và cố định tại vị trí chạm đầu tiên
- Biến mất khi nhả tay
- Xuất hiện lại ở vị trí chạm mới
- Kết hợp ưu điểm của cả 2 loại trên

---

## 6. TESTING VÀ TROUBLESHOOTING

### Testing

#### Test Audio Settings
1. Play Main Menu scene
2. Click Settings button
3. Chọn Audio tab
4. Kéo Music Volume slider → Music volume thay đổi real-time
5. Kéo SFX Volume slider → Nghe sound effect test
6. Close Settings → Reopen → Volume vẫn giữ nguyên (đã lưu)

#### Test Joystick Type
1. Play Main Menu scene
2. Settings → Controls tab
3. Chọn "Fixed" → Joystick sẽ ở vị trí cố định khi chơi
4. Chọn "Floating" → Joystick xuất hiện nơi bạn touch
5. Start game → Test joystick behavior

#### Test UI Customization
1. Settings → Controls tab → Click "Customize UI Layout"
2. Màn hình chuyển sang Customization Mode
3. Drag & drop Joystick, Buttons đến vị trí mong muốn
4. Click "Save" → Vị trí được lưu
5. Restart game → UI vẫn ở vị trí đã customize

**Test Reset:**
- Click "Reset" → Tất cả UI về vị trí mặc định

**Test Cancel:**
- Drag elements → Click "Cancel" → Vị trí không thay đổi

#### Test Pause Menu
1. Chạy game (Play Mode)
2. Click Pause Button → Game dừng
3. Click Continue → Game resume
4. Pause lại → Click Settings → Settings mở
5. Click Close → Quay lại Pause Menu
6. Click Main Menu → Về Main Menu scene

### Troubleshooting

#### Không có âm thanh?
1. Kiểm tra AudioData có trong `Assets/Resources/AudioData.asset`
2. Kiểm tra Console có error "AudioData not found"?
3. Kiểm tra các AudioClip đã được gán trong AudioData chưa?
4. Kiểm tra AudioBootstrap đã được add vào scene Main Menu chưa?
5. Kiểm tra volume settings trong AudioData (không phải = 0)

#### Một số âm thanh không chơi?
1. Kiểm tra AudioClip tương ứng đã được gán trong AudioData chưa?
2. Kiểm tra Console có warning nào không?

#### Settings không lưu?
- Kiểm tra `PlayerPrefs.Save()` được gọi trong SettingsManager
- Test bằng cách print `PlayerPrefs.GetFloat("MusicVolume")`
- Kiểm tra SettingsManager đã được tạo (singleton tự động)

#### Joystick không đổi mode?
- Kiểm tra SettingsManager đã được khởi tạo
- Debug.Log trong `TouchJoystick.SetJoystickType()`
- Kiểm tra `LoadJoystickType()` được gọi trong Awake
- Đảm bảo TouchJoystick script được update

#### Pause không hoạt động?
- Kiểm tra EventSystem có trong scene
- Verify Time.timeScale được set đúng
- Check Pause Button có Canvas Group blocking input không

#### Settings không mở được?
- Đảm bảo SettingsPanel GameObject được gán
- Kiểm tra SettingsPanelScript reference
- Verify Settings Panel có SettingsPanel component

#### UI không drag được?
- Kiểm tra DraggableUI component đã được add
- Verify UILayoutCustomizer đã enter customization mode
- Check Canvas có đúng Render Mode
- Kiểm tra `isDraggingEnabled` = true
- Kiểm tra Canvas có GraphicRaycaster

#### UI vị trí không đúng sau khi load?
- Kiểm tra `elementName` phải unique
- Kiểm tra `loadPositionOnStart` = true
- Debug.Log vị trí được load: `SettingsManager.Instance.GetUIPosition()`

#### Main Menu button không hoạt động?
- Kiểm tra Scene 0 là Main Menu trong Build Settings
- Verify SceneManager được import
- Check Time.timeScale được reset về 1

---

## 7. API REFERENCE

### SettingsManager

```csharp
// Audio
SettingsManager.Instance.GetMusicVolume() : float
SettingsManager.Instance.SetMusicVolume(float value)
SettingsManager.Instance.GetSFXVolume() : float
SettingsManager.Instance.SetSFXVolume(float value)

// Controls
SettingsManager.Instance.GetJoystickType() : JoystickType
SettingsManager.Instance.SetJoystickType(JoystickType type)

// UI Layout
SettingsManager.Instance.SaveUIPosition(string name, Vector2 pos)
SettingsManager.Instance.GetUIPosition(string name, Vector2 defaultPos) : Vector2
SettingsManager.Instance.ResetUILayout()

// Reset All
SettingsManager.Instance.ResetAllSettings()
```

### AudioManager

```csharp
// Music
AudioManager.Instance.PlayMusic(AudioClip clip)
AudioManager.Instance.StopMusic()
AudioManager.Instance.SetMusicVolume(float volume)

// SFX
AudioManager.Instance.PlaySFX(AudioClip clip)
AudioManager.Instance.PlayButtonClick()
AudioManager.Instance.PlayButtonHover()
AudioManager.Instance.PlayMenuOpen()
AudioManager.Instance.PlayMenuClose()
AudioManager.Instance.SetSFXVolume(float volume)

// Player
AudioManager.Instance.PlayPlayerHit()
AudioManager.Instance.PlayPlayerDeath()
AudioManager.Instance.PlayPlayerLevelUp()

// Enemy
AudioManager.Instance.PlayEnemyHit()
AudioManager.Instance.PlayEnemyDeath()
AudioManager.Instance.PlayEnemyAttack()

// Items
AudioManager.Instance.PlayCoinPickup()
AudioManager.Instance.PlayGemPickup()
AudioManager.Instance.PlayHealthPickup()
AudioManager.Instance.PlayChestOpen()
```

### TouchJoystick

```csharp
// Set joystick type programmatically
TouchJoystick joystick = FindObjectOfType<TouchJoystick>();
joystick.SetJoystickType(true); // true = Fixed, false = Floating
```

### DraggableUI

```csharp
DraggableUI element = GetComponent<DraggableUI>();
element.EnableDragging(true);  // Enable drag mode
element.SavePosition();         // Save current position
element.LoadPosition();         // Load saved position
element.ResetToDefaultPosition(); // Reset to default
```

### PauseMenu

```csharp
PauseMenu pauseMenu = FindObjectOfType<PauseMenu>();
pauseMenu.PauseGame();    // Pause the game
pauseMenu.ResumeGame();   // Resume the game
```

---

## 📝 PLAYER PREFS KEYS

Settings được lưu vào PlayerPrefs với các keys:

```
MusicVolume          → float (0.0 - 1.0)
SFXVolume            → float (0.0 - 1.0)
JoystickType         → int (0 = Floating, 1 = Fixed, 2 = Fixed On Touch)
UI_<ElementName>_X   → float (vị trí X)
UI_<ElementName>_Y   → float (vị trí Y)
```

**Clear PlayerPrefs (for testing):**
```csharp
PlayerPrefs.DeleteAll();
PlayerPrefs.Save();
```

---

## 🎯 WORKFLOW SỬ DỤNG

### Người chơi muốn điều chỉnh âm lượng:
1. Main Menu → Settings
2. Audio tab → Kéo sliders
3. Close → Settings tự động lưu

### Người chơi muốn đổi Joystick Type:
1. Main Menu → Settings
2. Controls tab → Chọn Fixed/Floating/Fixed On Touch
3. Start game → Joystick hoạt động theo mode đã chọn

### Người chơi muốn tùy chỉnh UI:
1. Settings → Controls → "Customize UI Layout"
2. Drag & drop UI elements
3. "Save" → Vị trí được lưu cho các lần chơi sau

### Trong gameplay muốn pause:
1. Click Pause Button
2. Chọn Continue/Settings/Main Menu
3. Nếu vào Settings → Điều chỉnh → Back to Pause Menu

---

## 📚 FILES LIÊN QUAN

### Scripts:
- `/Assets/Scripts/Managers/SettingsManager.cs`
- `/Assets/Scripts/Managers/AudioManager.cs`
- `/Assets/Scripts/Main Menu/MainMenu.cs`
- `/Assets/Scripts/Main Menu/MainMenuSettings.cs`
- `/Assets/Scripts/UI/SettingsPanel.cs`
- `/Assets/Scripts/UI/AudioSettings.cs`
- `/Assets/Scripts/UI/JoystickSettings.cs`
- `/Assets/Scripts/UI/UILayoutCustomizer.cs`
- `/Assets/Scripts/UI/DraggableUI.cs`
- `/Assets/Scripts/UI/PauseMenu.cs`
- `/Assets/Scripts/Character/TouchJoystick.cs`
- `/Assets/Scripts/Bootstrap/AudioBootstrap.cs`

### Documentation:
- `AUDIO_SETUP_GUIDE.md` - Audio system setup
- `PAUSE_MENU_SETUP_GUIDE.md` - Pause menu guide
- `SETTINGS_SETUP_GUIDE.md` - Settings system guide
- `SETTINGS_UI_SETUP_GUIDE.md` - Settings UI setup

---

## ✅ CHECKLIST

### Setup UI
- [ ] Tạo SettingsPanel với 2 tabs (Audio, Controls) trong Main Menu
- [ ] Tạo Audio Settings Panel với 2 sliders
- [ ] Tạo Controls Settings Panel với toggles và button
- [ ] Tạo UI Customization Panel với Save/Cancel/Reset buttons
- [ ] Thêm DraggableUI cho Joystick và Item Buttons
- [ ] Tạo Pause Menu Panel trong Gameplay scene
- [ ] Copy/Tạo Settings Panel trong Gameplay scene

### Setup Audio
- [ ] Tạo AudioData asset trong Resources folder
- [ ] Gán tất cả AudioClips vào AudioData
- [ ] Tạo AudioBootstrap trong Main Menu scene
- [ ] Test audio playback

### Setup Scripts
- [ ] SettingsManager đã được add (hoặc auto-init)
- [ ] AudioSettings script gán đúng sliders
- [ ] JoystickSettings script gán đúng toggles
- [ ] UILayoutCustomizer gán đúng buttons
- [ ] SettingsPanel gán đúng tabs và buttons
- [ ] PauseMenu script gán đúng buttons và panels

### Kết nối
- [ ] Main Menu có button mở Settings
- [ ] TouchJoystick trong Gameplay scene
- [ ] DraggableUI components đã set unique names
- [ ] Pause Button trong Gameplay scene
- [ ] Settings Panel accessible từ Pause Menu

### Testing
- [ ] Audio sliders hoạt động và lưu
- [ ] Joystick type toggle hoạt động
- [ ] UI Customization drag & drop hoạt động
- [ ] Settings persistent sau restart game
- [ ] Pause Menu pause/resume correctly
- [ ] Settings accessible từ both Main Menu và Pause Menu
- [ ] Main Menu button trong Pause Menu hoạt động

---

**🎮 Settings System Setup Complete!**

Hệ thống Settings đã hoàn chỉnh với đầy đủ chức năng Audio, Controls, UI Customization, và Pause Menu. Người chơi có thể tùy chỉnh trải nghiệm game theo ý thích và settings được lưu tự động qua PlayerPrefs.

Nếu có vấn đề, check Troubleshooting section hoặc debug với Console logs.
