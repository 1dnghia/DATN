# 🗺️ Infinite Map Setup Guide - Vampire Survivors Style

## 🎯 **Tổng Quan**

Infinite Map System tạo cảm giác thế giới không giới hạn cho game Vampire Survivors style bằng cách:
- **Tile Streaming**: Tạo và xóa background tiles xung quanh player
- **Object Pooling**: Tối ưu performance với tile reuse
- **Parallax Effects**: Tạo depth cho background layers
- **Seamless Transition**: Player không bao giờ thấy edge của map

---

## 🏗️ **Method 1: Basic Infinite Tilemap (Khuyến Nghị)**

### **Step 1: Create Tile Prefabs**
```
1. Create Empty GameObject → "BackgroundTile"
2. Add Component → SpriteRenderer
3. Add Component → BackgroundTile
4. Configure sprite và settings
5. Save as Prefab
```

**BackgroundTile Settings:**
```
Auto Configure Sprite: ✅
Sprite Variations: [Drag multiple grass sprites]
Sorting Order: -10
Sorting Layer Name: "Background"
```

### **Step 2: Setup Infinite Map Manager**
```
1. Create Empty GameObject → "InfiniteMapManager" 
2. Add Component → InfiniteMapManager
3. Configure settings trong Inspector
```

**InfiniteMapManager Settings:**
```
Map Settings:
- Tile Size: 20 (Unity units)
- Render Distance: 3 (7x7 grid around player)
- Unload Distance: 50

Tile Prefabs: [Drag your BackgroundTile prefabs]
Tile Weights: [1, 1, 1...] (equal chance)

Performance:
- Tiles Per Frame: 2 (tránh lag)
- Use Object Pooling: ✅

Debug:
- Show Debug Gizmos: ✅ (để test)
- Log Tile Operations: ❌ (sau khi test)
```

### **Step 3: Test Basic System**
```
1. Play Scene
2. Move Player around
3. Check Console for errors
4. Observe tiles being created/destroyed in Scene view
5. Green wireframes = render area
6. Yellow spheres = active tiles
```

---

## 🌄 **Method 2: Advanced Parallax Background**

### **Step 1: Create Background Layers**
```
Layer Structure:
- Background_Far (parallax = 0.1)
  - Mountains, clouds, distant objects
- Background_Mid (parallax = 0.3) 
  - Trees, buildings
- Background_Near (parallax = 0.7)
  - Grass, rocks, close objects
```

### **Step 2: Setup Parallax Manager**
```
1. Create Empty GameObject → "ParallaxManager"
2. Add Component → ParallaxBackground
3. Create child GameObjects for each layer
4. Configure layer settings
```

**ParallaxBackground Settings:**
```
Parallax Layers:
- Layer Transform: [Background_Far]
  - Parallax Speed: 0.1
  - Infinite Scroll: ✅
  - Texture Width: 40
  - Texture Height: 20
  
- Layer Transform: [Background_Mid]  
  - Parallax Speed: 0.3
  - Infinite Scroll: ✅
  - Texture Width: 30
  
- Layer Transform: [Background_Near]
  - Parallax Speed: 0.7
  - Infinite Scroll: ✅
  - Texture Width: 25
```

---

## 🎨 **Creating Background Art Assets**

### **Seamless Tiles Requirements:**
```
✅ Square textures (512x512, 1024x1024)
✅ Seamless edges (tile perfectly)
✅ Consistent lighting/style
✅ Multiple variations (3-5 per type)
```

### **Texture Import Settings:**
```
Texture Type: Sprite (2D and UI)
Sprite Mode: Single
Pixels Per Unit: 100 (hoặc match game PPU)
Filter Mode: Point (cho pixel art) hoặc Bilinear
Compression: None (quality) hoặc Low Quality (performance)
```

### **Creating Variations:**
```
Grass Tiles:
- grass_01.png (cỏ cơ bản)
- grass_02.png (có hoa)  
- grass_03.png (có đá)
- grass_04.png (có bụi cây)

Stone Tiles:
- stone_01.png
- stone_02.png
- stone_03.png
```

---

## 🔧 **Integration với Existing Systems**

### **Camera Integration**
```
InfiniteMapManager tự động:
- Find Player reference
- Track player movement
- Update tiles based on camera position
- Work với existing CameraFollow system
```

### **Performance Integration**
```
Object Pooling:
- Reuse tiles instead of Instantiate/Destroy
- Pool warm-up on game start
- Automatic cleanup

Tile Streaming:
- Only render tiles around player
- Unload distant tiles automatically
- Process limited tiles per frame
```

---

## 🎮 **Setup Priority Order**

### **For Vampire Survivors Clone:**
```
1️⃣ Setup CameraFollow system TRƯỚC
2️⃣ Create 3-5 grass tile variations
3️⃣ Setup InfiniteMapManager với basic tiles
4️⃣ Test movement và tile streaming
5️⃣ Add ParallaxBackground nếu muốn depth
6️⃣ Optimize settings cho mobile
```

---

## 🚀 **Testing & Debugging**

### **Debug Visualizations**
```
InfiniteMapManager Gizmos:
- Green wireframes: Render area around player
- Red wireframe: Current player tile
- Yellow spheres: Active tiles
- Check tile creation/destruction in Scene view
```

### **Performance Testing**
```
Stats to Monitor:
- Active Tile Count (should stay around 49 for 7x7 grid)
- Pool Size (should grow và reuse tiles)
- Frame Rate (should stay stable)

Profile Window:
- Check memory allocation
- Monitor Instantiate/Destroy calls
- Texture memory usage
```

### **Common Issues:**
```
❌ Tiles not appearing:
  ✅ Check Tile Prefabs assigned
  ✅ Check Player reference found
  ✅ Check Tile Size settings

❌ Performance lag:
  ✅ Reduce Render Distance
  ✅ Enable Object Pooling
  ✅ Lower Tiles Per Frame
  ✅ Optimize tile prefabs

❌ Seams between tiles:
  ✅ Check texture seamless edges
  ✅ Ensure exact Tile Size match
  ✅ Check sprite import PPU settings
```

---

## 💡 **Advanced Features (Optional)**

### **Biome System:**
```cs
// Different tile sets cho different areas
public BiomeData[] biomes;
// Choose tiles based on player position
```

### **Dynamic Loading:**
```cs  
// Load different environments
// Based on player progression
// Boss areas, different regions
```

### **Animated Tiles:**
```cs
// Water animation
// Wind effects on grass
// Particle systems on tiles
```

---

## 🎯 **Recommended Settings**

### **For Mobile Performance:**
```
Tile Size: 20-30 units
Render Distance: 2-3 (5x5 to 7x7 grid)  
Tiles Per Frame: 1-2
Object Pooling: ✅ Always
Debug Gizmos: ❌ (release build)
```

### **For Desktop Performance:**
```
Tile Size: 15-25 units  
Render Distance: 3-4 (7x7 to 9x9 grid)
Tiles Per Frame: 2-4
Object Pooling: ✅
Parallax Layers: 2-4 layers OK
```

---

## 🏁 **Next Steps After Setup**

1. ✅ **Test infinite scrolling** - Player đi mọi hướng không thấy edge
2. ✅ **Optimize performance** - Smooth 60fps trên target device  
3. ✅ **Add tile variations** - Tránh repetitive appearance
4. ✅ **Implement biomes** - Different areas, environments
5. ✅ **Add parallax depth** - Multiple background layers
6. ✅ **Mobile optimization** - Reduce draw calls, texture size

**Infinite Map System sẽ tạo cảm giác thế giới rộng lớn cho Vampire Survivors game! 🌍**