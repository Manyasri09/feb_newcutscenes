# PlayerProgressSystem User Manual

## Overview

The `PlayerProgressSystem` package is designed to simplify the management of player progress in Unity-based games. It provides a flexible and extensible structure for saving and retrieving player data such as level completion, rewards, and login tracking. The system utilizes an interface-driven design, allowing for easy integration with different storage backends (e.g., `PlayerPrefs`, databases, or custom storage solutions).

## Features
- **Username Management**: Save, retrieve, and reset player usernames.
- **Level Tracking**: Manage progress for main levels and sub-levels, including retries.
- **Login Tracking**: Record and retrieve timestamps for player logins.
- **Currency Management**: Manage in-game currency such as coins, gems, and experience points.
- **Reward Tracking**: Handle daily rewards and track reward claim times.

## Installation
1. Import the `PlayerProgressSystem` package into your Unity project.
2. Ensure that the namespace `PlayerProgressSystem` is referenced in scripts that use the system.

## Components

### 1. **Interfaces**
#### `IPlayerProgressStorage`
Defines the storage operations for player progress data.
- Methods:
  - `SetString(string key, string value)`
  - `GetString(string key, string defaultValue = "")`
  - `SetInt(string key, int value)`
  - `GetInt(string key, int defaultValue = 0)`
  - `HasKey(string key)`
  - `DeleteKey(string key)`
  - `Save()`

### 2. **Storage Implementations**
#### `PlayerPrefsStorage`
Implements `IPlayerProgressStorage` using Unity's `PlayerPrefs`.
- Example:
```csharp
var storage = new PlayerPrefsStorage();
```

### 3. **Manager Class**
#### `PlayerProgressManager`
Handles all player progress-related operations. Requires an implementation of `IPlayerProgressStorage`.
- Constructor:
```csharp
PlayerProgressManager(IPlayerProgressStorage storage)
```

## Usage Examples

### Initializing the Manager
```csharp
using PlayerProgressSystem;

var storage = new PlayerPrefsStorage();
var progressManager = new PlayerProgressManager(storage);
```

### Username Management
```csharp
// Set username
progressManager.SetUsername("Player1");

// Get username
string username = progressManager.GetUsername();

// Reset username
progressManager.ResetUsername();
```

### Level Tracking
#### Main Levels
```csharp
// Mark a main level as completed
progressManager.MainLevelCompleted("Level1");

// Check if a main level is completed
bool isCompleted = progressManager.HasCompletedMainLevel("Level1");

// Set retry count for a main level
progressManager.MainLevelRetries(3);
```
#### Sub-Levels
```csharp
// Mark a sub-level as completed
progressManager.CompleteSubLevel("SubLevel1");

// Check if a sub-level is completed
bool isCompleted = progressManager.HasCompletedSubLevel("SubLevel1");

// Set retry count for a sub-level
progressManager.SubLevelRetries(2);
```

### Login Tracking
```csharp
// Update login time
progressManager.UpdateLoginTime();

// Get last and latest login times
string lastLogin = progressManager.GetLastLoginTime();
string latestLogin = progressManager.GetLatestLoginTime();
```

### Currency Management
```csharp
// Update coins
progressManager.UpdateCoins(100);

// Get coins
int coins = progressManager.GetCoins();

// Update gems
progressManager.UpdateGems(50);

// Get gems
int gems = progressManager.GetGems();

// Update experience points
progressManager.UpdateExperience(200);

// Get experience points
int xp = progressManager.GetExperience();
```

### Reward Tracking
```csharp
// Set last reward claim time
progressManager.SetLastRewardClaimedDateTime(DateTime.Now);

// Get last reward claim time
string lastRewardClaim = progressManager.GetLastRewardClaimedDateTime();

// Set and get daily reward index
progressManager.SetDailyRewardIndex(2);
int dailyRewardIndex = progressManager.GetDailyRewardIndex();

// Check if reward claim time exists
bool hasClaimed = progressManager.HasLastRewardClaimedDateTime();
```

## Integration with Dependency Injection
To integrate `PlayerProgressManager` using a dependency injection framework like VContainer:

```csharp
using VContainer;
using VContainer.Unity;
using PlayerProgressSystem;

public class GameLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<IPlayerProgressStorage, PlayerPrefsStorage>();
        builder.Register<PlayerProgressManager>().WithParameter("storage", context => context.Resolve<IPlayerProgressStorage>());
    }
}
```

## Best Practices
- **Abstract Storage Logic**: Use the `IPlayerProgressStorage` interface to allow for easy swapping of storage implementations.
- **Frequent Saves**: Call `Save()` after significant updates to ensure data persistence.
- **Error Handling**: Handle exceptions and edge cases (e.g., null or invalid data).

## Troubleshooting
- Ensure that `PlayerPrefs` is not disabled or restricted on the target platform.
- Validate all input values before passing them to `PlayerProgressManager` methods.
- Use logs to debug issues with missing or incorrect data.

## Future Extensions
- Add support for cloud storage.
- Implement analytics to track player progress trends.
- Extend currency management to include custom types (e.g., tokens).

---
For further assistance, contact the package maintainer or consult the official documentation.

