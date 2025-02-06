using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Scene1_cutscene_animationplay : MonoBehaviour
{
    public Animator secondAnimator; // Reference to the second object's Animator

    public void PlaySecondAnimation() // This function is called by Animation Event
    {
        if (secondAnimator != null)
        {
            secondAnimator.SetTrigger("Play"); // Ensure the second animation is triggered
        }
    }
}
