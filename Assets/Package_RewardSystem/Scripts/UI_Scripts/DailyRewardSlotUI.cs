using UnityEngine;
using UnityEngine.UI;
using RewardSystem;
using TMPro;

public class DailyRewardSlotUI : MonoBehaviour
{
    [SerializeField] private TMP_Text dayLabel;
    [SerializeField] private Image rewardIcon;
    [SerializeField] private TMP_Text quantityLabel;

    public void SetupSlot(int dayNumber, Reward reward)
    {
        if (dayLabel != null)
            dayLabel.text = $"Day {dayNumber}";

        if (rewardIcon != null && reward.icon != null)
            rewardIcon.sprite = reward.icon;

        if (quantityLabel != null)
            quantityLabel.text = reward.quantity.ToString();
    }
}