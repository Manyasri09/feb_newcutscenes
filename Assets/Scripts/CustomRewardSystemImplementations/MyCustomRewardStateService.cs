using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RewardSystem;
using PlayerProgressSystem;
using System;
using JetBrains.Annotations;
using System.Globalization;

public class MyCustomRewardStateService : IRewardStateService
{

    private readonly PlayerProgressManager playerProgressManager;

    public MyCustomRewardStateService(PlayerProgressManager progressManager)
    {
        playerProgressManager = progressManager ?? throw new System.ArgumentNullException(nameof(progressManager));
    }


    #region Level Rewards

    public bool IsLevelRewardClaimed(int level)
    {
        return playerProgressManager.HasLevelRewardClaimed(level);
    }

    public void MarkLevelRewardClaimed(int level)
    {
        playerProgressManager.SetLevelRewardClaimed(level);
    }

    #endregion

    #region Daily Reward

    public bool IsDailyRewardAvailable(double requiredDelay)
    {
        string lastClaimDate = playerProgressManager.GetLastRewardClaimedDateTime();
        Debug.Log(lastClaimDate);

        if (string.IsNullOrEmpty(lastClaimDate))
        {
            Debug.LogWarning("Last claim date is null or empty. Defaulting to reward available.");
            return true; // If no date is stored, assume the reward is available
        }

        DateTime lastClaimedRewardDateTime = DateTime.Parse(lastClaimDate);
        DateTime currentDateTime = DateTime.Now;

        //Debug.Log(currentDateTime);

        double elapsedTime = (currentDateTime - lastClaimedRewardDateTime).TotalSeconds;
        Debug.Log($"elapsed = {elapsedTime}");
        Debug.Log($"requried = {requiredDelay}");
        if (elapsedTime >= requiredDelay)
        {
            return true;
        }
        return false;
    }
    public int GetDailyRewardIndex()
    {
        return playerProgressManager.GetDailyRewardIndex();
    }
    public void AdvanceDailyRewardIndex()
    {
        int index = GetDailyRewardIndex();
        playerProgressManager.SetDailyRewardIndex(++index);
    }
    public void MarkDailyRewardClaimedNow()
    {
        playerProgressManager.SetLastRewardClaimedDateTime(DateTime.Now);
    }

    #endregion

    #region Weekly rewards
    public bool IsWeeklyRewardAvailable(double requiredDelay)
    {
        return false;
    }
    public int GetWeeklyRewardIndex()
    {
        return 0;
    }
    public void AdvanceWeeklyRewardIndex()
    {

    }
    public void MarkWeeklyRewardClaimedNow()
    {

    }

    // Reset all rewards states
    public void ResetAllRewards()
    {

    }

    #endregion
}
