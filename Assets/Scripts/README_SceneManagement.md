# Scene Management System Setup Guide

## Tổng quan
Hệ thống này quản lý việc chuyển đổi giữa các scene với Loading UI mượt mà.

**Đặc điểm chính**: PersistentScene chạy liên tục từ lúc khởi động đến khi thoát game, không bao giờ bị unload.

## Cấu trúc Scene:
1. **PersistentScene** - **LUÔN CHẠY** - Chứa các Manager và Loading UI (layer dưới cùng)
2. **MainMenuScene** - Menu chính (load lên trên PersistentScene)
3. **HubScene** - Map hub để chọn level và upgrade (thay thế MainMenuScene)
4. **GameplayScene** - Scene gameplay chính (thay thế HubScene)

## Setup Instructions:

### 1. PersistentScene Setup:
- Tạo một GameObject tên "ManagerInitializer" (sẽ được DontDestroyOnLoad)
- Attach script `ManagerInitializer.cs`
- Tạo các GameObject con:
  - SceneGameManager (attach `SceneGameManager.cs`)
  - SceneManager (attach `SceneManager.cs`)
  - LoadingManager (attach `LoadingManager.cs`)
  - AudioListenerManager (attach `AudioListenerManager.cs`)
- Assign các manager references trong ManagerInitializer

#### Camera Setup cho PersistentScene:
- **Xóa Main Camera** nếu có hoặc set Camera Depth = -100 (thấp nhất)
- **Xóa Audio Listener** khỏi PersistentScene (để tránh conflict)
- PersistentScene không cần render gì cả, chỉ chứa Manager và Loading UI

#### Tạo Loading UI thủ công:
- Tạo Canvas với Render Mode = Screen Space - Overlay
- **Set Sort Order = 9999** (để đảm bảo hiển thị trên tất cả UI của scene khác)
- Tạo Loading Panel (Image với background màu đen trong suốt)
- Thêm Loading Text (TextMeshProUGUI) - "Loading..."
- Thêm Loading Bar (Slider) - Progress bar
- Thêm Progress Text (TextMeshProUGUI) - "0%"
- Assign tất cả UI components vào LoadingManager trong Inspector
- **Quan trọng**: Set Loading Panel = inactive ban đầu
- **Canvas sẽ tự động được DontDestroyOnLoad bởi LoadingManager**

### 2. MainMenuScene Setup:
- Tạo Canvas với UI buttons:
  - Start Game Button
  - Settings Button  
  - Credits Button
  - Exit Button
- Attach script `MainMenu.cs` vào một GameObject
- Assign button references trong inspector

### 3. HubScene Setup:
- Tạo HubManager GameObject với script `HubManager.cs`
- Tạo UI cho:
  - Back to Main Menu Button
  - Start Gameplay Button
  - Map Selection Panel với Map Buttons
  - Upgrade Panel với Upgrade Button
- Assign tất cả references trong inspector

### 4. GameplayScene Setup:
- Tạo GameplayManager GameObject với script `GameplayManager.cs`
- Tạo UI cho:
  - Pause Button
  - Back to Hub Button
  - Pause Panel
  - Map Name Text
- Assign references trong inspector

### 5. Build Settings:
Thêm các scene vào Build Settings theo thứ tự:
1. PersistentScene (index 0) - **Đây là scene đầu tiên sẽ được load**
2. MainMenuScene (index 1)
3. HubScene (index 2)
4. GameplayScene (index 3)

**Quan trọng**: PersistentScene phải là scene đầu tiên trong Build Settings để game bắt đầu từ đây.

### Thứ tự khởi động tự động:
1. PersistentScene load đầu tiên
2. Các Manager được khởi tạo
3. SceneManager tự động load MainMenuScene lên trên PersistentScene
4. User thấy MainMenu, PersistentScene chạy ngầm bên dưới

### 6. Loading UI Components:
LoadingManager cần các components được assign thủ công trong Inspector:
- Loading Panel (GameObject) - Panel chứa toàn bộ Loading UI
- Loading Bar (Slider) - Thanh progress bar
- Loading Text (TextMeshProUGUI) - Text "Loading..."
- Progress Text (TextMeshProUGUI) - Text hiển thị phần trăm "0%"

### 7. Cấu trúc Loading Canvas khuyến nghị:
```
LoadingCanvas (Canvas - Sort Order: 9999)
├── LoadingPanel (Image - màu đen 80% opacity, INACTIVE ban đầu)
    ├── LoadingText (TextMeshProUGUI) - "Loading..."
    ├── LoadingBar (Slider) - Progress bar
    └── ProgressText (TextMeshProUGUI) - "0%"
```

**Lưu ý Sort Order**: 9999 đảm bảo Loading UI luôn hiển thị trên tất cả UI của scene khác
**Lưu ý DontDestroyOnLoad**: Canvas sẽ tự động được persistent bởi LoadingManager

## Luồng hoạt động:

1. **Game Start**: PersistentScene load đầu tiên và **KHÔNG BAO GIỜ UNLOAD**
2. **DontDestroyOnLoad**: ManagerInitializer, tất cả Manager và Loading Canvas được persistent
3. **Loading UI Setup**: Canvas ẩn đi (inactive), chờ khi cần thiết
4. **Auto MainMenu**: SceneManager tự động load MainMenuScene lên trên
5. **Scene Transitions**: Khi chuyển scene:
   - Loading Panel được bật lên (active) từ PersistentScene
   - Scene hiện tại (MainMenu/Hub/Gameplay) bị unload
   - Scene mới load lên
   - Loading Panel được ẩn đi (inactive)
6. **PersistentScene**: Luôn chạy ngầm, cung cấp Manager services và Loading UI
7. **Game End**: Chỉ khi thoát game, PersistentScene mới dừng

## Scripts tham khảo:

### Manager Scripts:
- `SceneGameManager.cs` - Quản lý trạng thái scene và map selection
- `SceneManager.cs` - Quản lý chuyển scene với async loading
- `LoadingManager.cs` - Quản lý Loading UI
- `HubManager.cs` - Quản lý HubScene
- `GameplayManager.cs` - Quản lý GameplayScene
- `ManagerInitializer.cs` - Khởi tạo các Manager

### UI Scripts:
- `MainMenu.cs` - UI Main Menu

### Constants:
- `GameConstants.cs` - Chứa các constants như scene names, input keys

## Lưu ý:
- **PersistentScene, tất cả Manager và Loading Canvas đều DontDestroyOnLoad**
- **PersistentScene chạy 100% thời gian game từ start đến exit**
- **Loading Canvas có Sort Order = 9999 để hiển thị trên tất cả scene**
- **Loading Panel mặc định INACTIVE, chỉ active khi cần loading**
- Scene names có thể thay đổi trong GameConstants.cs
- **PersistentScene KHÔNG BAO GIỜ bị unload, chỉ các scene khác load/unload**
- Khi chuyển scene: Loading Panel active → Scene cũ unload → Scene mới load → Loading Panel inactive
- **PersistentScene không có Camera hoặc Camera Depth rất thấp (-100)**
- **PersistentScene không có Audio Listener để tránh conflict**
- **AudioListenerManager tự động fix duplicate Audio Listener khi load scene**
- SceneManager tự động load MainMenu sau khi PersistentScene khởi tạo xong
- **Managers luôn available vì PersistentScene luôn active**
- **Loading UI luôn sẵn sàng vì Canvas persistent**