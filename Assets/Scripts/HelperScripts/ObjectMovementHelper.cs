using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using PlayerProgressSystem;

public class ObjectMovementHelper : MonoBehaviour
{
    [Header("Prompt Settings")]
    [SerializeField] private bool enableAutoPrompt = true;
    [SerializeField] private float totalAnimationDuration = -1f;  // -1 for infinite loop
    [SerializeField] private string levelType;

    [Header("References")]
    [SerializeField] private Image handPromptImage;

    [Header("Animation Settings")]
    [SerializeField] private float idleTimeThreshold = 5f;      // Time before showing prompt
    [SerializeField] private float animationDuration = 1.5f;    // Duration of single animation
    [SerializeField] private float verticalDistance = 1000f;    // Distance to move down
    [SerializeField] private float curveOffset = 50f;           // How much the curve bends to the right


    //private PlayerProgressManager progressManager;

    private RectTransform handRectTransform;
    private Vector2 startPosition;
    private Vector2 endPosition;
    private bool isAnimating;
    private float currentAnimationTime;
    private Coroutine idleCheckCoroutine;
    private Coroutine animationCoroutine;

    private void Awake()
    {
        if (!ValidateComponents()) return;
        SetupPromptPositions();
        InitializePrompt();

    }

    private void Start()
    {
        //if (progressManager.HasCompletedSubLevel(levelType))
        //{
        //    enableAutoPrompt = false;
        //}

        if (enableAutoPrompt)
        {
            ScheduleAnimation();
        }
        Actions.onDrag += disableAnimating;
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
        currentAnimationTime = 0f;
    }

    private void ScheduleAnimation()
    {
        if (idleCheckCoroutine != null)
        {
            StopCoroutine(idleCheckCoroutine);
        }

        idleCheckCoroutine = StartCoroutine(IdleCheckRoutine());
    }

    private IEnumerator IdleCheckRoutine()
    {
        yield return new WaitForSeconds(idleTimeThreshold);
        
        if (enableAutoPrompt && !isAnimating)
        {
            StartAnimation();
        }
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
        currentAnimationTime = 0f;

        // Reset position and make fully visible
        handRectTransform.anchoredPosition = startPosition;
        handPromptImage.color = new Color(1f, 1f, 1f, 1f);

        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
        }
        animationCoroutine = StartCoroutine(AnimatePromptRoutine());
    }

    private IEnumerator AnimatePromptRoutine()
    {
        while (true)
        {
            // Curved movement animation
            float elapsedTime = 0f;
            Vector2 controlPoint = new Vector2(
                startPosition.x + curveOffset,
                startPosition.y + (endPosition.y - startPosition.y) * 0.5f
            );

            while (elapsedTime < animationDuration)
            {
                elapsedTime += Time.deltaTime;
                float normalizedTime = elapsedTime / animationDuration;

                // Quadratic Bezier curve
                Vector2 position = Vector2.Lerp(
                    Vector2.Lerp(startPosition, controlPoint, normalizedTime),
                    Vector2.Lerp(controlPoint, endPosition, normalizedTime),
                    normalizedTime
                );

                handRectTransform.anchoredPosition = position;
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

            currentAnimationTime += animationDuration + fadeDuration;
            bool shouldContinue = totalAnimationDuration < 0 || currentAnimationTime < totalAnimationDuration;

            if (!enableAutoPrompt || !shouldContinue)
            {
                StopAnimation();
                yield break;
            }

            // Reset for next cycle
            handRectTransform.anchoredPosition = startPosition;
            handPromptImage.color = new Color(1f, 1f, 1f, 1f);
        }
    }

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

    private void enableAnimating()
    {
        enableAutoPrompt = true;
    }

    private void disableAnimating()
    {
        StopAnimation();
        enableAutoPrompt = false;
    }
}