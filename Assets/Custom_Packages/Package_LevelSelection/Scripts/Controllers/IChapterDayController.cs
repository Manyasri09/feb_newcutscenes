using LevelSelectionPackage.Models;

namespace LevelSelectionPackage.Controllers
{
    /// <summary>
    /// Defines the contract for controllers in the Chapter-Day system.
    /// This interface establishes the required methods that any controller implementation
    /// must provide to handle the business logic between data providers and views.
    /// It ensures consistency in how chapter and day interactions are processed
    /// throughout the application.
    /// </summary>
    public interface IChapterDayController
    {
        /// <summary>
        /// Initiates the loading of chapters from the data provider.
        /// This method serves as the entry point for populating the UI with chapter data
        /// and should be called during the initial setup of the system.
        /// </summary>
        void LoadChapters();

        /// <summary>
        /// Processes the selection of a specific day by the user.
        /// This method handles the business logic for day selection, including
        /// checking lock status and determining appropriate view updates.
        /// </summary>
        /// <param name="day">The day model that was selected by the user</param>
        void OnDaySelected(DayModel day);

        /// <summary>
        /// Processes the selection of a specific chapter by the user.
        /// This method handles the business logic for chapter selection, including
        /// checking lock status and determining appropriate view updates.
        /// </summary>
        /// <param name="chapter">The chapter model that was selected by the user</param>
        void OnChapterSelected(ChapterModel chapter);
    }
}