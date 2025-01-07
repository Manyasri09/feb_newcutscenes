using VContainer.Unity;
using UnityEngine;
using VContainer;
using ezygamers.cmsv1;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using System.Runtime.CompilerServices;
using RewardSystem;

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

    private bool isTutorialShown = false; // Flag to track if tutorial has been shown

    //implemented all the logic for Question progression  -rohan37kumar
    private void OnEnable()
    {
        CMSGameEventManager.OnLoadQuestionData += UpdateUI;
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
    private void UpdateUI(QuestionBaseSO questionData)
    {
        LoadDropsUI();
        var dragDropUI = dropsUIInstance.GetComponent<PrefabUIManager>();
        dragDropUI.LoadQuestionData(questionData);
        if (questionData.optionType == OptionType.LineQuestion && !isTutorialShown)
        {
            ShowTutorialPopUp();
            isTutorialShown = true;
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

        ////// Show tutorial and prevent loading the line question UI
        //if (question.optionType == OptionType.LineQuestion && !isTutorialShown)
        //{
            
        //    ShowTutorialPopUp();
        //    isTutorialShown = true;
        //    //currentPrefab = null; // Do not assign a prefab yet
        //    return;
        //}


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

    public void SetCoinsAmount(int amount)
    {
        Debug.Log($"$<color=yellow>Amount = {amount}</color>");
        int coins_Amount = int.Parse(coinsAmount.text);
        coins_Amount += amount;
        coinsAmount.text = coins_Amount.ToString();

        Text coinText = playButtonPanel.GetComponentInChildren<Text>();
        coinText.text = coins_Amount.ToString();

    }

    private void ShowTutorialPopUp()
    {
        var tutorialPopUpInstance = Instantiate(TutorialPopUpWindow, transform);
        container.InjectGameObject(tutorialPopUpInstance);

        // Add a CanvasGroup to the root object
        var canvasGroup = tutorialPopUpInstance.AddComponent<CanvasGroup>();
        // Force the panel to block raycasts
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;

        // Get all the graphic elements (Image, Text etc.)
        var graphics = tutorialPopUpInstance.GetComponentsInChildren<Graphic>();
        foreach (var graphic in graphics)
        {
            // Set their sorting order higher than other UI elements
            graphic.canvas.overrideSorting = true;
            graphic.canvas.sortingOrder = 10;
        }

        var closeButton = tutorialPopUpInstance.GetComponentInChildren<Button>();
        closeButton.onClick.AddListener(() =>
        {
            Destroy(tutorialPopUpInstance);
        });
    }

}