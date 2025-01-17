using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using VContainer;
using PlayerProgressSystem;

/// <summary>
/// Manages the movement and animation of a hand prompt/tutorial element in the UI
/// This class handles user guidance by showing an animated hand movement between two points
/// </summary>
public class ObjectMovementHelper : MonoBehaviour
{
    [Header("Prompt Settings")]
    [SerializeField] private bool enableAutoPrompt = true;        // Toggle for automatic prompt animation
    [SerializeField] private float totalAnimationDuration = -1f;  // Total duration of animation cycles (-1 for infinite)
    [SerializeField] private string levelType;                    // Type of level this prompt is used in

    [Header("References")]
    [SerializeField] private Image handPromptImage;               // Reference to the hand image UI element
    [SerializeField] private RectTransform startTarget;          // Starting position for the hand movement
    [SerializeField] private RectTransform endTarget;            // Ending position for the hand movement

    [Header("Animation Settings")]
    [SerializeField] private float idleTimeThreshold = 5f;       // Time to wait before showing prompt
    [SerializeField] private float animationDuration = 1.5f;     // Duration of a single animation cycle

    private RectTransform handRectTransform;                     // Transform component of hand image
    private bool isAnimating;                                    // Flag to track animation state
    private float currentAnimationTime;                          // Tracks total animation time
    private Coroutine idleCheckCoroutine;                       // Coroutine for idle time checking
    private Coroutine animationCoroutine;                       // Coroutine for animation


    private PlayerProgressManager progressManager;

    [Inject]
    public void Construct(PlayerProgressManager progressManager)
    {
        this.progressManager = progressManager;
        
    }

    /// <summary>
    /// Initialize and validate required components
    /// </summary>
    private void Awake()
    {
        if (!ValidateComponents()) return;
        InitializePrompt();
    }

    private void OnDisable()
    {
        // Force stop all animations and reset state
        StopAllCoroutines();
        InitializePrompt();
        isAnimating = false;
        currentAnimationTime = 0f;
        
        if (handPromptImage != null)
        {
            handPromptImage.enabled = false;
            handPromptImage.color = new Color(1f, 1f, 1f, 1f); // Reset color
        }
        
        idleCheckCoroutine = null;
        animationCoroutine = null;
    }

    /// <summary>
    /// Start automatic prompt if enabled and level hasn't been played before
    /// </summary>
    private void Start()
    {
        // Only show the prompt if:
        // 1. Auto prompt is enabled
        // 2. Player hasn't completed this level type before
        CheckToStartAnimation();
    }

    public void CheckToStartAnimation()
    {
        // First, ensure we're in a clean state
        StopAnimation();
        
        // Then check if we should start the animation
        if (enableAutoPrompt && !HasPlayerCompletedLevelType())
        {
            ScheduleAnimation();
        }

    }

    /// <summary>
    /// Checks if the player has already completed this type of level
    /// </summary>
    /// <returns>True if the level type has been completed before</returns>
    private bool HasPlayerCompletedLevelType()
    {
        // Ensure levelType is not empty
        if (string.IsNullOrEmpty(levelType))
        {
            Debug.LogWarning("Level type is not set in ObjectMovementHelper");
            return false;
        }

        if (string.IsNullOrEmpty(levelType))
        {
            Debug.LogWarning("Level type is not set in ObjectMovementHelper");
            return false;
        }

        // Convert to match the same format
        string normalizedType = levelType.ToUpperInvariant(); // or ToLowerInvariant()
        Debug.Log($"<color=yellow>Checking completion for level type: {normalizedType} </color>");

        bool hasCompleted = progressManager.HasCompletedSubLevelType(normalizedType);
        Debug.Log($"<color=yellow>Completion status: {hasCompleted}</color>");
        return hasCompleted;
    }

    /// <summary>
    /// Cleanup on object destruction
    /// </summary>
    private void OnDestroy()
    {
        StopAnimation();
    }

    /// <summary>
    /// Validates that all required components are properly assigned
    /// </summary>
    /// <returns>True if all components are valid, false otherwise</returns>
    private bool ValidateComponents()
    {
        if (handPromptImage == null)
        {
            Debug.LogError("Hand Prompt Image not assigned!");
            enabled = false;
            return false;
        }

        if (startTarget == null || endTarget == null)
        {
            Debug.LogError("Start or End Target not assigned!");
            enabled = false;
            return false;
        }

        handRectTransform = handPromptImage.rectTransform;
        return true;
    }

    /// <summary>
    /// Initializes the prompt to its default state
    /// </summary>
    private void InitializePrompt()
    {
        handPromptImage.enabled = false;
        isAnimating = false;
        currentAnimationTime = 0f;
    }

    /// <summary>
    /// Schedules the animation to start after idle time
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
    /// Coroutine that checks for idle time before starting animation
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
    /// Public method to manually start the prompt animation if level hasn't been completed
    /// </summary>
    public void StartPromptAnimation()
    {
        if (!enableAutoPrompt || HasPlayerCompletedLevelType()) return;
        StartAnimation();
    }

    /// <summary>
    /// Public method to manually stop the prompt animation
    /// </summary>
    public void StopPromptAnimation()
    {
        StopAnimation();
    }

    /// <summary>
    /// Enables or disables the auto prompt feature
    /// </summary>
    /// <param name="enabled">Whether to enable or disable the prompt</param>
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
    /// Initiates the animation sequence
    /// </summary>
    private void StartAnimation()
    {
        // Ensure we're in a valid state to start animation
        if (!gameObject.activeInHierarchy) return;
        
        isAnimating = true;
        handPromptImage.enabled = true;
        currentAnimationTime = 0f;

        // Reset position and opacity
        handRectTransform.anchoredPosition = startTarget.anchoredPosition;
        handPromptImage.color = new Color(1f, 1f, 1f, 1f);

        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
        }
        animationCoroutine = StartCoroutine(AnimatePromptRoutine());
    }

    /// <summary>
    /// Coroutine that handles the actual animation sequence
    /// Moves the hand from start to end position and fades it out
    /// </summary>
    private IEnumerator AnimatePromptRoutine()
    {
        while (true)
        {
            // Movement animation
            float elapsedTime = 0f;
            while (elapsedTime < animationDuration)
            {
                elapsedTime += Time.deltaTime;
                float normalizedTime = elapsedTime / animationDuration;

                handRectTransform.anchoredPosition = Vector2.Lerp(
                    startTarget.anchoredPosition,
                    endTarget.anchoredPosition,
                    normalizedTime
                );

                yield return null;
            }

            // Fade out animation
            elapsedTime = 0f;
            float fadeDuration = 0.3f;
            Color startColor = handPromptImage.color;
            Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                float normalizedTime = elapsedTime / fadeDuration;
                handPromptImage.color = Color.Lerp(startColor, endColor, normalizedTime);
                yield return null;
            }

            // Check if animation should continue
            currentAnimationTime += animationDuration + fadeDuration;
            bool shouldContinue = totalAnimationDuration < 0 || currentAnimationTime < totalAnimationDuration;

            if (!enableAutoPrompt || !shouldContinue)
            {
                StopAnimation();
                yield break;
            }

            // Reset for next animation cycle
            handRectTransform.anchoredPosition = startTarget.anchoredPosition;
            handPromptImage.color = new Color(1f, 1f, 1f, 1f);
        }
    }

    /// <summary>
    /// Stops all animation coroutines and resets the prompt state
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
        currentAnimationTime = 0f;
    }
}