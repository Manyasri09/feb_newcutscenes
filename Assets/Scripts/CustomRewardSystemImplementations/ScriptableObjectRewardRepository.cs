using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RewardSystem
{
    public class ScriptableObjectRewardRepository : IRewardRepository
    {
        private readonly RewardConfig _config;

        public ScriptableObjectRewardRepository(RewardConfig config)
        {
            _config = config;
        }

        public RewardConfig GetRewardConfig() => _config;

        public IReward FetchLevelReward(int level)
        {
            foreach (var lr in _config.levelRewards)
            {
                if (lr.level == level)
                    return lr.reward;
            }
            return null;
        }

        public IReward FetchDailyReward(int index)
        {
            var rewards = _config.dailyRewards;
            if (rewards == null || rewards.Count == 0) return null;
            int loopedIndex = index % rewards.Count;
            return rewards[loopedIndex];
        }

        public IReward FetchWeeklyReward(int index)
        {
            var rewards = _config.weeklyRewards;
            if (rewards == null || rewards.Count == 0) return null;
            int loopedIndex = index % rewards.Count;
            return rewards[loopedIndex];
        }
    }
}