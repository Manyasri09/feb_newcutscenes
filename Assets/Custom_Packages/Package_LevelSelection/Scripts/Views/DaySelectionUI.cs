using UnityEngine;
using UnityEngine.UI;
using LevelSelectionPackage.Models;
using LevelSelectionPackage.Controllers;

namespace LevelSelectionPackage.Views
{
    /// <summary>
    /// Manages the UI display for days within a chapter. This class handles both the chapter title
    /// display and the container of individual day UI elements. It's responsible for populating
    /// and updating the day selection interface when a chapter is selected.
    /// </summary>
    public class DaySelectionUI : MonoBehaviour
    {
        /// <summary>
        /// Text component that displays the currently selected chapter's title
        /// </summary>
        [SerializeField] private Text chapterTitle;

        /// <summary>
        /// Transform component that acts as the parent container for all day UI elements.
        /// All day prefabs will be instantiated as children of this transform.
        /// </summary>
        [SerializeField] private Transform dayContainer;
        
        /// <summary>
        /// Populates the day selection UI with data from the provided chapter.
        /// This method handles both the chapter title update and the creation of individual day UI elements.
        /// </summary>
        /// <param name="chapter">The chapter model containing the days to display</param>
        /// <param name="dayUIPrefab">The prefab to use for creating each day's UI element</param>
        /// <param name="controller">Reference to the controller for handling day interactions</param>
        public void Populate(ChapterModel chapter, DayUI dayUIPrefab, ChapterDayController controller)
        {
            // Update chapter title if the text component exists
            if (chapterTitle != null)
                chapterTitle.text = chapter.chapterName;

            // Clean up any existing day UI elements to prevent duplicates
            // and memory leaks
            foreach (Transform child in dayContainer)
            {
                Destroy(child.gameObject);
            }

            // Create new UI elements for each day in the chapter
            // Each day is instantiated as a child of the container and
            // initialized with its corresponding data
            foreach (var dayData in chapter.days)
            {
                var dayObj = Instantiate(dayUIPrefab, dayContainer);
                dayObj.Initialize(dayData, controller);
            }
        }
    }
}