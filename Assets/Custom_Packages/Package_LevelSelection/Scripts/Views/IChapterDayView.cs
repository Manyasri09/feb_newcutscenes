using System.Collections.Generic;
using LevelSelectionPackage.Models;

/// <summary>
/// Defines the contract for views in the Chapter-Day system.
/// This interface establishes the required methods that any view implementation
/// must provide to handle chapter and day-related UI operations. It serves as
/// the bridge between the controller and the view layer in the MVC architecture.
/// </summary>
public interface IChapterDayView
{
    /// <summary>
    /// Displays a list of chapters in the UI.
    /// Implementations should handle the visual representation of multiple chapters,
    /// including their states and interactive elements.
    /// </summary>
    /// <param name="chapters">The list of chapter models to be displayed</param>
    public void DisplayChapters(List<ChapterModel> chapters);

    /// <summary>
    /// Shows a notification when a user attempts to access a locked day.
    /// Implementations should provide appropriate feedback to inform the user
    /// that the selected day is not yet accessible.
    /// </summary>
    /// <param name="day">The locked day model that was attempted to be accessed</param>
    public void ShowDayLockedMessage(DayModel day);

    /// <summary>
    /// Loads and displays the content for a specific day.
    /// Implementations should handle the transition to and presentation of
    /// the day's content when it is selected by the user.
    /// </summary>
    /// <param name="day">The day model containing the content to be loaded</param>
    public void LoadDayContent(DayModel day);

    /// <summary>
    /// Shows a notification when a user attempts to access a locked chapter.
    /// Implementations should provide appropriate feedback to inform the user
    /// that the selected chapter is not yet accessible.
    /// </summary>
    /// <param name="chapter">The locked chapter model that was attempted to be accessed</param>
    public void ShowChapterLockedMessage(ChapterModel chapter);

    /// <summary>
    /// Loads and displays the content for a specific chapter.
    /// Implementations should handle the transition to and presentation of
    /// the chapter's content, including any contained days.
    /// </summary>
    /// <param name="chapter">The chapter model containing the content to be loaded</param>
    public void LoadChapterContent(ChapterModel chapter);
}