using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleAnimation : MonoBehaviour
{
    [SerializeField] private Sprite questionImage;
    [SerializeField] private Sprite solvedImage;
    [SerializeField] private GameObject sadDoodle;
    [SerializeField] private GameObject idleDoodle;
    [SerializeField] private GameObject happyDoodle;

    // GameObject references
    [SerializeField] private Image questionImageHolder;
    //[SerializeField] private GameObject answerImageHolder;
    //[SerializeField] private GameObject targetPlace;

    private Vector3 originalAnswerPosition;
    private Vector3 targetPosition;
    private Animator idleAnimator;
    private Animator sadAnimator;
    private Animator happyAnimator;

    private void Start()
    {
        // Set initial positions
        //originalAnswerPosition = answerImageHolder.transform.position;
        //targetPosition = targetPlace.transform.position;

        // Get animators
        idleAnimator = idleDoodle.GetComponent<Animator>();
        sadAnimator = sadDoodle.GetComponent<Animator>();
        happyAnimator = happyDoodle.GetComponent<Animator>();

        // Ensure only idleDoodle is visible initially
        ShowIdleDoodle();
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
        idleDoodle.SetActive(true);
        sadDoodle.SetActive(false);
        happyDoodle.SetActive(false);
    }

    private void ShowSadDoodle()
    {
        idleDoodle.SetActive(false);
        sadDoodle.SetActive(true);
        happyDoodle.SetActive(false);
    }

    private void ShowHappyDoodle()
    {
        idleDoodle.SetActive(false);
        sadDoodle.SetActive(false);
        happyDoodle.SetActive(true);
    }

    public void QuestionSolved()
    {
        // Update UI to solved state
        questionImageHolder.sprite = solvedImage;

        // Show happy doodle and play animations
        ShowHappyDoodle();
        //LeanTween.move(answerImageHolder.gameObject, targetPosition, 2f).setEase(LeanTweenType.easeInOutQuad);
    }

    public void QuestionNotSolved()
    {
        // Show sad doodle
        ShowSadDoodle();
    }

    public void ReloadPuzzle()
    {
        // Reset visuals and animations to initial state
        questionImageHolder.sprite = questionImage;
        //answerImageHolder.transform.position = originalAnswerPosition;
        ShowIdleDoodle();
    }
}
