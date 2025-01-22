using VContainer.Unity;
using UnityEngine;
using VContainer;
using ezygamers.cmsv1;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using LevelSelectionPackage.Views;
using LevelSelectionPackage.Models;
using LevelSelectionPackage.Controllers;


// TODO: Refactor UIManager.cs to follow Single Responsibility Principle by:
// 1. Extract UI prefab management into PrefabManager class
// 2. Create separate ChapterDayUIManager for chapter/day selection logic
// 3. Move coin/rewards UI handling to dedicated RewardsUIManager
// 4. Create TutorialUIManager for tutorial popup logic
// 5. Extract settings panel logic to SettingsUIManager
// 6. Create QuestionUIManager for question display and updates
// 7. Move animation logic to dedicated AnimationUIManager



public class UIManager : MonoBehaviour, IChapterDayView 
{
    private IObjectResolver container;

    [Inject]
    public void Construct(IObjectResolver container)
    {
        this.container = container;
    }

    //There will be different prefabs for each kind of UI -rohan37kumar
    public GameObject LearningUIPrefab;  // Prefab reference
    public GameObject TwoImagePrefab;  // Prefab reference
    public GameObject FourImagePrefab;
    public GameObject TwoWordPrefab;
    public GameObject ThreeWordLineDrag;
    public GameObject TutorialPopUpWindow;

    

    private LevelConfigSO levelSO;     //private LevelSO to hold data -rohan37kumar
    private GameObject currentPrefab;
    private GameObject dropsUIInstance;

    [SerializeField]
    private GameObject dailyRewardsPanel; // Reference to DailyRewardsPanel prefab
    [SerializeField]
    private GameObject playButtonPanel; // Reference to PlayButtonPanel prefab
    [SerializeField] private GameObject settingsUIPanel;
    public GameObject settingsButton;
    private Button settingsCloseButton;
    private GameObject dailyRewardsInstance;
    private GameObject playButtonInstance;
    private GameObject settingsUIInstance;
    public Text coinsAmount;

    [Header("Chapter Selection")]
    public GameObject chapterSelectionPanel;
    public ChapterUI chapterSelectionButtonPrefab;
    public GameObject daySelectionPanel;
    public DayUI daySelectionButtonPrefab;
    public GameObject BackButton;
    private GameObject chapterSelectionPanelInstance;
    private GameObject daySelectionPanelInstance;
    private ChapterDayController chapterDayController;

    

    
    //implemented all the logic for Question progression  -rohan37kumar
    private void OnEnable()
    {
        CMSGameEventManager.OnLoadQuestionData += UpdateUI;
        GameManager.OnQuestionResult += HandleTutorialPopup;
        settingsButton.GetComponent<Button>().onClick.AddListener(LoadSettingsUIPanel); // Add listener for settings button click
        BackButton.GetComponent<Button>().onClick.AddListener(OnBackButtonClicked);
    }

    private void OnDisable()
    {
        CMSGameEventManager.OnLoadQuestionData -= UpdateUI;
    }

    private void OnBackButtonClicked()
    {
        // Destroy chapter/day selection panels if they exist
        if (chapterSelectionPanelInstance != null)
            Destroy(chapterSelectionPanelInstance);
        if (daySelectionPanelInstance != null)
            Destroy(daySelectionPanelInstance);

        // Re-enable settings button and hide back button
        settingsButton.gameObject.SetActive(true);
        BackButton.gameObject.SetActive(false);

        // Recreate the game UI
        if (GameManager.levelDictionary.TryGetValue(GameManager.CurrentLevelNumber, out LevelConfigSO currentLevel))
        {
            UpdateUI(currentLevel[GameManager.CurrentQuestionIndex]);
        }
    }


    // New method to load a level and its questions at the start of the game -rohan37kumar
    public void LoadLevel(LevelConfigSO levelData)
    {
        levelSO = levelData;
        CreateUI();

        //Trigger the loading of the first question in the level
        if (levelData.Questions != null && levelData.noOfSubLevel > 0)
        {
            CMSGameEventManager eventManager = container.Resolve<CMSGameEventManager>();
            eventManager.LoadNextQuestion(levelData[0]);  // Load the first question, can change index to preview particular question 
        }
    }

