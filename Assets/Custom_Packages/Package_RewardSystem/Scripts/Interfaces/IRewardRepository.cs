using RewardSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RewardSystem
{
    public interface IRewardRepository
    {
        RewardConfig GetRewardConfig();
        IReward FetchLevelReward(int level);
        IReward FetchDailyReward(int index);
        IReward FetchWeeklyReward(int index);
    }
}
