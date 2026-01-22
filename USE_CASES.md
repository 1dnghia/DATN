# 📋 USE CASES TỔNG QUÁT - Vampire Survivors Clone

## 🎭 Actors (Tác nhân)

### **1. Player (Người chơi)**
- Người dùng cuối chơi game

### **2. System (Hệ thống)**
- Tự động xử lý logic game

---

## 📊 USE CASES DIAGRAM STRUCTURE

```
┌─────────────────────────────────────────────────────────────┐
│                      VAMPIRE SURVIVORS                      │
├─────────────────────────────────────────────────────────────┤
│                                                             │
│  Player                                                     │
│    │                                                        │
│    ├──[UC01] Quản lý Menu                                 │
│    ├──[UC02] Chuẩn bị game                                │
│    ├──[UC03] Chơi game                                    │
│    ├──[UC04] Quản lý tiến trình                           │
│    └──[UC05] Xem thống kê                                 │
│                                                             │
│  System                                                     │
│    │                                                        │
│    ├──[UC06] Quản lý entities                             │
│    ├──[UC07] Xử lý chiến đấu                              │
│    └──[UC08] Xử lý kết thúc game                          │
│                                                             │
└─────────────────────────────────────────────────────────────┘
```

---

## 📝 CHI TIẾT USE CASES

---

### **UC01: Quản lý Menu**

| **Thuộc tính** | **Chi tiết** |
|----------------|--------------|
| **ID** | UC01 |
| **Tên** | Quản lý Menu |
| **Actor** | Player |
| **Mô tả** | Player điều hướng và tương tác với các menu trong game |
| **Điều kiện tiên quyết** | Game được khởi động |
| **Luồng chính** | 1. Player mở game và vào Main Menu<br>2. Player chọn các options: Start, Achievements, Collection, Settings, Exit<br>3. System điều hướng đến trang tương ứng<br>4. Player có thể quay lại Main Menu |
| **Luồng con** | - **Settings**: Điều chỉnh volume (Music/SFX)<br>- **Achievements**: Xem danh sách thành tích<br>- **Collection**: Xem monsters/weapons đã unlock<br>- **Exit**: Thoát game |
| **Kết quả** | Player điều hướng trong menu thành công |
| **File liên quan** | `MainMenu.cs`, `AchievementManager.cs`, `CollectionManager.cs` |

---

### **UC02: Chuẩn bị game**

| **Thuộc tính** | **Chi tiết** |
|----------------|--------------|
| **ID** | UC02 |
| **Tên** | Chuẩn bị game |
| **Actor** | Player |
| **Mô tả** | Player chọn nhân vật và map trước khi bắt đầu game |
| **Điều kiện tiên quyết** | Ở Main Menu, click "Start" |
| **Luồng chính** | 1. System hiển thị Character Selection<br>2. Player chọn nhân vật (hoặc mua nếu chưa có)<br>3. System hiển thị Map Selection<br>4. Player chọn map đã unlock<br>5. System load game scene |
| **Luồng thay thế** | - **Mua nhân vật**: Nếu chưa sở hữu, trả coins để mở khóa<br>- **Map bị khóa**: Phải hoàn thành map trước để unlock |
| **Kết quả** | Game bắt đầu với nhân vật và map đã chọn |
| **File liên quan** | `CharacterSelector.cs`, `CharacterCard.cs`, `MapSelector.cs`, `MapCard.cs` |

---

### **UC03: Chơi game**

| **Thuộc tính** | **Chi tiết** |
|----------------|--------------|
| **ID** | UC03 |
| **Tên** | Chơi game |
| **Actor** | Player |
| **Mô tả** | Player điều khiển nhân vật, chiến đấu và sinh tồn trong game |
| **Điều kiện tiên quyết** | Đã hoàn thành UC02 (Chuẩn bị game) |
| **Luồng chính** | 1. Player điều khiển nhân vật di chuyển (WASD/Arrow)<br>2. Weapons tự động tấn công quái vật<br>3. Player thu thập EXP gems, coins, items<br>4. Đủ EXP → Level up và chọn upgrade<br>5. Sống sót đủ 10 phút → Boss xuất hiện<br>6. Đánh bại Boss → Chiến thắng |
| **Luồng thay thế** | - **Pause**: Tạm dừng game (Resume/Restart/Exit)<br>- **HP = 0**: Game Over<br>- **Thu thập items**: Health potion, Magnet, Bomb, Chest |
| **Kết quả** | Player chiến đấu và tiến triển trong game |
| **File liên quan** | `Character.cs`, `Monster.cs`, `Ability.cs`, `Collectable.cs`, `PauseMenu.cs` |

---

### **UC04: Quản lý tiến trình**

| **Thuộc tính** | **Chi tiết** |
|----------------|--------------|
| **ID** | UC04 |
| **Tên** | Quản lý tiến trình |
| **Actor** | Player |
| **Mô tả** | Player nâng cấp nhân vật qua việc chọn upgrades khi level up |
| **Điều kiện tiên quyết** | Đủ EXP để level up |
| **Luồng chính** | 1. System tự động pause khi player level up<br>2. System hiển thị 3 upgrade cards ngẫu nhiên<br>3. Player chọn 1 upgrade (Damage, Health, Speed, Armor, etc.)<br>4. System áp dụng upgrade<br>5. Game tiếp tục |
| **Luồng thay thế** | - **Không còn upgrades**: Spawn chest thay thế |
| **Kết quả** | Stats của player được tăng cường |
| **File liên quan** | `AbilitySelectionDialog.cs`, `AbilityManager.cs`, `UpgradeCard.cs` |

---

### **UC05: Xem thống kê**

