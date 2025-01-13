using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowAcknowledgementPanel : MonoBehaviour
{

    public GameObject correctAcknowledgement;
    public GameObject mistakeAcknowledgement;

    public WordAnimationSettings wordAnimationSettings;

    private void OnEnable()
    {
        GameManager.OnQuestionResult += HandleQuestionResult;
    }

    private void OnDisable()
    {
        GameManager.OnQuestionResult -= HandleQuestionResult;
    }
    private void HandleQuestionResult(bool isCorrect)
    {
        float delay = wordAnimationSettings.moveDuration + 
                      wordAnimationSettings.moveDelay + 
                      wordAnimationSettings.delay + 
                      wordAnimationSettings.staggerDelay;

        if (isCorrect)
        {
            QuestionSolved(delay);
        }
        else
        {
            QuestionNotSolved(delay);
        }
    }

    public void QuestionSolved(float delay)
    {
        StartCoroutine(ShowCorrectAcknowledgementPanelAfterDelay(delay));
    }

    IEnumerator ShowCorrectAcknowledgementPanelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        correctAcknowledgement.SetActive(true);
        mistakeAcknowledgement.SetActive(false);
    }

    public void QuestionNotSolved(float delay)
    {
        StartCoroutine(ShowMistakeAcknowledgementPanelAfterDelay(delay));
    }

    IEnumerator ShowMistakeAcknowledgementPanelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        mistakeAcknowledgement.SetActive(true);
        correctAcknowledgement.SetActive(false);

    }
}
