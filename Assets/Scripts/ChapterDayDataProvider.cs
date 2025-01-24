using UnityEngine;
using LevelSelectionPackage.Models;
using System.Collections.Generic;
using ezygamers.cmsv1;
using System;
using PlayerProgressSystem;
using VContainer;
public class ChapterDayDataProvider : IChapterDayDataProvider
{
    private Dictionary<int, LevelConfigSO> levelDictionary;
    private List<ChapterModel> chapters = new List<ChapterModel>();

    private PlayerProgressManager playerProgressManager;

    // [Inject]
    // public void Construct(PlayerProgressManager playerProgressManager)
    // {
    //     this.playerProgressManager = playerProgressManager;
    // }

    public void SetDataToDataProvider(Dictionary<int, LevelConfigSO> levelDictionary, PlayerProgressManager playerProgressManager) //TODO: Mention that level in this project is same as chapter
    {
        this.levelDictionary = levelDictionary;
        this.playerProgressManager = playerProgressManager;
    }

    //This method returns a list of ChapterModels
    public List<ChapterModel> GetChapters()
    {
        //Clear the chapters list
        chapters.Clear();
        //Check if the levelDictionary is null or empty
        if (levelDictionary == null || levelDictionary.Count == 0)
        {
            //Log an error if the levelDictionary or its chapters are null/empty
            Debug.LogError("levelDictionary or its chapters are null/empty.");
            //Return an empty list of ChapterModels
            return new List<ChapterModel>();
        }

        //Loop through each level in the levelDictionary
        foreach (var level in levelDictionary)
        {
            //Create a new ChapterModel
            ChapterModel chapter = new ChapterModel();
            //Set the chapter name
            chapter.StrChapterName = "Chapter " + level.Key.ToString();
            //Set the chapter number
            chapter.ChapterNumber = level.Key;
            //Create a new list of DayModels
            chapter.DaysObj = new List<DayModel>();
            //Set the chapter to locked if the player has not started the main level
            chapter.IsLocked = !playerProgressManager.HasMainLevelStarted(level.Key.ToString());
            //Set the chapter to active if the chapter number is equal to the last started main level
            chapter.IsActive = chapter.ChapterNumber == int.Parse(playerProgressManager.GetLastStartedMainLevel()) ? true : false;

            //Loop through each question in the levelConfigSO
            SetDayDataForChapter(chapter.DaysObj, level.Value, level.Key);
            chapters.Add(chapter);

        }
        return chapters;
    }

    // This method sets the day data for a chapter
    public void SetDayDataForChapter(List<DayModel> dayList, LevelConfigSO levelConfig, int chapterNumber)
    {
        // Check if the dayList is null and throw an ArgumentNullException if it is
        if (dayList == null) throw new ArgumentNullException(nameof(dayList));
        // Check if the levelConfig is null and throw an ArgumentNullException if it is
        if (levelConfig == null) throw new ArgumentNullException(nameof(levelConfig));

        // Loop through the questions in the levelConfig
        for (int subLevelNumber = 0; subLevelNumber < levelConfig.Questions.Count; subLevelNumber++)
        {
            // Get the question at the current subLevelNumber
            QuestionBaseSO question = levelConfig.Questions[subLevelNumber];
           

            // Check if the question is not of type Learning
            if (question.optionType != OptionType.Learning)
            {
                // Create a DayModel object with the question, subLevelNumber, levelConfig, and chapterNumber
                DayModel dayObj = CreateDayModel(question, subLevelNumber, levelConfig, chapterNumber);
                // Add the DayModel object to the dayList
                dayList.Add(dayObj);
            }

        }

        // Log the total number of days initialized
        Debug.Log($"Total days initialized: {dayList.Count}");
    }

    private DayModel CreateDayModel(QuestionBaseSO question, int subLevelNumber, LevelConfigSO levelConfig, int chapterNumber)
    {
        DayModel dayObj = new DayModel();
        dayObj.DayNumber = subLevelNumber;
        dayObj.ChapterNumber = chapterNumber;

        bool isFirstDay = subLevelNumber == 1 && levelConfig.LevelNumber == 1; //Check if it's the first day of the first level

        bool isLocked = isFirstDay ||  HasCompletedPreviousDay(question); //  Check if the previous day is completed
        dayObj.IsLocked = !isLocked;                                      // Set the IsLocked property based on the condition

        bool isCompleted = playerProgressManager.HasCompletedSubLevel(question.questionNo.ToString()); // Check if the current day is completed
        dayObj.IsCompleted = isCompleted;
    
        bool isActive = isLocked && !isCompleted; // If the day is not locked and not completed, it's active
        dayObj.IsActive = isActive;

        return dayObj;

    }

    private bool HasCompletedPreviousDay(QuestionBaseSO question)
    {
        if (question.optionType == OptionType.LineQuestion)
        {
            return playerProgressManager.HasCompletedSubLevel((question.questionNo - 1).ToString()); // line question doesnt have learning question that is skipped so we are checking for previous question
        }
        return playerProgressManager.HasCompletedSubLevel((question.questionNo - 2).ToString()); // learning question is skipped so we are checking for previous question
    }

}
