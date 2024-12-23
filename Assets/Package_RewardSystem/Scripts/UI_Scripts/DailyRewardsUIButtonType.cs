using UnityEngine;
using UnityEngine.UI;
using RewardSystem;
using VContainer;
using System.Collections.Generic;

public class DailyRewardsUIButtonType : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject panel;             // The main panel for daily rewards
    //[SerializeField] private Image notificationImage;      // The notification Icon
    public Button claimButton;           // The Claim button
    //[SerializeField] private Button closeButton;           // Close/X button
    //[SerializeField] private Button openButton;            // Open button
    [SerializeField] private List<DailyRewardSlotUI> slots; // Pre-created slot references in the scene

    // The manager that actually knows if a reward is available, etc.
    public IRewardManager rewardManager;

    [Inject]
    public void Construct(IRewardManager rewardManager)
    {
        this.rewardManager = rewardManager;
    }

    private void Awake()
    {
        // Optionally hide the panel initially
        if (panel != null)
            panel.SetActive(false);

        // Wire up buttons
        //if (claimButton != null)
        //    claimButton.onClick.AddListener(OnClaimButtonClicked);

        //if (closeButton != null)
        //    closeButton.onClick.AddListener(OnCloseButtonClicked);

        //if (openButton != null)
        //    openButton.onClick.AddListener(OnOpenButtonClicked);
    }

    private void OnEnable()
    {
        RefreshUI();
    }

    public void OnClaimButtonClicked()
    {
        if (rewardManager == null)
        {
            Debug.LogWarning("No IRewardManager assigned to DailyRewardsUI.");
            return;
        }

        if (rewardManager.IsDailyRewardAvailable())
        {
            rewardManager.ClaimDailyReward();
            RefreshUI();
            
        }
        else
        {
            Debug.Log("Daily reward not available yet!");
        }
    }

    private void OnCloseButtonClicked()
    {
        HidePanel();
    }

    private void OnOpenButtonClicked()
    {
        ShowPanel();
    }

    public void ShowPanel()
    {
        if (panel != null)
            panel.SetActive(true);

        RefreshUI();
    }

    public void HidePanel()
    {
        if (panel != null)
            panel.SetActive(false);
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

        // Update the state of the claim button
        if (claimButton != null)
            claimButton.interactable = rewardManager.IsDailyRewardAvailable();
    }
}
