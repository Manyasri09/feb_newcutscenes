using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLaunchManager : MonoBehaviour
{
    [SerializeField] string introSceneName; // Scene to load initially
    [SerializeField] string introSceneNameTwo; // Optional second intro scene
    [SerializeField] string gameSceneName; // Scene to load after intros

    void Awake()
    {
        Debug.Log(PlayerPrefs.GetInt("PlayButtonClicked", 0));
        Debug.Log(gameSceneName);
        Debug.Log(introSceneName);
        // Check if the "Play" button has been clicked previously
        if (PlayerPrefs.GetInt("PlayButtonClicked", 0) == 1) 
        {
            // If clicked, load the main game scene directly
            SceneManager.LoadScene(gameSceneName); 
        }
        // else 
        // {
        //     // If not clicked, load the first intro scene
        //     SceneManager.LoadScene(introSceneName); 
        // }
    }
}