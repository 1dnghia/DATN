# MAGIC SURVIVORS - SEQUENCE DIAGRAMS

> **Tổng hợp sơ đồ sequence cho 17 use case chính**
> Định dạng PlantUML, đã kiểm tra và bổ sung chi tiết theo mẫu bạn gửi (actor, View/Controller/Entity, numbering, alt/opt block nếu cần).

---

## 1. Khởi động game và Main Menu (UC01)

### Cách 1: MVC Pattern (Controller điều khiển View)
```plantuml
@startuml
actor "Player" as Player
boundary "View: MainMenu" as MainMenu
control "Controller: GameManager" as GameManager

activate Player
Player -> MainMenu : 1. Click vào icon game để khởi động
activate MainMenu
MainMenu -> GameManager : 1.1. Load các resources cần thiết
activate GameManager
GameManager -> MainMenu : 1.2. Hiển thị Main Menu
deactivate GameManager
deactivate MainMenu
deactivate Player
@enduml
```
**Ưu điểm:** Rõ ràng về trách nhiệm, Controller orchestrate toàn bộ flow

### Cách 2: Unity Component-Based (View tự xử lý lifecycle)
```plantuml
@startuml
actor "Player" as Player
boundary "View: MainMenu" as MainMenu
control "Controller: GameManager" as GameManager

activate Player
Player -> MainMenu : 1. Chạm vào icon game để khởi động
activate MainMenu
MainMenu -> GameManager : 1.1. Load các resources cần thiết
activate GameManager
GameManager --> MainMenu : 1.2. Đã load xong
deactivate GameManager
MainMenu -> MainMenu : 1.3. Hiển thị Main Menu (OnEnable/Start)
deactivate MainMenu
deactivate Player
@enduml
```
**Ưu điểm:** Phản ánh đúng Unity MonoBehaviour lifecycle (Awake → OnEnable → Start)

---

## 2. Settings - Điều chỉnh Audio (UC02)
> **Lưu ý:** Settings tự hiển thị slider (bước 2.1) là hợp lý vì phản ánh Unity UI component tự render khi được kích hoạt

```plantuml
@startuml
actor "Player" as Player
boundary "View: MainMenu" as MainMenu
boundary "View: SettingsPanel" as Settings
control "Controller: AudioManager" as AudioManager

activate Player
Player -> MainMenu : 1. Chọn Settings
activate MainMenu
MainMenu -> Settings : 1.1. Hiển thị Settings với 2 option: Volume, Controls
deactivate MainMenu

activate Settings
Player -> Settings : 2. Nhấn Volume
Settings -> Settings : 2.1. Hiển thị 2 slider âm lượng
Player -> Settings : 3. Chạm và kéo slider để điều chỉnh
Settings -> AudioManager : 3.1. Cập nhật âm lượng real-time
activate AudioManager
AudioManager --> Settings : 3.2. Đã cập nhật âm lượng
deactivate AudioManager
deactivate Settings
deactivate Player
@enduml
```

---

## 3. Settings - Điều chỉnh Controls (UC03)
```plantuml
@startuml
actor "Player" as Player
boundary "View: MainMenu" as MainMenu
boundary "View: SettingsPanel" as Settings
control "Controller: JoystickSettings" as JoystickSettings

activate Player
Player -> MainMenu : 1. Chọn Settings
activate MainMenu
MainMenu -> Settings : 1.1. Hiển thị Settings với 2 option: Volume, Controls
deactivate MainMenu

activate Settings
Player -> Settings : 2. Chọn Controls
Settings -> JoystickSettings : 2.1. Yêu cầu hiển thị joystick options
activate JoystickSettings
JoystickSettings -> JoystickSettings : 2.2. Hiển thị các loại joystick
Player -> JoystickSettings : 3. Nhấn kiểu joystick mong muốn
JoystickSettings -> JoystickSettings : 3.1. Lưu và áp dụng cài đặt
deactivate JoystickSettings
deactivate Settings
deactivate Player
@enduml
```

---

