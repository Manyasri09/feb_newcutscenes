using RewardSystem;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using VContainer;

public class DailyRewardsUI : MonoBehaviour
{
    [SerializeField] private GameObject dailyRewardsPanel;
    [SerializeField] private List<DailyRewardSlotUI> slots;

    public IRewardManager rewardManager;

    [Inject]
    public void Construct(IRewardManager rewardManager)
    {
        this.rewardManager = rewardManager;
    }

    private void Awake()
    {
        // Optionally hide the panel initially
        if (dailyRewardsPanel != null)
            dailyRewardsPanel.SetActive(false);
    }

    private void OnEnable()
    {
        RefreshUI();
    }

    public void ShowDailyRewardsPopup()
    {
        RefreshUI();
        dailyRewardsPanel.SetActive(true);
        dailyRewardsPanel.transform.SetAsLastSibling();
    }

    public void CloseDailyRewardsPopup()
    {
        dailyRewardsPanel.SetActive(false);    
    }

    private void RefreshUI()
    {
        if (rewardManager == null) return;

        // Retrieve config from your reward manager (assuming it provides a method or property)
        var config = rewardManager.GetRewardConfig();
        if (config == null || config.dailyRewards == null || config.dailyRewards.Count == 0)
            return;

        // Update each slot with data from the config
        for (int i = 0; i < slots.Count; i++)
        {
            if (i < config.dailyRewards.Count)
            {
                Reward reward = config.dailyRewards[i];
                slots[i].gameObject.SetActive(true);
                slots[i].SetupSlot(i + 1, reward);
            }
            else
            {
                // Hide any extra slots
                slots[i].gameObject.SetActive(false);
            }
        }

        //// Update the state of the claim button
        //if (claimButton != null)
        //    claimButton.interactable = rewardManager.IsDailyRewardAvailable();
    }
}
