using VContainer.Unity;
using UnityEngine;
using VContainer;
using ezygamers.cmsv1;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using System.Runtime.CompilerServices;

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

    private LevelConfiggSO levelSO;     //private LevelSO to hold data -rohan37kumar
    private GameObject currentPrefab;
    private GameObject dropsUIInstance;


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
            {OptionType.LineQuestion, ThreeWordLineDrag }
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
    public void LoadCorrectUI()
    {
        dropsUIInstance.GetComponent<CorrectDisplayHelper>().DisplayCorrectUI();
    }

    void StartVibration(GameObject gameObject)
    {
        // Move the object left and right for the vibration effect
        
    }

}