using UnityEngine;
using UnityEngine.UI;

public class DragObjectPrompt : MonoBehaviour
{
    [Header("Prompt Settings")]
    [SerializeField] private bool enableAutoPrompt = true;

    [Header("References")]
    [SerializeField] private Image handPromptImage;

    [SerializeField] private float idleTimeThreshold = 5f;      // Time before showing prompt
    [SerializeField] private float animationDuration = 1.5f;    // Duration of single animation
    [SerializeField] private float verticalDistance = 1000f;    // Distance to move down
    [SerializeField] private float curveOffset = 50f;           // How much the curve bends to the right

    private RectTransform handRectTransform;
    private Vector2 startPosition;
    private Vector2 endPosition;
    private bool isAnimating;
    private int? currentDelayCallId;

    private void Awake()
    {
        if (!ValidateComponents()) return;
        SetupPromptPositions();
        InitializePrompt();
    }

    private void Start()
    {
        if (enableAutoPrompt)
        {
            ScheduleAnimation();
        }
    }

    private void OnDestroy()
    {
        StopAnimation();
    }

    private bool ValidateComponents()
    {
        if (handPromptImage == null)
        {
            Debug.LogError("Hand Prompt Image not assigned to DragHandPrompt script!");
            enabled = false;
            return false;
        }

        handRectTransform = handPromptImage.rectTransform;
        return true;
    }

    private void SetupPromptPositions()
    {
        startPosition = handRectTransform.anchoredPosition;
        endPosition = new Vector2(
            startPosition.x + curveOffset,
            startPosition.y - verticalDistance
        );
    }

    private void InitializePrompt()
    {
        handPromptImage.enabled = false;
        isAnimating = false;
    }

    private void ScheduleAnimation()
    {
        if (currentDelayCallId.HasValue)
        {
            AnimationHelper.CancelDelayedCall(currentDelayCallId.Value);
        }

        currentDelayCallId = AnimationHelper.DelayedCall(idleTimeThreshold, () =>
        {
            if (enableAutoPrompt && !isAnimating)
            {
                StartAnimation();
            }
        });
    }

    public void CallObjectMovement()
    {
        if (isAnimating)
        {
            StopAnimation();
        }
    }

    public void StartPromptAnimation()
    {
        if (!enableAutoPrompt) return;
        StartAnimation();
    }

    public void StopPromptAnimation()
    {
        StopAnimation();
    }

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

    private void StartAnimation()
    {
        isAnimating = true;
        handPromptImage.enabled = true;

        // Reset position and make fully visible
        handRectTransform.anchoredPosition = startPosition;
        handPromptImage.color = new Color(1f, 1f, 1f, 1f);

        // Start the curved movement animation
        AnimationHelper.AnimateCurvedUIMovement(
            handRectTransform,
            startPosition,
            endPosition,
            curveOffset,
            animationDuration,
            () =>
            {
                // On complete, fade out
                AnimationHelper.FadeUIElement(
                    handRectTransform,
                    1f,
                    0f,
                    0.3f,
                    () =>
                    {
                        if (enableAutoPrompt)
                        {
                            // Reset and restart
                            handRectTransform.anchoredPosition = startPosition;
                            handPromptImage.color = new Color(1f, 1f, 1f, 1f);
                            StartAnimation();
                        }
                    }
                );
            }
        );
    }

    private void StopAnimation()
    {
        if (!isAnimating) return;

        AnimationHelper.CancelAllAnimationsForObject(gameObject);
        if (currentDelayCallId.HasValue)
        {
            AnimationHelper.CancelDelayedCall(currentDelayCallId.Value);
            currentDelayCallId = null;
        }

        handPromptImage.enabled = false;
        isAnimating = false;
    }
}
