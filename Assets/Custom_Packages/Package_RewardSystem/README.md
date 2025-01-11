Here's an updated version of the `README.md` to address inconsistencies and align it better with your provided code structure and usage:  

---

# Reward System Plugin  

The Reward System Plugin is a modular Unity package for managing and distributing rewards in your game. It supports **daily rewards**, **weekly rewards**, and **level-based rewards**, providing flexibility for integration into any Unity project.  

---

## Features  

- **Daily Rewards**: Automatically track and reward players for consecutive logins.  
- **Weekly Rewards**: Reward players for weekly participation.  
- **Level Completion Rewards**: Grant rewards for completing levels.  
- **ScriptableObject Configuration**: Define rewards using ScriptableObjects for intuitive setup.  
- **Event-Driven Design**: Hook into reward claim events for custom behavior.  
- **Customizable Delivery**: Implement custom logic for delivering rewards using `IRewardDeliveryService`.  
- **Dependency Injection Support**: Designed to integrate seamlessly with DI frameworks like VContainer.  

---

## Installation  

1. Clone or download the plugin and add the `RewardSystem` folder to your Unity project.  
2. Ensure your project includes the following dependencies:  
   - `UnityEngine.UI`  
   - `UnityEventSystem`  

---

## Getting Started  

### Step 1: Add Reward Configurations  

1. Go to `Assets > Create > RewardSystem > Reward` to create individual rewards.  
2. Configure the reward properties:  
   - **Reward ID**: Unique identifier for the reward.  
   - **Description**: Description for UI or tooltips.  
   - **Quantity**: Numeric value (e.g., coins, XP, etc.).  
   - **Reward Type**: `Coins`, `Gems`, `Experience`, etc.  
   - **Delivery Type**: Choose between `Daily`, `Weekly`, or `LevelCompletion`.  
   - **Icon**: Optional image to represent the reward.  

3. Create a `RewardConfig` ScriptableObject to manage collections of daily, weekly, and level-based rewards.  

### Step 2: Setup the Dependency Injection Container  

1. Add the `GameLifetimeScope` or a custom `LifetimeScope` to your scene.  
2. Register your implementations for reward services, as shown below:  

```csharp  
using VContainer;  
using RewardSystem;  

public class GameLifetimeScope : LifetimeScope  
{  
    [SerializeField] private RewardConfig rewardConfig; // Assign via Unity Inspector  

    protected override void Configure(IContainerBuilder builder)  
    {  
        builder.Register<IRewardRepository>(c => new ScriptableObjectRewardRepository(rewardConfig), Lifetime.Singleton);  
        builder.Register<IRewardStateService, MyCustomRewardStateService>(Lifetime.Singleton);  
        builder.Register<IRewardDeliveryService, MyCustomRewardDeliveryService>(Lifetime.Singleton);  
        builder.Register<IRewardManager, DefaultRewardManager>(Lifetime.Singleton);  
    }  
}  
```  

### Step 3: Customize Delivery Logic  

Implement the `IRewardDeliveryService` interface to define how rewards are delivered to players:  

```csharp  
public class MyCustomRewardDeliveryService : IRewardDeliveryService  
{  
    public void DeliverReward(IReward reward)  
    {  
        switch (reward.Type)  
        {  
            case RewardType.Coins:  
                Debug.Log($"Delivered {reward.Quantity} Coins!");  
                break;  
            case RewardType.Gems:  
                Debug.Log($"Delivered {reward.Quantity} Gems!");  
                break;  
            // Add other reward types as needed  
        }  
    }  
}  
```  

---

## Example Usage  

### Claiming Rewards  

Use `IRewardManager` to claim rewards programmatically:  

```csharp  
using RewardSystem;  
using UnityEngine;  

public class RewardExample : MonoBehaviour  
{  
    [Inject] private IRewardManager rewardManager;  

    private void Start()  
    {  
        rewardManager.OnDailyRewardClaimed += HandleDailyRewardClaimed;  
        rewardManager.ClaimDailyReward();  
    }  

    private void HandleDailyRewardClaimed(int index, IReward reward)  
    {  
        Debug.Log($"Daily reward claimed: {reward.Quantity} {reward.Type}");  
    }  
}  
```  

---

## File Structure  

```  
RewardSystem/  
├── Scripts/  
│   ├── Reward.cs                     // ScriptableObject definition for rewards  
│   ├── RewardConfig.cs               // ScriptableObject for reward collections  
│   ├── DefaultRewardManager.cs       // Core logic for managing rewards  
│   ├── Interfaces/  
│       ├── IReward.cs                // Reward abstraction  
│       ├── IRewardRepository.cs      // Interface for reward data fetching  
│       ├── IRewardStateService.cs    // Interface for reward state management  
│       ├── IRewardDeliveryService.cs // Interface for reward delivery logic  
├── Editor/  
│   ├── RewardEditor.cs               // (Optional) Custom editor for rewards  
├── Resources/  
│   ├── SampleRewards/                // Example reward configurations  
│       ├── DailyReward.asset  
│       ├── WeeklyReward.asset  
│       ├── LevelReward.asset  
```  

---

## Advanced Configuration  

### Custom Reward Types  

1. Extend the `RewardType` enum in `Reward.cs` to add new types.  
2. Implement the necessary delivery logic in `IRewardDeliveryService`.  

### Custom Claim Logic  

Hook into `IRewardManager` events to add custom behavior:  

```csharp  
rewardManager.OnLevelRewardClaimed += (level, reward) => Debug.Log($"Level {level} reward claimed: {reward.Quantity} {reward.Type}");  
```  

---

## Requirements  

- Unity 2020.3 or higher.  
- TextMeshPro (Optional, for advanced UI).  
- VContainer (Optional, for dependency injection).  

---

## Contributing  

Contributions are welcome! Feel free to open an issue or submit a pull request.  

---

## License  

This plugin is licensed under the MIT License. See the `LICENSE` file for details.  

---

## Support  

For support or inquiries, contact [Your Email or Website].  

--- 

