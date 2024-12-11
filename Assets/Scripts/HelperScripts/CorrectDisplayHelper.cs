using ezygamers.cmsv1;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class CorrectDisplayHelper : MonoBehaviour
{
    [SerializeField] private GameObject previousUI;
    [SerializeField] private Image correctImage;
    [SerializeField] private Text subLevelText;
    [SerializeField] private Text hindiText;
    [SerializeField] private GameObject correctAnswerPanel;
    [SerializeField] private GameObject acknowledgePanel;
    [SerializeField] private GameObject confettiEffect;

    [SerializeField] private Image imageHolder;
    [SerializeField] private Text wordPair;
    


    public void DisplayCorrectUI(QuestionBaseSO questionData)
    {
        if (questionData.optionType == OptionType.Learning)
        {
            imageHolder.sprite = correctImage.sprite;
            wordPair.text = subLevelText.text + " - " + hindiText.text; 
        }
        for (int i = 0; i < questionData.options.Count; i++)
        {
            if (questionData.correctOptionID == questionData.options[i].optionID)
            {
                imageHolder.sprite = questionData.options[i].sprite;
                wordPair.text = questionData.options[i].text + " - " + hindiText.text;
            }
        }

        Debug.Log("displaying correct UI");
        previousUI.SetActive(false);
        correctAnswerPanel.SetActive(true);
        acknowledgePanel.LeanMoveLocalY(-1580, 0.5f).setEaseOutExpo().delay = 0.1f;
        if (confettiEffect != null)
        {
            confettiEffect.SetActive(true);
        }
        StartCoroutine(WaitAndPlayAudio());
    }

    private IEnumerator WaitAndPlayAudio()
    {
        yield return new WaitForSeconds(0.5f);
        //playing correct audio
        acknowledgePanel.GetComponent<AudioSource>().Play();
    }
    


}