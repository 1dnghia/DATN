# Vampire Survivors Clo## 📖 Project Overview
A **Vampire Survivors** clone built with Unity 2022.3+. This is a bullet hell survival roguelike featuring automatic combat mechanics and progressive upgrade systems.

## 🎮 Core Gameplay Featuresoject

## � Tech Stack & Framework Usage

**⚠️ IMPORTANT: When coding for this project, ALWAYS use these frameworks:**

### Core Frameworks
- **DOTween** - All animations, tweens, UI transitions (NO Unity Animation)
- **Cinemachine** - All camera management and transitions  
- **TextMeshPro** - All text display (NO Unity Text)
- **Input System** - All input handling (NO old Input Manager)
- **VContainer** - Dependency injection for all services/managers
- **Addressables** - Asset loading for large/dynamic content
- **NaughtyAttributes** - Inspector organization and conditional fields
- **Post Processing** - Visual effects and image enhancement

### Architecture Rules
- **PersistentScene Pattern** - Managers live in persistent scene, NO DontDestroyOnLoad
- **Singleton Pattern** - For managers with Instance property
- **Additive Scene Loading** - Keep PersistentScene, load others additively

---

## �📖 Mô tả dự án
Đây là một clone của game **Vampire Survivors** được phát triển bằng Unity 2022.3+. Game là một bullet hell survival roguelike với cơ chế tự động tấn công và hệ thống nâng cấp dần tiến.

## 📚 Documentation

## 🎮 Gameplay Core Features
- **Movement-only control**: Player only controls movement, no attack buttons
- **Auto attack system**: Weapons automatically target nearest enemies
- **Survival gameplay**: Survive as long as possible against endless waves
- **Progressive leveling**: Gain EXP from enemies → Level up → Choose upgrades
- **Wave-based spawning**: Enemy difficulty and quantity increase over time
- **Power scaling**: Both player and weapons become stronger with levels

## 🏗️ Project Architecture

### 📁 Scripts Structure:
```
Assets/Scripts/
├── Player/              # Player character logic
│   ├── PlayerController.cs      ✅ Main coordinator with auto-assignment
│   ├── PlayerMovement.cs        ✅ Physics-based movement (WASD + mobile)
│   ├── PlayerStats.cs           ✅ Health system with events
│   ├── PlayerExperience.cs      ✅ XP system with progressive scaling
│   └── PlayerAnimationController.cs ✅ Animation control
├── Enemy/               # Enemy AI and behavior
│   ├── EnemyController.cs       � In development
│   ├── EnemyMovement.cs         � In development
│   ├── EnemyStats.cs            � In development
│   └── EnemySpawner.cs          � In development
├── Weapons/             # Auto-attack weapon systems
│   ├── WeaponBase.cs            � In development
│   ├── ProjectileWeapon.cs      � In development
│   ├── MeleeWeapon.cs           � In development
│   ├── WeaponManager.cs         � In development
│   └── Projectile.cs            � In development
├── Managers/            # Core system managers
│   ├── GameManager.cs           ✅ Game state management
│   ├── SceneManager.cs          ✅ Scene loading system
│   ├── LoadingManager.cs        ✅ Loading UI management
│   └── SceneGameManager.cs      ✅ Game session management
├── UI/                  # User interface
│   ├── ExperienceBarUI.cs       ✅ XP bar with DOTween animations
│   ├── ScreenSpaceHealthBar.cs  ✅ Health bar with follow mechanics
│   └── SimplePlayerUIManager.cs ✅ UI coordination
└── Input/               # Input handling
    └── TouchInput.cs            ✅ Cross-platform input system
```

## 🎯 Input System
**Simple and effective input handling:**

### PC Controls:
```csharp
// WASD or Arrow Keys
moveInput.x = Input.GetAxis("Horizontal");  // A/D, Arrow Left/Right
moveInput.y = Input.GetAxis("Vertical");    // W/S, Arrow Up/Down

// Pause game
if (Input.GetKeyDown(KeyCode.Escape))
    PauseGame();
```

### Mobile Controls:
- **TouchInput.cs**: Virtual joystick with UI drag & drop
- **Cross-platform**: Auto-detects mobile and shows joystick
- **Static access**: `TouchInput.MoveInput` - no references needed

## � Development Status

### ✅ Completed Systems:
1. **Project Architecture** - Clean folder structure and organization
2. **Player System** - Complete player controller with stats and experience
3. **Input System** - Cross-platform movement controls
4. **UI System** - Health bars, XP bars with DOTween animations
5. **Manager System** - Game, Scene, and Loading managers
6. **Event Architecture** - Event-driven communication between systems

### � In Development:
1. **Enemy System** - AI, spawning, health/damage mechanics
2. **Weapon System** - Auto-attack, projectiles, upgrade system
3. **Item System** - Pickups, experience gems, power-ups
4. **Wave Management** - Progressive difficulty scaling
5. **Audio System** - Sound effects and background music

### 📋 Planned Features:
1. **Upgrade System** - Level up choices and weapon evolution
2. **Save System** - Game progress persistence
3. **Menu System** - Main menu and settings
4. **Polish** - Particle effects, screen shake, game juice

## 🛠️ Technical Stack
- **Unity 2022.3+ LTS** - Core engine
- **2D URP** - Universal Render Pipeline for 2D
- **C# Scripting** - Primary programming language
- **Traditional Input Manager** - Simple and authentic input handling
- **Event-driven Architecture** - Decoupled system communication
- **ScriptableObjects** - Data-driven configuration

## 🎯 Design Philosophy
- **Authentic gameplay**: Stay true to Vampire Survivors mechanics
- **Simple controls**: Movement-only, no complex input schemes
- **Progressive difficulty**: Balanced scaling for long-term engagement
- **Cross-platform ready**: Optimized for both PC and mobile
- **Clean codebase**: Maintainable and extensible architecture
- **Performance focused**: Efficient systems for hundreds of entities

---
**Last Updated**: October 9, 2025  
**Current Focus**: Enemy system implementation  
**Status**: Core player systems complete, expanding gameplay mechanics
