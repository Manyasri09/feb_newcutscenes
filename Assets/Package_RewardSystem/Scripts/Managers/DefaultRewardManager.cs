using PlayerProgressSystem;
using RewardSystem;
using System.Collections.Generic;
using UnityEngine;
using System;
//using VContainer;

namespace RewardSystem
{
    public class DefaultRewardManager : IRewardManager
    {
        public event System.Action<int, IReward> OnLevelRewardClaimed;
        public event System.Action<int, IReward> OnDailyRewardClaimed;
        public event System.Action<int, IReward> OnWeeklyRewardClaimed;

        private readonly IRewardRepository _repository;
        private readonly IRewardStateService _stateService;
        private readonly IRewardDeliveryService _deliveryService;

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
        public bool IsLevelRewardClaimed(int level)
        {
            return _stateService.IsLevelRewardClaimed(level);
        }

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
        public bool IsDailyRewardAvailable()
        {
            var config = _repository.GetRewardConfig();
            return _stateService.IsDailyRewardAvailable(config.dailyRewardDelay);
        }

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
        public bool IsWeeklyRewardAvailable()
        {
            var config = _repository.GetRewardConfig();
            return _stateService.IsWeeklyRewardAvailable(config.weeklyRewardDelay);
        }

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

        public void ResetAllRewards()
        {
            _stateService.ResetAllRewards();
            Debug.Log("All rewards have been reset via state service.");
        }
    }
}