## 4. Settings - Tùy chỉnh UI Layout (UC04)
```plantuml
@startuml
actor "Player" as Player
boundary "View: MainMenu" as MainMenu
boundary "View: SettingsPanel" as Settings
boundary "View: UICustomization" as UICustomization
control "Controller: UILayoutManager" as UILayoutManager

activate Player
Player -> MainMenu : 1. Nhấn nút Settings
activate MainMenu
MainMenu -> Settings : 1.1. Hiển thị Settings với 2 option: Volume, Controls
deactivate MainMenu

activate Settings
Player -> Settings : 2. Nhấn Controls
Settings -> Settings : 2.1. Hiển thị các loại joystick và Customize UI Layout
Player -> Settings : 3. Nhấn Customize UI Layout
Settings -> UICustomization : 3.1. Hiển thị chế độ tùy chỉnh UI
deactivate Settings

activate UICustomization
Player -> UICustomization : 4. Chạm và kéo thả các thành phần giao diện
UICustomization -> UILayoutManager : 4.1. Cập nhật vị trí UI
activate UILayoutManager
UILayoutManager --> UICustomization : 4.2. Đã cập nhật vị trí
deactivate UILayoutManager
Player -> UICustomization : 5. Nhấn Save/Reset/Cancel
UICustomization -> UILayoutManager : 5.1. Lưu/Hủy/Reset layout
activate UILayoutManager
UILayoutManager --> UICustomization : 5.2. Đã xử lý layout
deactivate UILayoutManager
deactivate UICustomization
deactivate Player
@enduml
```

---

## 5. Chọn nhân vật và xem thông tin (UC06)

### Cách 1: Unity-Style (View tự hiển thị sau khi nhận dữ liệu)
```plantuml
@startuml
actor "Player" as Player
boundary "View: MainMenu" as MainMenu
boundary "View: CharacterSelect" as CharacterSelect
entity "Entity: Progress" as Progress

activate Player
Player -> MainMenu : 1. Ấn nút "Bắt đầu"
activate MainMenu
MainMenu -> CharacterSelect : 1.1. Chuyển sang trang chọn nhân vật
deactivate MainMenu

activate CharacterSelect
CharacterSelect -> Progress : 1.2. Lấy danh sách nhân vật đã mở/mua
activate Progress
Progress --> CharacterSelect : 1.3. Trả về danh sách nhân vật
deactivate Progress
CharacterSelect -> CharacterSelect : 1.4. Hiển thị danh sách nhân vật
Player -> CharacterSelect : 2. Chạm chọn nhân vật
deactivate CharacterSelect
deactivate Player
@enduml
```
**Ưu điểm:** CharacterSelect tự quản lý UI rendering sau khi có data

### Cách 2: MVC Pattern (Controller điều khiển)
```plantuml
@startuml
actor "Player" as Player
boundary "View: MainMenu" as MainMenu
boundary "View: CharacterSelect" as CharacterSelect
control "Controller: CharacterController" as CharacterController
entity "Entity: Progress" as Progress

activate Player
Player -> MainMenu : 1. Ấn nút "Bắt đầu"
activate MainMenu
MainMenu -> CharacterController : 1.1. Yêu cầu hiển thị Character Select
activate CharacterController
CharacterController -> Progress : 1.1.1. Lấy danh sách nhân vật
activate Progress
Progress --> CharacterController : 1.1.2. Trả về danh sách
deactivate Progress
CharacterController -> CharacterSelect : 1.1.3. Hiển thị với dữ liệu
activate CharacterSelect
deactivate MainMenu
deactivate CharacterController
Player -> CharacterSelect : 2. Chạm chọn nhân vật
deactivate CharacterSelect
deactivate Player
@enduml
```
**Ưu điểm:** Tách biệt rõ ràng giữa logic và presentation

---

## 6. Mua/Mở khóa nhân vật (UC07)
> **Lưu ý:** Button "Buy" chỉ active khi đủ coins, nếu không đủ thì button bị disable

```plantuml
@startuml
actor "Player" as Player
boundary "View: CharacterSelect" as CharacterSelect
control "Controller: CurrencyManager" as CurrencyManager
entity "Entity: Progress" as Progress

activate Player
Player -> CharacterSelect : 1. Chạm chọn nhân vật bị khóa
activate CharacterSelect
CharacterSelect -> CurrencyManager : 1.1. Kiểm tra coins và lấy giá
activate CurrencyManager
CurrencyManager --> CharacterSelect : 1.2. Trả về thông tin giá và trạng thái đủ/thiếu coins
deactivate CurrencyManager
CharacterSelect -> CharacterSelect : 1.3. Hiển thị dialog (Buy button active nếu đủ coins, disable nếu thiếu)

alt Đủ coins (button active)
    Player -> CharacterSelect : 2. Nhấn nút Buy
    CharacterSelect -> CurrencyManager : 2.1. Thực hiện mua
    activate CurrencyManager
    CurrencyManager -> Progress : 2.1.1. Trừ coins, unlock nhân vật
    activate Progress
    Progress --> CurrencyManager : 2.1.2. Đã unlock
    deactivate Progress
    CurrencyManager -> CharacterSelect : 2.1.3. Thông báo "Character Unlocked!"
    deactivate CurrencyManager
else Không đủ coins (button disabled)
    note over Player, CharacterSelect: Button bị disable, không thể click
end
deactivate CharacterSelect
deactivate Player
@enduml
```

