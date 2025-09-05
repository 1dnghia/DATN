# 📚 Vampire Survivors Clone - Documentation

Thư mục này chứa tài liệu hướng dẫn cho dự án Vampire Survivors Clone.

---

## 📋 Essential Guides

### 🚀 Complete Setup
1. **[UNITY_SETUP_COMPLETE_GUIDE.md](./UNITY_SETUP_COMPLETE_GUIDE.md)** - Hướng dẫn setup Unity hoàn chỉnh (từ A→Z)
2. **[UNITY_SETUP_CHECKLIST.md](./UNITY_SETUP_CHECKLIST.md)** - Checklist nhanh để theo dõi tiến độ

### 🎮 System Guides  
3. **[PLAYER_SETUP_GUIDE.md](./PLAYER_SETUP_GUIDE.md)** - Player system chi tiết
4. **[UI_SETUP_WITH_CUSTOM_ASSETS.md](./UI_SETUP_WITH_CUSTOM_ASSETS.md)** - UI với custom sprites ⭐
5. **[SIMPLE_UI_SETUP_GUIDE.md](./SIMPLE_UI_SETUP_GUIDE.md)** - UI với Unity default sprites

---

## 🎯 Hướng dẫn sử dụng

### 👶 Người mới bắt đầu:
1. **Đọc** `UNITY_SETUP_COMPLETE_GUIDE.md` - Setup từ đầu (có GameManager)
2. **Theo dõi** `UNITY_SETUP_CHECKLIST.md` - Đánh dấu tiến độ  
3. **Test** với debug keys: WASD, H, J, K, ESC, R

### 👨‍💻 Người có kinh nghiệm:
1. **Scan** `UNITY_SETUP_CHECKLIST.md` - Tổng quan nhanh
2. **Tham khảo** `UI_SETUP_WITH_CUSTOM_ASSETS.md` - Nếu có custom sprites ⭐
3. **Xem** `PLAYER_SETUP_GUIDE.md` - System details

### 🎨 Có custom UI assets:
1. **Dùng** `UI_SETUP_WITH_CUSTOM_ASSETS.md` - Setup với sprites đẹp
2. **Import assets** đúng settings (Sprite 2D UI, Pixels Per Unit)
3. **9-slice** nếu cần stretch, **Color tinting** cho variations

---

## 📁 Project Structure

```
Assets/
├── Documentation/          ← 4 files đơn giản
│   ├── README.md          ← File này
│   ├── Complete Guide     ← Setup từ A→Z
│   ├── Checklist         ← Track progress  
│   └── System Guides     ← Chi tiết systems
│
├── Scripts/
│   ├── Core/              ← GameManager (1 file)
│   ├── Events/            ← EventManager (1 file)
│   ├── Player/            ← Player systems (5 files)
│   └── UI/                ← UI components (3 files)
│
├── Prefabs/               ← Game objects
└── Scenes/                ← Game scenes
```

---

## ⚡ Quick Debug

### Debug Keys (sau khi setup):
- **WASD**: Di chuyển player
- **H**: Damage (-20 HP)
- **J**: Heal (+20 HP)  
- **K**: Gain XP (+50)
- **ESC**: Pause/Resume
- **R**: Restart game

### Expected Results:
- ✅ Player di chuyển mượt
- ✅ Health bar follow player, đổi màu
- ✅ XP bar full width, level up
- ✅ Pause/resume working
- ✅ Game over → restart

---

## 🆘 Common Issues

### Scripts missing:
```Fix: Copy đúng code, check tên file```

### UI không hiển thị:
```Fix: Check Canvas = Screen Space Overlay```  

### Player không di chuyển:
```Fix: Check Rigidbody2D, Gravity Scale = 0```

### Events không work:
```Fix: Check GameObjects có scripts attached```

---

## 🚀 Next Phase

Sau khi hoàn thành Player setup:
- **Enemy System** - Spawning, AI, combat
- **Weapon System** - Auto-attack, projectiles  
- **Upgrade System** - Level up choices

**Ready to build a Vampire Survivors clone! 🎮**
