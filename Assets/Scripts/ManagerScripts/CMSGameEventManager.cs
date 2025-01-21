using System;
using UnityEngine;
using ezygamers.cmsv1;
public class CMSGameEventManager
{
    // Event for loading level data (passing LevelConfigSO)
    public static Action<LevelConfigSO> OnLoadLevelData;

    // Event for loading question data (passing QuestionBaseSO)
    public static Action<QuestionBaseSO> OnLoadQuestionData;

    //Event when opton is Selected -rohan37kumar
    public static Action<String> OnAnswerSelected;

    // Method to trigger the loading of a level
    public void LoadLevel(LevelConfigSO levelData)
    {
        Debug.Log("Loading level data...");
        OnLoadLevelData?.Invoke(levelData);
    }

    // Method to trigger the loading of a question -rohan37kumar
    public void LoadNextQuestion(QuestionBaseSO questionData)
    {
        Debug.Log("Loading question number..."+questionData.questionNo);
        OnLoadQuestionData?.Invoke(questionData);
    }

    //Method called after Option is selected or Image is Dropped -rohan37kumar
    public void OptionSelected(String optionString)
    {
        Debug.Log("checking answer...");
        OnAnswerSelected?.Invoke(optionString);
    }
}