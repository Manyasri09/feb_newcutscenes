using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStartManager : MonoBehaviour
{
    [SerializeField] string introSceneName;
    [SerializeField] string introSceneNameTwo;
    [SerializeField] string gameSceneName;


    void Awake()
    {
        // Check if the cutscene has already been watched
        // if (PlayerPrefs.GetInt("CutsceneCompleted", 0) == 1 && PlayerPrefs.GetString("CompletedSceneName").Equals(introSceneNameTwo))
        // {
        //     // Load the main game scene directly
        //     SceneManager.LoadScene(gameSceneName);
        // }

        //Check if play button is clicked
        if (PlayerPrefs.GetInt("PlayButtonClicked", 0) == 1 )
        {
             SceneManager.LoadScene(gameSceneName);
        }

    }
}
