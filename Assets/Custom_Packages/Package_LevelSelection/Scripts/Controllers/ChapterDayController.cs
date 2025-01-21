using System.Collections.Generic;
using LevelSelectionPackage.Models;
using LevelSelectionPackage.Views;

namespace LevelSelectionPackage.Controllers
{
    /// <summary>
    /// The Controller component in the MVC architecture for the Chapter-Day system.
    /// Orchestrates the flow of data between the Model (IChapterDayDataProvider) and
    /// View (IChapterDayView) layers, handling user interactions and updating the UI accordingly.
    /// This class ensures separation of concerns by mediating all interactions between
    /// the data provider and view components.
    /// </summary>
    public class ChapterDayController : IChapterDayController
    {
        /// <summary>
        /// Reference to the data provider interface that handles data operations
        /// such as retrieving chapters and their associated days
        /// </summary>
        private IChapterDayDataProvider dataProvider;

        /// <summary>
        /// Reference to the view interface that handles UI operations
        /// such as displaying chapters and showing lock messages
        /// </summary>
        private IChapterDayView view;
        List<ChapterModel> chapters = new List<ChapterModel>();

        

        /// <summary>
        /// Initializes a new instance of the ChapterDayController.
        /// Sets up the connections between the data provider and view components.
        /// </summary>
        /// <param name="provider">The data provider implementation for accessing chapter and day data</param>
        /// <param name="view">The view implementation for handling UI updates and user interactions</param>
        public ChapterDayController(IChapterDayDataProvider provider, IChapterDayView view)
        {
            dataProvider = provider;
            this.view = view;
        }

        /// <summary>
        /// Retrieves the list of chapters from the data provider and updates the view.
        /// This method serves as the initial entry point for populating the UI with chapter data.
        /// </summary>
        public void LoadChapters()
        {
            // Fetch chapters from the data provider
            chapters = dataProvider.GetChapters();
            // Update the view with the retrieved chapters
            
        }

        public void sendChaptersToView()
        {
            view.DisplayChapters(chapters);

        }


        /// <summary>
        /// Handles user selection of a specific day.
        /// Determines whether to show a locked message or load the day's content
        /// based on the day's locked status.
        /// </summary>
        /// <param name="day">The day model that was selected by the user</param>
        public void OnDaySelected(DayModel day)
        {
            // Check if the day is locked and handle accordingly
            if (day.isLocked)
            {
                view.ShowDayLockedMessage(day);
            }
            else
            {
                view.LoadDayContent(day);
            }
        }

        /// <summary>
        /// Handles user selection of a specific chapter.
        /// Determines whether to show a locked message or load the chapter's content
        /// based on the chapter's locked status.
        /// </summary>
        /// <param name="chapter">The chapter model that was selected by the user</param>
        public void OnChapterSelected(ChapterModel chapter)
        {
            // Check if the chapter is locked and handle accordingly
            if (chapter.isLocked)
            {
                view.ShowChapterLockedMessage(chapter);
            }
            else
            {
                view.LoadChapterContent(chapter);
            }
        }
    }
}