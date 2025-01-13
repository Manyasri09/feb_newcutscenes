using UnityEngine;
using TMPro;
using UnityEngine.UI;
using VContainer;
using ezygamers.cmsv1;
using System.Collections.Generic;
using ezygamers.dragndropv1;

/// <summary>
/// Manages UI prefabs for different types of questions in a learning/quiz system.
/// Handles loading, displaying, and resetting UI elements for various question types including
/// learning content, image options, and line-drawing questions.
/// </summary>
public class PrefabUIManager : MonoBehaviour
{
    /// <summary>
    /// Container for the learning mode image and text.
    /// Used when displaying educational content before questions.
    /// </summary>
    public OptionContainer LearningImageData;

    /// <summary>
    /// Displays the current sub-level or question text, typically in Hindi.
    /// </summary>
    public Text SubLevelText;

    /// <summary>
    /// Audio source for playing sub-level related sounds.
    /// </summary>
    public AudioSource SubLevelAudioSource;

    /// <summary>
    /// Audio source for playing correct answer feedback.
    /// </summary>
    [SerializeField] private AudioSource correctAudio;

    /// <summary>
    /// Reference to the draggable object used in drag-and-drop questions.
    /// </summary>
    public GameObject DraggableObject;

    /// <summary>
    /// List of containers for multiple-choice options.
    /// Used in two-image and four-image question types.
    /// </summary>
    public List<OptionContainer> OptionHolders;

    /// <summary>
    /// List of containers for line-drawing nodes.
    /// Used in line connection questions.
    /// </summary>
    public List<LineDrawNode> nodeContainers;

    /// <summary>
    /// List of drop handlers for drag-and-drop functionality.
    /// </summary>
    public List<DropHandler> dropHandlers;

    /// <summary>
    /// Reference to the game event manager for CMS-related events.
    /// </summary>
    private CMSGameEventManager eventManager;

    /// <summary>
    /// Dependency injection method for setting up the event manager.
    /// </summary>
    /// <param name="eventManager">The CMS game event manager instance.</param>
    [Inject]
    public void Construct(CMSGameEventManager eventManager)
    {
        this.eventManager = eventManager;
    }

    /// <summary>
    /// Loads and displays question data based on the question type.
    /// Resets UI before loading new content.
    /// </summary>
    /// <param name="questionData">The question data to load.</param>
    public void LoadQuestionData(QuestionBaseSO questionData)
    {
        ResetUI();  // Reset the UI before loading new data

        switch (questionData.optionType)
        {
            case OptionType.Learning:
                LoadLearningContent(questionData);
                break;
            case OptionType.TwoImageOpt:
                LoadQuestionContent(questionData);
                break;
            case OptionType.FourImageOpt:
                LoadQuestionContent(questionData);
                break;
            case OptionType.LineQuestion:
                LoadLineQuestionContent(questionData);
                break;
        }
    }

    /// <summary>
    /// Loads content for learning-type questions, including image, text, and audio.
    /// </summary>
    /// <param name="questionData">The learning content data to display.</param>
    private void LoadLearningContent(QuestionBaseSO questionData)
    {
        QuestionUIHelper.SetImage(LearningImageData.image, questionData.learningImage.image);
        QuestionUIHelper.SetText(LearningImageData.optionText, questionData.questionText.text);
        QuestionUIHelper.SetText(SubLevelText, questionData.hindiText.text);
        QuestionUIHelper.SetAudio(correctAudio, questionData.questionAudio.audioClip);
        QuestionUIHelper.SetAudio(SubLevelAudioSource, questionData.questionAudio.audioClip);
    }

    /// <summary>
    /// Loads content for multiple-choice questions with image options.
    /// Handles both two-image and four-image question types.
    /// </summary>
    /// <param name="questionData">The question data containing options and correct answer.</param>
    private void LoadQuestionContent(QuestionBaseSO questionData)
    {
        QuestionUIHelper.SetText(SubLevelText, questionData.hindiText.text);
        QuestionUIHelper.SetOptionsData(questionData.options, OptionHolders, dropHandlers);
        QuestionUIHelper.SetAudio(correctAudio, questionData.questionAudio.audioClip);
        QuestionUIHelper.SetAudio(SubLevelAudioSource, questionData.questionAudio.audioClip);
    }

    /// <summary>
    /// Loads content for line-drawing questions.
    /// Sets up node text and values for line connection puzzles.
    /// </summary>
    /// <param name="questionData">The line question data containing node information.</param>
    private void LoadLineQuestionContent(QuestionBaseSO questionData)
    {
        if (questionData.questionText.text != null && SubLevelText != null)
        {
            QuestionUIHelper.SetText(SubLevelText, questionData.hindiText.text);
        }

        if (nodeContainers.Count != 0 && questionData.options.Count != 0)
        {
            for (int i = 0; i < questionData.options.Count; i++)
            {
                if (nodeContainers[i].GetComponentInChildren<Text>() != null && questionData.options[i] != null)
                {
                    nodeContainers[i].GetComponent<LineDrawNode>().nodeValue = questionData.options[i].text;
                    QuestionUIHelper.SetText(nodeContainers[i].GetComponentInChildren<Text>(), questionData.options[i].text);
                }
            }
        }
        QuestionUIHelper.SetAudio(correctAudio, questionData.questionAudio.audioClip);
    }

    /// <summary>
    /// Resets all UI elements to their default state.
    /// Called before loading new question content to ensure clean state.
    /// </summary>
    private void ResetUI()
    {
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