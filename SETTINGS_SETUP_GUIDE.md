# 🎮 SETTINGS SYSTEM SETUP GUIDE

> **Magic Survivors - Settings Implementation**  
> **Date:** 15/01/2026  
> **Features:** Audio Settings, Control Settings (Joystick Type), UI Layout Customization

---

## 📋 Tổng Quan

Settings System bao gồm:
- **Audio Settings**: Điều chỉnh Music Volume và SFX Volume
- **Control Settings**: 
  - Chọn Joystick Type (Fixed/Floating)
  - Customize UI Layout (Drag & Drop)
- **Tự động lưu/load** qua PlayerPrefs

---

## 📦 Các Script Đã Tạo

### Core Manager
- `SettingsManager.cs` - Singleton quản lý tất cả settings và PlayerPrefs

### UI Components
- `SettingsPanel.cs` - Quản lý tabs (Audio, Controls)
- `AudioSettings.cs` - UI sliders cho volume
- `JoystickSettings.cs` - Chọn joystick type và mở UI customizer
- `UILayoutCustomizer.cs` - Chế độ drag & drop UI elements
- `DraggableUI.cs` - Component cho UI có thể drag

### Updated Scripts
- `TouchJoystick.cs` - Đã thêm support Fixed/Floating mode

---

## 🛠️ SETUP TRONG UNITY

### Bước 1: Tạo Settings Panel UI

#### 1.1. Tạo cấu trúc UI Hierarchy

```
Canvas (Main Menu)
└── SettingsPanel (Panel)
    ├── Background (Image - Semi-transparent)
    ├── Content Panel
    │   ├── Header
    │   │   ├── Title Text ("Settings")
    │   │   └── Close Button
    │   ├── Tab Buttons (Horizontal Layout)
    │   │   ├── Audio Tab Button
    │   │   └── Controls Tab Button
    │   └── Tab Content Area
    │       ├── Audio Settings Panel
    │       └── Controls Settings Panel
```

#### 1.2. Tạo Audio Settings Panel

```
Audio Settings Panel
├── Music Volume Section
│   ├── Label Text ("Music Volume")
│   ├── Slider (Music Volume Slider)
│   │   └── Fill Area > Fill (Image)
│   └── Value Label Text ("100%")
└── SFX Volume Section
    ├── Label Text ("SFX Volume")
    ├── Slider (SFX Volume Slider)
    │   └── Fill Area > Fill (Image)
    └── Value Label Text ("100%")
```

**Steps:**
1. Right-click trong Settings Panel → UI → Panel (tên: "AudioSettingsPanel")
2. Add component `AudioSettings.cs`
3. Tạo 2 sliders:
   - GameObject → UI → Slider (tên: "MusicVolumeSlider")
   - GameObject → UI → Slider (tên: "SFXVolumeSlider")
4. Tạo TextMeshProUGUI cho value labels (optional)
5. Gán references vào AudioSettings component:
   - Music Volume Slider → `musicVolumeSlider`
   - SFX Volume Slider → `sfxVolumeSlider`
   - Music Volume Label → `musicVolumeLabel`
   - SFX Volume Label → `sfxVolumeLabel`

#### 1.3. Tạo Controls Settings Panel

```
Controls Settings Panel
├── Joystick Type Section
│   ├── Label Text ("Joystick Type")
│   ├── Toggle Group
│   │   ├── Floating Toggle (+ Label "Floating")
│   │   └── Fixed Toggle (+ Label "Fixed")
│   └── Current Type Label ("Current: Floating")
└── UI Customization Section
    ├── Label Text ("UI Layout")
    └── Customize Button ("Customize UI Layout")
```

**Steps:**
1. Right-click trong Settings Panel → UI → Panel (tên: "ControlsSettingsPanel")
2. Add component `JoystickSettings.cs`
3. Tạo Toggle Group:
   - GameObject → UI → Toggle (tên: "FloatingToggle")
   - GameObject → UI → Toggle (tên: "FixedToggle")
   - Create Empty GameObject (tên: "ToggleGroup"), add component `ToggleGroup`
4. Tạo Button:
   - GameObject → UI → Button (tên: "CustomizeUIButton")
5. Gán references vào JoystickSettings component:
   - Floating Toggle → `floatingToggle`
   - Fixed Toggle → `fixedToggle`
   - Toggle Group → `toggleGroup`
   - Customize Button → `customizeUIButton`
   - UI Customization Panel → `uiCustomizationPanel` (tạo ở bước sau)

---

### Bước 2: Tạo UI Customization Panel

#### 2.1. Tạo Customization UI

```
Canvas (Main Menu hoặc Gameplay)
└── UICustomizationPanel (Panel - Full screen)
    ├── Background (Image - Darker)
    ├── Preview Area (hiển thị game UI)
    ├── Instructions Panel
    │   └── Text ("Drag UI elements to customize...")
    └── Control Buttons (Bottom)
        ├── Save Button
        ├── Cancel Button
        └── Reset Button
```

