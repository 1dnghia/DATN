# MAGIC SURVIVORS - CLASS DIAGRAM

> **Sơ đồ class tổng quan cho hệ thống Magic Survivors**
> Bao gồm View, Controller, Entity và các mối quan hệ

---

## Sơ đồ Class Tổng Quan

```plantuml
@startuml
skinparam linetype ortho

class GameManager {
  - instance : GameManager
  - currentGameState : GameState
  --
  + LoadGame()
  + StartGame(character, map)
  + PauseGame()
  + ResumeGame()
  + EndGame()
}

enum GameState {
  MAIN_MENU
  CHARACTER_SELECT
  PLAYING
  PAUSED
  GAME_OVER
}

abstract class View {
  --
  + Show()
  + Hide()
}

class MainMenu {
  - btnStart : Button
  --
  + OnStartClicked()
}

class CharacterSelect {
  - characterCards : List<CharacterCard>
  --
  + LoadCharacters()
  + OnCharacterSelected(character)
}

class UIManager {
  - healthBar : Slider
  - coinText : Text
  - joystick : Joystick
  --
  + UpdateHealth(current, max)
  + UpdateCoins(amount)
}

class PlayerController {
  - player : Player
  --
  + HandleMovement(input)
  + TakeDamage(damage)
}

class AudioManager {
  --
  + PlayMusic(clip)
  + PlaySFX(clip)
}

class CharacterController {
  - characters : List<Character>
  --
  + GetAvailableCharacters() : List<Character>
}

class CurrencyManager {
  - currentCoins : int
  --
  + GetCoins() : int
  + SpendCoins(amount) : bool
}

class StatsManager {
  - enemiesKilled : int
  - timeSurvived : float
  --
  + RecordKill()
  + GetStats() : GameStats
}

class Progress {
  - unlockedCharacters : List<int>
  - coins : int
  --
  + SaveProgress()
  + UnlockCharacter(id)
}

class Player {
  - currentHealth : float
  - level : int
  --
  + TakeDamage(damage)
  + LevelUp()
}

class Character {
  - id : int
  - name : string
  - baseHealth : float
  --
  + GetStats() : CharacterStats
}

class Map {
  - id : int
  - name : string
  --
  + GetMapData() : MapData
}

class PlayerPrefsManager {
  {static} + SaveInt(key, value)
  {static} + GetInt(key) : int
}

GameManager --> GameState
View <|-- MainMenu
View <|-- CharacterSelect
View <|-- UIManager

GameManager --> MainMenu
GameManager --> CharacterSelect
GameManager --> Progress
GameManager --> StatsManager
GameManager --> UIManager

MainMenu --> CharacterSelect

CharacterSelect --> CharacterController
CharacterSelect --> CurrencyManager
CharacterSelect --> Progress

UIManager --> PlayerController

PlayerController --> Player

CurrencyManager --> PlayerPrefsManager
Progress --> PlayerPrefsManager

CharacterController --> Character
Progress --> Character
Progress --> Map

Player --> Character

AudioManager ..> GameManager
StatsManager ..> PlayerPrefsManager

@enduml
```

---

## Hướng Dẫn Sử Dụng

**Import vào PlantUML:**

- **Online Editor:** http://www.plantuml.com/plantuml/uml/
- **VS Code:** Cài extension "PlantUML" by jebbs, press `Alt+D` để preview
- **IntelliJ IDEA:** Cài plugin "PlantUML integration"

**Layout:**
- `left to right direction` - Sắp xếp theo chiều ngang
- `skinparam linetype ortho` - Đường thẳng góc vuông
- Các package có màu sắc riêng để dễ phân biệt

**Relationships:**
- `-->` association
- `<|--` inheritance  
- `..|>` implements
- `..>` dependency
