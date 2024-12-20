using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RewardSystem
{
    public interface IRewardManager
    {
        event System.Action<int, IReward> OnLevelRewardClaimed;
        event System.Action<int, IReward> OnDailyRewardClaimed;
        event System.Action<int, IReward> OnWeeklyRewardClaimed;

        bool IsLevelRewardClaimed(int level);
        void ClaimLevelReward(int level);

        bool IsDailyRewardAvailable();
        void ClaimDailyReward();

        bool IsWeeklyRewardAvailable();
        void ClaimWeeklyReward();

        void ResetAllRewards();
    }
}