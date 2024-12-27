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
    [SerializeField]
    private Slider ProgressBar; //ProgressBar for each level -rohan37kumar
    private bool isProcessing = false;
    public static int CurrentIndex; //value for the Current Index of Question Loaded.
    public static int LevelIndex;
    
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
        StartGame();
    }

    public void StartGame()
    {
        //playerProgressManager.DeleteAllRecords();
        CurrentIndex = 0;
        LevelLoader();
        Debug.Log($"level index {LevelIndex}");
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
        
        bool isCorrect = AnswerChecker.CheckAnswer(gameLevels[LevelIndex].question[CurrentIndex], selectedOption.GetComponent<DropHandler>().OptionID);

        if (isCorrect)
        {
            CorrectAnswerSelected(gameLevels[LevelIndex].question[CurrentIndex]);
            playerProgressManager.CompleteSubLevel(gameLevels[LevelIndex].question[CurrentIndex].questionNo.ToString());
            playerProgressManager.SubLevelCompletedType(gameLevels[LevelIndex].question[CurrentIndex].optionType.ToString());
            defaultRewardManager.ClaimLevelReward(gameLevels[LevelIndex].question[CurrentIndex].questionNo);
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
        

        bool isCorrect = AnswerChecker.CheckAnswer(gameLevels[LevelIndex].question[CurrentIndex], outputText);
        OnQuestionResult?.Invoke(isCorrect);
        if (isCorrect)
        {
            playerProgressManager.CompleteSubLevel(gameLevels[LevelIndex].question[CurrentIndex].questionNo.ToString());
            playerProgressManager.SubLevelCompletedType(gameLevels[LevelIndex].question[CurrentIndex].optionType.ToString());
            defaultRewardManager.ClaimLevelReward(gameLevels[LevelIndex].question[CurrentIndex].questionNo);
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
        eventManager.LoadNextQuestion(gameLevels[LevelIndex].question[CurrentIndex]);
        isProcessing = false;
    }

    private void MoveToNextQuestion()
    {
        CurrentIndex++;
        ProgressBar.value++;

        Debug.Log("CurrentIndex: " + CurrentIndex);
        Debug.Log("Total Questions: " + gameLevels[LevelIndex].question.Count);

        if (CurrentIndex < gameLevels[LevelIndex].question.Count)
        {
            eventManager.LoadNextQuestion(gameLevels[LevelIndex].question[CurrentIndex]);
        }
        else
        {
            // All sub-levels in the current level are completed
            playerProgressManager.MainLevelCompleted(LevelIndex.ToString());

            if (LevelIndex + 1 < gameLevels.Count)
            {
                // Move to the next main level
                LevelIndex++;
                CurrentIndex = 0;
                LevelLoader();
            }
            else
            {
                // No more levels to play
                EndGame();
            }
        }
    }

    private void ProgressBarSet()
    {
        //code for Progress Bar Setup -rohan37kumar
        ProgressBar.maxValue = gameLevels[LevelIndex].question.Count + 1;
        ProgressBar.value = 1;
    }

    public void EndGame()
    {
        playerProgressManager.MainLevelCompleted(LevelIndex.ToString());
        
        //playerProgressManager.DeleteAllRecords();
        Debug.Log("Game Ended");
        //SceneManager.LoadScene("Scene 3");
    }

    public void OnPlayButtonClicked()
    {
        if (!defaultRewardManager.IsDailyRewardAvailable())
        {
            Destroy(playButtonPanel);
            uiManager.LoadLevel(gameLevels[LevelIndex]);
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
        uiManager.LoadLevel(gameLevels[LevelIndex]);
    }
   
    public void CloseDailyRewardPanel()
    {
        Destroy(playButtonPanel);
        Destroy(dailyRewardsInstance);
    }

    public void LevelLoader()
    {
        // Ensure LevelIndex doesn't exceed the available levels
        if (LevelIndex >= gameLevels.Count)
        {
            Debug.LogError("No more levels available to load.");
            EndGame();
            return;
        }

        Debug.Log($"Loading Level {LevelIndex}");

        // Set up the Progress Bar and UI for the new level
        ProgressBarSet();
        uiManager.LoadLevel(gameLevels[LevelIndex]);
        //audioManager.PlayBkgMusic();
    }

}
