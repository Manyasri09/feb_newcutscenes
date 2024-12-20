Here’s the revised version of the manual with instructions on how to use the `DefaultRewardDeliveryService` integrated seamlessly:

```markdown
# Reward System Manual

The **Reward System Plugin** is a Unity package designed to streamline the process of managing and distributing rewards in your game. This manual provides detailed instructions on how to integrate and configure the plugin effectively.

---

## Table of Contents

1. [Overview](#overview)
2. [Installation](#installation)
3. [Getting Started](#getting-started)
4. [Using DefaultRewardDeliveryService](#using-defaultrewarddeliveryservice)
5. [Customizing Reward Delivery](#customizing-reward-delivery)
6. [Using Dependency Injection with VContainer](#using-dependency-injection-with-vcontainer)
7. [Advanced Configuration](#advanced-configuration)
8. [Debugging and Troubleshooting](#debugging-and-troubleshooting)

---

## Overview

The Reward System Plugin simplifies the reward logic in Unity games, offering built-in support for:

- **Daily Rewards**
- **Level-Based Rewards**
- **Custom Reward Types**

The system is modular and easily integrates into projects of any complexity, including those using dependency injection frameworks like **VContainer**.

---

## Installation

1. Download the plugin as a `.unitypackage` or copy the `RewardSystem` folder into your Unity project.
2. Ensure your Unity project has the following dependencies:
   - **UnityEngine.UI**
   - **UnityEventSystem**
3. (Optional) For projects using VContainer, install the VContainer package via Unity Package Manager.

---

## Getting Started

### Step 1: Setting Up the Reward Manager

1. Add the `RewardManager` component to a GameObject in your scene.
2. Assign a `RewardConfig` ScriptableObject to the `RewardManager`.
3. Configure your rewards in the `RewardConfig` file.

### Step 2: Creating Rewards

1. Navigate to `Assets > Create > RewardSystem > Reward` to create a new reward.
2. Configure the reward:
   - **Name**: A descriptive name for the reward.
   - **Type**: Choose between `Daily` or `LevelCompletion`.
   - **Quantity**: Numeric value of the reward (e.g., coins, gems).
   - **Icon**: Optional icon for UI representation.

---

## Using DefaultRewardDeliveryService

The `DefaultRewardDeliveryService` provides a ready-to-use implementation for delivering rewards without requiring any custom setup. To use it:

1. **Setup with VContainer (Recommended)**:
   - Register the `DefaultRewardDeliveryService` in your custom `LifetimeScope`:

   ```csharp
   using VContainer;
   using VContainer.Unity;

   public class RewardSystemLifetimeScope : LifetimeScope
   {
       protected override void Configure(IContainerBuilder builder)
       {
           builder.Register<IRewardDeliveryService, DefaultRewardDeliveryService>().AsImplementedInterfaces();
           builder.RegisterComponentInHierarchy<RewardManager>().AsSelf();
       }
   }
   ```

2. **Reward Delivery Logic**:
   - The `DefaultRewardDeliveryService` handles common reward types like coins, gems, and experience automatically:

   ```csharp
   public class DefaultRewardDeliveryService : IRewardDeliveryService
   {
       public void DeliverCoins(int quantity) => Debug.Log($"Default Logic: Delivered {quantity} Coins");
       public void DeliverGems(int quantity) => Debug.Log($"Default Logic: Delivered {quantity} Gems");
       public void DeliverExperience(int quantity) => Debug.Log($"Default Logic: Delivered {quantity} Experience");
       public void DeliverCustomReward(Reward reward) => Debug.Log($"Default Logic: Delivered {reward.rewardName}");
   }
   ```

3. **No Custom Code Needed**:
   - Simply use the `RewardManager`, and the rewards will be delivered automatically using the default implementation.

---

## Customizing Reward Delivery

If you need custom reward logic, you can implement the `IRewardDeliveryService` interface. For example:

```csharp
public class CustomRewardDeliveryService : IRewardDeliveryService
{
    public void DeliverCoins(int quantity) => Debug.Log($"Delivered {quantity} coins");
    public void DeliverGems(int quantity) => Debug.Log($"Delivered {quantity} gems");
    public void DeliverExperience(int quantity) => Debug.Log($"Delivered {quantity} experience");
    public void DeliverCustomReward(Reward reward) => Debug.Log($"Delivered custom reward: {reward.rewardName}");
}
```

Register this custom implementation in your `LifetimeScope` as shown in the [Dependency Injection](#using-dependency-injection-with-vcontainer) section.

---

## Using Dependency Injection with VContainer

To integrate the reward system with VContainer:

1. Add a custom `LifetimeScope` to your project:

   ```csharp
   using VContainer;
   using VContainer.Unity;

   public class RewardSystemLifetimeScope : LifetimeScope
   {
       protected override void Configure(IContainerBuilder builder)
       {
           builder.Register<IRewardDeliveryService, DefaultRewardDeliveryService>().AsImplementedInterfaces();
           builder.RegisterComponentInHierarchy<RewardManager>().AsSelf();
       }
   }
   ```

2. Ensure that the `RewardSystemLifetimeScope` is added to your scene hierarchy.

3. The `RewardManager` will automatically resolve the `IRewardDeliveryService` dependency using VContainer.

---

## Advanced Configuration

### Adding Custom Reward Types

1. Extend the `RewardType` enum in `Reward.cs` to define your new reward type:

   ```csharp
   public enum RewardType
   {
       Coins,
       Gems,
       Experience,
       CustomType
   }
   ```

2. Update the `RewardManager` logic to handle the new reward type.

   ```csharp
   public void DeliverRewardToPlayer(Reward reward)
   {
       switch (reward.type)
       {
           case RewardType.Coins:
               _rewardDeliveryService.DeliverCoins(reward.quantity);
               break;
           case RewardType.CustomType:
               // Custom delivery logic
               break;
       }
   }
   ```

### Extending RewardManager Events

Hook into the `OnRewardClaimed` event to implement custom logic when rewards are claimed:

```csharp
RewardManager.OnRewardClaimed += (level, reward) => Debug.Log($"Level {level} reward claimed: {reward.rewardName}");
```

---

## Debugging and Troubleshooting

- **Issue**: Rewards are not delivered.
  - **Solution**: Check if the `IRewardDeliveryService` implementation is correctly registered.

- **Issue**: Daily rewards are unavailable.
  - **Solution**: Verify the `rewardDelay` setting in the `RewardConfig` ScriptableObject.

- **Issue**: Custom reward types are not working.
  - **Solution**: Ensure the `RewardManager` logic is updated to handle the new type.

---

For further assistance, contact the plugin author or check the GitHub repository for updates and documentation.
```