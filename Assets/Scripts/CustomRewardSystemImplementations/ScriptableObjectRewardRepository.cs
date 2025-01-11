using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RewardSystem
{
    /// <summary>
    /// Repository implementation that manages game rewards using ScriptableObject configurations.
    /// This class provides access to level-based, daily, and weekly rewards defined in a RewardConfig.
    /// </summary>
    public class ScriptableObjectRewardRepository : IRewardRepository
    {
        /// <summary>
        /// The configuration object containing all reward definitions.
        /// </summary>
        private readonly RewardConfig _config;

        /// <summary>
        /// Initializes a new instance of the repository with the specified reward configuration.
        /// </summary>
        /// <param name="config">The RewardConfig ScriptableObject containing reward definitions.</param>
        public ScriptableObjectRewardRepository(RewardConfig config)
        {
            _config = config;
        }

        /// <summary>
        /// Returns the current reward configuration.
        /// </summary>
        /// <returns>The RewardConfig instance used by this repository.</returns>
        public RewardConfig GetRewardConfig() => _config;

        /// <summary>
        /// Retrieves the reward associated with a specific player level.
        /// </summary>
        /// <param name="level">The player level to fetch the reward for.</param>
        /// <returns>The reward for the specified level, or null if no reward is defined for that level.</returns>
        public IReward FetchLevelReward(int level)
        {
            foreach (var lr in _config.levelRewards)
            {
                if (lr.level == level)
                    return lr.reward;
            }
            return null;
        }

        /// <summary>
        /// Retrieves a daily reward based on the provided index.
        /// The index loops around to the beginning if it exceeds the number of available rewards.
        /// </summary>
        /// <param name="index">The index of the daily reward to fetch.</param>
        /// <returns>The daily reward at the specified index, or null if no daily rewards are configured.</returns>
        public IReward FetchDailyReward(int index)
        {
            var rewards = _config.dailyRewards;
            if (rewards == null || rewards.Count == 0) return null;
            int loopedIndex = index % rewards.Count;
            return rewards[loopedIndex];
        }

        /// <summary>
        /// Retrieves a weekly reward based on the provided index.
        /// The index loops around to the beginning if it exceeds the number of available rewards.
        /// </summary>
        /// <param name="index">The index of the weekly reward to fetch.</param>
        /// <returns>The weekly reward at the specified index, or null if no weekly rewards are configured.</returns>
        public IReward FetchWeeklyReward(int index)
        {
            var rewards = _config.weeklyRewards;
            if (rewards == null || rewards.Count == 0) return null;
            int loopedIndex = index % rewards.Count;
            return rewards[loopedIndex];
        }
    }
}