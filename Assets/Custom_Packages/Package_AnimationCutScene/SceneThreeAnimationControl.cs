using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using GlobalAudioManagerPackage;

public class SceneThreeAnimationControl : MonoBehaviour
{
    [System.Serializable]
    public class AnimationObject
    {
        public GameObject objectWithAnimator;
        public string animationName;
    }

    [Header("Animation Sequence Settings")]
    [Tooltip("List of objects and their respective animations to play in sequence.")]
    public AnimationObject[] animationSequence;

    [Tooltip("Additional delay (in seconds) between animations.")]
    public float additionalDelay = 2f;

    private void Start()
    {
        GlobalAudioManager.Instance.PlayMusic(GlobalAudioManager.Instance.AudioConfig.cutSceneThree.BackgroundMusic); // Play the background music
        GlobalAudioManager.Instance.PlayAmbient(GlobalAudioManager.Instance.AudioConfig.cutSceneThree.AmbientSounds); // Play the ambient sounds
        
    }



    /// <summary>
    /// Start the animation sequence.
    /// </summary>
    public void StartAnimationSequence()
    {
        StartCoroutine(PlayAnimationsInSequence());
    }

    /// <summary>
    /// Coroutine to play animations in sequence.
    /// </summary>
    private IEnumerator PlayAnimationsInSequence()
    {
        foreach (var animObj in animationSequence)
        {
            if (animObj.objectWithAnimator != null && !string.IsNullOrEmpty(animObj.animationName))
            {
                Animator animator = animObj.objectWithAnimator.GetComponent<Animator>();
                if (animator != null)
                {
                    // Ensure the Animator doesn't start playing any default animations
                    animator.enabled = true;
                    
                    // Play the specified animation
                    animator.Play(animObj.animationName);

                    // Wait for the full animation length
                    yield return new WaitForSeconds(GetAnimationLength(animator, animObj.animationName));

                    // Add the additional delay
                    yield return new WaitForSeconds(additionalDelay);

                    // Optionally, disable the Animator after the animation completes
                    animator.enabled = false;
                }
                else
                {
                    Debug.LogWarning($"Animator not found on {animObj.objectWithAnimator.name}");
                }
            }
        }
    }

    /// <summary>
    /// Get the length of the animation clip.
    /// </summary>
    /// <param name="animator">Animator component.</param>
    /// <param name="animationName">Name of the animation clip.</param>
    /// <returns>Length of the animation clip in seconds.</returns>
    private float GetAnimationLength(Animator animator, string animationName)
    {
        foreach (var clip in animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == animationName)
            {
                return clip.length; // Return the length of the animation
            }
        }
        return 0f; // Default fallback if no matching animation is found
    }
}