    //this method is called once at start
    private void CreateUI()
    {
        DestroyCurrentInstance();
        ChoosePrefab(levelSO[0]);
        dropsUIInstance = Instantiate(currentPrefab, transform);
        container.InjectGameObject(dropsUIInstance);
    }

    //Update the UI at every question Change -rohan37kumar
    // This method updates the UI with the given question data
    private void UpdateUI(QuestionBaseSO questionData)
    {
        // Load the drop-down UI
        LoadDropsUI(questionData);
        PrefabUIManager dragDropUI = null;
        // Get the master panel from the dropsUIInstance
        var masterPanel = dropsUIInstance.GetComponent<LineDragMasterPanelManager>();
        
        if (masterPanel != null)
        {
            // If it's a master panel, call LoadQuestionData on it
            masterPanel.LoadQuestionData(questionData);
        }
        else
        {
            
            // Try getting PrefabUIManager directly if it's not a master panel
            dragDropUI = dropsUIInstance.GetComponent<PrefabUIManager>();
            if (dragDropUI == null)
            {
                Debug.LogError("No PrefabUIManager or LineDragMasterPanelManager found on UI instance");
                return;
            }
            dragDropUI.LoadQuestionData(questionData);
        }
        // If the question type is a line question and the tutorial has not been shown, show the tutorial pop-up
        if (questionData.optionType == OptionType.LineQuestion && PlayerPrefs.GetInt("TutorialShown") == 0)
        {
            ShowTutorialPopUp(masterPanel);
            
        }

    }

    private void UpdateUIByDay(QuestionBaseSO questionData)
    {
        LoadDropsUIByDay(questionData);
        PrefabUIManager dragDropUI = null;

        dragDropUI = dropsUIInstance.GetComponent<PrefabUIManager>();
        if (dragDropUI == null)
        {
            Debug.LogError("No PrefabUIManager or LineDragMasterPanelManager found on UI instance");
            return;
        }
        dragDropUI.LoadQuestionData(questionData);
    }

    //If the answer is correct, set Tutorial pop as 1 (as shown)
    private void HandleTutorialPopup(bool isCorrect)
    {
        if (isCorrect)
        {
            PlayerPrefs.SetInt("TutorialShown", 1);
        }
    }

    // Method to load DropsUI dynamically and inject dependencies
    public void LoadDropsUI(QuestionBaseSO questionData)
    {
        Debug.Log($"<color=yellow>LoadDropsUI called</color>");
        DestroyCurrentInstance();
        ChoosePrefab(questionData);
        dropsUIInstance = Instantiate(currentPrefab, transform);
        container.InjectGameObject(dropsUIInstance);
        Debug.Log($"<color=yellow>(From LoadDropsUI)Drop UI Instance: {dropsUIInstance.name}</color>");
    }

    public void LoadDropsUIByDay(QuestionBaseSO questionData)
    {
        Debug.Log($"<color=yellow>LoadDropsUIByDay called</color>");
        DestroyCurrentInstance();
        ChoosePrefab(questionData);
        dropsUIInstance = Instantiate(currentPrefab, transform);
        container.InjectGameObject(dropsUIInstance);
        Debug.Log($"<color=yellow>(From LoadDropsUIByDay)Drop UI Instance: {dropsUIInstance.name}</color>");
    }

    //method to destroy previously loaded Prefab instance -rohan37kumar
    private void DestroyCurrentInstance()
    {
        if (dropsUIInstance != null)
        {
            Destroy(dropsUIInstance);
            dropsUIInstance = null;
        }
    }

    //simple function to choose which prefab to instantiate (learning or question)  -rohan37kumar
    private void ChoosePrefab(QuestionBaseSO question)
    {
        Debug.Log($"<color=yellow>(From ChoosePrefab)Question type: {question.optionType}</color>");
        // Dictionary to map content type to the respective prefab
        var prefabMapping = new Dictionary<OptionType, GameObject>
        {
            { OptionType.Learning, LearningUIPrefab },
            { OptionType.TwoImageOpt, TwoImagePrefab },
            { OptionType.FourImageOpt, FourImagePrefab },
            { OptionType.TwoWordOpt, TwoWordPrefab },
            { OptionType.LineQuestion, ThreeWordLineDrag }
        };
        
        // Try to get the prefab from the dictionary based on content type
        if (prefabMapping.TryGetValue(question.optionType, out var prefab))
        {
            currentPrefab = prefab;
        }
        else
        {
            // Fallback to a default prefab if no match found
            currentPrefab = LearningUIPrefab;
        }
        Debug.Log($"<color=yellow>(From ChoosePrefab)Current Prefab: {currentPrefab.name}</color>");
    }

