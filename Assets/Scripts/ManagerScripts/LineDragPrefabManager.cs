using ezygamers.cmsv1;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class LineDragPrefabManager : MonoBehaviour
{
    public Text question;
    public List<LineDrawNode> drawNodes;


    private CMSGameEventManager eventManager;

    [Inject]
    public void Construct(CMSGameEventManager eventManager)
    {
        this.eventManager = eventManager;
    }


    public void LoadQuestionData(QuestionBaseSO questionData)
    {
        if (questionData.optionType == OptionType.LineQuestion)
        {
            LoadLineDragContent(questionData);
        }
        //if (questionData.questionText.text != null && question != null)
        //{
        //    question.text = questionData.questionText.text;
        //}

        //for (int i = 0; i < questionData.options.Count; i++)
        //{
        //    if (drawNodes[i].GetComponentInChildren<Text>() != null && questionData.options[i] != null)
        //    {
        //        drawNodes[i].GetComponent<LineDrawNode>().nodeValue = questionData.options[i].text;
        //        drawNodes[i].GetComponentInChildren<Text>().text = questionData.options[i].text;
        //    }
        //}
    }

    private void LoadLineDragContent(QuestionBaseSO questionData)
    {
        if (questionData.questionText.text != null && question != null)
        {
            QuestionUIHelper.SetText(question, questionData.questionText.text);
        }

        for (int i = 0; i < questionData.options.Count; i++)
        {
            if (drawNodes[i].GetComponentInChildren<Text>() != null && questionData.options[i] != null)
            {
                drawNodes[i].GetComponent<LineDrawNode>().nodeValue = questionData.options[i].text;
                QuestionUIHelper.SetText(drawNodes[i].GetComponentInChildren<Text>(), questionData.options[i].text);
            }
        }
    }
    //private void OnEnable()
    //{
    //    // Subscribe to the event to load question data
    //    CMSGameEventManager.OnLoadQuestionData += LoadQuestionData;
    //}

    //private void OnDisable()
    //{
    //    // Unsubscribe from the event to prevent memory leaks
    //    CMSGameEventManager.OnLoadQuestionData -= LoadQuestionData;
    //}
}
