using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using PlayerProgressSystem;

/// <summary>
/// Controls a hand-shaped UI prompt that guides users through dragging interactions.
/// The prompt follows a predefined path of nodes and can automatically appear after a period of user inactivity.
/// </summary>
public class HandDraggingPrompt : MonoBehaviour
{
    [Header("Prompt Settings")]
    /// <summary>
    /// Controls whether the prompt automatically appears after user inactivity.
    /// </summary>
    [SerializeField] private bool enableAutoPrompt = true;

    /// <summary>
    /// Total duration of the animation sequence. Set to -1 for infinite looping.
    /// </summary>
    [SerializeField] private float totalAnimationDuration = -1f;

    [Header("References")]
    /// <summary>
    /// The UI Image component representing the hand cursor.
    /// </summary>
    [SerializeField] private Image handPromptImage;

    /// <summary>
    /// Ordered list of Transform points defining the path the hand will follow.
    /// Minimum of 2 nodes required for animation.
    /// </summary>
    [SerializeField] private List<Transform> nodes;

    [Header("Animation Settings")]
    /// <summary>
    /// Time in seconds of user inactivity before showing the prompt.
    /// </summary>
    [SerializeField] private float idleTimeThreshold = 5f;

    /// <summary>
    /// Duration in seconds for the hand to move between two consecutive nodes.
    /// </summary>
    [SerializeField] private float animationDurationPerSegment = 1f;

    /// <summary>
    /// Position offset applied to the hand relative to each node position.
    /// </summary>
    [SerializeField] private Vector3 handOffset = Vector3.zero;

    // Private member variables for internal state
    private RectTransform handRectTransform;
    private bool isAnimating;
    private Coroutine idleCheckCoroutine;
    private Coroutine animationCoroutine;

    /// <summary>
    /// Validates required components and initializes the prompt on Awake.
    /// </summary>
    private void Awake()
    {
        ValidateComponents();
        InitializePrompt();
    }

    /// <summary>
    /// Sets up event listeners and starts the auto-prompt system if enabled.
    /// </summary>
    private void Start()
    {
        if (enableAutoPrompt)
        {
            ScheduleAnimation();
        }
        LineActions.OnLineStarted += EndAnimation;
    }

    /// <summary>
    /// Ensures all animations are stopped when the object is destroyed.
    /// </summary>
    private void OnDestroy()
    {
        StopAnimation();
    }

    /// <summary>
    /// Validates that all required components are properly assigned and configured.
    /// Disables the component if validation fails.
    /// </summary>
    private void ValidateComponents()
    {
        if (handPromptImage == null || nodes == null || nodes.Count < 2)
        {
            Debug.LogError("Hand prompt image or nodes are not properly assigned!");
            enabled = false;
        }
        else
        {
            handRectTransform = handPromptImage.rectTransform;
        }
    }

    /// <summary>
    /// Initializes the prompt's starting state.
    /// </summary>
    private void InitializePrompt()
    {
        handPromptImage.enabled = false;
        isAnimating = false;
    }

    /// <summary>
    /// Schedules the next animation sequence after the idle threshold.
    /// Cancels any existing scheduled animations first.
    /// </summary>
    private void ScheduleAnimation()
    {
        if (idleCheckCoroutine != null)
        {
            StopCoroutine(idleCheckCoroutine);
        }

        idleCheckCoroutine = StartCoroutine(IdleCheckRoutine());
    }

    /// <summary>
    /// Coroutine that waits for the idle threshold before starting the animation.
    /// </summary>
    private IEnumerator IdleCheckRoutine()
    {
        yield return new WaitForSeconds(idleTimeThreshold);

        if (enableAutoPrompt && !isAnimating)
        {
            StartAnimation();
        }
    }

    /// <summary>
    /// Publicly accessible method to manually start the prompt animation.
    /// Only works if auto-prompt is enabled.
    /// </summary>
    public void StartPromptAnimation()
    {
        if (!enableAutoPrompt) return;
        StartAnimation();
    }

    /// <summary>
    /// Publicly accessible method to manually stop the prompt animation.
    /// </summary>
    public void StopPromptAnimation()
    {
        StopAnimation();
    }

    /// <summary>
    /// Enables or disables the auto-prompt system.
    /// </summary>
    /// <param name="enabled">Whether auto-prompt should be enabled.</param>
    public void SetPromptEnabled(bool enabled)
    {
        enableAutoPrompt = enabled;
        if (!enabled)
        {
            StopAnimation();
        }
        else
        {
            ScheduleAnimation();
        }
    }

    /// <summary>
    /// Initiates the hand animation sequence.
    /// Sets up initial position and starts the animation coroutine.
    /// </summary>
    private void StartAnimation()
    {
        isAnimating = true;
        handPromptImage.enabled = true;
        handRectTransform.position = nodes[0].position + handOffset;

        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
        }
        animationCoroutine = StartCoroutine(AnimateHandRoutine());
    }

    /// <summary>
    /// Coroutine that handles the actual hand animation.
    /// Moves the hand smoothly between each node in sequence.
    /// </summary>
    private IEnumerator AnimateHandRoutine()
    {
        for (int i = 0; i < nodes.Count - 1; i++)
        {
            Transform startNode = nodes[i];
            Transform endNode = nodes[i + 1];
            float elapsedTime = 0f;

            while (elapsedTime < animationDurationPerSegment)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / animationDurationPerSegment;

                handRectTransform.position = Vector3.Lerp(
                    startNode.position + handOffset,
                    endNode.position + handOffset,
                    t
                );
                yield return null;
            }

            handRectTransform.position = endNode.position + handOffset;
        }

        handPromptImage.enabled = false;
        isAnimating = false;

        if (enableAutoPrompt && totalAnimationDuration < 0)
        {
            ScheduleAnimation();
        }
    }

    /// <summary>
    /// Stops all running animations and resets the prompt state.
    /// </summary>
    private void StopAnimation()
    {
        if (!isAnimating) return;

        if (idleCheckCoroutine != null)
        {
            StopCoroutine(idleCheckCoroutine);
            idleCheckCoroutine = null;
        }

        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
            animationCoroutine = null;
        }

        handPromptImage.enabled = false;
        isAnimating = false;
    }

    /// <summary>
    /// Permanently ends the animation and disables auto-prompt.
    /// Called when the line drawing action starts.
    /// </summary>
    private void EndAnimation()
    {
        StopAnimation();
        enableAutoPrompt = false;
    }
}