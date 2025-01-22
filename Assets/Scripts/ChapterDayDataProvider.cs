using UnityEngine;
using LevelSelectionPackage.Models;
using System.Collections.Generic;
using ezygamers.cmsv1;
using System;
using PlayerProgressSystem;
public class ChapterDayDataProvider : IChapterDayDataProvider
{
    private Dictionary<int, LevelConfigSO> levelDictionary;
    private List<ChapterModel> chapters = new List<ChapterModel>();

    private PlayerProgressManager playerProgressManager;

    public void SetDataToDataProvider(Dictionary<int, LevelConfigSO> levelDictionary, PlayerProgressManager playerProgressManager) //TODO: Mention that level in this project is same as chapter
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
            ChapterModel chapter = new ChapterModel();
            // {
            //     chapterName = "Chapter " + level.Key.ToString(),
            //     chapterNumber = level.Key,
            //     days = new List<DayModel>(),
            //     isLocked = !playerProgressManager.HasMainLevelStarted(level.Key.ToString()),
            // };
            chapter.StrChapterName = "Chapter " + level.Key.ToString();
            chapter.ChapterNumber = level.Key;
            chapter.DaysObj = new List<DayModel>();
            chapter.IsLocked = !playerProgressManager.HasMainLevelStarted(level.Key.ToString());


            SetDayData(chapter.DaysObj, level.Value, level.Key);
            chapters.Add(chapter);

        }
        return chapters;
    }

    // public void SetDayData(List<DayModel> days, LevelConfigSO levelConfig, int chapterNumber) //TODO : Naming convention Fix
    // {
    //     for(int questionNo = 0; questionNo < levelConfig.Questions.Count; questionNo++)
    //     {
    //         if (!(levelConfig[questionNo].optionType == OptionType.Learning))
    //         {
    //             DayModel day = new DayModel
    //             {
    //                 // dayName = (questionNo).ToString(),
    //                 dayNumber = questionNo,
    //                 isLocked = !playerProgressManager.HasStartedSubLevel(levelConfig[questionNo].questionNo.ToString()) 
    //                                 && !playerProgressManager.HasCompletedSubLevel(levelConfig[questionNo].questionNo.ToString()),
    //                 chapterNumber = chapterNumber
    //             };
    //             days.Add(day);
    //         }
    //     }
    //     Debug.Log($"Day count {days.Count}.");
        
    // }

    public void SetDayData(List<DayModel> dayList, LevelConfigSO levelConfig, int chapterNumber)
    {
        if (dayList == null) throw new ArgumentNullException(nameof(dayList));
        if (levelConfig == null) throw new ArgumentNullException(nameof(levelConfig));

        for (int questionIndex = 0; questionIndex < levelConfig.Questions.Count; questionIndex++)
        {
            QuestionBaseSO question = levelConfig.Questions[questionIndex];

            if (question.optionType != OptionType.Learning)
            {
                bool isFirstDay = questionIndex == 1;
                bool isLocked = !isFirstDay && 
                                !playerProgressManager.HasStartedSubLevel(question.questionNo.ToString()) &&
                                !playerProgressManager.HasCompletedSubLevel(question.questionNo.ToString());

                var dayObj = new DayModel();
                dayObj.DayNumber = questionIndex;
                dayObj.IsLocked = isLocked;
                dayObj.ChapterNumber = chapterNumber;


                dayList.Add(dayObj);
            }
        }

        Debug.Log($"Total days initialized: {dayList.Count}");
    }

}
