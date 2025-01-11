using PlayerProgressSystem;
using RewardSystem;
using System.Collections.Generic;
using UnityEngine;
using System;
//using VContainer;

namespace RewardSystem
{
    /// <summary>
    /// Default implementation of the IRewardManager interface.
    /// Handles the logic for claiming and managing different types of rewards.
    /// </summary>
    public class DefaultRewardManager : IRewardManager
    {
        /// <summary>
        /// Event triggered when a level reward is claimed.
        /// </summary>
        public event System.Action<int, IReward> OnLevelRewardClaimed;

        /// <summary>
        /// Event triggered when a daily reward is claimed.
        /// </summary>
        public event System.Action<int, IReward> OnDailyRewardClaimed;

        /// <summary>
        /// Event triggered when a weekly reward is claimed.
        /// </summary>
        public event System.Action<int, IReward> OnWeeklyRewardClaimed;

        /// <summary>
        /// Interface to interact with the reward data source (e.g., database, configuration).
        /// </summary>
        private readonly IRewardRepository _repository; 

        /// <summary>
        /// Interface to manage the reward claim states (e.g., which rewards are claimed, cooldowns).
        /// </summary>
        private readonly IRewardStateService _stateService; 

        /// <summary>
        /// Interface to handle the actual delivery of rewards to the player (e.g., update inventory, grant currency).
        /// </summary>
        private readonly IRewardDeliveryService _deliveryService; 

        /// <summary>
        /// Initializes a new instance of the DefaultRewardManager.
        /// </summary>
        /// <param name="repository">The reward repository.</param>
        /// <param name="stateService">The reward state service.</param>
        /// <param name="deliveryService">The reward delivery service.</param>
        public DefaultRewardManager(
            IRewardRepository repository,
            IRewardStateService stateService,
            IRewardDeliveryService deliveryService)
        {
            _repository = repository;
            _stateService = stateService;
            _deliveryService = deliveryService;
        }

        #region Level Rewards

        /// <summary>
        /// Checks if the level reward for the given level has already been claimed.
        /// </summary>
        /// <param name="level">The level index.</param>
        /// <returns>True if the reward has been claimed, otherwise false.</returns>
        public bool IsLevelRewardClaimed(int level)
        {
            return _stateService.IsLevelRewardClaimed(level);
        }

        /// <summary>
        /// Claims the level reward for the given level.
        /// </summary>
        /// <param name="level">The level index.</param>
        public void ClaimLevelReward(int level)
        {
            if (IsLevelRewardClaimed(level))
            {
                Debug.Log("Level reward already claimed.");
                return;
            }

            var reward = _repository.FetchLevelReward(level);
            if (reward == null)
            {
                Debug.LogWarning("No reward configured for this level.");
                return;
            }

            _deliveryService.DeliverReward(reward);
            _stateService.MarkLevelRewardClaimed(level);
            OnLevelRewardClaimed?.Invoke(level, reward); 
        }

        #endregion

        #region Daily Rewards

        /// <summary>
        /// Checks if the daily reward is currently available.
        /// </summary>
        /// <returns>True if the daily reward is available, otherwise false.</returns>
        public bool IsDailyRewardAvailable()
        {
            var config = _repository.GetRewardConfig();
            return _stateService.IsDailyRewardAvailable(config.dailyRewardDelay);
        }

        /// <summary>
        /// Claims the current daily reward.
        /// </summary>
        public void ClaimDailyReward()
        {
            if (!IsDailyRewardAvailable())
            {
                Debug.Log("Daily reward not available yet.");
                return;
            }

            int index = _stateService.GetDailyRewardIndex();
            var reward = _repository.FetchDailyReward(index);
            if (reward == null)
            {
                Debug.LogWarning("No daily reward configured.");
                return;
            }

            _deliveryService.DeliverReward(reward);
            _stateService.MarkDailyRewardClaimedNow();
            _stateService.AdvanceDailyRewardIndex();
            OnDailyRewardClaimed?.Invoke(index, reward);
        }

        #endregion

        #region Weekly Rewards

        /// <summary>
        /// Checks if the weekly reward is currently available.
        /// </summary>
        /// <returns>True if the weekly reward is available, otherwise false.</returns>
        public bool IsWeeklyRewardAvailable()
        {
            var config = _repository.GetRewardConfig();
            return _stateService.IsWeeklyRewardAvailable(config.weeklyRewardDelay);
        }

        /// <summary>
        /// Claims the current weekly reward.
        /// </summary>
        public void ClaimWeeklyReward()
        {
            if (!IsWeeklyRewardAvailable())
            {
                Debug.Log("Weekly reward not available yet.");
                return;
            }

            int index = _stateService.GetWeeklyRewardIndex();
            var reward = _repository.FetchWeeklyReward(index);
            if (reward == null)
            {
                Debug.LogWarning("No weekly reward configured.");
                return;
            }

            _deliveryService.DeliverReward(reward);
            _stateService.MarkWeeklyRewardClaimedNow();
            _stateService.AdvanceWeeklyRewardIndex();
            OnWeeklyRewardClaimed?.Invoke(index, reward);
        }

        #endregion

        /// <summary>
        /// Retrieves the current reward configuration.
        /// </summary>
        /// <returns>The reward configuration.</returns>
        public RewardConfig GetRewardConfig()
        {
            return _repository.GetRewardConfig();
        }

        /// <summary>
        /// Resets the claim states for all reward types.
        /// </summary>
        public void ResetAllRewards()
        {
            _stateService.ResetAllRewards();
            Debug.Log("All rewards have been reset via state service.");
        }
    }
}