    public void LoadWrongUI(GameObject selectedOption)
    {
        //currently working on this -rohan37kumar
        AnimationHelper.MakeNudge(selectedOption);
    }
    public void LoadCorrectUI(QuestionBaseSO questionData)
    {
        if (dropsUIInstance.GetComponent<CorrectDisplayHelper>() == null)
        {
            return;
        }
        dropsUIInstance.GetComponent<CorrectDisplayHelper>().DisplayCorrectUI(questionData);
    }

    public GameObject LoadPlayButtonPanel()
    {
        if (playButtonInstance == null)
        {
            playButtonInstance = Instantiate(playButtonPanel, transform);
            container.InjectGameObject(playButtonInstance);
        }
        return playButtonInstance; // Return the instantiated PlayButtonPanel
    }

    /// <summary>
    /// Dynamically loads the DailyRewardsPanel and injects its dependencies.
    /// </summary>
    public GameObject LoadDailyRewardsPanel()
    {
        if (dailyRewardsInstance == null)
        {
            dailyRewardsInstance = Instantiate(dailyRewardsPanel, transform);
            container.InjectGameObject(dailyRewardsInstance);
        }
        return dailyRewardsInstance;
    }
    public void LoadSettingsUIPanel()
    {
        if (settingsUIInstance == null)
        {
            // Make sure any existing animations are properly stopped
            var helper = dropsUIInstance.GetComponentInChildren<ObjectMovementHelper>();
            if (helper != null)
            {
                helper.StopPromptAnimation();
            }
            
            dropsUIInstance.SetActive(false);
            settingsUIInstance = Instantiate(settingsUIPanel, transform);
            container.InjectGameObject(settingsUIInstance);
            var settingsPanel = settingsUIInstance.GetComponentInChildren<SettingsPanelUI>();
            settingsPanel.settingsCloseButton.onClick.AddListener(CloseSettingsPanel);
        }
    }

    public void CloseSettingsPanel()
    {
        Destroy(settingsUIInstance);
        dropsUIInstance.SetActive(true);
    }

    

    //This method sets the amount of coins on the play button panel
    public void SetCoinsAmountOnPlayButton(int amount)
    {

        //Get the text component of the play button panel
        Text coinText = playButtonPanel.GetComponentInChildren<Text>();
        //Log the previous text of the play button panel
        Debug.Log($"<color=yellow>Previous playButtonPanel text = {coinText.text}</color>");
        //Set the text of the play button panel to the amount of coins
        coinText.text = amount.ToString();
        //Log the new text of the play button panel
        Debug.Log($"<color=yellow>New playButtonPanel text = {coinText.text}</color>");
    }

       

    public void SetCoinsAmount(int amount)
    {
        int startAmount = int.Parse(coinsAmount.text);
        int targetAmount = startAmount + amount;
        
        // Start the coin animation coroutine
        StartCoroutine(AnimateCoins(startAmount, targetAmount));
    }
    private IEnumerator AnimateCoins(int startAmount, int targetAmount)
    {
        float animationDuration = 3f; // Animation duration in seconds
        float elapsedTime = 0f;
        
        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / animationDuration;
            
            // Use smooth interpolation
            int currentAmount = (int)Mathf.Lerp(startAmount, targetAmount, progress);
            
            // Update both UI texts
            coinsAmount.text = currentAmount.ToString();
            Text coinText = playButtonPanel.GetComponentInChildren<Text>();
            coinText.text = currentAmount.ToString();
            
            yield return null;
        }
        
