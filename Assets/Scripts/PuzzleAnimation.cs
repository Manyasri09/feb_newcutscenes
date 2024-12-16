using System.Collections;
using UnityEngine;

public class PuzzleAnimation : MonoBehaviour
{
    [SerializeField] private Sprite questionImage;
    [SerializeField] private Sprite solvedImage;
    [SerializeField] private GameObject sadDoodle;
    [SerializeField] private GameObject idleDoodle;
    [SerializeField] private GameObject questionDoodle;
    [SerializeField] private GameObject happyDoodle;
    [SerializeField] private GameObject correctAcknowledgement;
    [SerializeField] private GameObject mistakeAcknowledgement;

    [SerializeField] private AudioSource happyDoodleAudioSource;
    [SerializeField] private AudioClip happySoundClip;

    private Animator idleAnimator;
    private Animator sadAnimator;
    private Animator happyAnimator;

    private Coroutine idleDoodleCoroutine;
    private bool isCorrectAnswerTriggered = false;

    private void Start()
    {
        idleAnimator = idleDoodle.GetComponent<Animator>();
        sadAnimator = sadDoodle.GetComponent<Animator>();
        happyAnimator = happyDoodle.GetComponent<Animator>();

        StartIdleDoodle();
    }

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

    private void ShowIdleDoodle()
    {
        if (isCorrectAnswerTriggered) return;

        idleDoodle.SetActive(true);
        sadDoodle.SetActive(false);
        happyDoodle.SetActive(false);
        correctAcknowledgement.SetActive(false);
        mistakeAcknowledgement.SetActive(false);
    }

    private void ShowSadDoodle()
    {
        idleDoodle.SetActive(false);
        questionDoodle.SetActive(false);
        sadDoodle.SetActive(true);
        happyDoodle.SetActive(false);
        correctAcknowledgement.SetActive(false);
        mistakeAcknowledgement.SetActive(true);
    }

    private void ShowHappyDoodle()
    {
        idleDoodle.SetActive(false);
        sadDoodle.SetActive(false);
        questionDoodle.SetActive(false);
        happyDoodle.SetActive(true);
        correctAcknowledgement.SetActive(true);
        mistakeAcknowledgement.SetActive(false);

        PlayHappyDoodleSound(); // Trigger sound directly here
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
