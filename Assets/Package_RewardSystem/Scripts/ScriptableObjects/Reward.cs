using UnityEngine;

namespace RewardSystem
{
    [CreateAssetMenu(fileName = "Reward", menuName = "RewardSystem/Reward")]
    public class Reward : ScriptableObject, IReward
    {
        public string rewardID;         // Name of the reward
        public string description;        // Description for UI or tooltip
        public int quantity;                 // Reward value (e.g., coins, XP, items)
        public Sprite icon;               // Optional: Icon for the reward
        public RewardType type;           // Reward type (Daily, Weekly, etc.)
        public DeliveryType rewardDelivery;
        public string customData;

        public string RewardID => rewardID;
        public string Description => description;
        public int Quantity => quantity;
        public Sprite Icon => icon;
        public RewardType Type => type;
        public DeliveryType Delivery => rewardDelivery;
        public string CustomData => customData;
    }
}
