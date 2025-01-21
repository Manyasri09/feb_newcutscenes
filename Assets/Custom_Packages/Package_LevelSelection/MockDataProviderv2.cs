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
                chapterName = "Chapter 1: Beginning",
                isLocked = false,
                days = new List<DayModel>
                {
                    new DayModel { dayName = "1", isLocked = false },
                    new DayModel { dayName = "2", isLocked = false },
                    new DayModel { dayName = "3", isLocked = true },
                    new DayModel { dayName = "4", isLocked = true }
                }
            };
            chapters.Add(chapter1);

            // Chapter 2 - Unlocked with all days locked
            var chapter2 = new ChapterModel
            {
                chapterName = "Chapter 2: Intermediate",
                isLocked = false,
                days = new List<DayModel>
                {
                    new DayModel { dayName = "1", isLocked = false },
                    new DayModel { dayName = "2", isLocked = true },
                    new DayModel { dayName = "3", isLocked = true }
                }
            };
            chapters.Add(chapter2);

            // Chapter 3 - Locked
            var chapter3 = new ChapterModel
            {
                chapterName = "Chapter 3: Advanced",
                isLocked = true,
                days = new List<DayModel>
                {
                    new DayModel { dayName = "1", isLocked = true },
                    new DayModel { dayName = "2", isLocked = true },
                    new DayModel { dayName = "3", isLocked = true }
                }
            };
            chapters.Add(chapter3);

            Debug.Log($"Mock data provider created {chapters.Count} chapters");
            return chapters;
        }
    }
}