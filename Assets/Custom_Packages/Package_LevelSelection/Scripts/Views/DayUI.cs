using UnityEngine;
using UnityEngine.UI;
using LevelSelectionPackage.Models;
using LevelSelectionPackage.Controllers;

namespace LevelSelectionPackage.Views
{
    /// <summary>
    /// Represents the UI component for a single day within a chapter.
    /// Handles the visual representation and user interactions for individual day elements,
    /// including the display of the day's name, lock status, and click functionality.
    /// </summary>
    public class DayUI : MonoBehaviour
    {
        /// <summary>
        /// Text component that displays the day's name in the UI
        /// </summary>
        [SerializeField] private Text dayNameText;

        /// <summary>
        /// Visual indicator that shows when the day is locked
        /// This GameObject is toggled based on the day's lock status
        /// </summary>
        [SerializeField] private GameObject lockIcon;

        /// <summary>
        /// Button component that handles user interaction with the day
        /// Can be disabled when the day is locked
        /// </summary>
        [SerializeField] private Button dayButton;

        // Reference to the day's data model
        private DayModel dayData;
        // Reference to the controller for handling day interactions
        private ChapterDayController controllerRef;

        /// <summary>
        /// Initializes the Day UI element with the provided data and controller reference.
        /// Sets up the visual elements and configures user interactions based on the day's state.
        /// </summary>
        /// <param name="day">The data model containing day information</param>
        /// <param name="controller">Reference to the controller for handling day interactions</param>
        public void Initialize(DayModel day, ChapterDayController controller)
        {
            // Store references for later use
            dayData = day;
            controllerRef = controller;

            // Update day name display if text component exists
            if (dayNameText != null)
                dayNameText.text = day.dayName;

            // Toggle lock icon visibility based on day state
            if (lockIcon != null)
            {
                dayNameText.gameObject.SetActive(!day.isLocked);
                lockIcon.SetActive(day.isLocked);
            }

            // Configure button interactivity and click handling
            if (dayButton != null)
            {
                // Disable button if day is locked
                dayButton.interactable = !day.isLocked;
                // Clear any existing listeners to prevent duplicates
                dayButton.onClick.RemoveAllListeners();
                // Add click handler
                dayButton.onClick.AddListener(OnDayClicked);
            }
        }

        /// <summary>
        /// Handles the day button click event.
        /// Forwards the interaction to the controller for processing.
        /// </summary>
        private void OnDayClicked()
        {
            controllerRef.OnDaySelected(dayData);
        }
    }
}