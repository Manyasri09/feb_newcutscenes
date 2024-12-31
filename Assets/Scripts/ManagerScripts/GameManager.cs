using UnityEngine;
using VContainer;
using ezygamers.cmsv1;
using ezygamers.dragndropv1;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System;
using PlayerProgressSystem;
using RewardSystem;
using Unity.VisualScripting;
using CoinAnimationPackage;
using System.Collections.Generic;


public class GameManager : MonoBehaviour
{
    private CMSGameEventManager eventManager;  // Reference to CMSGameEventManager
    private UIManager uiManager;               // Reference to UIManager
    private AudioManager audioManager;          //Reference to AudioManager
    private PlayerProgressManager playerProgressManager; // Reference to PlayerProgressManager
    private IRewardManager defaultRewardManager;  // Reference to Reward Manager

    [SerializeField]
    private List<LevelConfiggSO> gameLevels;  // ScriptableObject containing level data
    private Dictionary<int, LevelConfiggSO> levelDictionary; // Map LevelNumber to LevelConfigSO


    [SerializeField]
    private Slider ProgressBar; //ProgressBar for each level -rohan37kumar
    private bool isProcessing = false;
    public static int CurrentIndex; //value for the Current Index of Question Loaded.
    public int CurrentLevelNumber;
    
    private GameObject playButtonPanel;
    private GameObject dailyRewardsInstance;

    private Button playButton;
    private Button claimButton;

    [SerializeField] private AnimationManager animationManager;

    public Button settingsButton;


    public static event Action<bool> OnQuestionResult;


    [Inject]
    public void Construct(
        CMSGameEventManager eventManager, 
        UIManager uiManager, 
        AudioManager audioManager, 
        PlayerProgressManager playerProgressManager,
        IRewardManager defaultRewardManager
        )
    {
        this.eventManager = eventManager;
        this.uiManager = uiManager;
        this.audioManager = audioManager;
        this.playerProgressManager = playerProgressManager;
        this.defaultRewardManager = defaultRewardManager;
    }

    private void OnEnable()
    {
        //CMSGameEventManager.OnAnswerSelected += OnAnswerSelected;
        Actions.onItemDropped += OnAnswerSelected;
        LineActions.OnLineEnded += DraggingLine_OnLineEnded;
        defaultRewardManager.OnLevelRewardClaimed += DefaultRewardManager_OnLevelRewardClaimed;
        defaultRewardManager.OnDailyRewardClaimed += DefaultRewardManager_OnDailyRewardClaimed;
    }

    private void DefaultRewardManager_OnDailyRewardClaimed(int Day, IReward reward)
    {
        Debug.Log($"<color=yellow>Day = {Day}, Reward = {reward.Type} X {reward.Quantity}</color>");
        
        //uiManager.SetCoinsAmount( reward.Quantity, playerProgressManager.GetCoins());
        uiManager.SetCoinsAmount( reward.Quantity);
    }

    private void DefaultRewardManager_OnLevelRewardClaimed(int level, IReward reward)
    {
        Debug.Log($"<color=yellow>Level = {level}, Reward = {reward.Type} X {reward.Quantity}</color>");
        //uiManager.SetCoinsAmount(reward.Quantity, playerProgressManager.GetCoins());
        uiManager.SetCoinsAmount(reward.Quantity);
    }


    private void OnDisable()
    {
        //CMSGameEventManager.OnAnswerSelected -= OnAnswerSelected;
        Actions.onItemDropped -= OnAnswerSelected;
        LineActions.OnLineEnded -= DraggingLine_OnLineEnded;
    }

    private void Start()
    {
        InitializeLevelDictionary();
        StartGame();
    }

    private void InitializeLevelDictionary()
    {
        levelDictionary = new Dictionary<int, LevelConfiggSO>();
        foreach (var level in gameLevels)
        {
            levelDictionary[level.LevelNumber] = level;
        }
    }

    public void StartGame()
    {
        playerProgressManager.DeleteAllRecords();

        CurrentIndex = 0;

        // Retrieve the last completed main level
        string lastCompletedMainLevel = playerProgressManager.GetLastCompletedMainLevel();
        Debug.Log($"Last completed main level: {lastCompletedMainLevel}");

        // Parse the last completed main level and set the next level to start
        if (int.TryParse(lastCompletedMainLevel, out int lastLevelNumber))
        {
            CurrentLevelNumber = lastLevelNumber + 1;
        }
        else
        {
            // Start from the first level if no valid data is found
            CurrentLevelNumber = 1;
        }

        Debug.Log($"Starting from Level Number: {CurrentLevelNumber}");

        Debug.Log($"Current Level Number: {CurrentLevelNumber}");

        // Check if the level exists
        if (!levelDictionary.ContainsKey(CurrentLevelNumber))
        {
            Debug.LogError($"Level Number {CurrentLevelNumber} not found! Starting from the first level.");
            CurrentLevelNumber = 1; // Reset to the first level as a fallback
        }

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
        uiManager.SetCoinsAmount(playerProgressManager.GetCoins());

        playButtonPanel = uiManager.LoadPlayButtonPanel();
        playButton = playButtonPanel.GetComponentInChildren<Button>();

        if (playButton != null)
        {
            playButton.onClick.AddListener(OnPlayButtonClicked);
        }
        else
        {
            Debug.LogError("PlayButton not found in PlayButtonPanel.");
        }
    }

