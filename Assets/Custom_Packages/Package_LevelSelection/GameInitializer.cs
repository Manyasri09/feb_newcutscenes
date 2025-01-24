using UnityEngine;
using LevelSelectionPackage.Controllers;
using LevelSelectionPackage.Views;
using LevelSelectionPackage.Models;

namespace MyChapterDayPackage
{
    /// <summary>
    /// Initializes the Chapter-Day system with mock data.
    /// Attach this component to a GameObject in your initial scene to set up the system.
    /// </summary>
    public class GameInitializer : MonoBehaviour
    {
        /// <summary>
        /// Reference to the main view manager in the scene
        /// </summary>
        [SerializeField] private ChapterDayViewManager viewManager;

        void Start()
        {
            if (viewManager == null)
            {
                Debug.LogError("ChapterDayViewManager reference is missing! Please assign it in the inspector.");
                return;
            }

            // Create and set up the system components
            IChapterDayDataProvider dataProvider = new MockDataProvider();
            ChapterDayController controller = new ChapterDayController(dataProvider, viewManager);
            
            // Initialize the view manager
            viewManager.Initialize(controller);
            controller.sendChaptersToView();

            Debug.Log("Chapter-Day system initialized successfully!");
        }
    }
}