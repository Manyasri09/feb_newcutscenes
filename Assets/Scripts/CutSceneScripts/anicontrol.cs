using System.Collections;
using UnityEngine;

public class AnimationController : MonoBehaviour // Renamed from AnimationSequenceController
{
    [System.Serializable]
    public class AnimationStep
    {
        public GameObject animatedObject; // Object with Animator
        public string animationName; // Name of the animation
    }

    public AnimationStep[] animationSequence; // Sequence of animations
    public GameObject thinkingDoodle; // Object for the thinking doodle animation
    private Animator[] animators; // Array of animators
    private Animator thinkingDoodleAnimator; // Animator for the thinking doodle

    void Start()
    {
        InitializeAnimators();
        StartCoroutine(PlayAnimationSequence());
    }

    private void InitializeAnimators()
    {
        // Initialize animators for each object in the animation sequence
        animators = new Animator[animationSequence.Length];
        for (int i = 0; i < animationSequence.Length; i++)
        {
            if (animationSequence[i].animatedObject != null)
            {
                animators[i] = animationSequence[i].animatedObject.GetComponent<Animator>();
                if (animators[i] == null)
                {
                    Debug.LogError($"Animator component missing on {animationSequence[i].animatedObject.name}");
                }
                else if (animators[i].runtimeAnimatorController == null)
                {
                    Debug.LogError($"AnimatorController missing on {animationSequence[i].animatedObject.name}");
                }
                else
                {
                    animators[i].speed = 0; // Pause animations at the start
                }
            }
        }

        // Initialize the thinking doodle's animator
        if (thinkingDoodle != null)
        {
            thinkingDoodleAnimator = thinkingDoodle.GetComponent<Animator>();
            if (thinkingDoodleAnimator == null || thinkingDoodleAnimator.runtimeAnimatorController == null)
            {
                Debug.LogError("Thinking doodle is missing an Animator or AnimatorController.");
            }
            else
            {
                thinkingDoodle.SetActive(false); // Hide thinking doodle at start
            }
        }
    }

    private IEnumerator PlayAnimationSequence()
    {
        // Sequentially play animations based on the animation sequence
        for (int i = 0; i < animationSequence.Length; i++)
        {
            if (i == 1 && thinkingDoodle != null) // Special case for thinking doodle
            {
                thinkingDoodle.SetActive(true);
                thinkingDoodleAnimator.Play("ThinkingAnimation"); // Replace with your doodle animation name
                yield return WaitForAnimationToFinish(thinkingDoodleAnimator);
                thinkingDoodle.SetActive(false);
            }
            else
            {
                yield return PlayAndPauseOnLastFrame(i);
            }
        }
    }

    private IEnumerator PlayAndPauseOnLastFrame(int index)
    {
        if (index < 0 || index >= animators.Length || animators[index] == null)
        {
            Debug.LogError($"Animator at index {index} is not configured correctly.");
            yield break;
        }

        Animator animator = animators[index];
        if (animator.runtimeAnimatorController == null)
        {
            Debug.LogError($"Animator at index {index} is missing an AnimatorController.");
            yield break;
        }

        Debug.Log($"Playing animation '{animationSequence[index].animationName}' on '{animationSequence[index].animatedObject.name}'");

        animator.speed = 1; // Start animation
        animator.Play(animationSequence[index].animationName); // Play animation

        // Wait for animation to finish
        yield return new WaitUntil(() =>
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 &&
            !animator.IsInTransition(0));

        Debug.Log($"Animation '{animationSequence[index].animationName}' completed on '{animationSequence[index].animatedObject.name}'");

        animator.speed = 0; // Pause on the last frame
    }

    private IEnumerator WaitForAnimationToFinish(Animator animator)
    {
        yield return new WaitUntil(() =>
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 &&
            !animator.IsInTransition(0));
    }
}
