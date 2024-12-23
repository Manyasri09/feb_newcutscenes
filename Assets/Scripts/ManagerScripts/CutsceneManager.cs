using PlayerProgressSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CutsceneManager : MonoBehaviour
{
    public PlayerProgressManager playerProgressManager; // Reference to manage player progress
    public string sceneOne; // Scene for the first cutscene
    public string sceneTwo; // Scene for the second cutscene
   // public DailyRewardsUI dailyRewardsUI; // Reference to the Daily Rewards UI
    public Button button;
    private bool hasPlayed;

    private void Start()
    {
        CutsceneChecker();
    }

    private void Awake()
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnPlayButtonClicked);
    }

    public void CutsceneChecker()
    {
        //Check if the player has completed at least one level
        if (playerProgressManager != null && playerProgressManager.HasCompletedSubLevel(playerProgressManager.GetLastCompletedSubLevel()) && hasPlayed )
        {
            // Start from the second cutscene
            ChangeScene(sceneTwo);
        }
        else
        {
            // Start from the first cutscene
            hasPlayed = true;
            ChangeScene(sceneOne);
        }
    }

    public void ChangeScene(string sceneName)
    {
        // Load the specified scene
        SceneManager.LoadScene(sceneName);
    }

    public void OnPlayButtonClicked()
    {
        StartGame();
        //if (playerProgressManager != null && playerProgressManager.HasCompletedSubLevel(playerProgressManager.GetLastCompletedSubLevel()))
        //{
        //    // Show the daily rewards popup after the play button is pressed
        //    if (dailyRewardsUI != null)
        //    {
        //        dailyRewardsUI.ShowDailyRewardsPopup();
        //    }
        //    else
        //    {
        //        Debug.LogWarning("DailyRewardsUI reference is not assigned!");
        //    }
        //}
        //else
        //{
        //    // Start the game directly if no levels have been completed
        //    StartGame();
        //}
    }

    public void StartGame()
    {
        // Logic to start the game (e.g., load the main game scene)
        SceneManager.LoadScene("SampleScene");
    }
}
