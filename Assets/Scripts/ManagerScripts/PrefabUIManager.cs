using UnityEngine;
using TMPro;
using UnityEngine.UI;
using VContainer;
using ezygamers.cmsv1;
using System.Collections.Generic;
using ezygamers.dragndropv1;

public class PrefabUIManager : MonoBehaviour
{
    public OptionContainer LearningImageData;
    public Text SubLevelText;
    public AudioSource SubLevelAudioSource;
    public GameObject DraggableObject;
    public List<OptionContainer> OptionHolders;
    public List<DropHandler> dropHandlers;
    //[SerializeField] private GameObject center;
    private CMSGameEventManager eventManager;

    [Inject]
    public void Construct(CMSGameEventManager eventManager)
    {
        this.eventManager = eventManager;
    }

    // Method to load question data and populate the UI based on the type of Question
    public void LoadQuestionData(QuestionBaseSO questionData)
    {
        ResetUI();  // Reset the UI before loading new data

        if (questionData.optionType == OptionType.Learning)
        {
            LoadLearningContent(questionData);
        }
        else if (questionData.optionType == OptionType.TwoImageOpt)
        {
            LoadQuestionContent(questionData);
        }
        else if (questionData.optionType == OptionType.FourImageOpt)
        {
            LoadQuestionContent(questionData);
        }
        
    }

    private void LoadLearningContent(QuestionBaseSO questionData)
    {
        QuestionUIHelper.SetImage(LearningImageData.image, questionData.learningImage.image);
        QuestionUIHelper.SetText(LearningImageData.optionText, questionData.questionText.text);
        QuestionUIHelper.SetText(SubLevelText, questionData.hindiText.text);
        QuestionUIHelper.SetAudio(SubLevelAudioSource, questionData.questionAudio.audioClip);
    }

    private void LoadQuestionContent(QuestionBaseSO questionData)
    {
        QuestionUIHelper.SetText(SubLevelText, questionData.hindiText.text);
        QuestionUIHelper.SetOptionsData(questionData.imageOptions, OptionHolders, dropHandlers);
        QuestionUIHelper.SetAudio(SubLevelAudioSource, questionData.questionAudio.audioClip);
    }

    private void ResetUI()
    {
        //DraggableObject.transform.position = center.transform.position;
        QuestionUIHelper.ResetImage(LearningImageData.image);
        QuestionUIHelper.ResetText(LearningImageData.optionText);
        QuestionUIHelper.ResetText(SubLevelText);
        QuestionUIHelper.ResetAudio(SubLevelAudioSource);

        foreach (var imgOption in OptionHolders)
        {
            QuestionUIHelper.ResetImage(imgOption.image);
        }
    }
}