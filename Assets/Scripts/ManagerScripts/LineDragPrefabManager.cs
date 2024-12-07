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
        
    }

    private void LoadLineDragContent(QuestionBaseSO questionData)
    {
        Debug.Log("LoadLineDragContent function called");
        if (questionData.questionText.text != null && question != null)
        {
            Debug.Log("LoadLineDragContent function if statement called");
            QuestionUIHelper.SetText(question, questionData.questionText.text);
        }

        for (int i = 0; i < questionData.options.Count; i++)
        {
            Debug.Log("LoadLineDragContent function for statement called");
            if (drawNodes[i].GetComponentInChildren<Text>() != null && questionData.options[i] != null)
            {
                Debug.Log("LoadLineDragContent function if statement inside for called");
                drawNodes[i].GetComponent<LineDrawNode>().nodeValue = questionData.options[i].text;
                QuestionUIHelper.SetText(drawNodes[i].GetComponentInChildren<Text>(), questionData.options[i].text);
            }
        }
    }
    
}