    //Methods for Level Progression -rohan37kumar
    private void OnAnswerSelected(GameObject selectedOption)
    {
        var currentLevel = levelDictionary[CurrentLevelNumber];
        var currentQuestion = currentLevel.question[CurrentIndex];

        bool isCorrect = AnswerChecker.CheckAnswer(currentQuestion, selectedOption.GetComponent<DropHandler>().OptionID);

        if (isCorrect)
        {
            CorrectAnswerSelected(currentQuestion);
            playerProgressManager.CompleteSubLevel(currentQuestion.questionNo.ToString());
            playerProgressManager.SubLevelCompletedType(currentQuestion.optionType.ToString());
            defaultRewardManager.ClaimLevelReward(currentQuestion.questionNo);
            return;
        }
        WrongAnswerSelected(selectedOption);
    }
    private void DraggingLine_OnLineEnded(string outputText)
    {
        if (string.IsNullOrWhiteSpace(outputText))
        {
            Debug.LogWarning("Invalid outputText received. Skipping validation.");
            return;
        }

        var currentLevel = levelDictionary[CurrentLevelNumber];
        var currentQuestion = currentLevel.question[CurrentIndex];

        bool isCorrect = AnswerChecker.CheckAnswer(currentQuestion, outputText);
        OnQuestionResult?.Invoke(isCorrect);
        if (isCorrect)
        {
            playerProgressManager.CompleteSubLevel(currentQuestion.questionNo.ToString());
            playerProgressManager.SubLevelCompletedType(currentQuestion.optionType.ToString());
            defaultRewardManager.ClaimLevelReward(currentQuestion.questionNo);
            audioManager.PlayCorrectAudio(); 
            StartCoroutine(WaitAndMoveToNext()); 
        }
        else
        {
            audioManager.PlayWrongAudio();
            StartCoroutine(WaitAndReload());
        }
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

    private void CorrectAnswerSelected(QuestionBaseSO questionData)
    {
        if (isProcessing) return;
        isProcessing = true;
        //acknowledge the user
        audioManager.PlayCorrectAudio();
        uiManager.LoadCorrectUI(questionData);
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
        yield return new WaitForSeconds(2f);

        //called LoadNextLevelQuestion without incrementing the index...hence we reload the same level
        eventManager.LoadNextQuestion(levelDictionary[CurrentLevelNumber].question[CurrentIndex]);
        isProcessing = false;
    }

    private void MoveToNextQuestion()
    {
        CurrentIndex++;
        ProgressBar.value++;

        if (!levelDictionary.TryGetValue(CurrentLevelNumber, out var currentLevel))
        {
            Debug.LogError($"Level {CurrentLevelNumber} not found in dictionary!");
            return;
        }

        if (CurrentIndex < currentLevel.question.Count)
        {
            eventManager.LoadNextQuestion(currentLevel.question[CurrentIndex]);
        }
        else
        {
            // All sublevels of the current main level are completed
            TransitionToNextLevel();
        }
    }

    private void ProgressBarSet()
    {
        if (!levelDictionary.TryGetValue(CurrentLevelNumber, out var currentLevel))
        {
            Debug.LogError($"Level {CurrentLevelNumber} not found in dictionary!");
            return;
        }

        ProgressBar.maxValue = currentLevel.question.Count + 1;
        ProgressBar.value = 1;
    }

    public void EndGame()
    {
        //playerProgressManager.DeleteAllRecords();
        Debug.Log("Game Ended");
        SceneManager.LoadScene("Scene 3");
    }

    public void OnPlayButtonClicked()
    {
        if (!defaultRewardManager.IsDailyRewardAvailable())
        {
            Destroy(playButtonPanel);
            if (levelDictionary.TryGetValue(CurrentLevelNumber, out var currentLevel))
            {
                uiManager.LoadLevel(currentLevel);
            }
        }
        else
        {
            
            dailyRewardsInstance = uiManager.LoadDailyRewardsPanel();
            DailyRewardsUIButtonType rewardPane = dailyRewardsInstance.GetComponentInChildren<DailyRewardsUIButtonType>();
            
            if (rewardPane != null)
            {
                Destroy(playButtonPanel);
                rewardPane.claimButton.onClick.AddListener(ClaimDailyReward);
                rewardPane.closeButton.onClick.AddListener(CloseDailyRewardPanel);
                rewardPane.ShowPanel();
            }
            else
            {
                Debug.LogError("Reward Image not found in Daily Rewards Panel.");
            }
        }

    }

    public void ClaimDailyReward()
    {
        DailyRewardsUIButtonType rewardPane = dailyRewardsInstance.GetComponentInChildren<DailyRewardsUIButtonType>();
        rewardPane.OnClaimButtonClicked();
        animationManager.RewardPileOfCoins(10);
        CloseDailyRewardPanel();
        if (levelDictionary.TryGetValue(CurrentLevelNumber, out var currentLevel))
        {
            uiManager.LoadLevel(currentLevel);
        }
    }
   
    public void CloseDailyRewardPanel()
    {
        Destroy(playButtonPanel);
        Destroy(dailyRewardsInstance);
    }

    private void TransitionToNextLevel()
    {
        Debug.Log($"Level {CurrentLevelNumber} completed. Transitioning to next level...");

        // Save the progress of the current level
        playerProgressManager.MainLevelCompleted(CurrentLevelNumber.ToString());

        // Increment the level number
        CurrentLevelNumber++;

        // Check if the next level exists
        if (levelDictionary.ContainsKey(CurrentLevelNumber))
        {
            Debug.Log($"Starting Level {CurrentLevelNumber}");
            CurrentIndex = 0; // Reset the sublevel index
            ProgressBarSet(); // Reset progress bar for the new level
            uiManager.LoadLevel(levelDictionary[CurrentLevelNumber]); // Load the new level UI
        }
        else
        {
            Debug.Log("All levels completed. Game over or restart logic here.");
            EndGame();
        }
    }

}
