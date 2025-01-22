using UnityEngine;
using System.Collections.Generic;
using LevelSelectionPackage.Models;
using LevelSelectionPackage.Controllers;
using UnityEngine.UI;

namespace LevelSelectionPackage.Views
{
    /// <summary>
    /// Manages the UI view components for the Chapter-Day system in Unity.
    /// This class acts as the primary view layer in the MVC architecture, handling all UI-related
    /// operations and user interactions for both chapters and days.
    /// </summary>
    public class ChapterDayViewManager : MonoBehaviour, IChapterDayView
    {
        [Header("UI Setup Prefabs")]
        // Reference to the parent GameObject that contains all chapter-related UI elements
        [SerializeField] private GameObject chaptersParentPrefab;
        // Reference to the parent GameObject that contains all day-related UI elements
        [SerializeField] private GameObject daysParentPrefab;
        // Prefab for individual chapter UI elements that will be instantiated
        [SerializeField] private ChapterUI chapterUIPrefab;
        // Prefab for individual day UI elements that will be instantiated
        [SerializeField] private DayUI dayUIPrefab;

        [Space(10)]

        // Button reference for navigating back through the UI hierarchy
        public Button backButton;

        // Reference to the controller component for handling business logic and data operations
        private ChapterDayController controller;

        /// <summary>
        /// Unity callback that's called when the GameObject becomes enabled and active.
        /// Sets up event listeners for UI interactions.
        /// </summary>
        void OnEnable()
        {
            backButton.onClick.AddListener(OnBackButtonClicked);
        }

        /// <summary>
        /// Handles the back button click event.
        /// Manages navigation between chapter and day views by toggling their visibility.
        /// </summary>
        private void OnBackButtonClicked()
        {
            if (daysParentPrefab.activeSelf)
            {
                chaptersParentPrefab.SetActive(true);
                daysParentPrefab.SetActive(false);
            }
        }

        /// <summary>
        /// Initializes the view manager with a controller instance.
        /// This method should be called before any other operations are performed.
        /// </summary>
        /// <param name="chapterDayController">The controller instance that will handle business logic</param>
        public void Initialize(ChapterDayController chapterDayController)
        {
            controller = chapterDayController;
            controller.LoadChapters();
        }

        #region IChapterDayView Implementation

        /// <summary>
        /// Displays the list of available chapters in the UI.
        /// Sets up the chapter selection interface and handles the visibility of UI elements.
        /// </summary>
        /// <param name="chapters">List of chapter models to display</param>
        public void DisplayChapters(List<ChapterModel> chapters)
        {
            var chapterSelectionUI = chaptersParentPrefab.GetComponent<ChapterSelectionUI>();
            chapterSelectionUI.DisplayChaptersSelectionUI(chapters, chapterUIPrefab, controller);
            chaptersParentPrefab.SetActive(true);
            daysParentPrefab.SetActive(false);
        }

        /// <summary>
        /// Displays a message when attempting to access a locked day.
        /// Currently implements basic debug logging, but can be extended to show UI notifications.
        /// </summary>
        /// <param name="day">The locked day model</param>
        public void ShowDayLockedMessage(DayModel day)
        {
            Debug.Log($"Day {day.StrDayName} is locked!");
            // TODO: Implement UI notification system for locked content
        }

        /// <summary>
        /// Handles the loading of specific day content.
        /// Currently implements basic debug logging, but should be extended to load actual day content.
        /// </summary>
        /// <param name="day">The day model to load</param>
        public void LoadDayContent(DayModel day)
        {
            Debug.Log($"Loading Day {day.StrDayName} content...");
            // TODO: Implement actual content loading logic
        }

        /// <summary>
        /// Displays a message when attempting to access a locked chapter.
        /// Currently implements basic debug logging, but can be extended to show UI notifications.
        /// </summary>
        /// <param name="chapter">The locked chapter model</param>
        public void ShowChapterLockedMessage(ChapterModel chapter)
        {
            Debug.Log($"Chapter {chapter.StrChapterName} is locked!");
            // TODO: Implement UI notification system for locked content
        }

        /// <summary>
        /// Handles the loading of specific chapter content and sets up the day selection UI.
        /// Manages the visibility of chapter and day UI elements.
        /// </summary>
        /// <param name="chapter">The chapter model to load</param>
        public void LoadChapterContent(ChapterModel chapter)
        {
            Debug.Log($"Loading Chapter {chapter.StrChapterName} content...");
            var daySelectionUI = daysParentPrefab.GetComponent<DaySelectionUI>();
            daySelectionUI.Populate(chapter, dayUIPrefab, controller);
            chaptersParentPrefab.SetActive(false);
            daysParentPrefab.SetActive(true);
        }

        #endregion
    }
}