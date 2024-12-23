
# Reward System Manual  

The **Reward System Plugin** is a Unity package designed to streamline the process of managing and distributing rewards in your game. This manual provides detailed instructions for integrating, configuring, and customizing the plugin effectively.  

---  

## Table of Contents  

1. [Overview](#overview)  
2. [Installation](#installation)  
3. [Getting Started](#getting-started)  
4. [DefaultRewardDeliveryService](#defaultrewarddeliveryservice)  
5. [Customizing Reward Delivery](#customizing-reward-delivery)  
6. [Dependency Injection with VContainer](#dependency-injection-with-vcontainer)  
7. [Advanced Configuration](#advanced-configuration)  
8. [Troubleshooting](#troubleshooting)  

---  

## Overview  

The Reward System Plugin simplifies reward management in Unity games with built-in support for:  

- **Daily Rewards**  
- **Weekly Rewards**  
- **Level-Based Rewards**  
- **Custom Reward Types**  

The modular design allows seamless integration into projects of any complexity, including support for dependency injection using **VContainer**.  

---  

## Installation  

1. Clone or download the plugin and copy the `RewardSystem` folder into your Unity project.  
2. Ensure your Unity project includes the following dependencies:  
   - **UnityEngine.UI**  
   - **UnityEventSystem**  
3. (Optional) For projects using VContainer, install the VContainer package via the Unity Package Manager.  

---  

## Getting Started  

### Step 1: Configure Rewards  

1. Navigate to `Assets > Create > RewardSystem > Reward` to create individual rewards.  
2. Configure the reward properties in the Inspector:  
   - **Reward ID**: Unique identifier for the reward.  
   - **Type**: Select from `Daily`, `Weekly`, or `LevelCompletion`.  
   - **Quantity**: Value of the reward (e.g., coins, XP, gems).  
   - **Description**: Optional description for UI purposes.  
   - **Icon**: Optional image for UI representation.  

3. Use a `RewardConfig` ScriptableObject to group rewards for daily, weekly, or level-based delivery.  

### Step 2: Add the Reward Manager  

1. Add the `RewardManager` component to a GameObject in your scene.  
2. Assign the `RewardConfig` ScriptableObject to the `RewardManager`.  

---  

## DefaultRewardDeliveryService  

The `DefaultRewardDeliveryService` is a ready-to-use implementation for delivering rewards without requiring custom logic.  

### Integration  

1. Add a `LifetimeScope` to your project, registering the `DefaultRewardDeliveryService`:  

   ```csharp  
   using VContainer;  
   using VContainer.Unity;  
   using RewardSystem;  

   public class RewardSystemLifetimeScope : LifetimeScope  
   {  
       [SerializeField] private RewardConfig rewardConfig;  

       protected override void Configure(IContainerBuilder builder)  
       {  
           builder.Register<IRewardRepository>(c => new ScriptableObjectRewardRepository(rewardConfig), Lifetime.Singleton);  
           builder.Register<IRewardDeliveryService, DefaultRewardDeliveryService>(Lifetime.Singleton);  
           builder.Register<IRewardManager, DefaultRewardManager>(Lifetime.Singleton);  
           builder.RegisterComponentInHierarchy<RewardManager>();  
       }  
   }  
   ```  

2. Attach the `RewardSystemLifetimeScope` to a GameObject in your scene.  

### Reward Delivery Logic  

The `DefaultRewardDeliveryService` automatically handles common reward types, such as:  

- **Coins**: Adds coins to the player’s account.  
- **Gems**: Grants premium currency.  
- **Experience**: Awards XP to the player.  

```csharp  
public class DefaultRewardDeliveryService : IRewardDeliveryService  
{  
    public void DeliverCoins(int quantity) => Debug.Log($"Delivered {quantity} Coins.");  
    public void DeliverGems(int quantity) => Debug.Log($"Delivered {quantity} Gems.");  
    public void DeliverExperience(int quantity) => Debug.Log($"Delivered {quantity} XP.");  
}  
```  

No additional configuration is needed when using `DefaultRewardDeliveryService`.  

---  

## Customizing Reward Delivery  

To implement custom logic, create a new class that implements the `IRewardDeliveryService` interface:  

```csharp  
public class CustomRewardDeliveryService : IRewardDeliveryService  
{  
    public void DeliverCoins(int quantity) => Debug.Log($"Custom: Delivered {quantity} Coins!");  
    public void DeliverGems(int quantity) => Debug.Log($"Custom: Delivered {quantity} Gems!");  
    public void DeliverExperience(int quantity) => Debug.Log($"Custom: Delivered {quantity} XP!");  
    public void DeliverCustomReward(Reward reward) => Debug.Log($"Custom Reward Delivered: {reward.rewardName}");  
}  
```  

Register the custom service in the `LifetimeScope`:  

```csharp  
builder.Register<IRewardDeliveryService, CustomRewardDeliveryService>(Lifetime.Singleton);  
```  

---  

## Dependency Injection with VContainer  

The plugin integrates seamlessly with VContainer for dependency injection. To set up:  

1. Add a custom `LifetimeScope` as shown above.  
2. Ensure that the `RewardManager` is registered in the container.  
3. Use the `LifetimeScope` to resolve dependencies for reward delivery and management.  

---  

## Advanced Configuration  

### Adding New Reward Types  

1. Extend the `RewardType` enum in `Reward.cs`:  

   ```csharp  
   public enum RewardType  
   {  
       Coins,  
       Gems,  
       Experience,  
       CustomReward  
   }  
   ```  

2. Implement custom delivery logic for the new type in your `IRewardDeliveryService`.  

### Listening to Reward Events  

Subscribe to `RewardManager` events to handle reward claims:  

```csharp  
rewardManager.OnRewardClaimed += (reward) => Debug.Log($"Claimed: {reward.rewardName} - {reward.quantity}");  
```  

---  

## Troubleshooting  

- **Rewards Not Delivered**:  
  - Ensure `IRewardDeliveryService` is correctly registered in the container.  

- **Daily Rewards Unavailable**:  
  - Verify `RewardConfig` is assigned to the `RewardManager`.  

- **Custom Rewards Not Working**:  
  - Check if the `RewardType` enum and delivery logic are updated.  

For further assistance, consult the documentation or contact the support team.  
```  

