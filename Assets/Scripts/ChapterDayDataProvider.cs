using UnityEngine;
using LevelSelectionPackage.Models;
using System.Collections.Generic;
using ezygamers.cmsv1;
using VContainer;
using PlayerProgressSystem;
public class ChapterDayDataProvider : IChapterDayDataProvider
{
    private Dictionary<int, LevelConfiggSO> levelDictionary;
    private List<ChapterModel> chapters = new List<ChapterModel>();

    private PlayerProgressManager playerProgressManager;

    public void SetDatatoDataProvider(Dictionary<int, LevelConfiggSO> levelDictionary, PlayerProgressManager playerProgressManager) //TODO: Mention that level in this project is same as chapter
    {
        this.levelDictionary = levelDictionary;
        this.playerProgressManager = playerProgressManager;
    }

    public List<ChapterModel> GetChapters()
    {
        if (levelDictionary == null || levelDictionary.Count == 0)
        {
            Debug.LogError("levelDictionary or its chapters are null/empty.");
            return new List<ChapterModel>();
        }

        foreach (var level in levelDictionary)
        {
            ChapterModel chapter = new ChapterModel
            {
                chapterName = "Chapter " + level.Key.ToString(),
                days = new List<DayModel>(),
                isLocked = !playerProgressManager.HasCompletedMainLevel(level.Key.ToString())
            };

            SetDayData(chapter.days, level.Value, level.Key.ToString());
            chapters.Add(chapter);

        }
        return chapters;
    }

    public void SetDayData(List<DayModel> days, LevelConfiggSO levelConfig, string chapterName)
    {
        foreach (var question in levelConfig.question)
        {
            DayModel day = new DayModel
            {
                dayName = question.questionNo.ToString(),
                isLocked = !playerProgressManager.HasCompletedSubLevel(question.questionNo.ToString()),
                chapterName = chapterName
            };
            days.Add(day);
        }
    }
}
