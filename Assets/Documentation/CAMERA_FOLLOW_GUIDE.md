# 🎥 Camera Follow Setup Guide

## 📋 **Phương Pháp 1: Simple Camera Follow (Khuyến Nghị)**

### **Setup:**
```
1. Select Main Camera
2. Add Component → Camera Follow
3. Configure settings trong Inspector
4. Test game
```

### **Settings Gợi Ý:**
```
Vampire Survivors style:
- Offset: (0, 0, -10)
- Follow Speed: 0.1-0.2 (smooth)
- Smooth Follow: ✅

Platformer style:
- Offset: (0, 2, -10)  
- Follow Speed: 0.05-0.1 (slower)

Racing/Fast game:
- Follow Speed: 0.3-0.5 (faster)
- Hoặc Smooth Follow: ❌ (instant)
```

---

## 📋 **Phương Pháp 2: Cinemachine (Advanced)**

### **Cài Đặt Cinemachine:**
```
1. Window → Package Manager
2. Search "Cinemachine" → Install
3. GameObject → Cinemachine → Virtual Camera
4. Set Follow = Player
5. Set Body = "Transposer"
6. Adjust Follow Offset
```

### **Ưu điểm Cinemachine:**
- ✅ Camera shake effects
- ✅ Dead zones và damping
- ✅ Multiple camera blending  
- ✅ Confiner (giới hạn camera trong map)
- ✅ Look ahead prediction

### **Nhược điểm:**
- ❌ Phức tạp hơn cho beginner
- ❌ Package dependency
- ❌ Overhead performance

---

## 📋 **Phương Pháp 3: Manual Parent (Đơn Giản Nhất)**

### **Setup:**
```
1. Drag Main Camera thành child của Player
2. Set Camera Position: (0, 0, -10)
3. ✅ Done!
```

### **Ưu điểm:**
- ✅ Cực kỳ đơn giản
- ✅ Zero scripts, zero setup
- ✅ Perfect sync với Player

### **Nhược điểm:**
- ❌ Không có smooth follow
- ❌ Camera sẽ rotate theo Player
- ❌ Khó custom behaviors

---

## 🎯 **Khuyến Nghị:**

### **Cho Vampire Survivors Clone:**
```
✅ Sử dụng CameraFollow script
✅ Settings: Offset (0,0,-10), Speed 0.125, Smooth ✅
✅ Đơn giản, hiệu quả, dễ customize
```

### **Nếu Cần Advanced Features sau này:**
```
⬆️ Upgrade lên Cinemachine
⬆️ Camera shake, screen effects, multiple cameras
```

---

## 🧪 **Testing:**

### **In Unity Editor:**
```
1. Play Scene
2. Move Player với WASD
3. Camera should follow smoothly
4. Adjust Follow Speed nếu cần
```

### **Debug Tips:**
```
- Nếu camera jerky/giật:
  1. Thử Follow Method = "SmoothDamp"
  2. Hoặc enable "Use Fixed Update"
  3. Hoặc enable "Pixel Perfect" cho 2D games
  4. Giảm Follow Speed xuống 0.05-0.1

- Nếu camera lag: tăng Follow Speed lên 0.3-0.5
- Nếu offset sai: adjust Offset values  
- Check Console có errors không
```

---

## 🔧 **Fix Camera Jitter (Giật):**

### **Nguyên nhân camera giật:**
1. **Frame rate không đồng bộ** - Player movement trong FixedUpdate, Camera trong LateUpdate
2. **Lerp interpolation** - Có thể tạo micro-stuttering
3. **Pixel misalignment** - Đối với 2D pixel art games
4. **Physics timestep mismatch**

### **Solutions (theo thứ tự ưu tiên):**

#### **1. Sử dụng SmoothDamp:**
```
Follow Method: SmoothDamp
Smooth Time: 0.1-0.15 seconds
→ Smooth nhất, ít giật nhất
```

#### **2. Enable Use Fixed Update:**
```
Use Fixed Update: ✅
→ Sync với Player movement physics
```

#### **3. Enable Pixel Perfect (cho 2D):**
```
Pixel Perfect: ✅  
Pixels Per Unit: 16 (hoặc sprite PPU)
→ Fix pixel misalignment
```

#### **4. Điều chỉnh Project Settings:**
```
Edit → Project Settings → Time:
- Fixed Timestep: 0.02 (50 FPS)
- Maximum Allowed Timestep: 0.1

Edit → Project Settings → Quality:
- VSync Count: Every V Blank (hoặc Don't Sync)
```
