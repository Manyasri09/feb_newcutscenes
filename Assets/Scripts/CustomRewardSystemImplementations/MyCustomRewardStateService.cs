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

    // Check if the level reward has been claimed
    public bool IsLevelRewardClaimed(int level)
    {
        // Return true if the level reward has been claimed, otherwise return false
        return playerProgressManager.HasLevelRewardClaimed(level);
    }

    // Mark the level reward as claimed
    public void MarkLevelRewardClaimed(int level)
    {
        // Set the level reward as claimed
        playerProgressManager.SetLevelRewardClaimed(level);
    }

    #endregion

    #region Daily Reward

    /// <summary>
    /// Checks if the daily reward is available for the player based on the required delay.
    /// </summary>
    /// <param name="requiredDelay">The required delay in seconds before the daily reward can be claimed again.</param>
    /// <returns>True if the daily reward is available, false otherwise.</returns>
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
    /// <summary>
    /// Gets the current index of the daily reward.
    /// </summary>
    /// <returns>The current index of the daily reward.</returns>
    public int GetDailyRewardIndex()
    {
        return playerProgressManager.GetDailyRewardIndex();
    }
    /// <summary>
    /// Advances the index of the daily reward.
    /// </summary>
    /// <remarks>
    /// This method retrieves the current daily reward index, increments it, and then updates the daily reward index in the player progress manager.
    /// </remarks>
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
