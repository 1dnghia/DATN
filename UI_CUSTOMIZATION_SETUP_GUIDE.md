# UI CUSTOMIZATION SETUP GUIDE
## Hướng dẫn tạo UI Customization Panel (SIMPLIFIED VERSION)

Tính năng này cho phép người chơi tùy chỉnh vị trí của:
- **Joystick** (chỉ khi ở chế độ Fixed)
- **4 Inventory Slots**

---

## 📁 CẤU TRÚC ĐƠN GIẢN - CHỈ 1 FILE!

### **UICustomizationPanel.cs** - All-in-one solution
- Tự xử lý drag & drop bằng Input.GetMouseButton
- Tự lưu/load vị trí qua PlayerPrefs
- Không cần thêm script vào từng UI element
- **Chỉ cần add script vào 1 GameObject duy nhất!**

**Methods chính:**
- `OpenCustomization()` - Mở panel, bật drag mode
- `CloseCustomization()` - Đóng panel, tắt drag mode  
- `LoadAllSavedPositions()` - Load vị trí đã lưu (gọi khi Start)
- `ResetAllPositions()` - Reset tất cả về vị trí mặc định

---

## 🎨 BƯỚC 1: TẠO UI CUSTOMIZATION PANEL

### Hierarchy Structure:
```
UICustomizationPanel (GameObject)
├─ Background (Image - Semi-transparent overlay)
│  └─ CustomizationContainer (Panel)
│     ├─ Header
│     │  ├─ TitleText - "Customize UI Position"
│     │  └─ CloseButton (X)
│     │
│     ├─ InstructionText - "Drag to reposition..."
│     ├─ InfoText - Thông báo
│     │
│     └─ BottomButtons (Horizontal Layout)
│        ├─ ResetButton - "Reset to Default"
│        └─ CloseButton - "Done"
```

### Tạo Panel:
1. **Right-click trong Canvas → UI → Panel**
2. **Đổi tên: "UICustomizationPanel"**
3. **Add component: UICustomizationPanel.cs** ← CHỈ CẦN ADD Ở ĐÂY!

---

## 🔗 BƯỚC 2: ASSIGN REFERENCES TRONG INSPECTOR

### Chọn UICustomizationPanel GameObject:

**Panel References:**
```
Customization Panel: [Kéo chính UICustomizationPanel GameObject]
```

**UI Elements to Customize:**
```
Joystick Transform: [Kéo Joystick GameObject - chỉ cần RectTransform]
Inventory Slots: [Size = 4]
  Element 0: [Kéo Slot 0 RectTransform]
  Element 1: [Kéo Slot 1 RectTransform]
  Element 2: [Kéo Slot 2 RectTransform]
  Element 3: [Kéo Slot 3 RectTransform]
```

**Control Buttons:**
```
Reset Button: [Kéo Reset Button]
Close Button: [Kéo Close/Done Button]
```

**Info Text:**
```
Info Text: [Kéo TextMeshPro thông báo]
Instruction Text: [Kéo TextMeshPro hướng dẫn]
```

**Drag Settings:**
```
Parent Canvas: [Kéo Canvas chứa Joystick và Slots - thường là Gameplay Canvas]
```

⚠️ **QUAN TRỌNG:** 
- **KHÔNG CẦN** add thêm script vào Joystick hay Inventory Slots!
- Chỉ cần kéo **RectTransform** của chúng vào Inspector
- Tất cả logic drag & save đã có trong UICustomizationPanel.cs

---

## 🎮 BƯỚC 3: LINK VÀO JOYSTICK SETTINGS

### Mở Control Panel → JoystickSettings GameObject:

**UI Customization Section:**
```
Customize UI Button: [Kéo button "Customize UI"]
UI Customization Panel: [Kéo UICustomizationPanel component]
```

---

## 🚀 BƯỚC 4: HOÀN TẤT!

### Test ngay:
1. **Play game → Vào Settings → Control → Click "Customize UI"**
2. **Kéo thả Joystick (nếu Fixed mode) và 4 Inventory Slots**
3. **Click "Done" → Reload game → Vị trí được giữ nguyên!**

---

## 🎨 UI DESIGN SUGGESTIONS

