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
using CoinAnimationPackage;
using System.Collections.Generic;
using GlobalAudioManagerPackage;
using LevelSelectionPackage.Models;
using LevelSelectionPackage.Controllers;



public class GameManager : MonoBehaviour  
{
    private CMSGameEventManager eventManager;  // Reference to CMSGameEventManager
    private UIManager uiManager;               // Reference to UIManager         
    private PlayerProgressManager playerProgressManager; // Reference to PlayerProgressManager
    private IRewardManager defaultRewardManager;  // Reference to Reward Manager

    private ChapterDayController controller;

    [SerializeField]
    private List<LevelConfigSO> gameLevels;  // ScriptableObject containing level data
    public static Dictionary<int, LevelConfigSO> levelDictionary; // Map LevelNumber to LevelConfigSO


    [SerializeField]
    private Slider ProgressBar; //ProgressBar for each level -rohan37kumar
    private bool isProcessing = false;
    public static int CurrentQuestionIndex; //value for the Current Index of Question Loaded.
    public static int CurrentLevelNumber;
    
    private GameObject playButtonPanel;
    private GameObject dailyRewardsInstance;

    

    private Button playButton;
    private Button claimButton;

    [SerializeField] private AnimationManager animationManager;
    [SerializeField] private Button dayButton;


    public static event Action<bool> OnQuestionResult;
    


    [Inject]
    public void Construct(
        CMSGameEventManager eventManager, 
        UIManager uiManager,  
        PlayerProgressManager playerProgressManager,
        IRewardManager defaultRewardManager
        )
    {
        this.eventManager = eventManager;
        this.uiManager = uiManager;
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
        dayButton.onClick.AddListener(OnDayButtonClick);
        
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
        dayButton.onClick.RemoveAllListeners();
    }

    private void Start()
    {
        InitializeLevelDictionary();
        SetupLevelSelection();
        StartGame();
    }

    /// <summary>
    /// Initializes the levelDictionary by iterating through the gameLevels array and adding each level to the dictionary, using the level number as the key.
    /// </summary>
    private void InitializeLevelDictionary() //TODO: Remove this replace with lists
    {
        levelDictionary = new Dictionary<int, LevelConfigSO>();
        foreach (var level in gameLevels)
        {
            levelDictionary[level.LevelNumber] = level;
        }
    }

    private void SetupLevelSelection()
    {

        // Create and set up the system components
        ChapterDayDataProvider dataProvider = new ChapterDayDataProvider();
        dataProvider.SetDataToDataProvider(levelDictionary, playerProgressManager);
        controller = new ChapterDayController(dataProvider, uiManager);
        
        // Initialize the view manager
        uiManager.SetChapterDayController(controller);
        controller.LoadChapters();

        Debug.Log("Chapter-Day system initialized successfully!");
    }

    private void OnDayButtonClick()
    {
        controller.sendChaptersToView();
    }

    
    private void OnApplicationQuit()
    {
        // Reset the screen timeout to the default value
        Screen.sleepTimeout = SleepTimeout.SystemSetting;
    }

    public void StartGame()
    {

        CurrentQuestionIndex = 0;

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
        if (levelDictionary.ContainsKey(CurrentLevelNumber))
        {
            playerProgressManager.SubLevelStarted(levelDictionary[CurrentLevelNumber][CurrentQuestionIndex].questionNo.ToString());
        }

        // audioManager.PlayBkgMusic();
        if(GlobalAudioManager.Instance != null)
            GlobalAudioManager.Instance.PlayMusic(GlobalAudioManager.Instance.AudioConfig.levelBackgroundMusic, true, 0.1f);
        
        Debug.Log(uiManager);
        Debug.Log("Game Started");
       
        // Load the first level's UI and questions
        ProgressBarSet();
        uiManager.SetCoinsAmount(playerProgressManager.GetCoins());
        uiManager.SetCoinsAmountOnPlayButton(playerProgressManager.GetCoins());
        

        // Load the PlayButtonPanel from the UIManager
        playButtonPanel = uiManager.LoadPlayButtonPanel();
        // Get the Button component from the PlayButtonPanel
        playButton = playButtonPanel.GetComponentInChildren<Button>();

        // Check if the Button component is not null
        if (playButton != null)
        {
            // Add a listener to the Button's onClick event
            playButton.onClick.AddListener(OnPlayButtonClicked);
        }
        else
        {
            // Log an error if the Button component is null
            Debug.LogError("PlayButton not found in PlayButtonPanel.");
        }
    }

