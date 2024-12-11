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
    [SerializeField] private AudioSource correctAudio;
    public GameObject DraggableObject;
    public List<OptionContainer> OptionHolders;
    public List<LineDrawNode> nodeContainers;
    public List<DropHandler> dropHandlers;
    private CMSGameEventManager eventManager;
    public Text questionHeading;

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
        else if (questionData.optionType == OptionType.LineQuestion)
        {
             LoadLineQuestionContent(questionData);
        }

    }

    private void LoadLearningContent(QuestionBaseSO questionData)
    {
        QuestionUIHelper.SetImage(LearningImageData.image, questionData.learningImage.image);
        QuestionUIHelper.SetText(LearningImageData.optionText, questionData.questionText.text);
        QuestionUIHelper.SetText(SubLevelText, questionData.hindiText.text);
        QuestionUIHelper.SetAudio(correctAudio, questionData.questionAudio.audioClip);
    }

    private void LoadQuestionContent(QuestionBaseSO questionData)
    {
        QuestionUIHelper.SetText(SubLevelText, questionData.hindiText.text);
        QuestionUIHelper.SetOptionsData(questionData.options, OptionHolders, dropHandlers);
        //QuestionUIHelper.SetDraggableID(QuestionDropHandler, questionData.correctOptionID);
        QuestionUIHelper.SetAudio(correctAudio, questionData.questionAudio.audioClip);
    }

    private void LoadLineQuestionContent(QuestionBaseSO questionData)
    {
        
        if (questionData.questionText.text != null && SubLevelText != null)
        {
            QuestionUIHelper.SetText(SubLevelText, questionData.hindiText.text);
            QuestionUIHelper.SetText(questionHeading, questionData.questionInfo);
        }

        for (int i = 0; i < questionData.options.Count; i++)
        {
            if (nodeContainers[i].GetComponentInChildren<Text>() != null && questionData.options[i] != null)
            {
                nodeContainers[i].GetComponent<LineDrawNode>().nodeValue = questionData.options[i].text;
                QuestionUIHelper.SetText(nodeContainers[i].GetComponentInChildren<Text>(), questionData.options[i].text);
            }
        }
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