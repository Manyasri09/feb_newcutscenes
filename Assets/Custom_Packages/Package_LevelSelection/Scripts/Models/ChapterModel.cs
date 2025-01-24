using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelSelectionPackage.Models
{
    /// <summary>
    /// Represents a chapter in the game's content structure.
    /// This class serves as a data model for chapters and their contained days.
    /// The [Serializable] attribute allows Unity to serialize this class for
    /// saving/loading and inspector visibility.
    /// </summary>
    [System.Serializable]
    public class ChapterModel
    {
        /// <summary>
        /// Gets or sets the display name of the chapter.
        /// </summary>
        public string StrChapterName { get; set; }

        /// <summary>
        /// Gets or sets the numerical identifier for the chapter.
        /// </summary>
        public int ChapterNumber { get; set; }

        /// <summary>
        /// Gets or sets the collection of days contained within this chapter.
        /// Each day represents a discrete content unit or level.
        /// </summary>
        public List<DayModel> DaysObj { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this chapter is currently locked.
        /// Used to control access to chapter content.
        /// </summary>
        public bool IsLocked { get; set; }

        public bool IsActive { get; set; }
    }
}
