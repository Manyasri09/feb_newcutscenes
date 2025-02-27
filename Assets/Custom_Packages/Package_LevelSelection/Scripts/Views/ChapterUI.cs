using LevelSelectionPackage.Models;
using LevelSelectionPackage.Controllers;
using UnityEngine;
using UnityEngine.UI;

namespace LevelSelectionPackage.Views
{
    /// <summary>
    /// Represents the UI component for a single chapter in the game.
    /// Handles the visual representation and user interactions for individual chapter elements.
    /// </summary>
    public class ChapterUI : MonoBehaviour
    {
        /// <summary>
        /// Text component that displays the chapter's name in the UI
        /// </summary>
        [SerializeField] private Text chapterNameText;
        /// <summary>
        /// Visual indicator that shows when the chapter is locked
        /// This GameObject is toggled based on the chapter's lock status
        /// </summary>
        [SerializeField] private GameObject lockIcon;
        /// <summary>
        /// Button component that handles user interaction with the chapter
        /// Can be disabled when the chapter is locked
        /// </summary>
        [SerializeField] private Button chapterButton;

        [SerializeField] private Outline activeOutline;

        [SerializeField] private Color inactiveOutlineColor;
        [SerializeField] private Color activeOutlineColor;

        // Reference to the chapter's data model
        private ChapterModel chapterDataObj;
        // Reference to the controller for handling chapter interactions
        private ChapterDayController controllerRef;

        /// <summary>
        /// Initializes the Chapter UI element with the provided data and controller reference.
        /// Sets up the visual elements and configures user interactions based on the chapter's state.
        /// </summary>
        /// <param name="chapter">The data model containing chapter information</param>
        /// <param name="controller">Reference to the controller for handling chapter interactions</param>
        public void Initialize(ChapterModel chapterObj, ChapterDayController controller)
        {
            // Store references for later use
            chapterDataObj = chapterObj;
            controllerRef = controller;

            
            // Update chapter name display if text component exists
            if (chapterNameText != null)
                chapterNameText.text = chapterObj.StrChapterName;
            // Toggle lock icon visibility based on chapter state
            if (lockIcon != null)
            {
                // chapterNameText.gameObject.SetActive(!chapter.IsLocked);
                lockIcon.SetActive(chapterObj.IsLocked);
            }

            // Adjust button interactability if locked
            if (chapterButton != null)
            {
                // Disable button if chapter is locked
                chapterButton.interactable = !chapterObj.IsLocked;
                // Clear any existing listeners to prevent duplicates
                chapterButton.onClick.RemoveAllListeners();
                // Add click handler to the button
                chapterButton.onClick.AddListener(OnChapterClicked);
            }

            activeOutline.effectColor = chapterObj.IsActive ? activeOutlineColor : inactiveOutlineColor;
        }

        /// <summary>
        /// Handles the chapter button click event.
        /// Forwards the interaction to the controller for processing.
        /// </summary>
        private void OnChapterClicked()
        {
            // Forward to the controller
            controllerRef.OnChapterSelected(chapterDataObj);
        }

        

    }
}