        // Ensure we end up with the exact target amount
        coinsAmount.text = targetAmount.ToString();
        Text finalCoinText = playButtonPanel.GetComponentInChildren<Text>();
        finalCoinText.text = targetAmount.ToString();
    }

    // This method shows a tutorial pop-up window and hides the dragDropUI
    private void ShowTutorialPopUp(LineDragMasterPanelManager masterPanel)
    {
        // Instantiate the tutorial pop-up window and inject it into the container
        var tutorialPopUpInstance = Instantiate(TutorialPopUpWindow, transform);
        container.InjectGameObject(tutorialPopUpInstance);

        // Log the dragDropUI to the console
        Debug.Log($"<color=yellow>{masterPanel}</color>");
        masterPanel.gameObject.SetActive(false); // Hide the dragDropUI

        // Get the close button from the tutorial pop-up window
        var closeButton = tutorialPopUpInstance.GetComponentInChildren<Button>();
        // Add an event listener to the close button
        closeButton.onClick.AddListener(() =>
        {
            // Destroy the tutorial pop-up window
            Destroy(tutorialPopUpInstance);
            masterPanel.gameObject.SetActive(true); // Show the dragDropUI
        });
    }

    public void SetChapterDayController(ChapterDayController controller)
    {
        chapterDayController = controller;
    }

    #region IChapterDayView Implementation

    public void DisplayChapters(List<ChapterModel> chapters)
    {
        settingsButton.SetActive(false);
        BackButton.SetActive(true);
        Destroy(dropsUIInstance);
        print("Destroyed dropsUIInstance");
        Destroy(chapterSelectionPanelInstance);
        print("Destroyed chapterSelectionPanelInstance");
        Destroy(daySelectionPanelInstance);
        chapterSelectionPanelInstance = Instantiate(chapterSelectionPanel, transform);
        print("Instantiated chapterSelectionPanelInstance");
        container.InjectGameObject(chapterSelectionPanelInstance);
        print("Injected chapterSelectionPanelInstance into container");
        var chapterSelectionUI = chapterSelectionPanelInstance.GetComponent<ChapterSelectionUI>();
        print("Retrieved ChapterSelectionUI component");
        chapterSelectionUI.DisplayChaptersSelectionUI(chapters, chapterSelectionButtonPrefab, chapterDayController);
        print("Called DisplayChaptersSelectionUI on ChapterSelectionUI");
    }

    public void ShowDayLockedMessage(DayModel day)
    {

    }

    public void LoadDayContent(DayModel day)
    {
        Destroy(daySelectionPanelInstance);
        Debug.Log($"<color=yellow>Loading day content for day: {day.dayName}</color>");
        if (day == null)
        {
            Debug.LogError("DayModel is null. Cannot load content.");
            return;
        }

        // Parse the chapterName and dayName from the DayModel
        int questionNumber = day.dayNumber;
        int levelNumber = day.chapterNumber;
        // Update the GameManager's current level and question index
        GameManager.CurrentQuestionIndex = questionNumber;
        GameManager.CurrentLevelNumber = levelNumber;

        // Check if the specified level exists in the dictionary
        if (!GameManager.levelDictionary.TryGetValue(levelNumber, out LevelConfigSO levelConfig))
        {
            Debug.LogError($"Level {levelNumber} not found in the level dictionary.");
            return;
        }
        QuestionBaseSO question = levelConfig[questionNumber]; 
        if (questionNumber > 0 && levelConfig[questionNumber - 1].optionType == OptionType.Learning)
        {
            GameManager.CurrentQuestionIndex = questionNumber - 1;
            UpdateUI(levelConfig[questionNumber - 1]);

        }
        else
            UpdateUI(question);

        Destroy(daySelectionPanelInstance);
        // UpdateUIByDay(question);

        Debug.Log($"Loaded Level {levelNumber}, Question {questionNumber}.");
    }

    public void ShowChapterLockedMessage(ChapterModel chapter)
    {

    }

    public void LoadChapterContent(ChapterModel chapter)
    {
        // Debug.Log($"<color=yellow>Load Chapter Content Called from UIManager</color>");

        // chapterSelectionPanelInstance.SetActive(false);
        Destroy(chapterSelectionPanelInstance);
        daySelectionPanelInstance = Instantiate(daySelectionPanel, transform);
        container.InjectGameObject(chapterSelectionPanelInstance);
        var daySelectionUI = daySelectionPanelInstance.GetComponent<DaySelectionUI>();
        daySelectionUI.Populate(chapter, daySelectionButtonPrefab, chapterDayController);

    }




    #endregion

}