**Steps:**
1. Tạo Panel full screen (tên: "UICustomizationPanel")
2. Add component `UILayoutCustomizer.cs`
3. Tạo 3 buttons: Save, Cancel, Reset
4. Gán references:
   - Save Button → `saveButton`
   - Cancel Button → `cancelButton`
   - Reset Button → `resetButton`
   - Preview Canvas → `previewCanvas`
   - Instructions Panel → `instructionsPanel`

#### 2.2. Thêm DraggableUI cho UI Elements

**Các UI Elements cần customize:**
- Joystick
- Item Buttons (Bomb, Magnet, Health Potion, etc.)
- Pause Button

**Steps cho mỗi element:**
1. Select UI Element (VD: Joystick)
2. Add component `DraggableUI.cs`
3. Trong Inspector:
   - **Element Name**: Đặt tên unique (VD: "Joystick", "BombButton", "MagnetButton")
   - **Load Position On Start**: ✓ (check)
   - **Constrain To Screen**: ✓ (check)
   - **Background Image**: Gán Image component của element
4. Lưu default position:
   - Right-click component → "Set as Default Position"

---

### Bước 3: Setup SettingsPanel Script

1. Select SettingsPanel GameObject
2. Add component `SettingsPanel.cs`
3. Setup Tabs:
   - **Tab 0**:
     - Tab Button → Audio Tab Button
     - Tab Content → Audio Settings Panel
   - **Tab 1**:
     - Tab Button → Controls Tab Button
     - Tab Content → Controls Settings Panel
4. Gán Close Button → `closeButton`

---

### Bước 4: Kết nối với Main Menu

1. Mở `MainMenu.cs` scene
2. Tìm Settings Button trong Main Menu
3. Add button vào `buttonPageMappings` list:
   - Button → Settings Button
   - Page → SettingsPanel

**Hoặc** thêm code vào MainMenu.cs:

```csharp
[Header("Settings")]
[SerializeField] private SettingsPanel settingsPanel;

// Trong Start() hoặc button click event
public void OpenSettings()
{
    if (settingsPanel != null)
    {
        settingsPanel.Open(0); // 0 = Audio tab
    }
}
```

---

### Bước 5: Setup TouchJoystick

1. Tìm TouchJoystick trong Gameplay scene
2. Joystick sẽ tự động load settings từ SettingsManager
3. **Không cần** thay đổi gì trong Inspector
4. Script sẽ tự động:
   - Load Joystick Type khi Start
   - Update khi player thay đổi trong Settings

---

### Bước 6: Setup SettingsManager (Auto)

SettingsManager tự động khởi tạo khi:
- Lần đầu gọi `SettingsManager.Instance`
- Hoặc add vào scene đầu tiên (Main Menu)

**Optional:** Thêm SettingsManager vào scene:
1. Create Empty GameObject (tên: "SettingsManager")
2. Add component `SettingsManager.cs`
3. **DontDestroyOnLoad** sẽ tự động apply

---

## 🎮 TESTING

### Test Audio Settings

1. Play Main Menu scene
2. Click Settings button
3. Chọn Audio tab
4. Kéo Music Volume slider → Music volume thay đổi real-time
5. Kéo SFX Volume slider → Nghe sound effect test
6. Close Settings → Reopen → Volume vẫn giữ nguyên (đã lưu)

### Test Joystick Type

1. Play Main Menu scene
2. Settings → Controls tab
3. Chọn "Fixed" → Joystick sẽ ở vị trí cố định khi chơi
4. Chọn "Floating" → Joystick xuất hiện nơi bạn touch
5. Start game → Test joystick behavior

### Test UI Customization

1. Settings → Controls tab → Click "Customize UI Layout"
2. Màn hình chuyển sang Customization Mode
3. Drag & drop Joystick, Buttons đến vị trí mong muốn
4. Click "Save" → Vị trí được lưu
5. Restart game → UI vẫn ở vị trí đã customize

**Test Reset:**
- Click "Reset" → Tất cả UI về vị trí mặc định

**Test Cancel:**
- Drag elements → Click "Cancel" → Vị trí không thay đổi

---

## 📝 PLAYER PREFS KEYS

Settings được lưu vào PlayerPrefs với các keys:

```
MusicVolume          → float (0.0 - 1.0)
SFXVolume            → float (0.0 - 1.0)
JoystickType         → int (0 = Floating, 1 = Fixed)
UI_<ElementName>_X   → float (vị trí X)
UI_<ElementName>_Y   → float (vị trí Y)
```

**Clear PlayerPrefs (for testing):**
```csharp
PlayerPrefs.DeleteAll();
PlayerPrefs.Save();
```

---

## 🐛 TROUBLESHOOTING

### Settings không lưu?
- Kiểm tra `PlayerPrefs.Save()` được gọi trong SettingsManager
- Test bằng cách print `PlayerPrefs.GetFloat("MusicVolume")`

### Joystick không đổi mode?
- Kiểm tra SettingsManager đã được khởi tạo
- Debug.Log trong `TouchJoystick.SetJoystickType()`
- Kiểm tra `LoadJoystickType()` được gọi trong Awake

