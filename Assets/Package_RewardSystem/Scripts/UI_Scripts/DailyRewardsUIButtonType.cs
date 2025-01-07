using UnityEngine;
using UnityEngine.UI;
using RewardSystem;
using VContainer;
using System.Collections.Generic;
using System.Collections;
using System;

public class DailyRewardsUIButtonType : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject panel;             // The main panel for daily rewards
    //[SerializeField] private Image notificationImage;      // The notification Icon
    public Button claimButton;           // The Claim button
    public Button closeButton;           // The Close button
    [SerializeField] private List<DailyRewardSlotUI> slots; // Pre-created slot references in the scene
    [SerializeField] public Color highLightColor;

    public static event Action OnRewardClaimed;

    // The manager that actually knows if a reward is available, etc.
    public IRewardManager rewardManager;
    private IRewardStateService _stateService;

    [Inject]
    public void Construct(IRewardManager rewardManager, IRewardStateService stateService)
    {
        this.rewardManager = rewardManager;
        _stateService = stateService;
    }

    private void Awake()
    {
        // Optionally hide the panel initially
        if (panel != null)
            panel.SetActive(false);

        SetupSlotClickHandlers();
        
    }

    private void OnEnable()
    {
        RefreshUI();
    }

    // This method is called when the claim button is clicked
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

        int index = _stateService.GetDailyRewardIndex() % slots.Count;


        // Update each slot with data from the config
        for (int i = 0; i < slots.Count; i++)
        {
            if (i < config.dailyRewards.Count)
            {
                Reward reward = config.dailyRewards[i];
                slots[i].gameObject.SetActive(true);
                slots[i].SetupSlot(i + 1, reward);

                if (i == index)
                {
                    Image slotBackGround = slots[i].GetComponent<Image>();
                    slotBackGround.color = highLightColor;
                }
                    
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

    // This method sets up click handlers for each slot in the slots list
    private void SetupSlotClickHandlers()
    {
        // Loop through each slot in the slots list
        for (int i = 0; i < slots.Count; i++)
        {
            // Get the current slot
            var slot = slots[i];
            // Get the button component of the current slot
            var button = slot.GetComponent<Button>();
            // If the button component does not exist, add it to the slot
            if (button == null)
            {
                button = slot.gameObject.AddComponent<Button>();
            }
            
            int index = i; // Capture the index for the lambda
            button.onClick.AddListener(() => OnSlotClicked(index));
        }
    }

    // This method is called when a slot is clicked
    private void OnSlotClicked(int slotIndex)
    {
        // Get the current index of the daily reward
        int currentIndex = _stateService.GetDailyRewardIndex() % slots.Count;
        // Check if the clicked slot is the current index and if the daily reward is available
        if (slotIndex == currentIndex && rewardManager.IsDailyRewardAvailable())
        {
            // Invoke the OnRewardClaimed event
            OnRewardClaimed?.Invoke();
        }
    }

}
