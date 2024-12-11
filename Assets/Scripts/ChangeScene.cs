using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneManager : MonoBehaviour
{
    public List<GameObject> cutsceneAnimators;
    public List<string> nextSceneName;


    //public void StartCutscenes()
    //{
    //    StartCoroutine(PlayCutscenes());
    //}

    //private IEnumerator PlayCutscenes()
    //{
    //    //foreach (Animator animator in cutsceneAnimators)
    //    //{
    //    //    animator.Play("YourAnimationName"); // Replace with the actual animation name
    //    //    yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0)[0].clip.length);
    //    //}

    //    //SceneManager.LoadScene(nextSceneName);
    //}
}