### UI không drag được?
- Kiểm tra DraggableUI component đã được add
- Kiểm tra `isDraggingEnabled` = true (được set trong UILayoutCustomizer.EnterCustomizationMode())
- Kiểm tra Canvas có GraphicRaycaster

### UI vị trí không đúng sau khi load?
- Kiểm tra `elementName` phải unique
- Kiểm tra `loadPositionOnStart` = true
- Debug.Log vị trí được load: `SettingsManager.Instance.GetUIPosition()`

---

## 🎯 WORKFLOW SỬ DỤNG

### Người chơi muốn điều chỉnh âm lượng:
1. Main Menu → Settings
2. Audio tab → Kéo sliders
3. Close → Settings tự động lưu

### Người chơi muốn đổi Joystick Type:
1. Main Menu → Settings
2. Controls tab → Chọn Fixed/Floating
3. Start game → Joystick hoạt động theo mode đã chọn

### Người chơi muốn tùy chỉnh UI:
1. Settings → Controls → "Customize UI Layout"
2. Drag & drop UI elements
3. "Save" → Vị trí được lưu cho các lần chơi sau

---

## 🔧 CUSTOMIZATION

### Thêm Settings mới

**Ví dụ: Thêm Master Volume**

1. **SettingsManager.cs:**
```csharp
private const string MASTER_VOLUME_KEY = "MasterVolume";
private const float DEFAULT_MASTER_VOLUME = 1f;

public float GetMasterVolume()
{
    return PlayerPrefs.GetFloat(MASTER_VOLUME_KEY, DEFAULT_MASTER_VOLUME);
}

public void SetMasterVolume(float volume)
{
    volume = Mathf.Clamp01(volume);
    PlayerPrefs.SetFloat(MASTER_VOLUME_KEY, volume);
    PlayerPrefs.Save();
    
    AudioManager.Instance.SetMasterVolume(volume);
}
```

2. **AudioSettings.cs:**
- Thêm Slider mới
- Add listener trong `InitializeSliders()`
- Load trong `LoadCurrentSettings()`

### Thêm UI Element mới để customize

1. Tạo UI Element trong game scene
2. Add component `DraggableUI.cs`
3. Set `elementName` unique
4. Set default position
5. UILayoutCustomizer sẽ tự động detect (nếu dùng `GetComponentsInChildren`)

---

## 📚 API REFERENCE

### SettingsManager

```csharp
// Audio
SettingsManager.Instance.GetMusicVolume()
SettingsManager.Instance.SetMusicVolume(float value)
SettingsManager.Instance.GetSFXVolume()
SettingsManager.Instance.SetSFXVolume(float value)

// Controls
SettingsManager.Instance.GetJoystickType()
SettingsManager.Instance.SetJoystickType(JoystickType type)

// UI Layout
SettingsManager.Instance.SaveUIPosition(string name, Vector2 pos)
SettingsManager.Instance.GetUIPosition(string name, Vector2 defaultPos)
SettingsManager.Instance.ResetUILayout()

// Reset All
SettingsManager.Instance.ResetAllSettings()
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

---

## ✅ CHECKLIST

### Setup UI
- [ ] Tạo SettingsPanel với 2 tabs (Audio, Controls)
- [ ] Tạo Audio Settings Panel với 2 sliders
- [ ] Tạo Controls Settings Panel với toggles và button
- [ ] Tạo UI Customization Panel với Save/Cancel/Reset buttons
- [ ] Thêm DraggableUI cho Joystick và Item Buttons

### Setup Scripts
- [ ] SettingsManager đã được add (hoặc auto-init)
- [ ] AudioSettings script gán đúng sliders
- [ ] JoystickSettings script gán đúng toggles
- [ ] UILayoutCustomizer gán đúng buttons
- [ ] SettingsPanel gán đúng tabs và buttons

### Kết nối
- [ ] Main Menu có button mở Settings
- [ ] TouchJoystick trong Gameplay scene
- [ ] DraggableUI components đã set unique names

### Testing
- [ ] Audio sliders hoạt động và lưu
- [ ] Joystick type toggle hoạt động
- [ ] UI Customization drag & drop hoạt động
- [ ] Settings persistent sau restart game

---

## 🎨 UI DESIGN TIPS

### Audio Settings Panel
- Dùng Slider với Handle tròn
- Fill color gradient (green → yellow → red theo volume)
- Label hiển thị % để dễ nhìn

### Controls Settings Panel
- Toggle với icon rõ ràng (Floating icon vs Fixed icon)
- Preview thumbnail cho mỗi mode
- Button "Customize" nổi bật với icon edit

### UI Customization Mode
- Background darker để focus vào UI elements
- Highlight element đang được drag (glow effect)
- Grid overlay để align dễ hơn (optional)
- Undo/Redo buttons (advanced feature)

---

**🎮 Settings System Setup Complete!**

Nếu có vấn đề, check Troubleshooting section hoặc debug với Console logs.