---

## 7. Chọn map để chơi (UC08)
```plantuml
@startuml
actor "Player" as Player
boundary "View: CharacterSelect" as CharacterSelect
boundary "View: MapSelect" as MapSelect
entity "Entity: Progress" as Progress
control "Controller: GameManager" as GameManager
boundary "View: UIManager" as UIManager

activate Player
Player -> CharacterSelect : 1. Chạm chọn nhân vật
activate CharacterSelect
CharacterSelect -> MapSelect : 1.1. Chuyển sang trang chọn bản đồ
deactivate CharacterSelect

activate MapSelect
MapSelect -> Progress : 1.2. Lấy danh sách bản đồ đã mở
activate Progress
Progress --> MapSelect : 1.3. Trả về danh sách bản đồ
deactivate Progress
MapSelect -> MapSelect : 1.4. Hiển thị danh sách bản đồ
Player -> MapSelect : 2. Chạm chọn bản đồ
MapSelect -> GameManager : 2.1. Bắt đầu game với nhân vật và bản đồ đã chọn
deactivate MapSelect

activate GameManager
GameManager -> UIManager : 2.1.1. Khởi tạo UI và gameplay
activate UIManager
deactivate UIManager
GameManager -> GameManager : 2.1.2. Bắt đầu gameplay
deactivate GameManager
deactivate Player
@enduml
```

---

## 8. Di chuyển nhân vật (UC09)
> **Lưu ý:** Game mobile sử dụng virtual joystick trên màn hình

```plantuml
@startuml
actor "Player" as Player
boundary "View: Joystick" as Joystick
control "Controller: InputHandler" as InputHandler
control "Controller: PlayerController" as PlayerController
entity "Entity: Player" as PlayerEntity
boundary "View: UIManager" as UIManager

activate Player
Player -> Joystick : 1. Chạm và kéo joystick trên màn hình
activate Joystick
Joystick -> InputHandler : 1.1. Gửi vector di chuyển (x, y)
activate InputHandler
InputHandler -> PlayerController : 1.1.1. Gửi tín hiệu điều hướng
activate PlayerController
PlayerController -> PlayerEntity : 1.1.1.1. Tính toán vị trí mới
PlayerController -> PlayerEntity : 1.1.1.2. Cập nhật vị trí nhân vật
PlayerController -> UIManager : 1.1.1.3. Cập nhật UI/camera
activate UIManager
deactivate UIManager
deactivate PlayerController
deactivate InputHandler
deactivate Joystick
deactivate Player
@enduml
```

---

## 9. Tạm dừng game (UC10)

### Cách 1: MVC Pattern (Controller điều khiển)
```plantuml
@startuml
actor "Player" as Player
control "Controller: InputHandler" as InputHandler
control "Controller: GameManager" as GameManager
boundary "View: PauseMenu" as PauseMenu

activate Player
Player -> InputHandler : 1. Nhấn nút Pause trên màn hình
activate InputHandler
InputHandler -> GameManager : 1.1. Pause game (Time.timeScale = 0)
activate GameManager
GameManager -> PauseMenu : 1.2. Hiển thị Pause Menu
activate PauseMenu
deactivate PauseMenu
deactivate GameManager
deactivate InputHandler
deactivate Player
@enduml
```

### Cách 2: Unity-Style (PauseMenu tự hiển thị)
```plantuml
@startuml
actor "Player" as Player
control "Controller: InputHandler" as InputHandler
control "Controller: GameManager" as GameManager
boundary "View: PauseMenu" as PauseMenu

activate Player
Player -> InputHandler : 1. Nhấn nút Pause trên màn hình
activate InputHandler
InputHandler -> GameManager : 1.1. Pause game (Time.timeScale = 0)
activate GameManager
GameManager -> PauseMenu : 1.2. Enable PauseMenu
activate PauseMenu
PauseMenu -> PauseMenu : 1.3. Hiển thị UI (OnEnable)
deactivate PauseMenu
deactivate GameManager
deactivate InputHandler
deactivate Player
@enduml
```
**Ưu điểm Cách 2:** PauseMenu component tự xử lý animation, layout khi được enable

---

