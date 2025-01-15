using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleAnimation : MonoBehaviour
{
    [Header("Doodle References")]
    // Sprite for the question image
    [SerializeField] private Sprite questionImage;
    // Sprite for the solved image
    [SerializeField] private Sprite solvedImage;
    // GameObject for the sad doodle
    [SerializeField] private GameObject sadDoodle;
    // GameObject for the idle doodle
    [SerializeField] private GameObject idleDoodle;
    // GameObject for the question doodle
    [SerializeField] private GameObject questionDoodle;
    // GameObject for the happy doodle
    [SerializeField] private GameObject happyDoodle;

    [Space(10)]

    [Header("Dialouge Box References")]
    // GameObject for the correct acknowledgement
    [SerializeField] private Image dialougeBox;
    [SerializeField] private GameObject dialougeText;
    

    [Space(10)]

    [Header("Audio References")]
    // AudioSource for the happy doodle
    [SerializeField] private AudioSource happyDoodleAudioSource;
    // AudioClip for the happy sound
    [SerializeField] private AudioClip happySoundClip;

    [Space(10)]
    // Word animation settings
    [SerializeField] private WordAnimationSettings wordAnimationSettings;

    // Animator for the idle doodle
    private Animator idleAnimator;
    // Animator for the sad doodle
    private Animator sadAnimator;
    // Animator for the happy doodle
    private Animator happyAnimator;

    // Coroutine for the idle doodle
    private Coroutine idleDoodleCoroutine;
    // Boolean to check if the correct answer has been triggered
    private bool isCorrectAnswerTriggered = false;

    private void Start()
    {
        idleAnimator = idleDoodle.GetComponent<Animator>();
        sadAnimator = sadDoodle.GetComponent<Animator>();
        happyAnimator = happyDoodle.GetComponent<Animator>();

        StartIdleDoodle();
    }

    /// <summary>
    /// Subscribes and unsubscribes the HandleQuestionResult method to/from the OnQuestionResult event of the GameManager.
    /// This allows the PuzzleAnimation class to be notified when a question is answered, so it can handle the animation accordingly.
    /// </summary>
    private void OnEnable()
    {
        GameManager.OnQuestionResult += HandleQuestionResult;
    }

    private void OnDisable()
    {
        GameManager.OnQuestionResult -= HandleQuestionResult;
    }

    /// <summary>
    /// Handles the result of a question being answered in the game.
    /// If the answer is correct, it calls the QuestionSolved method.
    /// If the answer is incorrect, it calls the QuestionNotSolved method.
    /// </summary>
    /// <param name="isCorrect">True if the question was answered correctly, false otherwise.</param>
    private void HandleQuestionResult(bool isCorrect)
    {
        StopIdleDoodle(); // Stop any idle doodle animation
        isCorrectAnswerTriggered = isCorrect;

        if (isCorrect)
        {
            QuestionSolved();
        }
        else
        {
            QuestionNotSolved();
        }
    }

    /// <summary>
    /// Shows the idle doodle animation, unless the correct answer has already been triggered.
    /// This method sets the active state of the various doodle and acknowledgement game objects to display the idle doodle.
    /// </summary>
    private void ShowIdleDoodle()
    {
        if (isCorrectAnswerTriggered) return;

        idleDoodle.SetActive(true);
        sadDoodle.SetActive(false);
        happyDoodle.SetActive(false);
        // correctAcknowledgement.SetActive(false);
        // mistakeAcknowledgement.SetActive(false);
    }

    /// <summary>
    /// Shows the sad doodle animation by setting the active state of the various doodle and acknowledgement game objects.
    /// </summary>
    private void ShowSadDoodle()
    {
        float delayBeforeSadDoodle = wordAnimationSettings.moveDuration + 
                      wordAnimationSettings.moveDelay + 
                      wordAnimationSettings.delay + 
                      wordAnimationSettings.staggerDelay;


        StartCoroutine(ShowSadDoodleCoroutine(delayBeforeSadDoodle));
    }

    private IEnumerator ShowSadDoodleCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);

        idleDoodle.SetActive(false);
        questionDoodle.SetActive(false);
        sadDoodle.SetActive(true);
        happyDoodle.SetActive(false);
        // correctAcknowledgement.SetActive(false);
        // mistakeAcknowledgement.SetActive(true);
    }


    /// <summary>
    /// Coroutine that shows the happy doodle animation after a specified delay.
    /// This method sets the active state of the various doodle and acknowledgement game objects to display the happy doodle, and plays the happy doodle sound.
    /// </summary>
    /// <param name="delay">The delay in seconds before the happy doodle animation should be shown.</param>
    private IEnumerator ShowHappyDoodleCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
    
        idleDoodle.SetActive(false);
        sadDoodle.SetActive(false);
        questionDoodle.SetActive(false);
        happyDoodle.SetActive(true);
        // correctAcknowledgement.SetActive(true);
        // mistakeAcknowledgement.SetActive(false);

        PlayHappyDoodleSound();
    }
    /// <summary>
    /// Shows the happy doodle animation after a calculated delay.
    /// The delay is determined by the duration and delay settings of the word animation.
    /// This method starts a coroutine to show the happy doodle animation after the calculated delay.
    /// </summary>
    private void ShowHappyDoodle()
    {
        float delayBeforeHappyDoodle = wordAnimationSettings.moveDuration + 
                      wordAnimationSettings.moveDelay + 
                      wordAnimationSettings.delay + 
                      wordAnimationSettings.staggerDelay;


        StartCoroutine(ShowHappyDoodleCoroutine(delayBeforeHappyDoodle));
    }

    public void QuestionSolved()
    {
        isCorrectAnswerTriggered = true;
        ShowHappyDoodle();
    }

    public void QuestionNotSolved()
    {
        StartCoroutine(ResetPuzzleCoroutine());
    }

    public void ReloadPuzzle()
    {
        StartIdleDoodle();
    }

    private void StartIdleDoodle()
    {
        if (idleDoodleCoroutine != null)
        {
            StopCoroutine(idleDoodleCoroutine);
        }
        idleDoodleCoroutine = StartCoroutine(ShowIdleDoodleCoroutine());
    }

    private void StopIdleDoodle()
    {
        if (idleDoodleCoroutine != null)
        {
            StopCoroutine(idleDoodleCoroutine);
            idleDoodleCoroutine = null;
        }
    }

    private IEnumerator ResetPuzzleCoroutine()
    {
        ShowSadDoodle();
        yield return new WaitForSeconds(3f);
        isCorrectAnswerTriggered = false;
        ReloadPuzzle();
    }

    private IEnumerator ShowIdleDoodleCoroutine()
    {
        ShowIdleDoodle();
        yield return new WaitForSeconds(3f);

        if (!isCorrectAnswerTriggered) // Ensure no transition if the correct answer is triggered
        {
            ShowQuestionDoodle();
        }
    }

    private void ShowQuestionDoodle()
    {
        idleDoodle.SetActive(false);
        sadDoodle.SetActive(false);
        happyDoodle.SetActive(false);
        questionDoodle.SetActive(true);

    }

    private void PlayHappyDoodleSound()
    {
        if (happyDoodleAudioSource != null && happySoundClip != null)
        {
            happyDoodleAudioSource.clip = happySoundClip;
            happyDoodleAudioSource.Play();
            Debug.Log("Happy doodle sound played!");
        }
        else
        {
            Debug.LogWarning("Happy doodle audio source or clip is missing!");
        }
    }
}
