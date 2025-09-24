# 🧟 Enemy System Setup Guide - Vampire Survivors Style

## 🎯 Overview
Enemy System đã được implement với 4 components chính:
- **EnemyStats** - Health, damage, experience values
- **EnemyMovement** - Follow player, collision avoidance  
- **EnemyController** - AI states, attack logic
- **EnemySpawner** - Spawn management, difficulty scaling

---

## 🏗️ How to Create Enemy Prefab

### Step 1: Create Basic Enemy GameObject
```
1. Create Empty GameObject → "Enemy_Basic"
2. Add components:
   - Rigidbody2D
   - Collider2D (CircleCollider2D hoặc BoxCollider2D)
   - SpriteRenderer
```

### Step 2: Configure Rigidbody2D
```
- Body Type: Dynamic
- Simulated: ✓
- Freeze Rotation Z: ✓ (prevent spinning)
- Linear Drag: 1-2 (for smooth movement)
- Angular Drag: 5+
```

### Step 3: Configure Collider2D
```
- Is Trigger: ✓ (for damage detection)
- Size/Radius: Adjust to sprite size
- Layer: Create "Enemies" layer (Layer 6)
```

### Step 4: Add Enemy Scripts
```
Add these scripts in order:
1. EnemyStats
2. EnemyMovement  
3. EnemyController
```

### Step 5: Configure EnemyStats
```
Base Stats:
- Max Health: 50
- Base Damage: 10
- Move Speed: 2
- Experience Value: 10

Combat:
- Attack Range: 1
- Attack Cooldown: 1.5
```

### Step 6: Configure EnemyMovement
```
Movement:
- Move Speed: 2 (will sync with EnemyStats)
- Stopping Distance: 0.8

Collision Avoidance:
- Avoidance Radius: 1
- Avoidance Force: 2
- Enemy Layer Mask: Enemies (Layer 6)
```

### Step 7: Configure EnemyController
```
AI Settings:
- Detection Range: 8
- Attack Range: 1
- Attack Cooldown: 1.5
```

---

## 🎨 Creating Enemy Data ScriptableObject

### Step 1: Create Enemy Data
```
Right-click Project window → Create → Game → Enemy Data
Name: "EnemyData_Basic"
```

### Step 2: Configure Enemy Data
```
Enemy Info:
- Enemy Name: "Basic Enemy"
- Enemy Type: Basic
- Enemy Sprite: [Your enemy sprite]

Base Stats:
- Max Health: 50
- Movement Speed: 2
- Damage: 10  
- Experience Value: 10
- Spawn Weight: 1

Behavior:
- Detection Range: 8
- Attack Range: 1
- Attack Cooldown: 1.5

Prefab:
- Enemy Prefab: [Drag your enemy prefab here]
```

---

## 🎮 Setting up Enemy Spawner

### Step 1: Create Spawner GameObject
```
Create Empty GameObject → "EnemySpawner"
Add Script: EnemySpawner
```

### Step 2: Configure Spawner
```
Spawning Settings:
- Spawn Rate: 2 (enemies per second)
- Max Enemies: 50
- Spawn Distance: 12
- Despawn Distance: 20

Spawn Area:
- Min Spawn Radius: 10
- Max Spawn Radius: 15

Enemy Types:
- Enemy Types: [Drag your EnemyData assets]
- Spawn Weights: [1, 1, 1...] (equal chance)

Difficulty Scaling:
- Enable Difficulty Scaling: ✓
- Difficulty Increase Rate: 0.1
- Max Difficulty Multiplier: 5
```

---

## 🔧 Integration with Existing Systems

### GameManager Integration
Enemy Spawner tự động:
- Start spawning khi game starts
- Stop spawning khi game over
- Subscribe to EventManager events

### Player Integration  
Enemies tự động:
- Find và follow player
- Deal damage to PlayerStats
- Give experience to PlayerExperience

### Layer Setup
Recommended layers:
```
- Layer 0: Default
- Layer 6: Enemies
- Layer 7: Player
- Layer 8: Projectiles (for future weapons)
```

---

## 🎯 Testing Enemy System

### Debug Controls
```
EnemyController Context Menu:
- Test Attack
- Test Death

EnemySpawner Context Menu:
- Spawn Test Enemy
- Clear All Enemies
```

### Debug Visualization
Select enemy to see:
- Detection range (yellow circle)
- Attack range (red circle)
- Movement/avoidance vectors
- Line to player

Select spawner to see:
- Spawn area (green/yellow circles)
- Despawn area (red circle)

---

## 🚀 Next Steps

After setting up basic enemies:

1. **Create Enemy Variations**:
   - Fast enemies (higher speed, lower health)
   - Heavy enemies (lower speed, higher health)
   - Flying enemies (different movement)

2. **Add Enemy Health Bars** (optional):
   - World space health bars
   - Similar to player health bar

3. **Implement Weapon System**:
   - Now that enemies exist, weapons can target them
   - Projectiles can collide with enemies

4. **Add Drop System**:
   - XP gems on death
   - Health potions
   - Power-up items

---

## 💡 Tips & Best Practices

### Performance
- Use object pooling for frequently spawned enemies
- Limit Physics2D.OverlapCircleAll calls
- Despawn distant enemies automatically

### Balance
- Start with low spawn rates, increase gradually
- Test with different player movement speeds
- Adjust detection/attack ranges based on feel

### Visual Feedback
- Flash red when taking damage (already implemented)
- Add death particle effects
- Screen shake on player damage

---

**Enemy System Status**: ✅ **COMPLETE & READY FOR TESTING**

The enemy system is now fully functional and ready to test with your existing Player System!
