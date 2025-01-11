using RewardSystem;
using PlayerProgressSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCustomRewardDeliveryService : IRewardDeliveryService
{
    PlayerProgressManager progressManager;

    public MyCustomRewardDeliveryService(PlayerProgressManager progressManager)
    {
        this.progressManager = progressManager;
    }

    /// <summary>
    /// Delivers the specified reward to the player's progress manager.
    /// </summary>
    /// <param name="reward">The reward to be delivered.</param>
    public void DeliverReward(IReward reward)
    {

        switch (reward.Type)
        {
            case RewardType.Coins: progressManager.UpdateCoins(reward.Quantity); break;
            case RewardType.Experience: progressManager.UpdateExperience(reward.Quantity); break;
        }
    }
}
