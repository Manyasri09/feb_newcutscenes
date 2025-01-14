using System.Collections;
using System.Collections.Generic;
using ezygamers.cmsv1;
using UnityEngine;

// Serializable class to map animation names to their corresponding top panel prefabs
[System.Serializable]
public class TopPanelPrefabMapping
{
    public string animationName;  // The animation name specified in QuestionBaseSO
    public GameObject prefab;     // The corresponding prefab to be instantiated
}

// Serializable class to map option counts to their corresponding bottom panel prefabs
[System.Serializable]
public class BottomPanelPrefabMapping
{
    public int optionCount;       // The number of options in the question
    public GameObject prefab;     // The corresponding prefab to be instantiated
}

/// <summary>
/// Manages the creation and setup of top and bottom panels for line-dragging questions.
/// Handles dynamic prefab selection based on question configuration.
/// </summary>
public class LineDragMasterPanelManager : MonoBehaviour
{
    // Parent containers for the panels
    public RectTransform TopPanel;      // Container for the top panel
    public RectTransform BottomPanel;   // Container for the bottom panel
    
    // Lists of prefab mappings that will be configured in the Unity Inspector
    public List<TopPanelPrefabMapping> TopPanelPrefabs = new List<TopPanelPrefabMapping>();
    public List<BottomPanelPrefabMapping> BottomPanelPrefabs = new List<BottomPanelPrefabMapping>();

    // References to the UI managers of the currently active panels
    private PrefabUIManager bottomPanelPrefabUIManager;
    private PrefabUIManager topPanelPrefabUIManager;
    
    // References to the currently instantiated panel objects
    private GameObject currentTopInstance;
    private GameObject currentBottomInstance;

    /// <summary>
    /// Finds the appropriate top panel prefab based on the animation name
    /// </summary>
    /// <param name="animationName">The animation name from the question data</param>
    /// <returns>The matching prefab or null if not found</returns>
    private GameObject GetTopPanelPrefab(string animationName)
    {
        var mapping = TopPanelPrefabs.Find(x => x.animationName == animationName);
        if (mapping == null)
        {
            Debug.LogError($"No top panel prefab found for animation name: {animationName}");
            return null;
        }
        return mapping.prefab;
    }

    /// <summary>
    /// Finds the appropriate bottom panel prefab based on the number of options
    /// </summary>
    /// <param name="optionCount">The number of options in the question</param>
    /// <returns>The matching prefab or null if not found</returns>
    private GameObject GetBottomPanelPrefab(int optionCount)
    {
        var mapping = BottomPanelPrefabs.Find(x => x.optionCount == optionCount);
        if (mapping == null)
        {
            Debug.LogError($"No bottom panel prefab found for option count: {optionCount}");
            return null;
        }
        return mapping.prefab;
    }

    /// <summary>
    /// Initializes both panels with the appropriate prefabs based on question data
    /// </summary>
    /// <param name="questionData">The question data containing configuration information</param>
    /// <returns>True if initialization was successful, false otherwise</returns>
    public bool Initialize(QuestionBaseSO questionData)
    {
        // Clean up any existing instances to prevent memory leaks
        if (currentTopInstance != null) Destroy(currentTopInstance);
        if (currentBottomInstance != null) Destroy(currentBottomInstance);

        // Get the appropriate prefabs based on the question configuration
        GameObject topPrefab = GetTopPanelPrefab(questionData.questionAnimationName);
        GameObject bottomPrefab = GetBottomPanelPrefab(questionData.options.Count);

        // Verify that we found appropriate prefabs
        if (topPrefab == null || bottomPrefab == null)
        {
            Debug.LogError("Failed to find appropriate prefabs for the question");
            return false;
        }

        // Initialize top panel
        currentTopInstance = Instantiate(topPrefab, TopPanel);
        if (!ConfigureRectTransform(currentTopInstance, "Top"))
        {
            return false;
        }

        // Initialize bottom panel
        currentBottomInstance = Instantiate(bottomPrefab, BottomPanel);
        if (!ConfigureRectTransform(currentBottomInstance, "Bottom"))
        {
            return false;
        }

        // Get and verify TopPanel PrefabUIManager component
        topPanelPrefabUIManager = currentTopInstance.GetComponent<PrefabUIManager>();
        if (topPanelPrefabUIManager == null)
        {
            Debug.LogError("PrefabUIManager component missing from top panel prefab");
            return false;
        }

        // Get and verify BottomPanel PrefabUIManager component
        bottomPanelPrefabUIManager = currentBottomInstance.GetComponent<PrefabUIManager>();
        if (bottomPanelPrefabUIManager == null)
        {
            Debug.LogError("PrefabUIManager component missing from bottom panel prefab");
            return false;
        }

        return true;
    }

    /// <summary>
    /// Configures the RectTransform component of a panel instance
    /// </summary>
    /// <param name="instance">The panel instance to configure</param>
    /// <param name="panelName">The name of the panel for error reporting</param>
    /// <returns>True if configuration was successful, false otherwise</returns>
    private bool ConfigureRectTransform(GameObject instance, string panelName)
    {
        RectTransform rect = instance.GetComponent<RectTransform>();
        if (rect == null)
        {
            Debug.LogError($"{panelName} panel missing RectTransform component");
            return false;
        }

        // Configure the RectTransform to fill its parent container
        rect.anchorMin = Vector2.zero;    // Set bottom-left anchor to (0,0)
        rect.anchorMax = Vector2.one;     // Set top-right anchor to (1,1)
        rect.anchoredPosition = Vector2.zero;  // Center the panel
        rect.sizeDelta = Vector2.zero;    // Match the parent's size
        return true;
    }

    /// <summary>
    /// Loads question data into both panels after ensuring the correct prefabs are instantiated
    /// </summary>
    /// <param name="questionData">The question data to load</param>
    public void LoadQuestionData(QuestionBaseSO questionData)
    {
        // First initialize/reinitialize with the correct prefabs
        if (!Initialize(questionData))
        {
            Debug.LogError("Failed to initialize panels for question data");
            return;
        }

        // Load the question data into both panels
        topPanelPrefabUIManager.LoadQuestionData(questionData);
        bottomPanelPrefabUIManager.LoadQuestionData(questionData);
    }
}