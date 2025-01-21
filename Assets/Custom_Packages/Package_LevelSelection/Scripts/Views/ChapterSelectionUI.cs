using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using LevelSelectionPackage.Models;
using LevelSelectionPackage.Controllers;

namespace LevelSelectionPackage.Views
{
    /// <summary>
    /// Handles the UI management for chapter selection in the game.
    /// This class is responsible for creating and managing the visual representation
    /// of chapters in the UI hierarchy.
    /// </summary>
    public class ChapterSelectionUI : MonoBehaviour
    {
        /// <summary>
        /// The Transform component that will serve as the parent container
        /// for all instantiated chapter UI elements. All chapter prefabs
        /// will be instantiated as children of this transform.
        /// </summary>
        [SerializeField] private Transform chapterContainer;
        // Keep track of instantiated chapter objects
        
        

        /// <summary>
        /// Creates and initializes UI elements for each chapter in the provided list.
        /// This method handles both the cleanup of existing UI elements and the creation
        /// of new ones.
        /// </summary>
        /// <param name="chapters">List of chapter data to be displayed</param>
        /// <param name="chapterPrefab">The prefab to use for creating each chapter's UI</param>
        /// <param name="controller">Reference to the controller for handling chapter interactions</param>
        public void DisplayChaptersSelectionUI(List<ChapterModel> chapters, ChapterUI chapterPrefab, ChapterDayController controller)
        {
            // Clean up any existing chapter UI elements to prevent duplicates
            // and memory leaks
            Debug.Log($"<color=red>Chapter container has {chapterContainer.childCount} children before cleanup.</color>");
            foreach (Transform child in chapterContainer)
            {
                Debug.Log($"Destroying child object: {child.gameObject.name}");
                Destroy(child.gameObject);
            }
            Debug.Log($"<color=red>Chapter container has {chapterContainer.childCount} children after cleanup.</color>");

            // Create new UI elements for each chapter in the provided list
            // Each chapter is instantiated and initialized with its corresponding data
            Debug.Log($"Creating {chapters.Count} chapter UI elements.");
            foreach (var chapterData in chapters)
            {
                var chapterObj = Instantiate(chapterPrefab, chapterContainer);
                chapterObj.Initialize(chapterData, controller);
                Debug.Log($"Initialized chapter UI element for chapter: {chapterData.chapterName}");
            }

        }

    }
}