# Player Progress Manager

The `PlayerProgressManager` class provides an easy-to-use system for managing player progress in games. It leverages an abstraction layer through the `IPlayerProgressStorage` interface, allowing flexibility in how data is stored and retrieved. This makes the system extensible and adaptable for various game development scenarios.

---

## Features

### **Username Management**
- **Set Username:** Save the player's username.
- **Get Username:** Retrieve the player's username (default: `"Guest"`).
- **Reset Username:** Reset the username to its default state.

### **Main Level Management**
- **Complete Main Level:** Mark a main level as completed.
- **Check Main Level Completion:** Verify if a specific main level is completed.
- **Track Main Level Retries:** Save and update retry counts for main levels.

### **Sub-Level Management**
- **Complete Sub-Level:** Mark a sub-level as completed.
- **Check Sub-Level Completion:** Verify if a specific sub-level is completed.
- **Track Sub-Level Retries:** Save and update retry counts for sub-levels.

### **Login Tracking**
- **Update Login Time:** Save timestamps for the latest login and the last login.
- **Get Last Login Time:** Retrieve the time of the previous login.
- **Get Latest Login Time:** Retrieve the time of the most recent login.

### **Currency Management**
- **Manage Coins:** Save and retrieve the player's coin count.
- **Manage Gems:** Save and retrieve the player's gem count.
- **Manage Experience Points:** Save and retrieve the player's XP.

### **Reward Tracking**
- **Track Reward Claims:** Save the date and time of the last reward claim.
- **Daily Reward Index:** Save and retrieve the current index for daily rewards.
- **Check Reward Claim Existence:** Verify if the last reward claim date is stored.

---

## Usage

### **Initialization**
To use `PlayerProgressManager`, you need an implementation of `IPlayerProgressStorage` (e.g., `PlayerPrefsStorage`).

```csharp
using PlayerProgressSystem;

var storage = new PlayerPrefsStorage();
var progressManager = new PlayerProgressManager(storage);
```

### **Examples**

#### **Username Management**
```csharp
progressManager.SetUsername("Player1");
string username = progressManager.GetUsername();
progressManager.ResetUsername();
```

#### **Main Level Management**
```csharp
progressManager.MainLevelCompleted("Level_1");
bool isCompleted = progressManager.HasCompletedMainLevel("Level_1");
progressManager.MainLevelRetries(2);
```

#### **Sub-Level Management**
```csharp
progressManager.CompleteSubLevel("SubLevel_1");
bool isCompleted = progressManager.HasCompletedSubLevel("SubLevel_1");
progressManager.SubLevelRetries(3);
```

#### **Login Tracking**
```csharp
progressManager.UpdateLoginTime();
string lastLogin = progressManager.GetLastLoginTime();
string latestLogin = progressManager.GetLatestLoginTime();
```

#### **Currency Management**
```csharp
progressManager.UpdateCoins(100);
int coins = progressManager.GetCoins();

progressManager.UpdateGems(50);
int gems = progressManager.GetGems();

progressManager.UpdateExperience(200);
int xp = progressManager.GetExperience();
```

#### **Reward Tracking**
```csharp
progressManager.SetLastRewardClaimedDateTime(DateTime.Now);
string lastClaim = progressManager.GetLastRewardClaimedDateTime();

progressManager.SetDailyRewardIndex(2);
int rewardIndex = progressManager.GetDailyRewardIndex();

bool hasClaimed = progressManager.HasLastRewardClaimedDateTime();
```

---

## Benefits

- **Flexibility:** Easily switch storage implementations (e.g., `PlayerPrefs`, file-based storage, cloud storage).
- **Scalability:** Add new features like achievements or leaderboards with minimal changes.
- **Testability:** Mock the storage layer for unit testing.

---

## Storage Interface

### **`IPlayerProgressStorage`**

The `PlayerProgressManager` relies on the following methods from the storage interface:

```csharp
public interface IPlayerProgressStorage
{
    void SetString(string key, string value);
    string GetString(string key, string defaultValue = "");
    void SetInt(string key, int value);
    int GetInt(string key, int defaultValue = 0);
    bool HasKey(string key);
    void DeleteKey(string key);
    void Save();
}
```

---

## Integration with Dependency Injection

For Dependency Injection frameworks (e.g., **VContainer**), register the storage and manager:

```csharp
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<IPlayerProgressStorage, PlayerPrefsStorage>();
        builder.Register<PlayerProgressManager>().WithParameter(
            "storage", context => context.Resolve<IPlayerProgressStorage>());
    }
}
```

---

## License

This package is distributed under the MIT License. See `LICENSE` for more details.
