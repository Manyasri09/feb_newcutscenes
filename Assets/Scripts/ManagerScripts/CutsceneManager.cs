using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneManager : MonoBehaviour
{
    
    public string nextSceneName;
    public void ChangeScene()
    {
        //Debug.Log("ChangeSceneOne function called");
        SceneManager.LoadScene(nextSceneName);
    }
}