### Colors:
- **Background:** Đen với Alpha = 150 (#000000, A=150)
- **Container:** Trắng/Xám nhạt (#F5F5F5)
- **Reset Button:** Đỏ (#FF4444)
- **Done Button:** Xanh (#44FF44)

### Text:
- **Title:** "Customize UI Position" / "Tùy chỉnh vị trí UI"
- **Instruction (Fixed):** "Drag to reposition Joystick and Inventory Slots"
- **Instruction (Other):** "Drag to reposition Inventory Slots only\n(Joystick only in Fixed mode)"
- **Reset:** "Reset to Default" / "Đặt lại"
- **Close:** "Done" / "Xong"

---

## 🔧 TESTING CHECKLIST

### ✅ Basic:
- [ ] Click "Customize UI" → Panel mở
- [ ] Kéo thả 4 Inventory Slots hoạt động
- [ ] Vị trí được lưu và load lại khi restart

### ✅ Joystick Modes:
- [ ] **Fixed Mode:** Joystick hiện và kéo được
- [ ] **Floating/FixedOnTouch:** Joystick bị ẩn
- [ ] Instruction text thay đổi theo mode

### ✅ Reset:
- [ ] Click "Reset" → Tất cả về vị trí ban đầu
- [ ] PlayerPrefs bị xóa

### ✅ Boundaries:
- [ ] Không kéo ra ngoài màn hình
- [ ] Elements luôn trong Canvas

---

## 🐛 TROUBLESHOOTING

### Không kéo được:
- Check **Parent Canvas** đã assign chưa
- Check **Camera** của Canvas (Screen Space - Camera cần Main Camera)

### Vị trí không lưu:
- Check Console có lỗi không
- Verify PlayerPrefs keys: `JoystickPosX/Y`, `InventorySlot0_PosX/Y`, etc.

### Elements nhảy lung tung:
- Check **originalPositions** được lưu đúng trong Start()
- Verify Anchors của Joystick/Slots không bị sai

---

## 📊 PLAYERPREFS KEYS

**Joystick:**
- `JoystickPosX`, `JoystickPosY`

**Inventory Slots:**
- `InventorySlot0_PosX/Y`
- `InventorySlot1_PosX/Y`
- `InventorySlot2_PosX/Y`
- `InventorySlot3_PosX/Y`

**Clear:** Gọi `ResetAllPositions()`

---

## ✨ SO SÁNH VỚI PHIÊN BẢN CŨ

### Cũ (3 files):
❌ UIPositionManager.cs
❌ DraggableUIElement.cs (add vào 5 GameObjects)
❌ UICustomizationPanel.cs
= **Phức tạp, nhiều scripts**

### Mới (1 file):
✅ UICustomizationPanel.cs (all-in-one)
= **Đơn giản, chỉ 1 script trên 1 GameObject!**

---

**Hoàn tất!** 🎉 Chỉ cần 1 script duy nhất!


### Hierarchy Structure:
```
UICustomizationPanel (GameObject)
├─ Background (Image - Semi-transparent dark overlay)
│  └─ CustomizationContainer (Panel)
│     ├─ Header (Panel)
│     │  ├─ TitleText (TextMeshPro) - "Customize UI Position"
│     │  └─ CloseButton (Button)
│     │
│     ├─ InstructionPanel (Panel)
│     │  └─ InstructionText (TextMeshPro) - "Drag to reposition..."
│     │
│     ├─ InfoText (TextMeshPro) - Hiển thị thông báo
│     │
│     └─ BottomButtons (Panel with Horizontal Layout Group)
│        ├─ ResetButton (Button) - "Reset to Default"
│        └─ CloseButton (Button) - "Done"
```

### Tạo Panel:
1. **Right-click trong Canvas → UI → Panel**
2. **Đổi tên thành "UICustomizationPanel"**
3. **Add component UICustomizationPanel.cs**

### Background Setup:
- **Image Component:**
  - Color: Đen với Alpha = 150-180 (semi-transparent)
  - Raycast Target: **Bật** (để chặn input qua panel)
- **RectTransform:**
  - Anchors: Stretch to full screen (0,0,1,1)
  - Offsets: 0,0,0,0

### CustomizationContainer:
- **Panel con chứa nội dung chính**
- **Vertical Layout Group:**
  - Padding: 20
  - Spacing: 15
  - Child Force Expand: Width & Height

---

## 🕹️ BƯỚC 2: SETUP JOYSTICK DRAGGABLE

### Trong Gameplay Scene:
1. **Tìm Joystick GameObject** (thường có tên Joystick, FixedJoystick, hoặc JoystickContainer)

2. **Add Component → DraggableUIElement.cs**

3. **Configure Inspector:**
   ```
   Element Type: Joystick
   Rect Transform: [Auto-filled]
   Parent Canvas: [Auto-filled or drag Gameplay Canvas]
   Can Drag: true (sẽ tự động được control bởi UICustomizationPanel)
   ```

4. **⚠️ LƯU Ý:** 
   - Joystick chỉ có thể kéo thả khi đang ở **Fixed mode**
   - Khi ở Floating hoặc Fixed On Touch mode, Joystick sẽ bị ẩn trong Customization Panel

---

## 📦 BƯỚC 3: SETUP INVENTORY SLOTS DRAGGABLE

### Tìm 4 Inventory Slots:
1. **Thường nằm trong Canvas → InventoryContainer → Slot0, Slot1, Slot2, Slot3**

2. **Với MỖI slot (0 → 3):**
   - Add Component → **DraggableUIElement.cs**
   - Configure:
     ```
     Element Type: InventorySlot0 (hoặc 1, 2, 3 tùy slot)
     Rect Transform: [Auto-filled]
     Parent Canvas: [Auto-filled]
     Can Drag: true
     ```

3. **Đảm bảo mỗi slot có đúng Element Type:**
   - Slot thứ 1: `InventorySlot0`
   - Slot thứ 2: `InventorySlot1`
   - Slot thứ 3: `InventorySlot2`
   - Slot thứ 4: `InventorySlot3`

---

## 🔗 BƯỚC 4: LINK REFERENCES TRONG UICUSTOMIZATIONPANEL

### Chọn UICustomizationPanel GameObject:

**Panel References:**
```
Customization Panel: [Kéo chính UICustomizationPanel GameObject]
```

**UI Elements to Customize:**
```
Joystick Element: [Kéo Joystick GameObject có DraggableUIElement]
Inventory Slots: [Size = 4]
  Element 0: [Kéo Slot 0]
  Element 1: [Kéo Slot 1]
  Element 2: [Kéo Slot 2]
  Element 3: [Kéo Slot 3]
```

**Control Buttons:**
```
Reset Button: [Kéo Reset Button]
Close Button: [Kéo Close/Done Button]
```

**Info Text:**
```
Info Text: [Kéo TextMeshPro hiển thị thông báo]
```

**Visual Feedback:**
```
Instruction Panel: [Kéo InstructionPanel GameObject]
Instruction Text: [Kéo InstructionText TextMeshPro]
```

---

## 🎮 BƯỚC 5: LINK VÀO JOYSTICK SETTINGS (CONTROL PANEL)

### Mở Control Panel → JoystickSettings GameObject:

**UI Customization Section:**
```
Customize UI Button: [Kéo button "Customize UI"]
UI Customization Panel: [Kéo UICustomizationPanel GameObject]
```

⚠️ **Chú ý:** Type đã đổi từ `GameObject` sang `UICustomizationPanel` component

---

## 🚀 BƯỚC 6: TẠO UIPOSITIONMANAGER SINGLETON (Tùy chọn)

### Cách 1: Tự động tạo (Recommended)
- UIPositionManager sẽ tự động tạo singleton khi cần
- Không cần setup gì thêm

### Cách 2: Tạo GameObject tĩnh
1. **Create Empty GameObject trong Scene**
2. **Đổi tên: "UIPositionManager"**
3. **Add Component → UIPositionManager.cs**
4. **Check "DontDestroyOnLoad" được gọi trong Awake**

---

## 📱 BƯỚC 7: LOAD SAVED POSITIONS KHI START GAME

### Trong Gameplay Scene, tìm GameObject quản lý UI (hoặc GameManager):

```csharp
using UnityEngine;

namespace Vampire
{
    public class GameplayUIManager : MonoBehaviour
    {
        [SerializeField] private UICustomizationPanel customizationPanel;

        private void Start()
        {
            // Load vị trí đã lưu cho tất cả UI elements
            if (customizationPanel != null)
            {
                customizationPanel.LoadAllSavedPositions();
            }
        }
    }
}
```

**HOẶC** gọi trực tiếp trong UICustomizationPanel:

```csharp
private void Awake()
{
    // Load saved positions khi scene load
    LoadAllSavedPositions();
}
```

---

## 🎨 UI DESIGN SUGGESTIONS

### Colors:
- **Background Overlay:** Đen với Alpha = 150 (#000000, A=150)
- **CustomizationContainer:** Trắng hoặc xám nhạt (#F5F5F5)
- **Header:** Màu chính của game (ví dụ: #FF6B6B)
- **Buttons:** 
  - Reset: Đỏ (#FF4444)
  - Done/Close: Xanh lá (#44FF44)

### Text Suggestions:
- **Title:** "Customize UI Position" / "Tùy chỉnh vị trí UI"
- **Instruction (Fixed mode):** 
  - "Drag to reposition Joystick and Inventory Slots"
  - "Kéo thả để di chuyển Joystick và các Inventory Slot"
- **Instruction (Other modes):**
  - "Drag to reposition Inventory Slots only"
  - "(Joystick can only be customized in Fixed mode)"
- **Reset Button:** "Reset to Default" / "Đặt lại mặc định"
- **Close Button:** "Done" / "Xong"

---

## 🔧 TESTING CHECKLIST

### ✅ Test Basic Functionality:
- [ ] Click "Customize UI" button trong Control Panel
- [ ] UICustomizationPanel hiện ra
- [ ] Có thể kéo thả 4 Inventory Slots
- [ ] Vị trí được lưu sau khi kéo (check PlayerPrefs)

### ✅ Test Joystick Modes:
- [ ] **Fixed Mode:** Joystick hiện ra và có thể kéo
- [ ] **Floating Mode:** Joystick BỊ ẨN trong customization
- [ ] **Fixed On Touch Mode:** Joystick BỊ ẨN trong customization
- [ ] Instruction text cập nhật đúng theo mode

### ✅ Test Save/Load:
- [ ] Kéo thả các elements, đóng panel
- [ ] Restart game hoặc reload scene
- [ ] Vị trí được load đúng như đã lưu

### ✅ Test Reset:
- [ ] Click "Reset to Default" button
- [ ] Tất cả elements về vị trí ban đầu
- [ ] PlayerPrefs bị xóa (check với PlayerPrefs.DeleteAll trong test)

### ✅ Test Boundaries:
- [ ] Không thể kéo elements ra ngoài màn hình
- [ ] Elements luôn trong giới hạn Canvas

---

## 🐛 TROUBLESHOOTING

### Vấn đề 1: Không kéo được UI elements
**Giải pháp:**
- Check `Can Drag = true` trong DraggableUIElement
- Đảm bảo UICustomizationPanel đã gọi `EnableDragging(true)` khi mở
- Check có Canvas Raycaster trên Canvas không

### Vấn đề 2: Joystick vẫn hiện khi không phải Fixed mode
**Giải pháp:**
- Check code trong `UICustomizationPanel.UpdateJoystickVisibility()`
- Đảm bảo `SettingsManager.Instance.GetJoystickType()` trả về đúng

### Vấn đề 3: Vị trí không được lưu
**Giải pháp:**
- Check `UIPositionManager.Instance` có tồn tại không
- Check Console log xem có lỗi PlayerPrefs không
- Verify `OnEndDrag` được gọi trong DraggableUIElement

### Vấn đề 4: Vị trí không load khi start game
**Giải pháp:**
- Đảm bảo đã gọi `LoadAllSavedPositions()` trong Start/Awake
- Check UICustomizationPanel được assigned đúng references

### Vấn đề 5: Elements bị kéo ra ngoài màn hình
**Giải pháp:**
- Check method `ClampToCanvas()` trong DraggableUIElement
- Verify `canvasRect` được gán đúng

---

## 📊 DATA STORAGE (PlayerPrefs Keys)

### Joystick:
- `JoystickPosX` - float
- `JoystickPosY` - float

### Inventory Slots:
- `InventorySlot0_PosX` - float
- `InventorySlot0_PosY` - float
- `InventorySlot1_PosX` - float
- `InventorySlot1_PosY` - float
- `InventorySlot2_PosX` - float
- `InventorySlot2_PosY` - float
- `InventorySlot3_PosX` - float
- `InventorySlot3_PosY` - float

**Clear All:** Gọi `UIPositionManager.Instance.ResetAllPositions()`

---

## 🎯 WORKFLOW TÓM TẮT

1. **Player vào Settings → Control Panel**
2. **Click "Customize UI" button**
3. **UICustomizationPanel mở:**
   - Nếu Fixed mode → Hiện Joystick + 4 Slots
   - Nếu other modes → Chỉ hiện 4 Slots
4. **Player kéo thả các elements**
5. **Vị trí tự động lưu vào PlayerPrefs khi thả chuột**
6. **Click "Done" để đóng panel**
7. **Khi start game lần sau → Vị trí được load từ PlayerPrefs**

---

## 🔄 FUTURE ENHANCEMENTS (Tùy chọn)

- [ ] Thêm preview ghost khi drag
- [ ] Snap to grid functionality
- [ ] Undo/Redo system
- [ ] Save multiple layouts (Layout 1, Layout 2, etc.)
- [ ] Export/Import layout configs
- [ ] Visual feedback khi element được drop (animation)

---

**Hoàn tất!** 🎉 UI Customization system đã sẵn sàng!
