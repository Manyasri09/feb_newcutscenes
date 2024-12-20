using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RewardSystem
{
    public interface IRewardStateService
    {
        // Level rewards
        bool IsLevelRewardClaimed(int level);
        void MarkLevelRewardClaimed(int level);

        // Daily rewards
        bool IsDailyRewardAvailable(double requiredDelay);
        int GetDailyRewardIndex();
        void AdvanceDailyRewardIndex();
        void MarkDailyRewardClaimedNow();

        // Weekly rewards
        bool IsWeeklyRewardAvailable(double requiredDelay);
        int GetWeeklyRewardIndex();
        void AdvanceWeeklyRewardIndex();
        void MarkWeeklyRewardClaimedNow();

        // Reset all rewards states
        void ResetAllRewards();
    }
}