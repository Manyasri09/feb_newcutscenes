using VContainer.Unity;
using UnityEngine;
using VContainer;
using ezygamers.cmsv1;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using System.Runtime.CompilerServices;
using RewardSystem;
using Unity.VisualScripting;

public class UIManager : MonoBehaviour
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

    private LevelConfiggSO levelSO;     //private LevelSO to hold data -rohan37kumar
    private GameObject currentPrefab;
    private GameObject dropsUIInstance;

    [SerializeField]
    private GameObject dailyRewardsPanel; // Reference to DailyRewardsPanel prefab
    [SerializeField]
    private GameObject playButtonPanel; // Reference to PlayButtonPanel prefab
    [SerializeField] private GameObject optionPanel;
    private GameObject dailyRewardsInstance;
    private GameObject playButtonInstance;
    private GameObject optionsInstance;

    public Text coinsAmount;

    
    //implemented all the logic for Question progression  -rohan37kumar
    private void OnEnable()
    {
        CMSGameEventManager.OnLoadQuestionData += UpdateUI;
        GameManager.OnQuestionResult += HandleTutorialPopup;
    }

    private void OnDisable()
    {
        CMSGameEventManager.OnLoadQuestionData -= UpdateUI;
    }

    // New method to load a level and its questions at the start of the game -rohan37kumar
    public void LoadLevel(LevelConfiggSO levelData)
    {
        levelSO = levelData;
        CreateUI();

        //Trigger the loading of the first question in the level
        if (levelData.question != null && levelData.noOfSubLevel > 0)
        {
            CMSGameEventManager eventManager = container.Resolve<CMSGameEventManager>();
            eventManager.LoadNextQuestion(levelData.question[0]);  // Load the first question, can change index to preview particular question 
        }
    }

    //this method is called once at start
    private void CreateUI()
    {
        DestroyCurrentInstance();
        ChoosePrefab(levelSO.question[0]);
        dropsUIInstance = Instantiate(currentPrefab, transform);
        container.InjectGameObject(dropsUIInstance);
    }

    //Update the UI at every question Change -rohan37kumar
    // This method updates the UI with the given question data
    private void UpdateUI(QuestionBaseSO questionData)
    {
        // Load the drop-down UI
        LoadDropsUI();
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

    //If the answer is correct, set Tutorial pop as 1 (as shown)
    private void HandleTutorialPopup(bool isCorrect)
    {
        if (isCorrect)
        {
            PlayerPrefs.SetInt("TutorialShown", 1);
        }
    }

    // Method to load DropsUI dynamically and inject dependencies
    public void LoadDropsUI()
    {
        DestroyCurrentInstance();
        ChoosePrefab(levelSO.question[GameManager.CurrentIndex]);
        dropsUIInstance = Instantiate(currentPrefab, transform);
        container.InjectGameObject(dropsUIInstance);
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
    public GameObject LoadOptionsPanel()
    {
        if (optionsInstance == null)
        {
            optionsInstance = Instantiate(optionPanel, transform);
            container.InjectGameObject(optionsInstance);
        }
        return optionsInstance;
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

}