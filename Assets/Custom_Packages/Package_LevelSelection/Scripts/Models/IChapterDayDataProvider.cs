using System.Collections.Generic;

namespace LevelSelectionPackage.Models
{
    /// <summary>
    /// Defines the contract for data providers in the Chapter-Day system.
    /// This interface specifies the methods required to retrieve chapter data,
    /// allowing for different implementations (e.g., local storage, server-side, etc.)
    /// without affecting the rest of the system.
    /// </summary>
    public interface IChapterDayDataProvider
    {
        /// <summary>
        /// Retrieves the complete list of chapters available in the system.
        /// Implementations should handle the actual data retrieval logic,
        /// whether from local storage, server, or other data sources.
        /// </summary>
        /// <returns>A list of ChapterModel objects containing chapter data and their associated days</returns>
        List<ChapterModel> GetChapters();
    }
}