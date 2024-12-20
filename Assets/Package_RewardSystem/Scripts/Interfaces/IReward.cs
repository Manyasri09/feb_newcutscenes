using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static RewardSystem.Reward;

public interface IReward
{
    string RewardID { get; }
    string Description { get; }
    int Quantity { get; }
    UnityEngine.Sprite Icon { get; }
    RewardType Type { get; }
    DeliveryType Delivery { get; }
    string CustomData { get; }
}
