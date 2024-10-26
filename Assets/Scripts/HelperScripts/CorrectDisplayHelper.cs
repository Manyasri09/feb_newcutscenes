using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CorrectDisplayHelper : MonoBehaviour
{
    [SerializeField] private GameObject previousUI;
    [SerializeField] private Image correctImage;
    [SerializeField] private Text subLevelText;
    [SerializeField] private Text hindiText;
    [SerializeField] private GameObject correctAnswerGroup;
    [SerializeField] private GameObject acknowledgePanel;
    [SerializeField] private GameObject confettiEffect;

    [SerializeField] private Image imageHolder;
    [SerializeField] private Text englishTextHolder;
    [SerializeField] private Text hindiTextHolder;


    public void DisplayCorrectUI()
    {
        imageHolder.sprite = correctImage.sprite;
        englishTextHolder.text = subLevelText.text;
        hindiTextHolder.text = hindiText.text;
        
        Debug.Log("displaying correct UI");
        previousUI.SetActive(false);
        correctAnswerGroup.SetActive(true);
        acknowledgePanel.LeanMoveLocalY(-1580, 0.5f).setEaseOutExpo().delay = 0.1f;
        if(confettiEffect != null)
        {
            confettiEffect.SetActive(true);
        }
        
    }
    


}