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

    public void DeliverReward(IReward reward)
    {

        switch (reward.Type)
        {
            case RewardType.Coins: progressManager.UpdateCoins(reward.Quantity); break;
            case RewardType.Experience: progressManager.UpdateExperience(reward.Quantity); break;
        }
    }
}