| **Thuộc tính** | **Chi tiết** |
|----------------|--------------|
| **ID** | UC05 |
| **Tên** | Xem thống kê |
| **Actor** | Player |
| **Mô tả** | Player xem thống kê khi game kết thúc (Victory/Game Over) |
| **Điều kiện tiên quyết** | Game kết thúc |
| **Luồng chính** | 1. System hiển thị Game Over Dialog<br>2. System hiển thị stats: Coins gained, Enemies killed, Damage dealt, Damage taken<br>3. Player chọn: Restart hoặc Return to Main Menu |
| **Luồng thay thế** | - **Victory**: Hiển thị "Victory", unlock map tiếp theo<br>- **Defeat**: Hiển thị "Defeat" |
| **Kết quả** | Player xem kết quả game và stats |
| **File liên quan** | `GameOverDialog.cs`, `StatsManager.cs`, `LevelManager.cs` |

---

### **UC06: Quản lý entities**

| **Thuộc tính** | **Chi tiết** |
|----------------|--------------|
| **ID** | UC06 |
| **Tên** | Quản lý entities |
| **Actor** | System |
| **Mô tả** | System tự động spawn và quản lý quái vật, boss, chests |
| **Điều kiện tiên quyết** | Game đang chơi |
| **Luồng chính** | 1. System spawn quái vật theo waves (difficulty curve)<br>2. Quái vật tự động di chuyển về player<br>3. System spawn chests định kỳ<br>4. Đủ 10 phút: spawn Final Boss<br>5. Spawn Mini Boss theo timer |
| **Luồng thay thế** | - System track monsters/weapons vào Collection |
| **Kết quả** | Quái vật, boss, chests được spawn |
| **File liên quan** | `EntityManager.cs`, `MonsterPool.cs`, `Chest.cs`, `GameTimer.cs` |

---

### **UC07: Xử lý chiến đấu**

| **Thuộc tính** | **Chi tiết** |
|----------------|--------------|
| **ID** | UC07 |
| **Tên** | Xử lý chiến đấu |
| **Actor** | System |
| **Mô tả** | System xử lý combat logic giữa player và monsters |
| **Điều kiện tiên quyết** | Game đang chơi, có entities tồn tại |
| **Luồng chính** | 1. Weapons của player tự động tấn công<br>2. System tính toán damage và knockback<br>3. Monster chết → drop EXP gems/coins<br>4. Monster chạm player → player nhận damage<br>5. System áp dụng Map Debuffs |
| **Luồng thay thế** | - System cập nhật stats (damage dealt, damage taken, kills) |
| **Kết quả** | Combat được xử lý, entities nhận damage/chết |
| **File liên quan** | `Monster.cs`, `Character.cs`, `Ability.cs`, `MapBlueprint.cs` |

---

### **UC08: Xử lý kết thúc game**

| **Thuộc tính** | **Chi tiết** |
|----------------|--------------|
| **ID** | UC08 |
| **Tên** | Xử lý kết thúc game |
| **Actor** | System |
| **Mô tả** | System xử lý logic khi game kết thúc (thắng/thua) |
| **Điều kiện tiên quyết** | Player HP = 0 HOẶC Boss bị giết |
| **Luồng chính** | 1. System detect điều kiện kết thúc<br>2. System pause game (Time.timeScale = 0)<br>3. System check và unlock achievements<br>4. System cộng coins và lưu vào PlayerPrefs<br>5. System unlock map tiếp theo (nếu thắng)<br>6. System hiển thị Game Over Dialog |
| **Luồng thay thế** | - **Victory**: Phát nhạc chiến thắng, lưu map completion<br>- **Defeat**: Phát nhạc thất bại |
| **Kết quả** | Game kết thúc, dữ liệu được lưu |
| **File liên quan** | `LevelManager.cs`, `AchievementManager.cs`, `GameOverDialog.cs` |

---

## 🔗 QUAN HỆ GIỮA CÁC USE CASES

### **Include Relationships**
- **UC02 (Chuẩn bị game)** includes:
  - Chọn nhân vật
  - Mua nhân vật (nếu cần)
  - Chọn map

- **UC03 (Chơi game)** includes:
  - Điều khiển nhân vật
  - Chiến đấu
  - Thu thập items
  - Pause game

- **UC08 (Xử lý kết thúc)** includes:
  - Lưu tiến trình (coins, map completion)
  - Check achievements
  - Track collection

### **Extend Relationships**
- **UC04 (Quản lý tiến trình)** extends **UC03 (Chơi game)** khi level up
- **UC05 (Xem thống kê)** extends **UC03 (Chơi game)** khi kết thúc

### **Dependencies**
- UC03 → UC02: Phải chuẩn bị game trước khi chơi
- UC04 → UC03: Level up xảy ra trong lúc chơi
- UC05 → UC03: Xem stats sau khi game kết thúc
- UC08 → UC03: Xử lý kết thúc dựa trên trạng thái game

---

## 📊 TỔNG KẾT

- **Tổng số Use Cases:** 8 (tổng quát)
- **Use Cases của Player:** 5 (UC01-UC05)
- **Use Cases của System:** 3 (UC06-UC08)
- **Include relationships:** 3 groups
- **Extend relationships:** 2
- **Dependencies:** 4

---

## 🎯 ĐẶC ĐIỂM

✅ **Tập trung vào chức năng chính**  
✅ **Dễ vẽ sơ đồ Use Case UML**  
✅ **Bao quát toàn bộ game flow**  
✅ **Phù hợp cho tài liệu phân tích**

---

**Lưu ý:** Các use case này đã được rút gọn từ 23 use cases chi tiết thành 8 use cases tổng quát, phù hợp cho sơ đồ Use Case Diagram.
