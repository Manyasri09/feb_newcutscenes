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
        /// The display name of the chapter
        /// </summary>
        public string chapterName;

        public int chapterNumber;

        /// <summary>
        /// Collection of days contained within this chapter
        /// Each day represents a discrete content unit or level
        /// </summary>
        public List<DayModel> days;

        /// <summary>
        /// Indicates whether this chapter is currently locked
        /// Used to control access to chapter content
        /// </summary>
        public bool isLocked;

        

    }
}
