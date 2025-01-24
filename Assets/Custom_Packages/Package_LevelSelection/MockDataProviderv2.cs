using System.Collections.Generic;
using LevelSelectionPackage.Models;
using UnityEngine;

namespace MyChapterDayPackage
{
    /// <summary>
    /// Provides mock data for testing and demonstration purposes.
    /// Implements IChapterDayDataProvider to provide sample chapters and days.
    /// </summary>
    public class MockDataProvider : IChapterDayDataProvider
    {
        public List<ChapterModel> GetChapters()
        {
            // Create a list to hold our mock chapters
            var chapters = new List<ChapterModel>();

            // Chapter 1 - Unlocked with some locked days
            var chapter1 = new ChapterModel
            {
                StrChapterName = "Chapter 1: Beginning",
                IsLocked = false,
                DaysObj = new List<DayModel>
                {
                    new DayModel { StrDayName = "1", IsLocked = false, IsCompleted = true },
                    new DayModel { StrDayName = "2", IsLocked = false},
                    new DayModel { StrDayName = "3", IsLocked = true },
                    new DayModel { StrDayName = "4", IsLocked = true }
                }
            };
            chapters.Add(chapter1);

            // Chapter 2 - Unlocked with all days locked
            var chapter2 = new ChapterModel
            {
                StrChapterName = "Chapter 2: Intermediate",
                IsLocked = false,
                DaysObj = new List<DayModel>
                {
                    new DayModel { StrDayName = "1", IsLocked = false },
                    new DayModel { StrDayName = "2", IsLocked = true },
                    new DayModel { StrDayName = "3", IsLocked = true }
                }
            };
            chapters.Add(chapter2);

            // Chapter 3 - Locked
            var chapter3 = new ChapterModel
            {
                StrChapterName = "Chapter 3: Advanced",
                IsLocked = true,
                DaysObj = new List<DayModel>
                {
                    new DayModel { StrDayName = "1", IsLocked = true },
                    new DayModel { StrDayName = "2", IsLocked = true },
                    new DayModel { StrDayName = "3", IsLocked = true }
                }
            };
            chapters.Add(chapter3);

            Debug.Log($"Mock data provider created {chapters.Count} chapters");
            return chapters;
        }
    }
}