## 10. Tiếp tục game (UC11)
```plantuml
@startuml
actor "Player" as Player
boundary "View: PauseMenu" as PauseMenu
control "Controller: GameManager" as GameManager

activate Player
Player -> PauseMenu : 1. Nhấn nút Resume
activate PauseMenu
PauseMenu -> GameManager : 1.1. Resume game (Time.timeScale = 1)
activate GameManager
GameManager -> PauseMenu : 1.2. Đóng Pause Menu
deactivate GameManager
deactivate PauseMenu
deactivate Player
@enduml
```

---

## 11. Thoát về menu (UC12, UC05)
```plantuml
@startuml
actor "Player" as Player
boundary "View: MainMenu" as MainMenu
boundary "View: PauseMenu" as PauseMenu
control "Controller: GameManager" as GameManager
entity "Entity: Progress" as Progress

activate Player
Player -> PauseMenu : 1. Nhấn nút Exit
activate PauseMenu
PauseMenu -> GameManager : 1.1. Hiển thị dialog xác nhận
activate GameManager
Player -> PauseMenu : 2. Nhấn Yes
GameManager -> Progress : 2.1. Lưu coins
activate Progress
Progress --> GameManager : 2.2. Đã lưu coins
deactivate Progress
GameManager -> MainMenu : 2.3. Load Main Menu scene
deactivate GameManager
deactivate PauseMenu
deactivate Player
@enduml
```

---

## 12. Chọn upgrade khi level up (UC13)
> **Lưu ý:** UpgradePanel tự hiển thị UI (1.1.4) phản ánh Unity UI tự render sau khi nhận data từ Controller

```plantuml
@startuml
actor "Player" as Player
control "Controller: GameController" as GameController
boundary "View: UpgradePanel" as UpgradePanel
control "Controller: UpgradeController" as UpgradeController
entity "Entity: PlayerPrefs" as PlayerPrefs
boundary "View: UIManager" as UIManager

activate Player
GameController -> UpgradePanel : 1. Hiển thị panel chọn nâng cấp
activate UpgradePanel
UpgradePanel -> UpgradeController : 1.1. Lấy danh sách nâng cấp hợp lệ
activate UpgradeController
UpgradeController -> PlayerPrefs : 1.1.1. Đọc trạng thái nâng cấp đã lưu
activate PlayerPrefs
PlayerPrefs --> UpgradeController : 1.1.2. Trả về trạng thái nâng cấp
deactivate PlayerPrefs
UpgradeController -> UpgradePanel : 1.1.3. Trả về danh sách hợp lệ
deactivate UpgradeController
UpgradePanel -> UpgradePanel : 1.1.4. Hiển thị UI lựa chọn
Player -> UpgradePanel : 2. Chạm chọn nâng cấp
UpgradePanel -> UpgradeController : 2.1. Xử lý lựa chọn
activate UpgradeController
UpgradeController -> PlayerPrefs : 2.1.1. Lưu trạng thái mới (PlayerPrefs.SetInt/SetString)
activate PlayerPrefs
PlayerPrefs --> UpgradeController : 2.1.2. Đã lưu
deactivate PlayerPrefs
UpgradeController -> GameController : 2.2. Thông báo nâng cấp mới
deactivate UpgradeController
GameController -> UIManager : 2.3. Cập nhật UI/hiệu ứng
activate UIManager
deactivate UIManager
deactivate UpgradePanel
deactivate Player
@enduml
```

---

## 13. Xem Achievements (UC14)
```plantuml
@startuml
actor "Player" as Player
boundary "View: MainMenu" as MainMenu
boundary "View: AchievementScreen" as AchievementScreen
entity "Entity: AchievementManager" as AchievementManager

activate Player
Player -> MainMenu : 1. Nhấn nút Achievements
activate MainMenu
MainMenu -> AchievementScreen : 1.1. Hiển thị Achievement Screen
deactivate MainMenu

activate AchievementScreen
AchievementScreen -> AchievementManager : 1.2. Lấy danh sách achievements
activate AchievementManager
AchievementManager --> AchievementScreen : 1.3. Trả về danh sách
deactivate AchievementManager
AchievementScreen -> AchievementScreen : 1.4. Hiển thị achievements
Player -> AchievementScreen : 2. Nhấn nút Back
AchievementScreen -> MainMenu : 2.1. Quay về Main Menu
deactivate AchievementScreen
deactivate Player
@enduml
```

---

