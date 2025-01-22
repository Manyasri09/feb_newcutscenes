using ezygamers.cmsv1;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using GlobalAudioManagerPackage;
using TMPro;

/// <summary>
/// Manages the display of correct answer UI elements and animations in a quiz/learning game.
/// This class handles the transition between question and correct answer states, including
/// visual feedback and audio cues.
/// </summary>
public class CorrectDisplayHelper : MonoBehaviour
{
    /// <summary>
    /// Reference to the UI that should be hidden when showing the correct answer.
    /// </summary>
    [SerializeField] private GameObject previousUI;

    /// <summary>
    /// Image component displaying the correct answer visual.
    /// </summary>
    [SerializeField] private Image correctImage;

    /// <summary>
    /// Text component showing the current sub-level or category.
    /// </summary>
    [SerializeField] private Text subLevelText;

    /// <summary>
    /// Text component displaying the Hindi translation or text.
    /// </summary>
    [SerializeField] private Text hindiText;

    /// <summary>
    /// Panel that contains the correct answer display elements.
    /// </summary>
    [SerializeField] private GameObject correctAnswerPanel;

    /// <summary>
    /// Panel that slides up to acknowledge the correct answer.
    /// Contains audio feedback component.
    /// </summary>
    [SerializeField] private GameObject acknowledgePanel;

    /// <summary>
    /// Optional particle effect system for celebrating correct answers.
    /// </summary>
    [SerializeField] private GameObject confettiEffect;

    /// <summary>
    /// Image component that displays either the learning content or the correct option sprite.
    /// </summary>
    [SerializeField] private Image imageHolder;

    /// <summary>
    /// Text component that displays the word pair (question text - Hindi translation).
    /// </summary>
    [SerializeField] private Text wordPair;

    [SerializeField] private TMP_Text dayAcknowledgementText;
    

    /// <summary>
    /// Displays the correct answer UI with appropriate visuals and animations.
    /// Handles both learning-type questions and regular questions with options.
    /// </summary>
    /// <param name="questionData">The question data containing correct answer information and options.</param>
    public void DisplayCorrectUI(QuestionBaseSO questionData)
    {
        // Handle learning-type questions differently from regular questions
        if (questionData.optionType == OptionType.Learning)
        {
            // For learning questions, use the correct image directly
            imageHolder.sprite = correctImage.sprite;
            wordPair.text = subLevelText.text + " - " + hindiText.text; 
        }
        else
        {
            // For regular questions, find and display the correct option
            for (int i = 0; i < questionData.options.Count; i++)
            {
                if (questionData.correctOptionID == questionData.options[i].optionID)
                {
                    imageHolder.sprite = questionData.options[i].sprite;
                    wordPair.text = questionData.options[i].text + " - " + hindiText.text;
                }
            }

            dayAcknowledgementText.text = "Day " + (questionData.questionNo - 1).ToString() + " Completed"; 


        }

        Debug.Log("displaying correct UI");
        // Hide the question UI and show the correct answer UI
        previousUI.SetActive(false);
        correctAnswerPanel.SetActive(true);

        // Animate the acknowledge panel sliding up using LeanTween
        acknowledgePanel.LeanMoveLocalY(-1580, 0.5f).setEaseOutExpo().delay = 0.1f;

        // Show celebration effect if available
        if (confettiEffect != null)
        {
            confettiEffect.SetActive(true);
        }

        // Start the audio feedback coroutine
        StartCoroutine(WaitAndPlayAudio());
    }

    /// <summary>
    /// Coroutine that waits briefly before playing the correct answer audio feedback.
    /// The delay allows for visual elements to appear before the sound plays.
    /// </summary>
    private IEnumerator WaitAndPlayAudio()
    {
        yield return new WaitForSeconds(0.5f);
        // Play the audio feedback attached to the acknowledge panel
        // acknowledgePanel.GetComponent<AudioSource>().Play();
        GlobalAudioManager.Instance.PlayVoiceOver();
    }
}