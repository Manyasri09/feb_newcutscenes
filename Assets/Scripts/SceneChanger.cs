using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public string SceneName;
    public string currentSceneName;
    
    public void SwitchScene()
    {
        PlayerPrefs.SetInt("CutsceneCompleted", 1);
        PlayerPrefs.SetString("CompletedSceneName", currentSceneName);
        SceneManager.LoadScene(SceneName);
    }
}
