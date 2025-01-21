using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ezygamers.cmsv1;

namespace LevelSelectionPackage.Models
{
    /// <summary>
    /// Represents a single day within a chapter.
    /// This class serves as a data model for individual content units or levels.
    /// The [Serializable] attribute allows Unity to serialize this class for
    /// saving/loading and inspector visibility.
    /// </summary>
    [System.Serializable]
    public class DayModel
    {
        /// <summary>
        /// The display name of the day
        /// </summary>
        public string dayName;

        /// <summary>
        /// The display number of the day
        /// </summary>
        public int dayNumber;

        /// <summary>
        /// Indicates whether this day is currently locked
        /// Used to control access to day content
        /// </summary>
        public bool isLocked;
        /// <summary>
        /// Name of the chapter this day belongs to
        /// </summary>
        public int chapterNumber;  
    }
}
