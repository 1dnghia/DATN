# 🎮 Vampire Survivors Clone - Game Documentation

> **Version:** 1.0  
> **Last Updated:** November 18, 2025  
> **Engine:** Unity 2022+

---

## 📋 Mục lục

### 🎯 Core Gameplay
1. [Tổng quan Game](#-1-tổng-quan-game)
2. [Cơ chế chiến thắng](#-2-cơ-chế-chiến-thắng)
3. [Hệ thống vũ khí](#-3-hệ-thống-vũ-khí)

### 💻 Technical
4. [Kiến trúc Code](#-4-kiến-trúc-code)
5. [UI System](#-5-ui-system)
6. [Hệ thống lưu trữ](#-6-hệ-thống-lưu-trữ)

### 🔧 Systems
7. [Background System](#-7-background-system)
8. [Entity Management](#-8-entity-management)
9. [Character System](#-9-character-system)

### 🛠️ Tools & Others
10. [Testing Tools](#-10-testing-tools)
11. [Localization](#-11-localization-system)
12. [Performance](#-12-performance-optimization)
13. [Known Issues](#-13-known-issues--limitations)
14. [Future Plans](#-14-future-improvements)

---

```
╔═══════════════════════════════════════════════════════════╗
║                                                           ║
║              🎯 CORE GAMEPLAY MECHANICS                   ║
║                                                           ║
╚═══════════════════════════════════════════════════════════╝
```

## 🎮 1. Tổng quan Game

### Thể loại
- **Roguelike Survivor Game** - Clone của Vampire Survivors
- Gameplay: Sinh tồn, thu thập EXP, nâng cấp vũ khí, đánh boss

### Tech Stack
- **Engine**: Unity
- **Language**: C#
- **UI**: Unity UGUI
- **Localization**: Unity Localization Package

---

## 🏆 2. Cơ chế chiến thắng

### Điều kiện thắng
```csharp
// File: LevelManager.cs

// 1. Game có thời gian giới hạn (mặc định 600 giây = 10 phút)
public float levelTime = 600;

// 2. Khi hết thời gian → Final Boss xuất hiện
if (!finalBossSpawned && levelTime > levelBlueprint.levelTime)
{
    finalBossSpawned = true;
    Monster finalBoss = entityManager.SpawnMonsterRandomPosition(...);
    finalBoss.OnKilled.AddListener(LevelPassed);  // ← Điều kiện thắng
}

// 3. Khi Final Boss chết → Người chơi thắng
public void LevelPassed(Monster finalBossKilled)
{
    Time.timeScale = 0;
    int coinCount = PlayerPrefs.GetInt("Coins");
    PlayerPrefs.SetInt("Coins", coinCount + statsManager.CoinsGained);
    gameOverDialog.Open(true, statsManager);  // true = Victory
}
```

### Điều kiện thua
```csharp
// Khi nhân vật chính chết
public void GameOver()
{
    Time.timeScale = 0;
    int coinCount = PlayerPrefs.GetInt("Coins");
    PlayerPrefs.SetInt("Coins", coinCount + statsManager.CoinsGained);
    gameOverDialog.Open(false, statsManager);  // false = Defeat
}
```

### Timeline Game
```
0:00 - Game bắt đầu
  ↓
  ├─ Spawn quái liên tục (theo MonsterSpawnTable)
  ├─ Player thu EXP, level up, chọn upgrade
  ├─ Spawn Mini Boss (thời điểm được định trước)
  ↓
10:00 - Hết thời gian
  ↓
  └─ Final Boss xuất hiện → Giết Boss = Thắng
```

---

## ⚔️ 3. Hệ thống vũ khí

### Phân loại vũ khí

#### 1. Projectile Weapons (3)
```
- Bazooka Gun: Súng phóng rocket mạnh
- Machine Gun: Súng liên thanh tốc độ cao
- Shuriken: Phi tiêu ninja
```

#### 2. Melee Weapons (3)
```
- Bat: Vũ khí cận chiến cơ bản
- Dagger: Dao găm có hiệu ứng chảy máu (bleed)
- Fixed Direction Stab: Đâm theo hướng cố định
```

#### 3. Boomerang Weapons (3)
```
- Boomerang: Boomerang cơ bản
- Lightsaber: Kiếm laser
- Machete: Dao rựa lớn
```

#### 4. Throwable Weapons (2)
```
- Grenade: Lựu đạn nổ
- Molotov: Bom xăng tạo lửa
```

#### 5. Proximity Weapons (1)
```
- Book: Sách quay xung quanh nhân vật
```

#### 6. Healing Abilities (2)
```
- Lifesteal: Hút máu từ kẻ địch
- Recovery: Hồi máu theo thời gian
```

#### 7. Upgrade Abilities (8)
```
- AOE Upgrade: Tăng phạm vi
- Armor Upgrade: Tăng giáp
- Cooldown Upgrade: Giảm thời gian hồi chiêu
- Damage Upgrade: Tăng sát thương
- Ice Skate: Tăng tốc độ di chuyển
- Knockback Upgrade: Tăng đẩy lùi
- Projectile Count Upgrade: Tăng số đạn
- Projectile Speed Upgrade: Tăng tốc độ đạn
```

### Cơ chế Upgrade
```csharp
// File: Ability.cs

// Mỗi ability có level và maxLevel
protected int level = 0;
protected int maxLevel;

// Khi chọn ability
public virtual void Select()
{
    if (!owned)
    {
        owned = true;
        Use();  // Lần đầu: Kích hoạt ability
    }
    else
    {
        Upgrade();  // Lần sau: Nâng cấp ability
    }
    level++;
}

// Upgrade tăng các thông số
protected virtual void Upgrade()
{
    upgradeableValues.ForEach(x => x.Upgrade());
}
```

### **KHÔNG CÓ** cơ chế Evolution/Combine
- Không như Vampire Survivors gốc
- Không có việc kết hợp 2 vũ khí để tạo vũ khí mới
- Chỉ có nâng cấp từng ability độc lập

### Requirements System
```csharp
// Một số upgrade chỉ xuất hiện khi có ability tương ứng

// Ví dụ: Cooldown Upgrade
public override bool RequirementsMet()
{
    bool baseRequirementsMet = base.RequirementsMet();
    bool cooldownAbilitiesInUse = abilityManager.WeaponCooldownUpgradeablesCount > 0;
    return baseRequirementsMet && cooldownAbilitiesInUse;
}
```

---

```
╔═══════════════════════════════════════════════════════════╗
║                                                           ║
║              💻 TECHNICAL ARCHITECTURE                    ║
║                                                           ║
╚═══════════════════════════════════════════════════════════╝
```

## 🏗️ 4. Kiến trúc Code

### Core Scripts Structure
```
Assets/Scripts/
├── Character/
│   ├── Character.cs              # Player base class
│   ├── MainCharacter.cs          # Main player controller
│   └── Abilities/
│       ├── Ability.cs            # Base ability class
│       ├── AbilityManager.cs     # Quản lý abilities
│       ├── ProjectileAbility.cs
│       ├── MeleeAbility.cs
│       └── [Các ability cụ thể...]
│
├── Gameplay/
│   ├── LevelManager.cs           # Quản lý level, win/lose
│   ├── EntityManager.cs          # Quản lý spawn entities
│   ├── GameTimer.cs              # Hiển thị thời gian
│   ├── StatsManager.cs           # Thống kê game
│   └── Inventory/
│       └── Inventory.cs          # Quản lý items
│
├── Monsters/
│   ├── Monster.cs                # Base monster class
│   ├── BossMonster.cs            # Boss base class
│   └── [Các loại monster...]
│
├── UI/
│   ├── AbilitySelectionDialog.cs # Dialog chọn ability khi level up
│   ├── GameOverDialog.cs         # Dialog kết thúc game
│   ├── PauseMenu.cs              # Menu pause
│   └── DialogBox.cs              # Base dialog class
│
├── Main Menu/
│   ├── MainMenu.cs               # Main menu UI manager
│   ├── CharacterSelector.cs      # Chọn nhân vật
│   ├── CharacterCard.cs          # Card nhân vật
│   └── PageManager.cs            # Quản lý pages
│
├── ScriptableObjects/
│   ├── LevelBlueprint.cs         # Config level
│   ├── CharacterBlueprint.cs     # Config character
│   ├── MonsterBlueprint.cs       # Config monster
│   └── [Các blueprint khác...]
│
└── Utilities/
    ├── InfiniteBackground.cs     # Background vô tận
    ├── UpgradeableValues.cs      # Hệ thống upgrade
    └── [Utilities khác...]
```

### Design Patterns Sử dụng

#### 1. Object Pooling
```csharp
// File: EntityManager.cs
// Quản lý pool của monsters, projectiles, collectables

private ObjectPool<Monster> monsterPool;
private ObjectPool<Projectile> projectilePool;
```

#### 2. Observer Pattern
```csharp
// Events cho các hành động game
public UnityEvent OnDeath;
public UnityEvent OnLevelUp;
public UnityEvent<float> OnDealDamage;

// Subscribe
character.OnDeath.AddListener(GameOver);
finalBoss.OnKilled.AddListener(LevelPassed);
```

#### 3. Strategy Pattern
```csharp
// Các ability khác nhau extend từ base class
public abstract class Ability : MonoBehaviour
{
    public abstract void Use();
    public abstract void Upgrade();
}
```

#### 4. Factory Pattern
```csharp
// EntityManager spawn entities theo blueprint
public Monster SpawnMonster(MonsterBlueprint blueprint)
{
    Monster monster = monsterPool.Get();
    monster.Init(blueprint);
    return monster;
}
```

---

## 🖥️ 5. UI System

### Main Menu Navigation

#### Cấu trúc UI
```
Canvas
├── MainMenuPage
│   ├── Button Start
│   ├── Button Achievements
│   ├── Button Upgrades
│   ├── Button Collection
│   ├── Button Settings
│   └── Button Exit
│
├── CharacterSelectionPage
│   ├── Character Cards
│   └── Button Back
│
├── AchievementsPage
│   └── Button Back
│
├── UpgradesPage
│   └── Button Back
│
├── CollectionPage
│   └── Button Back
│
└── SettingsPage
    └── Button Back
```

#### MainMenu.cs - Button-Page Mapping
```csharp
[System.Serializable]
public struct ButtonPageMapping
{
    public Button button;      // Button trong Main Menu
    public GameObject page;    // Page tương ứng
}

// Danh sách mappings
[SerializeField] private List<ButtonPageMapping> buttonPageMappings;

// Khi click button → Show page tương ứng
mapping.button.onClick.AddListener(() => ShowPage(mapping.page));
```

#### Navigation Flow
```
Main Menu → Click "Start" → Character Selection
                              ↓
                         Select Character → Load Game Scene
                         
Main Menu → Click "Settings" → Settings Page → Click "Back" → Main Menu
Main Menu → Click "Exit" → Quit Game
```

### In-Game UI

#### HUD Elements
```
- Health Bar: Hiển thị HP
- Experience Bar: Hiển thị EXP và level
- Game Timer: Đếm thời gian
- Coin Counter: Số coins hiện tại
- Stats Panel: Kill count, damage dealt
```

#### Ability Selection Dialog
```csharp
// Khi level up → Hiện dialog chọn ability
public class AbilitySelectionDialog : DialogBox
{
    // Chọn 3-4 abilities ngẫu nhiên
    List<Ability> selectedAbilities = abilityManager.SelectAbilities();
    
    // Hiển thị cards
    foreach (var ability in selectedAbilities)
    {
        AbilityCard card = CreateCard(ability);
    }
}
```

#### Ability Selection Logic
```csharp
// File: AbilityManager.cs

// Xác suất có 4 options thay vì 3
private float FourthChance() => 1 - 1/playerCharacter.Luck;

// Xác suất hiện ability đã có (để upgrade)
private float OwnedChance()
{
    float x = playerCharacter.CurrentLevel % 2 == 0 ? 2 : 1;
    return 1 + 0.3f*x - 1/playerCharacter.Luck;
}

// Chọn abilities theo trọng số (rarity)
public enum Rarity
{
    Common = 50,
    Uncommon = 25,
    Rare = 15,
    Legendary = 9,
    Exotic = 1
}
```

---

## 💾 6. Hệ thống lưu trữ

### Phương pháp hiện tại: PlayerPrefs

#### Dữ liệu được lưu
```csharp
// 1. Coins (Tiền xu)
PlayerPrefs.GetInt("Coins");
PlayerPrefs.SetInt("Coins", value);

// 2. Character owned status (Chỉ runtime)
characterBlueprint.owned = true;  // Không persist khi tắt game
```

#### Vị trí lưu/load
```csharp
// CharacterCard.cs - Mua nhân vật
int coinCount = PlayerPrefs.GetInt("Coins");
PlayerPrefs.SetInt("Coins", coinCount - characterBlueprint.cost);

// LevelManager.cs - Kết thúc game
PlayerPrefs.SetInt("Coins", coinCount + statsManager.CoinsGained);

// CoinDisplay.cs - Hiển thị
coinText.text = PlayerPrefs.GetInt("Coins").ToString();
```

### Vấn đề của hệ thống hiện tại

❌ **Character owned không persist** - Tắt game là mất  
❌ **Chỉ lưu coins** - Không lưu achievements, upgrades, collection  
❌ **Không an toàn** - PlayerPrefs dễ cheat, dễ mất dữ liệu  
❌ **Không có SaveGame system** - Không lưu progress  
❌ **Không có backup/cloud save**

### Đề xuất cải thiện

#### SaveManager System (Nên implement)
```csharp
public class SaveManager
{
    // Save data structure
    [Serializable]
    public class SaveData
    {
        public int coins;
        public List<string> ownedCharacters;
        public List<string> unlockedAbilities;
        public Dictionary<string, int> achievements;
        public PlayerStats stats;
    }
    
    // Save/Load với JSON
    public void SaveGame()
    {
        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(savePath, json);
    }
    
    public SaveData LoadGame()
    {
        string json = File.ReadAllText(savePath);
        return JsonUtility.FromJson<SaveData>(json);
    }
}
```

---

```
╔═══════════════════════════════════════════════════════════╗
║                                                           ║
║              🔧 GAME SYSTEMS                              ║
║                                                           ║
╚═══════════════════════════════════════════════════════════╝
```

## 🌌 7. Background System

### InfiniteBackground.cs

#### Nguyên lý hoạt động
```
Background sử dụng shader material để tạo background vô tận
mà KHÔNG cần tạo nhiều tiles.
```

#### Cơ chế "Reset"
```csharp
// Mỗi khi player di chuyển 15 units → Trigger reset
void Update()
{
    Vector2 toReset = previousResetPosition - (Vector2)playerTransform.position;
    if (toReset.sqrMagnitude > resetDistance * resetDistance)  // 15 units
    {
        StartCoroutine(ResetBackground(toReset));
        previousResetPosition = playerTransform.position;
    }
}

// Reset = Blend texture position mượt mà trong 5 giây
IEnumerator ResetBackground(Vector2 toReset)
{
    backgroundMaterial.SetInt("_Resetting", 1);
    backgroundMaterial.SetVector("_TempResetOffset", toReset);
    
    float t = 0;
    while (t < resetDuration)  // 5 seconds
    {
        t += Time.deltaTime;
        backgroundMaterial.SetFloat("_ResetBlend", t/resetDuration);
        yield return null;
    }
    
    resetOffset += toReset;
    backgroundMaterial.SetVector("_ResetOffset", resetOffset);
}
```

#### Hiệu ứng Shockwave
```csharp
// Tạo sóng lan tỏa từ vị trí player
public IEnumerator Shockwave(float distance)
{
    float d = 0;
    while (d < distance)
    {
        d += Time.deltaTime * 16;  // Tốc độ 16 units/s
        backgroundMaterial.SetFloat("_Shockwave", d);
        backgroundMaterial.SetVector("_PlayerPosition", playerTransform.position);
        yield return null;
    }
}
```

#### Ví dụ thực tế
```
Giống như chạy trên máy chạy bộ (treadmill):
- Bạn chạy nhưng vẫn đứng tại chỗ
- Băng chuyền dưới chân cuộn mãi
- Từ góc nhìn của bạn = đang chạy trên đường dài vô tận

Background chỉ là 1 plane, nhưng texture bên trong "cuộn"
→ Player cảm giác đang di chuyển trên bản đồ lớn
```

#### Ưu điểm
✅ Hiệu năng cao - Chỉ 1 plane duy nhất  
✅ Mượt mà - Blend 5s thay vì teleport  
✅ Shader-based - Tính toán trên GPU  
✅ Vô tận thực sự - Player đi mãi không hết background

---

## 👾 8. Entity Management

### Tổng quan

EntityManager chịu trách nhiệm spawn và quản lý tất cả entities trong game thông qua **Object Pooling Pattern**.

### 📊 Các loại Entities được quản lý

| Entity Type | Pool Type | Mục đích |
|------------|-----------|----------|
| **Monster** | MonsterPool[] | Quái thường + Boss |
| **Projectile** | ProjectilePool | Đạn từ vũ khí Ranged |
| **Throwable** | ThrowablePool | Vật thể ném (dao, búa) |
| **Boomerang** | BoomerangPool | Vũ khí boomerang |
| **ExpGem** | ExpGemPool | Gem kinh nghiệm |
| **Coin** | CoinPool | Tiền vàng |
| **Chest** | ChestPool | Rương kho báu |
| **DamageText** | DamageTextPool | Số damage hiển thị |

---

### 🎯 Cơ chế Spawn Monster

#### Bước 0: Tính toán kích thước màn hình (Init)

```csharp
// EntityManager.Init() - Chạy 1 lần lúc bắt đầu game
public void Init(LevelBlueprint levelBlueprint, Character character, ...)
{
    // Lấy kích thước màn hình trong world space
    Vector2 bottomLeft = playerCamera.ViewportToWorldPoint(new Vector3(0, 0, nearClipPlane));
    Vector2 topRight = playerCamera.ViewportToWorldPoint(new Vector3(1, 1, nearClipPlane));
    
    screenWidthWorldSpace = topRight.x - bottomLeft.x;      // Ví dụ: 20 units
    screenHeightWorldSpace = topRight.y - bottomLeft.y;     // Ví dụ: 12 units
    screenDiagonalWorldSpace = (topRight - bottomLeft).magnitude; // √(20² + 12²) ≈ 23.3
    
    minSpawnDistance = screenDiagonalWorldSpace / 2;        // ≈ 11.65 units
}
```

**Giải thích:**
- `ViewportToWorldPoint`: Chuyển tọa độ viewport (0-1) sang world space
- `screenWidthWorldSpace`: Chiều rộng màn hình (world units)
- `screenHeightWorldSpace`: Chiều cao màn hình (world units)
- `minSpawnDistance`: Khoảng cách tối thiểu từ player để spawn

---

#### Bước 1: Chọn phương thức spawn

```csharp
public Monster SpawnMonsterRandomPosition(int monsterPoolIndex, MonsterBlueprint blueprint, float hpBuff)
{
    // Chọn phương thức spawn dựa trên player có đang di chuyển không
    Vector2 spawnPosition = (playerCharacter.Velocity != Vector2.zero) 
        ? GetRandomMonsterSpawnPositionPlayerVelocity()  // Player đang di chuyển
        : GetRandomMonsterSpawnPosition();                // Player đứng yên
    
    return SpawnMonster(monsterPoolIndex, spawnPosition, blueprint, hpBuff);
}
```

---

#### Bước 2A: Random Position (Player đứng yên)

```csharp
private Vector2 GetRandomMonsterSpawnPosition()
{
    // 4 cạnh màn hình: Trái(0), Trên(1), Phải(2), Dưới(3)
    Vector2[] sideDirections = { Vector2.left, Vector2.up, Vector2.right, Vector2.down };
    
    // Random chọn 1 trong 4 cạnh (tỉ lệ đều 25% mỗi cạnh)
    int sideIndex = Random.Range(0, 4);
    
    Vector2 spawnPosition;
    
    // ===== SPAWN Ở CẠNH TRÁI/PHẢI (sideIndex = 0 hoặc 2) =====
    if (sideIndex % 2 == 0) 
    {
        spawnPosition = playerPosition 
            + sideDirections[sideIndex] * (screenWidthWorldSpace/2 + monsterSpawnBufferDistance)
            + Vector2.up * Random.Range(
                -screenHeightWorldSpace/2 - monsterSpawnBufferDistance,
                +screenHeightWorldSpace/2 + monsterSpawnBufferDistance
            );
    }
    // ===== SPAWN Ở CẠNH TRÊN/DƯỚI (sideIndex = 1 hoặc 3) =====
    else 
    {
        spawnPosition = playerPosition 
            + sideDirections[sideIndex] * (screenHeightWorldSpace/2 + monsterSpawnBufferDistance)
            + Vector2.right * Random.Range(
                -screenWidthWorldSpace/2 - monsterSpawnBufferDistance,
                +screenWidthWorldSpace/2 + monsterSpawnBufferDistance
            );
    }
    
    return spawnPosition;
}
```

**Minh họa (Top-Down View):**

```
                    ┌────────────── Screen Width ──────────────┐
                    │                                          │
        ╔═══════════╪══════════════════════════════════════════╪═══════════╗
        ║  BUFFER   │            CẠNH TRÊN (UP)               │  BUFFER   ║
        ║           │  ● ● ● ● ● ● ● ● ● ● ● ● ● ● ● ●       │           ║
        ╠═══════════╪══════════════════════════════════════════╪═══════════╣
        ║           │                                          │           ║
        ║  CẠNH     │           🎮 PLAYER VIEW                │   CẠNH    ║ Screen
        ║  TRÁI  ●  │                                          │ ● PHẢI    ║ Height
        ║  (LEFT) ● │          (Visible Area)                 │ ● (RIGHT) ║
        ║         ● │                                          │ ●         ║
        ║           │                                          │           ║
        ╠═══════════╪══════════════════════════════════════════╪═══════════╣
        ║  BUFFER   │       ● ● ● ● ● ● ● ● ● ● ● ●          │  BUFFER   ║
        ║           │        CẠNH DƯỚI (DOWN)                 │           ║
        ╚═══════════╪══════════════════════════════════════════╪═══════════╝
                    
Legend:
🎮 = Player position (center of screen)
● = Possible spawn positions
Buffer = monsterSpawnBufferDistance (extra distance outside screen)
```

**Chi tiết tính toán cho từng cạnh:**

**1. CẠNH TRÁI (LEFT - sideIndex = 0):**
```csharp
// X position: Ngoài cạnh trái màn hình
x = playerX + (-1) * (screenWidth/2 + buffer)
  = playerX - (screenWidth/2 + buffer)

// Y position: Random dọc theo chiều cao màn hình + buffer
y = playerY + Random.Range(-screenHeight/2 - buffer, +screenHeight/2 + buffer)

// Ví dụ: 
// playerX = 0, playerY = 0
// screenWidth = 20, screenHeight = 12, buffer = 2
x = 0 - (10 + 2) = -12
y = Random.Range(-8, +8)  // Có thể spawn từ y=-8 đến y=+8
```

**2. CẠNH PHẢI (RIGHT - sideIndex = 2):**
```csharp
x = playerX + (+1) * (screenWidth/2 + buffer)
  = playerX + (screenWidth/2 + buffer)
  
y = playerY + Random.Range(-screenHeight/2 - buffer, +screenHeight/2 + buffer)

// Ví dụ:
x = 0 + (10 + 2) = +12
y = Random.Range(-8, +8)
```

**3. CẠNH TRÊN (UP - sideIndex = 1):**
```csharp
// Y position: Ngoài cạnh trên màn hình
y = playerY + (+1) * (screenHeight/2 + buffer)
  = playerY + (screenHeight/2 + buffer)

// X position: Random ngang theo chiều rộng màn hình + buffer
x = playerX + Random.Range(-screenWidth/2 - buffer, +screenWidth/2 + buffer)

// Ví dụ:
y = 0 + (6 + 2) = +8
x = Random.Range(-12, +12)
```

**4. CẠNH DƯỚI (DOWN - sideIndex = 3):**
```csharp
y = playerY + (-1) * (screenHeight/2 + buffer)
  = playerY - (screenHeight/2 + buffer)
  
x = playerX + Random.Range(-screenWidth/2 - buffer, +screenWidth/2 + buffer)

// Ví dụ:
y = 0 - (6 + 2) = -8
x = Random.Range(-12, +12)
```

---

#### Bước 2B: Weighted Random (Player đang di chuyển)

```csharp
private Vector2 GetRandomMonsterSpawnPositionPlayerVelocity()
{
    Vector2[] sideDirections = { Vector2.left, Vector2.up, Vector2.right, Vector2.down };
    
    // ===== BƯỚC 1: Tính weight cho mỗi cạnh =====
    // Sử dụng Dot Product để đo độ "cùng hướng" với velocity
    float[] sideWeights = new float[4];
    for (int i = 0; i < 4; i++)
    {
        sideWeights[i] = Vector2.Dot(
            playerCharacter.Velocity.normalized,  // Hướng di chuyển player
            sideDirections[i]                      // Hướng cạnh màn hình
        );
    }
    
    // Dot Product results:
    //  +1.0 = Cùng hướng 100%
    //   0.0 = Vuông góc (không liên quan)
    //  -1.0 = Ngược hướng 100%
    
    // ===== BƯỚC 2: Xử lý weight âm (cạnh phía sau player) =====
    float extraWeight = sideWeights.Sum() / playerDirectionSpawnWeight;
    int badSideCount = sideWeights.Where(x => x <= 0).Count();
    
    for (int i = 0; i < 4; i++)
    {
        if (sideWeights[i] <= 0)
            sideWeights[i] = extraWeight / badSideCount;
    }
    
    // ===== BƯỚC 3: Weighted Random Selection =====
    float totalWeight = sideWeights.Sum();
    float rand = Random.Range(0f, totalWeight);
    float cumulative = 0;
    int selectedSide = -1;
    
    for (int i = 0; i < 4; i++)
    {
        cumulative += sideWeights[i];
        if (rand < cumulative)
        {
            selectedSide = i;
            break;
        }
    }
    
    // ===== BƯỚC 4: Tính toán vị trí spawn (giống như Random Position) =====
    // ... (same logic as GetRandomMonsterSpawnPosition)
    
    return spawnPosition;
}
```

**Ví dụ cụ thể: Player đang chạy sang PHẢI**

```
Player Velocity = (1, 0) → normalized = (1, 0)

Dot Product với mỗi cạnh:
- LEFT  (−1, 0): Dot = 1×(−1) + 0×0 = −1.0  ❌ (phía sau)
- UP    ( 0, 1): Dot = 1×0    + 0×1 =  0.0  ⚪ (vuông góc)
- RIGHT (+1, 0): Dot = 1×1    + 0×0 = +1.0  ✅ (phía trước)
- DOWN  ( 0,−1): Dot = 1×0    + 0×(−1)= 0.0 ⚪ (vuông góc)

Giả sử playerDirectionSpawnWeight = 4:
- extraWeight = (−1 + 0 + 1 + 0) / 4 = 0
- badSideCount = 1 (chỉ LEFT ≤ 0)
- sideWeights[LEFT] = 0 / 1 = 0

Final weights:
- LEFT:  0.0  →  0% spawn chance
- UP:    0.0  →  0% spawn chance  
- RIGHT: 1.0  → 100% spawn chance (phía trước player!)
- DOWN:  0.0  →  0% spawn chance

→ Monster sẽ spawn ở cạnh PHẢI với 100% tỉ lệ!
```

**Ví dụ 2: Player chạy CHÉO (phải-trên)**

```
Player Velocity = (1, 1) → normalized = (0.707, 0.707)

Dot Product:
- LEFT  (−1, 0): Dot = 0.707×(−1) + 0.707×0    = −0.707 ❌
- UP    ( 0, 1): Dot = 0.707×0    + 0.707×1    = +0.707 ✅
- RIGHT (+1, 0): Dot = 0.707×1    + 0.707×0    = +0.707 ✅
- DOWN  ( 0,−1): Dot = 0.707×0    + 0.707×(−1) = −0.707 ❌

Giả sử playerDirectionSpawnWeight = 4:
- extraWeight = (−0.707 + 0.707 + 0.707 − 0.707) / 4 = 0
- badSideCount = 2 (LEFT và DOWN)
- sideWeights[LEFT] = sideWeights[DOWN] = 0

Final weights:
- LEFT:  0.0    →  0% 
- UP:    0.707  → 50% spawn chance
- RIGHT: 0.707  → 50% spawn chance
- DOWN:  0.0    →  0%

→ Monster spawn đều ở cạnh TRÊN và PHẢI (hướng player đi tới)
```

**Tác dụng của `playerDirectionSpawnWeight`:**

```csharp
// playerDirectionSpawnWeight = 1 (default strong bias)
//   → Spawn rất nhiều ở phía trước, ít ở phía sau

// playerDirectionSpawnWeight = 10 (weak bias)
//   → Spawn gần như đồng đều 4 cạnh (ít bị ảnh hưởng bởi velocity)
```

---

#### Bước 3: Spawn Monster vào Pool

```csharp
public Monster SpawnMonster(int monsterPoolIndex, Vector2 position, 
                            MonsterBlueprint blueprint, float hpBuff = 0)
{
    // Lấy monster từ pool (reuse hoặc tạo mới)
    Monster newMonster = monsterPools[monsterPoolIndex].Get();
    
    // Setup monster với blueprint và HP buff
    newMonster.Setup(monsterPoolIndex, position, blueprint, hpBuff);
    
    // Thêm vào Spatial Hash Grid (để tối ưu collision detection)
    grid.InsertClient(newMonster);
    
    return newMonster;
}
```

---

### 📐 Khoảng cách Spawn (Distance Values)

#### Giá trị cấu hình (SerializeField trong Inspector)

```csharp
[Header("Monster Spawning Settings")]
[SerializeField] private float monsterSpawnBufferDistance;  
// Giá trị thực tế: Inspector setting (thường là 1-3 units)
```

#### Công thức tính khoảng cách từ Player

```csharp
// Từ Init():
screenWidthWorldSpace  = topRight.x - bottomLeft.x;        // Ví dụ: 20 units
screenHeightWorldSpace = topRight.y - bottomLeft.y;        // Ví dụ: 12 units
screenDiagonalWorldSpace = √(width² + height²);            // Ví dụ: 23.3 units
minSpawnDistance = screenDiagonalWorldSpace / 2;           // Ví dụ: 11.65 units
```

#### Khoảng cách spawn thực tế cho từng cạnh

**Giả sử giá trị cụ thể:**
- `screenWidthWorldSpace = 20 units`
- `screenHeightWorldSpace = 12 units`
- `monsterSpawnBufferDistance = 2 units` (cài đặt trong Inspector)

---

**1. CẠNH TRÁI/PHẢI (Horizontal):**

```
Khoảng cách từ player đến spawn point:
= screenWidth/2 + monsterSpawnBufferDistance
= 20/2 + 2
= 10 + 2
= 12 units

┌─────── 12 units ───────┐
│                        │
🎮 Player ────────────────► ● Monster spawn (bên phải)

Player ◄──────────────── ● Monster spawn (bên trái)
                         │
                         └─── 12 units ────┘
```

**Phạm vi dọc (Y range):**
```
Chiều dọc spawn range = screenHeight + 2 × buffer
                      = 12 + 2 × 2
                      = 16 units

Spawn Y từ: playerY - 8  đến  playerY + 8
            (12/2 + 2)        (12/2 + 2)

        ● ● ● ← Spawn từ Y = +8
        ● ● ●
        ● ● ●
🎮 ────────────► 12 units ngang
        ● ● ●
        ● ● ●
        ● ● ● ← Spawn từ Y = -8
```

---

**2. CẠNH TRÊN/DƯỚI (Vertical):**

```
Khoảng cách từ player đến spawn point:
= screenHeight/2 + monsterSpawnBufferDistance
= 12/2 + 2
= 6 + 2
= 8 units

       ● ● ● ● ● ← Monster spawn (cạnh trên)
       ↑
       8 units
       ↓
      🎮 Player
       ↓
       8 units
       ↑
       ● ● ● ● ● ← Monster spawn (cạnh dưới)
```

**Phạm vi ngang (X range):**
```
Chiều ngang spawn range = screenWidth + 2 × buffer
                        = 20 + 2 × 2
                        = 24 units

Spawn X từ: playerX - 12  đến  playerX + 12
            (20/2 + 2)         (20/2 + 2)

        ● ● ● ● ● ● ● ● ● ●
        ↑                  ↑
   X = -12           X = +12
        └─── 24 units ────┘
```

---

#### So sánh khoảng cách các cạnh

| Cạnh | Khoảng cách từ Player | Phạm vi spawn |
|------|----------------------|---------------|
| **Trái/Phải** | **12 units** | Y: -8 → +8 (16 units dọc) |
| **Trên/Dưới** | **8 units** | X: -12 → +12 (24 units ngang) |

**Lưu ý quan trọng:**
- ❗ **Cạnh TRÁI/PHẢI xa hơn** (12 units) vì màn hình rộng hơn
- ❗ **Cạnh TRÊN/DƯỚI gần hơn** (8 units) vì màn hình thấp hơn
- ✅ Tất cả đều spawn **ngoài tầm nhìn** của camera

---

#### Minh họa tổng thể (Top-Down View)

```
                    ├──── 24 units ────┤
                    
        ╔═══════════════════════════════════════╗  ↑
        ║  ● ● ● ● ● ● ● ● ● ● ● ● ● ● ● ● ● ║  │
        ║  ●                                 ● ║  │ 2u buffer
        ║  ●   ┌──────────────────────┐    ● ║  │
        ║  ●   │                      │    ● ║  ↓
        ║  ●   │                      │    ● ║  ← 8 units (spawn distance UP)
        ╠══●═══╪══════════════════════╪════●═╣
        ║  ●   │                      │    ● ║
        ║  ●   │   🎮 PLAYER VIEW     │    ● ║  ← Screen Height = 12 units
        ║  ●   │   (Visible Area)     │    ● ║
        ║  ●   │   20 × 12 units      │    ● ║
        ║  ●   │                      │    ● ║
        ╠══●═══╪══════════════════════╪════●═╣
        ║  ●   │                      │    ● ║  ← 8 units (spawn distance DOWN)
        ║  ●   └──────────────────────┘    ● ║  
        ║  ●                                 ● ║
        ║  ● ● ● ● ● ● ● ● ● ● ● ● ● ● ● ● ● ║
        ╚═══════════════════════════════════════╝
        ↑                                       ↑
    12 units                               12 units
   (LEFT spawn)                          (RIGHT spawn)

Legend:
🎮 = Player position
● = Monster spawn positions
Buffer = 2 units extra distance
Visible screen = 20 × 12 units
Total spawn area = 24 × 16 units
```

---

#### Ví dụ tọa độ cụ thể

**Giả sử Player ở (0, 0):**

| Cạnh | Spawn X | Spawn Y | Khoảng cách |
|------|---------|---------|-------------|
| **Trái** | **-12** | Random(-8, +8) | 12 units |
| **Phải** | **+12** | Random(-8, +8) | 12 units |
| **Trên** | Random(-12, +12) | **+8** | 8 units |
| **Dưới** | Random(-12, +12) | **-8** | 8 units |

**Ví dụ spawn point cụ thể:**
```csharp
// Player at (0, 0), spawn right side with Y = 3
Monster spawn position = (12, 3)
Distance from player = √(12² + 3²) = √153 ≈ 12.37 units

// Player at (0, 0), spawn top side with X = -5
Monster spawn position = (-5, 8)
Distance from player = √(5² + 8²) = √89 ≈ 9.43 units

// Player at (10, 5), spawn left side with Y = 5
Monster spawn position = (10-12, 5+0) = (-2, 5)
Distance from player = 12 units (straight horizontal)
```

---

#### Tại sao cần Buffer Distance?

```csharp
// Không có buffer (monsterSpawnBufferDistance = 0):
spawnDistance = screenWidth/2 + 0 = 10 units
→ Monster spawn ĐÚNG Ở BIÊN màn hình
→ Player có thể thấy monster "pop in" đột ngột ❌

// Có buffer (monsterSpawnBufferDistance = 2):
spawnDistance = screenWidth/2 + 2 = 12 units
→ Monster spawn 2 units BÊN NGOÀI màn hình
→ Monster "đi vào" màn hình một cách mượt mà ✅
```

**Lợi ích:**
- ✅ Tránh hiệu ứng "pop-in" (xuất hiện đột ngột)
- ✅ Cho player thời gian phản ứng khi thấy monster
- ✅ Tạo cảm giác monster đến từ xa, không phải spawn giữa màn hình

---

### 🎯 Tóm tắt công thức

```
KHOẢNG CÁCH SPAWN = (Chiều màn hình / 2) + Buffer

Cạnh Trái/Phải:
  - Ngang: screenWidth/2 + buffer = 10 + 2 = 12 units
  - Dọc: Random trong phạm vi (-8, +8) = 16 units

Cạnh Trên/Dưới:
  - Dọc: screenHeight/2 + buffer = 6 + 2 = 8 units
  - Ngang: Random trong phạm vi (-12, +12) = 24 units

Buffer Distance:
  - Configurable trong Unity Inspector
  - Giá trị thông thường: 1-3 units
  - Mục đích: Spawn ngoài màn hình để tránh "pop-in"
```

---

### 🎯 Tóm tắt Algorithm

```
SPAWN MONSTER ALGORITHM:
├─ IF player.velocity == 0:
│  ├─ Random.Range(0, 4) → chọn cạnh
│  └─ Spawn tại cạnh đó (tỉ lệ đều 25% mỗi cạnh)
│
└─ ELSE (player đang di chuyển):
   ├─ Tính Dot Product với 4 cạnh
   ├─ Weight cao → Cạnh cùng hướng player
   ├─ Weight thấp → Cạnh vuông góc
   ├─ Weight âm → Cạnh ngược hướng (gán = 0)
   └─ Weighted Random → Spawn nhiều ở phía trước

SPAWN POSITION:
├─ Cạnh Trái/Phải:
│  ├─ X = playerX ± (screenWidth/2 + buffer)
│  └─ Y = playerY + Random(-screenHeight/2-buffer, +screenHeight/2+buffer)
│
└─ Cạnh Trên/Dưới:
   ├─ Y = playerY ± (screenHeight/2 + buffer)
   └─ X = playerX + Random(-screenWidth/2-buffer, +screenWidth/2+buffer)
```

#### 3. Timeline Spawn (LevelManager.cs)

```csharp
// LevelManager.cs - Update()
void Update()
{
    levelTime += Time.deltaTime;
    
    // ========== MONSTER SPAWN ==========
    if (levelTime < levelBlueprint.levelTime)
    {
        timeSinceLastMonsterSpawned += Time.deltaTime;
        
        // Lấy spawn rate từ spawn table (tăng theo thời gian)
        float progress = levelTime / levelBlueprint.levelTime; // 0.0 -> 1.0
        float spawnRate = monsterSpawnTable.GetSpawnRate(progress);
        float spawnDelay = 1.0f / spawnRate;
        
        if (timeSinceLastMonsterSpawned >= spawnDelay)
        {
            // Chọn monster type và HP multiplier
            (int monsterIndex, float hpMultiplier) = 
                monsterSpawnTable.SelectMonsterWithHPMultiplier(progress);
            
            MonsterBlueprint blueprint = GetMonsterBlueprint(monsterIndex);
            
            // Spawn với HP tăng dần theo thời gian
            entityManager.SpawnMonsterRandomPosition(
                poolIndex, 
                blueprint, 
                blueprint.hp * hpMultiplier
            );
            
            timeSinceLastMonsterSpawned = 0;
        }
    }
}
```

**Timeline:**
```
0:00 ──────► 10:00 ──────────────► BOSS TIME
│            │                      │
│            │                      └─ Final Boss spawn
│            └─ Mini Boss spawn       Chiến thắng nếu giết được
│
└─ Spawn rate & HP tăng dần theo thời gian
```

---

### 👑 Cơ chế Spawn Boss

#### 1. Mini Boss

```csharp
// Spawn theo thời điểm cố định
if (!miniBossSpawned && levelTime > levelBlueprint.miniBosses[0].spawnTime)
{
    miniBossSpawned = true;
    entityManager.SpawnMonsterRandomPosition(
        levelBlueprint.monsters.Length,  // Pool index riêng cho boss
        levelBlueprint.miniBosses[0].bossBlueprint
    );
}
```

**Đặc điểm:**
- Spawn 1 lần duy nhất tại thời điểm định trước (vd: 5 phút)
- Không respawn khi bị giết
- Có pool riêng, không dùng chung với monster thường

#### 2. Final Boss

```csharp
// Spawn khi hết thời gian level (600s)
if (!finalBossSpawned && levelTime > levelBlueprint.levelTime)
{
    finalBossSpawned = true;
    
    // (Optional) Giết hết monster thường
    // entityManager.KillAllMonsters();
    
    Monster finalBoss = entityManager.SpawnMonsterRandomPosition(
        levelBlueprint.monsters.Length, 
        levelBlueprint.finalBoss.bossBlueprint
    );
    
    // Subscribe event chiến thắng
    finalBoss.OnKilled.AddListener(LevelPassed);
}
```

**Đặc điểm:**
- Spawn sau `levelBlueprint.levelTime` (mặc định 600s = 10 phút)
- **Điều kiện chiến thắng:** Giết Final Boss
- Event-driven: `OnKilled` → `LevelPassed()`

---

### 📦 Cơ chế Spawn Chest (Rương)

#### 1. Spawn theo interval

```csharp
// LevelManager.cs
timeSinceLastChestSpawned += Time.deltaTime;

if (timeSinceLastChestSpawned >= levelBlueprint.chestSpawnDelay)
{
    for (int i = 0; i < levelBlueprint.chestSpawnAmount; i++)
    {
        entityManager.SpawnChest(levelBlueprint.chestBlueprint);
    }
    timeSinceLastChestSpawned = 0;
}
```

**Blueprint Settings:**
- `chestSpawnDelay`: Thời gian giữa các lần spawn (giây)
- `chestSpawnAmount`: Số rương spawn mỗi lần (thường = 1)

#### 2. Spawn vị trí (EntityManager.cs)

```csharp
public Chest SpawnChest(ChestBlueprint chestBlueprint)
{
    Chest newChest = chestPool.Get();
    
    // Spawn ngẫu nhiên xung quanh player (ngoài màn hình)
    Vector2 spawnDirection = Random.insideUnitCircle.normalized;
    Vector2 spawnPosition = playerPosition 
        + spawnDirection * (minSpawnDistance + bufferDistance + Random.Range(0, chestSpawnRange));
    
    // Anti-overlap: Kiểm tra không spawn chồng lên chest khác
    int tries = 0;
    bool overlapsOtherChest;
    do
    {
        overlapsOtherChest = false;
        foreach (Chest existingChest in chests)
        {
            if (Vector2.Distance(existingChest.position, spawnPosition) < 0.5f)
            {
                overlapsOtherChest = true;
                spawnPosition = GetNewRandomPosition(); // Thử lại
                break;
            }
        }
    } while (overlapsOtherChest && tries++ < 100);
    
    newChest.transform.position = spawnPosition;
    newChest.Setup(chestBlueprint);
    chests.Add(newChest);
    return newChest;
}
```

**Đặc điểm:**
- Spawn **ngoài tầm nhìn** player
- **Anti-overlap logic:** Không spawn chồng lên chest cũ (distance check)
- Max 100 attempts để tìm vị trí hợp lệ

#### 3. Chest ban đầu

```csharp
// LevelManager.Init()
entityManager.SpawnChest(levelBlueprint.chestBlueprint); // 1 chest lúc bắt đầu
```

---

### 💎 Cơ chế Spawn ExpGem & Coin

#### 1. Spawn từ Monster chết

```csharp
// Monster.cs - OnKilled()
void Killed()
{
    // Drop EXP gem
    entityManager.SpawnExpGem(transform.position, gemType);
    
    // Drop coin (chance-based)
    if (Random.value < coinDropChance)
        entityManager.SpawnCoin(transform.position, coinType);
}
```

#### 2. Spawn ban đầu (Initial Gems)

```csharp
// LevelManager.Init()
entityManager.SpawnGemsAroundPlayer(
    levelBlueprint.initialExpGemCount,  // Số lượng gem
    levelBlueprint.initialExpGemType    // Loại gem
);

// EntityManager.SpawnGemsAroundPlayer()
public void SpawnGemsAroundPlayer(int gemCount, GemType gemType)
{
    for (int i = 0; i < gemCount; i++)
    {
        Vector2 spawnDirection = Random.insideUnitCircle.normalized;
        float randomDistance = Mathf.Sqrt(Random.Range(1, minSpawnDistance²));
        Vector2 spawnPosition = playerPosition + spawnDirection * randomDistance;
        
        SpawnExpGem(spawnPosition, gemType, spawnAnimation: false);
    }
}
```

**Đặc điểm:**
- Initial gems: Rải đều xung quanh player khi bắt đầu màn
- Không có animation spawn (spawnAnimation = false)

---

### ⚔️ Cơ chế Spawn Vũ khí/Projectile

**Vũ khí không spawn ngẫu nhiên!** Player nhận vũ khí qua:
1. **Level Up** → Ability Selection Dialog
2. **Mở Chest** → Ability Selection Dialog hoặc stats buff

#### Projectile Spawn (từ vũ khí đã có)

```csharp
// RangedAbility.cs
void Fire()
{
    int projectileIndex = entityManager.GetProjectileIndex(projectilePrefab);
    
    Projectile proj = entityManager.SpawnProjectile(
        projectileIndex,
        firePosition,
        damage,
        knockback,
        speed,
        targetLayer
    );
}
```

**Lưu ý:**
- Projectile được pool và reuse (Object Pooling)
- Mỗi loại projectile có pool riêng
- Pool tự động tạo thêm nếu cần (expandable)

---

### 🧪 Object Pooling Details

#### Pool Structure

```csharp
// Base Pool (generic)
public class ObjectPool<T> where T : Component
{
    private Stack<T> availableObjects;
    private GameObject prefab;
    private Transform parent;
    
    public T Get()
    {
        if (availableObjects.Count > 0)
            return availableObjects.Pop(); // Reuse
        else
            return Instantiate(prefab, parent); // Create new
    }
    
    public void Release(T obj)
    {
        obj.gameObject.SetActive(false);
        availableObjects.Push(obj);
    }
}
```

**Ưu điểm:**
- ✅ Giảm Garbage Collection (không Instantiate/Destroy liên tục)
- ✅ Tăng performance (reuse objects)
- ✅ Tự động expand khi cần

**Pools trong game:**
- `MonsterPool[]`: Mảng pool cho mỗi loại quái
- `ProjectilePool`: Shared pool cho tất cả projectiles
- `ExpGemPool`, `CoinPool`, `ChestPool`: Single pool

---

### 📐 Spatial Hash Grid

```csharp
// Tối ưu collision detection
public class SpatialHashGrid
{
    private Dictionary<Vector2Int, List<Monster>> grid;
    
    // Insert monster vào cell tương ứng
    public void InsertClient(Monster monster)
    {
        Vector2Int cell = WorldToCell(monster.position);
        grid[cell].Add(monster);
    }
    
    // Query monsters gần player (chỉ check các cell lân cận)
    public List<Monster> GetNearbyMonsters(Vector2 position, float radius)
    {
        // Chỉ check 9 cells xung quanh thay vì toàn bộ monsters
    }
}
```

**Đặc điểm:**
- Chia world thành grid cells
- Chỉ check collision trong các cells gần nhau
- Rebuild grid khi player di chuyển xa (CloseToEdge check)

---

### 📊 Spawn Configuration (LevelBlueprint)

```csharp
[CreateAssetMenu(fileName = "Level", menuName = "Blueprints/Level Blueprint")]
public class LevelBlueprint : ScriptableObject
{
    // Monster spawning
    public MonsterSpawnTable monsterSpawnTable;
    public MonsterGroup[] monsters;
    public Boss[] miniBosses;
    public Boss finalBoss;
    
    // Chest spawning
    public ChestBlueprint chestBlueprint;
    public float chestSpawnDelay = 30f;      // Spawn mỗi 30s
    public int chestSpawnAmount = 1;         // Spawn 1 rương/lần
    
    // Initial spawning
    public int initialExpGemCount = 50;      // 50 gems ban đầu
    public GemType initialExpGemType = GemType.White1;
    
    // Timeline
    public float levelTime = 600f;           // 10 phút đến boss
}
```

---

### 🎯 Key Takeaways

| Aspect | Implementation |
|--------|----------------|
| **Monster Spawn** | Offscreen, weighted by player velocity |
| **Boss Spawn** | Time-triggered, event-driven victory |
| **Chest Spawn** | Interval-based, anti-overlap logic |
| **Gem/Coin** | Drop from monsters + initial spawn |
| **Weapon** | NOT spawned, gained via level-up/chest |
| **Optimization** | Object Pooling + Spatial Hash Grid |

---

## 🧙 9. Character System

### Character.cs - Base Class

#### Thông số nhân vật
```csharp
public class Character : MonoBehaviour, IDamageable
{
    // Stats
    protected float hp;
    protected float maxHp;
    protected int armor;
    protected float moveSpeed;
    protected float luck;
    
    // Components
    protected AbilityManager abilityManager;
    protected EntityManager entityManager;
    
    // Events
    public UnityEvent OnDeath;
    public UnityEvent OnLevelUp;
    public UnityEvent<float> OnDealDamage;
    public UnityEvent<float> OnTakeDamage;
}
```

#### Level System
```csharp
// Tích lũy EXP để level up
public void GainExp(float exp)
{
    currentExp += exp;
    
    while (currentExp >= expToNextLevel)
    {
        currentExp -= expToNextLevel;
        LevelUp();
    }
    
    UpdateExpBar();
}

private void LevelUp()
{
    currentLevel++;
    expToNextLevel += blueprint.LevelToExpIncrease(currentLevel);
    OnLevelUp.Invoke();
}
```

#### Movement
```csharp
void Update()
{
    // Input movement
    Vector2 movement = GetMovementInput();
    
    // Apply movement
    rb.velocity = movement * moveSpeed;
    
    // Animation
    if (movement.magnitude > 0)
        StartWalkAnimation();
    else
        StopWalkAnimation();
}
```

### CharacterBlueprint.cs

#### Character Config
```csharp
[CreateAssetMenu(fileName = "Character", menuName = "Blueprints/Character")]
public class CharacterBlueprint : ScriptableObject
{
    public string name;
    public bool owned = false;
    public int cost = 999;
    
    // Stats
    public float hp;
    public float recovery;
    public int armor;
    public float movespeed;
    public float luck;
    
    // Visuals
    public Sprite[] walkSpriteSequence;
    public float walkFrameTime;
    
    // Starting abilities
    public GameObject[] startingAbilities;
    
    // EXP curve
    public float LevelToExpIncrease(int level)
    {
        if (level < 10) return 10;
        if (level < 20) return 13;
        if (level < 30) return 16;
        else return 20;
    }
}
```

---

```
╔═══════════════════════════════════════════════════════════╗
║                                                           ║
║              🛠️ TOOLS & UTILITIES                         ║
║                                                           ║
╚═══════════════════════════════════════════════════════════╝
```

## 🧪 10. Testing Tools

### TestVictory.cs
```csharp
// Test màn hình chiến thắng
// Nhấn phím V → Trigger victory ngay lập tức

public class TestVictory : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
            TriggerVictory();
    }
    
    public void TriggerVictory()
    {
        levelManager.LevelPassed(null);
    }
}
```

### TestBossSpawn.cs
```csharp
// Test spawn boss
// Nhấn phím B → Boss spawn ngay
// Nhấn phím T → Tua thời gian đến lúc boss spawn
// Nhấn phím N → Xem thời gian hiện tại

public class TestBossSpawn : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
            ForceSpawnBoss();
            
        if (Input.GetKeyDown(KeyCode.T))
            SkipToBossTime();
            
        if (Input.GetKeyDown(KeyCode.N))
            ShowCurrentTime();
    }
}
```

---

## 🌍 11. Localization System

### Unity Localization Package
```
Hỗ trợ đa ngôn ngữ qua Unity Localization Package
- String Tables: Text trong game
- Asset Tables: Sprites, Audio theo ngôn ngữ
```

### LocaleDropdown.cs
```csharp
// Dropdown để chọn ngôn ngữ
public class LocaleDropdown : MonoBehaviour
{
    private void Start()
    {
        // Populate dropdown với available locales
        dropdown.options = GetLocaleOptions();
        
        // Load saved locale
        int savedIndex = PlayerPrefs.GetInt("LocaleIndex", 0);
        dropdown.value = savedIndex;
        
        // Subscribe to change event
        dropdown.onValueChanged.AddListener(OnLocaleChanged);
    }
    
    private void OnLocaleChanged(int index)
    {
        LocalizationSettings.SelectedLocale = 
            LocalizationSettings.AvailableLocales.Locales[index];
        PlayerPrefs.SetInt("LocaleIndex", index);
    }
}
```

---

## ⚡ 12. Performance Optimization

### Object Pooling
```
Tất cả entities sử dụng Object Pooling:
- Monsters
- Projectiles
- EXP Gems
- Coins
- Effects

→ Giảm Garbage Collection, tăng FPS
```

### Spatial Partitioning
```csharp
// Chỉ update entities gần player
// Tắt/bật entities dựa vào khoảng cách
```

### Shader-based Background
```
Background không dùng nhiều sprites
→ 1 plane duy nhất với shader material
→ Tiết kiệm Draw Calls
```

---

```
╔═══════════════════════════════════════════════════════════╗
║                                                           ║
║              ⚠️ ISSUES & FUTURE PLANS                     ║
║                                                           ║
╚═══════════════════════════════════════════════════════════╝
```

## ⚠️ 13. Known Issues & Limitations

### 1. Save System
- ❌ Character owned không persist
- ❌ Không có cloud save
- ❌ Dễ mất dữ liệu

### 2. Weapon System
- ❌ Không có weapon evolution/combine
- ❌ Giới hạn số weapon có thể trang bị

### 3. Content
- ❌ Chỉ có 1 level
- ❌ Không có level selection
- ❌ Chưa có meta progression system

### 4. UI/UX
- ⚠️ Button events phải setup thủ công trong Unity
- ⚠️ Không có tutorial
- ⚠️ Không có settings âm thanh

---

## 🚀 14. Future Improvements

### 1. Save System Upgrade
```
- Implement SaveManager với JSON
- Encrypt save file để chống cheat
- Cloud save integration (PlayFab, Firebase)
- Auto-save system
```

### 2. Content Expansion
```
- Thêm levels mới
- Level selection system
- Endless mode
- More characters, weapons, enemies
- Boss rush mode
```

### 3. Meta Progression
```
- Permanent upgrades shop
- Unlock new characters/weapons
- Achievement system
- Collection/Codex system
```

### 4. UI/UX Polish
```
- Tutorial system
- Better settings menu (audio, graphics)
- Keybinding customization
- Controller support
```

### 5. Weapon Evolution
```
- Implement weapon combine system
- Evolution requirements
- Evolved weapon effects
```

---

## 📚 15. Credits & References

### Original Game
- **Vampire Survivors** by Poncle

### Assets & Packages
- Unity UGUI
- Unity Localization Package
- Unity Addressables
- TextMesh Pro

---

*Document created: November 18, 2025*
*Last updated: November 18, 2025*
