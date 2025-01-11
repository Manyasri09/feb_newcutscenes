using ezygamers.cmsv1;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

/// <summary>
/// Manages the UI prefab for line-drawing questions, handling the loading and display
/// of question data and configuring line drawing nodes.
/// </summary>
public class LineDragPrefabManager : MonoBehaviour
{
    /// <summary>
    /// Text component that displays the main question text.
    /// </summary>
    public Text question;

    /// <summary>
    /// List of nodes that the user can draw lines between.
    /// Each node represents a point in the line-drawing puzzle.
    /// </summary>
    public List<LineDrawNode> drawNodes;

    /// <summary>
    /// Reference to the game event manager for handling CMS-related events.
    /// Injected via VContainer dependency injection.
    /// </summary>
    private CMSGameEventManager eventManager;

    /// <summary>
    /// Dependency injection method for setting up the event manager reference.
    /// Called automatically by VContainer.
    /// </summary>
    /// <param name="eventManager">The CMS game event manager instance to inject.</param>
    [Inject]
    public void Construct(CMSGameEventManager eventManager)
    {
        this.eventManager = eventManager;
    }

    /// <summary>
    /// Loads question data into the UI components if it's a line-drawing type question.
    /// Acts as a type-check gateway before loading specific content.
    /// </summary>
    /// <param name="questionData">The question data to load.</param>
    public void LoadQuestionData(QuestionBaseSO questionData)
    {
        if (questionData.optionType == OptionType.LineQuestion)
        {
            LoadLineDragContent(questionData);
        }
    }

    /// <summary>
    /// Configures the UI elements with the line-drawing question content.
    /// Sets up the question text and configures each drawing node with its corresponding option text.
    /// </summary>
    /// <param name="questionData">The line question data to load.</param>
    /// <remarks>
    /// This method:
    /// 1. Sets the main question text (in Hindi)
    /// 2. Configures each drawing node with its corresponding option text
    /// 3. Logs debug information at key points for troubleshooting
    /// </remarks>
    private void LoadLineDragContent(QuestionBaseSO questionData)
    {
        Debug.Log("LoadLineDragContent function called");
        if (questionData.questionText.text != null && question != null)
        {
            Debug.Log("LoadLineDragContent function if statement called");
            QuestionUIHelper.SetText(question, questionData.hindiText.ToString());
        }

        for (int i = 0; i < questionData.options.Count; i++)
        {
            Debug.Log("LoadLineDragContent function for statement called");
            if (drawNodes[i].GetComponentInChildren<Text>() != null && questionData.options[i] != null)
            {
                Debug.Log("LoadLineDragContent function if statement inside for called");
                // Set both the node's internal value and displayed text
                drawNodes[i].GetComponent<LineDrawNode>().nodeValue = questionData.options[i].text;
                QuestionUIHelper.SetText(drawNodes[i].GetComponentInChildren<Text>(), questionData.options[i].text);
            }
        }
    }
}