    //Methods for Level Progression -rohan37kumar
    /// <summary>
    /// Handles the logic when an answer is selected by the user.
    /// Checks if the selected answer is correct or not, and performs the appropriate actions accordingly.
    /// </summary>
    /// <param name="selectedOption">The GameObject representing the selected answer option.</param>
    private void OnAnswerSelected(GameObject selectedOption)
    {
        var currentLevel = levelDictionary[CurrentLevelNumber];
        var currentQuestion = currentLevel[CurrentQuestionIndex];

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

    /// <summary>
    /// Handles the end of a dragging line interaction, validating the output text and performing the appropriate actions based on whether the answer is correct or not.
    /// </summary>
    /// <param name="outputText">The text output from the dragging line interaction.</param>
    private void DraggingLine_OnLineEnded(string outputText)
    {
        if (string.IsNullOrWhiteSpace(outputText))
        {
            Debug.LogWarning("Invalid outputText received. Skipping validation.");
            return;
        }

        LevelConfigSO currentLevel = levelDictionary[CurrentLevelNumber];
        QuestionBaseSO currentQuestion = currentLevel[CurrentQuestionIndex];

        bool isCorrect = AnswerChecker.CheckAnswer(currentQuestion, outputText);
        OnQuestionResult?.Invoke(isCorrect);
        if (isCorrect)
        {
            playerProgressManager.CompleteSubLevel(currentQuestion.questionNo.ToString());
            playerProgressManager.SubLevelCompletedType(currentQuestion.optionType.ToString());
            defaultRewardManager.ClaimLevelReward(currentQuestion.questionNo);
            GlobalAudioManager.Instance.PlaySFX(GlobalAudioManager.Instance.AudioConfig.correctAnswerSFX);
            StartCoroutine(WaitAndMoveToNext(currentQuestion));   
        }
        else
        {
            GlobalAudioManager.Instance.PlaySFX(GlobalAudioManager.Instance.AudioConfig.incorrectAnswerSFX);
            StartCoroutine(WaitAndReload());
        }
    }

    /// <summary>
    /// Handles the case when the user selects a wrong answer.
    /// </summary>
    /// <param name="selectedOption">The GameObject representing the selected option.</param>
    private void WrongAnswerSelected(GameObject selectedOption)
    {
        if (isProcessing) return;
        isProcessing = true;
        //nudge or red logic
        // audioManager.PlayWrongAudio();
        GlobalAudioManager.Instance.PlaySFX(GlobalAudioManager.Instance.AudioConfig.incorrectAnswerSFX);
        uiManager.LoadWrongUI(selectedOption);
        Debug.Log("Wrong Answer");
        StartCoroutine(WaitAndReload());
    }

    /// <summary>
    /// Handles the case when the user selects a correct answer.
    /// </summary>
    /// <param name="questionData">The QuestionBaseSO object containing the data for the current question.</param>
    private void CorrectAnswerSelected(QuestionBaseSO questionData)
    {
        if (isProcessing) return;
        isProcessing = true;
        //acknowledge the user
        // audioManager.PlayCorrectAudio();
        GlobalAudioManager.Instance.PlaySFX(GlobalAudioManager.Instance.AudioConfig.correctAnswerSFX);
        uiManager.LoadCorrectUI(questionData);
        Debug.Log("Correct Answer");
        StartCoroutine(WaitAndMoveToNext(questionData));
    }

    private IEnumerator WaitAndMoveToNext(QuestionBaseSO questionData)
    {
        yield return new WaitForSeconds(2);
        uiManager.ShowDayCompletionUI(questionData);
        Debug.Log("Waiting before moving to next question...");
        yield return new WaitForSeconds(3);
        uiManager.DestroyDayCompletionUI();
        MoveToNextQuestion();
        isProcessing = false;
    }
    private IEnumerator WaitAndReload()
    {
        Debug.Log("Waiting to reload the sublevel");
        yield return new WaitForSeconds(2.5f);

        //called LoadNextLevelQuestion without incrementing the index...hence we reload the same level
        eventManager.LoadNextQuestion(levelDictionary[CurrentLevelNumber][CurrentQuestionIndex]);
        isProcessing = false;
    }

    /// <summary>
    /// Moves to the next question in the current level. If all questions in the current level have been answered, it transitions to the next level.
    /// </summary>
    private void MoveToNextQuestion()
    {
        CurrentQuestionIndex++;
        ProgressBar.value++;

        if (!levelDictionary.TryGetValue(CurrentLevelNumber, out var currentLevel))
        {
            Debug.LogError($"Level {CurrentLevelNumber} not found in dictionary!");
            return;
        }

        if (CurrentQuestionIndex < currentLevel.Questions.Count)
        {
            playerProgressManager.SubLevelStarted(currentLevel[CurrentQuestionIndex].questionNo.ToString());
            controller.LoadChapters();
            eventManager.LoadNextQuestion(currentLevel[CurrentQuestionIndex]);
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

        ProgressBar.maxValue = currentLevel.Questions.Count + 1;
        ProgressBar.value = 1;
    }

    public void EndGame()
    {
        //playerProgressManager.DeleteAllRecords();
        Debug.Log("Game Ended");
        SceneManager.LoadScene("NewAnimationScene_3");
    }

    /// <summary>
    /// Handles the logic when the play button is clicked in the game. 
    /// It checks if a daily reward is available, and if so, it loads the daily rewards panel. Otherwise, it loads the current level.
    /// </summary>
    public void OnPlayButtonClicked()
    {
        //Register if play button is clicked
        PlayerPrefs.SetInt("PlayButtonClicked", 1);

        if (!defaultRewardManager.IsDailyRewardAvailable())
        {
            Destroy(playButtonPanel);
            if (levelDictionary.TryGetValue(CurrentLevelNumber, out var currentLevel))
            {
                playerProgressManager.MainLevelStarted(CurrentLevelNumber.ToString());
                controller.LoadChapters();
                uiManager.LoadLevel(currentLevel);
            }
        }
        else
        {
            
            dailyRewardsInstance = uiManager.LoadDailyRewardsPanel();
            DailyRewardsUIButtonType rewardPane = dailyRewardsInstance.GetComponentInChildren<DailyRewardsUIButtonType>();
            
            DailyRewardsUIButtonType.OnRewardClaimed += ClaimDailyReward;

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

    /// <summary>
    /// Claims the daily reward and performs the necessary actions, such as triggering the reward animation and closing the daily reward panel.
    /// </summary>
    public void ClaimDailyReward()
    {
        DailyRewardsUIButtonType rewardPane = dailyRewardsInstance.GetComponentInChildren<DailyRewardsUIButtonType>();
        rewardPane.OnClaimButtonClicked();
        animationManager.RewardPileOfCoins(10);
        CloseDailyRewardPanel();
    }

    /// <summary>
    /// Closes the daily reward panel and loads the current level.
    /// </summary>
    public void CloseDailyRewardPanel()
    {
        Destroy(playButtonPanel);
        Destroy(dailyRewardsInstance);
        if (levelDictionary.TryGetValue(CurrentLevelNumber, out var currentLevel))
        {
            playerProgressManager.MainLevelStarted(CurrentLevelNumber.ToString());
            controller.LoadChapters();
            uiManager.LoadLevel(currentLevel);
        }
    }

    /// <summary>
    /// Transitions the game to the next level. This method is responsible for handling the progression of the game, including saving the progress of the current level, incrementing the level number, and loading the next level UI.
    /// </summary>
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
            CurrentQuestionIndex = 0; // Reset the sublevel index
            ProgressBarSet(); // Reset progress bar for the new level
            playerProgressManager.MainLevelStarted(CurrentLevelNumber.ToString());
            if (levelDictionary.ContainsKey(CurrentLevelNumber))
            {
                playerProgressManager.SubLevelStarted(levelDictionary[CurrentLevelNumber][CurrentQuestionIndex].questionNo.ToString());
            }
            controller.LoadChapters();
            uiManager.LoadLevel(levelDictionary[CurrentLevelNumber]); // Load the new level UI
        }
        else
        {
            Debug.Log("All levels completed. Game over or restart logic here.");
            EndGame();
        }
    }

}
