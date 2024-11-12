using UnityEngine;
using VContainer;
using ezygamers.cmsv1;
using ezygamers.dragndropv1;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private CMSGameEventManager eventManager;  // Reference to CMSGameEventManager
    private UIManager uiManager;               // Reference to UIManager
    private AudioManager audioManager;          //Reference to AudioManager

    [SerializeField]
    private LevelConfiggSO currentLevel;  // ScriptableObject containing level data
    [SerializeField]
    private Slider ProgressBar; //ProgressBar for each level -rohan37kumar
    private bool isProcessing = false;
    public static int CurrentIndex; //value for the Current Index of Question Loaded.


    [Inject]
    public void Construct(CMSGameEventManager eventManager, UIManager uiManager, AudioManager audioManager)
    {
        this.eventManager = eventManager;
        this.uiManager = uiManager;
        this.audioManager = audioManager;
    }

    private void OnEnable()
    {
        //CMSGameEventManager.OnAnswerSelected += OnAnswerSelected;
        Actions.onItemDropped += OnAnswerSelected;
    }

    private void OnDisable()
    {
        //CMSGameEventManager.OnAnswerSelected -= OnAnswerSelected;
        Actions.onItemDropped -= OnAnswerSelected;
    }

    private void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        CurrentIndex = 0;
        if (uiManager == null)
        {
            Debug.LogError("UIManager not injected!");
            return;
        }

        audioManager.PlayBkgMusic();
        Debug.Log(uiManager);
        Debug.Log("Game Started");
        // Load the first level's UI and questions
        ProgressBarSet();
        uiManager.LoadLevel(currentLevel);
    }

    //Methods for Level Progression -rohan37kumar
    private void OnAnswerSelected(GameObject selectedOption)
    {

        bool isCorrect = AnswerChecker.CheckAnswer(currentLevel.question[CurrentIndex], selectedOption.GetComponent<DropHandler>().OptionID);

        if (isCorrect)
        {
            CorrectAnswerSelected();
            return;
        }
        WrongAnswerSelected(selectedOption);
    }


    private void WrongAnswerSelected(GameObject selectedOption)
    {
        if (isProcessing) return;
        isProcessing = true;
        //nudge or red logic
        audioManager.PlayWrongAudio();
        uiManager.LoadWrongUI(selectedOption);
        Debug.Log("Wrong Answer");
        StartCoroutine(WaitAndReload());
    }

    private void CorrectAnswerSelected()
    {
        if (isProcessing) return;
        isProcessing = true;
        //acknowledge the user
        audioManager.PlayCorrectAudio();
        uiManager.LoadCorrectUI();
        Debug.Log("Correct Answer");
        StartCoroutine(WaitAndMoveToNext());
    }

    private IEnumerator WaitAndMoveToNext()
    {
        Debug.Log("Waiting before moving to next question...");
        yield return new WaitForSeconds(3);

        MoveToNextQuestion();
        isProcessing = false;
    }
    private IEnumerator WaitAndReload()
    {
        Debug.Log("Waiting to reload the sublevel");
        yield return new WaitForSeconds(0.85f);

        //called LoadNextLevelQuestion without incrementing the index...hence we reload the same level
        eventManager.LoadNextQuestion(currentLevel.question[CurrentIndex]);
        isProcessing = false;
    }

    private void MoveToNextQuestion()
    {
        CurrentIndex++;
        ProgressBar.value++;
        if (CurrentIndex < currentLevel.noOfSubLevel)
        {
            eventManager.LoadNextQuestion(currentLevel.question[CurrentIndex]);
        }
        else
        {
            EndGame();
        }
    }

    private void ProgressBarSet()
    {
        //code for Progress Bar Setup -rohan37kumar
        ProgressBar.maxValue = currentLevel.question.Count + 1;
        ProgressBar.value = 1;
    }

    public void EndGame()
    {
        Debug.Log("Game Ended");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
