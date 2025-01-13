using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDragMasterPanelManager : MonoBehaviour
{
    public RectTransform TopPanel;
    public RectTransform BottomPanel;
    public GameObject TopPanelPrefab;
    public GameObject BottomPanelPrefab;

    private PrefabUIManager bottomPanelPrefabUIManager;
    private PrefabUIManager topPanelPrefabUIManager;
    
    // Add an initialization method that returns a bool to indicate success
    public bool Initialize()
    {
        if (TopPanelPrefab == null || BottomPanelPrefab == null)
        {
            Debug.LogError("Prefabs not assigned in LineDragMasterPanelManager");
            return false;
        }

        // Initialize top panel
        GameObject topInstance = Instantiate(TopPanelPrefab, TopPanel);
        if (!ConfigureRectTransform(topInstance, "Top"))
        {
            return false;
        }

        // Initialize bottom panel
        GameObject bottomInstance = Instantiate(BottomPanelPrefab, BottomPanel);
        if (!ConfigureRectTransform(bottomInstance, "Bottom"))
        {
            return false;
        }
        // Get and verify TopPanel PrefabUIManager
        topPanelPrefabUIManager = topInstance.GetComponent<PrefabUIManager>();
        if (topPanelPrefabUIManager == null)
        {
            Debug.LogError("PrefabUIManager component missing from top panel prefab");
            return false;
        }

        // Get and verify BottomPanel PrefabUIManager
        bottomPanelPrefabUIManager = bottomInstance.GetComponent<PrefabUIManager>();
        if (bottomPanelPrefabUIManager == null)
        {
            Debug.LogError("PrefabUIManager component missing from bottom panel prefab");
            return false;
        }

        return true;
    }

    private bool ConfigureRectTransform(GameObject instance, string panelName)
    {
        RectTransform rect = instance.GetComponent<RectTransform>();
        if (rect == null)
        {
            Debug.LogError($"{panelName} panel missing RectTransform component");
            return false;
        }

        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.anchoredPosition = Vector2.zero;
        rect.sizeDelta = Vector2.zero;
        return true;
    }

    public PrefabUIManager GetBottomPanelPrefabUIManager()
    {
        if (bottomPanelPrefabUIManager == null)
        {
            Debug.LogError("PrefabUIManager not initialized in LineDragMasterPanelManager");
        }
        return bottomPanelPrefabUIManager;
    }

    public PrefabUIManager GetTopPanelPrefabUIManager()
    {
        if (topPanelPrefabUIManager == null)
        {
            Debug.LogError("PrefabUIManager not initialized in LineDragMasterPanelManager");
        }
        return topPanelPrefabUIManager;    
    }
}