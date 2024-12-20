# Reward System Plugin

The Reward System Plugin is a modular Unity package designed to manage and distribute rewards in your game. It supports **daily rewards** and **level-based rewards**, making it highly flexible and easy to integrate into any Unity project.

---

## Features

- **Daily Rewards**: Automatically track and reward players daily.
- **Level Completion Rewards**: Grant rewards for completing levels.
- **ScriptableObject Configuration**: Use ScriptableObjects to define reward data.
- **Event-Driven Design**: Hook into reward claim events for custom behavior.
- **Customizable UI**: Integrate the reward system into your own UI or use the provided sample UI.
- **Dependency Injection Support**: Works seamlessly with DI frameworks like VContainer for advanced customization.

---

## Installation

1. Download the plugin as a `.unitypackage` or copy the `RewardSystem` folder into your Unity project.
2. Ensure your project includes the following dependencies:
   - UnityEngine.UI
   - UnityEventSystem

---

## Getting Started

### Step 1: Add the Reward Manager

1. Add the `RewardManager` component to a GameObject in your scene.
2. Assign rewards to the `RewardManager` in the Inspector.

### Step 2: Create Rewards

1. Go to `Assets > Create > RewardSystem > Reward` to create a new reward.
2. Configure the reward:
   - **Name**: Name of the reward.
   - **Description**: Description for UI purposes.
   - **Value**: Numeric value (e.g., coins, XP, etc.).
   - **Type**: Choose between `Daily` or `LevelCompletion`.
   - **Icon**: (Optional) Assign an image to represent the reward.

### Step 3: Setup Dependency Injection (Optional)

1. Add the `RewardSystemLifetimeScope` component to your scene.
2. Register custom implementations of `IRewardDeliveryService` if needed:

```csharp
using VContainer;
using VContainer.Unity;

public class RewardSystemLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<IRewardDeliveryService, CustomRewardDeliveryService>().AsImplementedInterfaces();
        builder.RegisterComponentInHierarchy<RewardManager>().AsSelf();
    }
}
```

3. Create custom reward delivery logic by implementing `IRewardDeliveryService`.

### Step 4: Setup the UI

1. Use the `RewardUI` script for a simple integration.
2. Assign buttons and text fields to display reward status and claim actions.
3. Customize or replace the provided UI as needed.

---

## Example Usage

### RewardManager Example

```csharp
using RewardSystem;
using UnityEngine;

public class RewardExample : MonoBehaviour
{
    [SerializeField] private RewardManager rewardManager;

    private void Start()
    {
        // Subscribe to reward events
        rewardManager.OnRewardClaimed += HandleRewardClaimed;
    }

    private void HandleRewardClaimed(int level, Reward reward)
    {
        Debug.Log($"Reward claimed: Level {level}, Name: {reward.rewardName}, Value: {reward.quantity}");
        // Add your custom logic here (e.g., updating inventory or UI)
    }
}
```

### UI Integration Example

The `RewardUI` script is included to provide basic UI for claiming rewards. You can customize this or create your own interface.

---

## File Structure

```
RewardSystem/
├── Scripts/
│   ├── Reward.cs           // ScriptableObject for rewards
│   ├── RewardManager.cs    // Core logic for managing rewards
│   ├── RewardUI.cs         // Example UI integration
│   ├── Interfaces/
│       ├── IRewardDeliveryService.cs // Interface for reward delivery
├── Editor/
│   ├── RewardEditor.cs     // (Optional) Custom editor for rewards
├── Resources/
│   ├── SampleRewards/      // Example rewards (ScriptableObjects)
│       ├── DailyReward.asset
├── Documentation/
│   ├── RewardSystemManual.md // Documentation for using the plugin
```

---

## Advanced Configuration

### Custom Reward Types

1. Extend the `RewardType` enum in `Reward.cs` to add new reward types.
2. Modify the `RewardManager` logic to handle your new reward type.

### Custom Reward Logic

Hook into the `OnRewardClaimed` event to add game-specific behavior when a reward is claimed.

```csharp
rewardManager.OnRewardClaimed += (level, reward) => Debug.Log($"Player claimed Level {level}: {reward.rewardName}");
```

### Using Dependency Injection with VContainer

1. Define a custom `LifetimeScope` to register your implementations:

```csharp
public class CustomLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<IRewardDeliveryService, MyCustomRewardDeliveryService>().AsImplementedInterfaces();
    }
}
```

2. Implement your own reward delivery logic:

```csharp
public class MyCustomRewardDeliveryService : IRewardDeliveryService
{
    public void DeliverCoins(int quantity) => Debug.Log($"Custom Logic: Delivered {quantity} Coins");
    public void DeliverGems(int quantity) => Debug.Log($"Custom Logic: Delivered {quantity} Gems");
    public void DeliverExperience(int quantity) => Debug.Log($"Custom Logic: Delivered {quantity} XP");
}
```

---

## Requirements

- Unity 2020.3 or higher.
- TextMeshPro (Optional, for advanced UI customization).
- VContainer (Optional, for dependency injection).

---

## Contributing

If you have suggestions or improvements for the plugin, feel free to open an issue or submit a pull request on the repository.

---

## License

This plugin is licensed under the MIT License. See the `LICENSE` file for details.

---

## Support

For support or inquiries, please contact [Your Email or Website].

