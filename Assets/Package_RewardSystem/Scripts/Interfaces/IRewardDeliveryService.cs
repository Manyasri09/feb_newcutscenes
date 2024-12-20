using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RewardSystem
{
    public interface IRewardDeliveryService
    {
        void DeliverReward(IReward reward);
    }
}
