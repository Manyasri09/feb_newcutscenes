using UnityEngine;
using LevelSelectionPackage.Models;
using System.Collections.Generic;
using ezygamers.cmsv1;
using VContainer;
using PlayerProgressSystem;
public class ChapterDayDataProvider : IChapterDayDataProvider
{
    private Dictionary<int, LevelConfigSO> levelDictionary;
    private List<ChapterModel> chapters = new List<ChapterModel>();

    private PlayerProgressManager playerProgressManager;

    public void SetDatatoDataProvider(Dictionary<int, LevelConfigSO> levelDictionary, PlayerProgressManager playerProgressManager) //TODO: Mention that level in this project is same as chapter
    {
        this.levelDictionary = levelDictionary;
        this.playerProgressManager = playerProgressManager;
    }

    public List<ChapterModel> GetChapters()
    {
        chapters.Clear();
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
                chapterNumber = level.Key,
                days = new List<DayModel>(),
                isLocked = !playerProgressManager.HasMainLevelStarted(level.Key.ToString()),
            };

            SetDayData(chapter.days, level.Value, level.Key);
            chapters.Add(chapter);

        }
        return chapters;
    }

    public void SetDayData(List<DayModel> days, LevelConfigSO levelConfig, int chapterNumber)
    {
        for(int i = 0; i < levelConfig.Questions.Count; i++)
        {
            if (!(levelConfig[i].optionType == OptionType.Learning))
            {
                DayModel day = new DayModel
                {
                    dayName = (i + 1).ToString(),
                    dayNumber = i,
                    isLocked = !playerProgressManager.HasStartedSubLevel(levelConfig[i].questionNo.ToString()) && !playerProgressManager.HasCompletedSubLevel(levelConfig[i].questionNo.ToString()),
                    chapterNumber = chapterNumber
                };
                days.Add(day);
            }
        }
    }




}