## 14. Xem Collection (UC15)
```plantuml
@startuml
actor "Player" as Player
boundary "View: MainMenu" as MainMenu
boundary "View: CollectionScreen" as CollectionScreen
entity "Entity: CollectionManager" as CollectionManager

activate Player
Player -> MainMenu : 1. Nhấn nút Collection
activate MainMenu
MainMenu -> CollectionScreen : 1.1. Hiển thị Collection Screen
deactivate MainMenu

activate CollectionScreen
CollectionScreen -> CollectionManager : 1.2. Lấy danh sách collection
activate CollectionManager
CollectionManager --> CollectionScreen : 1.3. Trả về danh sách
deactivate CollectionManager
CollectionScreen -> CollectionScreen : 1.4. Hiển thị collection
Player -> CollectionScreen : 2. Nhấn nút Back
CollectionScreen -> MainMenu : 2.1. Quay về Main Menu
deactivate CollectionScreen
deactivate Player
@enduml
```

---

## 15. Restart game sau khi kết thúc (UC16)

### Cách 1: Unity-Style (GameOverDialog tự hiển thị)
```plantuml
@startuml
actor "Player" as Player
control "Controller: GameManager" as GameManager
boundary "View: GameOverDialog" as GameOverDialog
entity "Entity: StatsManager" as StatsManager
entity "Entity: Progress" as Progress

activate Player
GameManager -> GameOverDialog : 1. Enable Game Over Screen
activate GameOverDialog
GameOverDialog -> GameOverDialog : 1.1. Hiển thị kết quả, thống kê (OnEnable)
Player -> GameOverDialog : 2. Nhấn nút Restart
GameOverDialog -> GameManager : 2.1. Yêu cầu Restart
activate GameManager
GameManager -> Progress : 2.1.1. Lưu coins
activate Progress
Progress --> GameManager : 2.1.2. Đã lưu coins
deactivate Progress
GameManager -> GameManager : 2.1.3. Reload và bắt đầu lại game
deactivate GameManager
deactivate GameOverDialog
deactivate Player
@enduml
```
**Ưu điểm:** GameOverDialog tự load stats và hiển thị khi được enable

### Cách 2: MVC Pattern
```plantuml
@startuml
actor "Player" as Player
control "Controller: GameManager" as GameManager
boundary "View: GameOverDialog" as GameOverDialog
entity "Entity: StatsManager" as StatsManager
entity "Entity: Progress" as Progress

activate Player
GameManager -> StatsManager : 1. Lấy kết quả game
activate StatsManager
StatsManager --> GameManager : 1.1. Trả về stats
deactivate StatsManager
GameManager -> GameOverDialog : 1.2. Hiển thị với stats
activate GameOverDialog
Player -> GameOverDialog : 2. Nhấn nút Restart
GameOverDialog -> GameManager : 2.1. Yêu cầu Restart
activate GameManager
GameManager -> Progress : 2.1.1. Lưu coins
activate Progress
Progress --> GameManager : 2.1.2. Đã lưu coins
deactivate Progress
GameManager -> GameManager : 2.1.3. Reload và bắt đầu lại game
deactivate GameManager
deactivate GameOverDialog
deactivate Player
@enduml
```
**Ưu điểm:** GameManager điều khiển toàn bộ flow, tách biệt data và view

---

## 16. Quay về Main Menu sau khi kết thúc game (UC17)
```plantuml
@startuml
actor "Player" as Player
control "Controller: GameManager" as GameManager
boundary "View: GameOverDialog" as GameOverDialog
entity "Entity: StatsManager" as StatsManager
entity "Entity: Progress" as Progress
boundary "View: MainMenu" as MainMenu

activate Player
GameManager -> GameOverDialog : 1. Hiển thị Game Over Screen
activate GameOverDialog
GameOverDialog -> GameOverDialog : 1.1. Hiển thị kết quả, thống kê
Player -> GameOverDialog : 2. Nhấn nút Main Menu
GameOverDialog -> GameManager : 2.1. Yêu cầu về Main Menu
activate GameManager
GameManager -> Progress : 2.1.1. Lưu coins
activate Progress
Progress --> GameManager : 2.1.2. Đã lưu coins
deactivate Progress
GameManager -> MainMenu : 2.1.3. Load Main Menu scene
activate MainMenu
deactivate MainMenu
deactivate GameManager
deactivate GameOverDialog
deactivate Player
@enduml
```

---

> **Lưu ý:**
> - Đã bổ sung các chi tiết actor, View/Controller/Entity, numbering, alt block, các bước xác nhận, trả về dữ liệu, hiển thị UI, ...
> - Nếu cần thêm chi tiết cho từng sơ đồ, chỉ cần chỉnh sửa trực tiếp trong file này.
