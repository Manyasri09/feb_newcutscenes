using UnityEngine;

public static class AnimationHelper
{
    public static void StartPulse(GameObject target, Vector3 targetScale, float pulseSpeed)
    {
        // Cancel any existing tweens on this object first
        LeanTween.cancel(target);

        // Start new pulse animation
        LeanTween.scale(target, targetScale, pulseSpeed)
            .setEase(LeanTweenType.easeInOutSine)
            .setLoopPingPong();
    }

    //ripple effect animation
    public static void CreateRipple(GameObject target, float maxScale, float animationDuration)
    {
        if (target == null) return; //safety checks

        // Cancel any existing tweens on this object to prevent conflicts
        LeanTween.cancel(target);

        // Store the original scale
        Vector3 originalScale = target.transform.localScale;

        // Ensure we have a CanvasGroup for alpha manipulation
        CanvasGroup canvasGroup = target.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = target.AddComponent<CanvasGroup>();
        }

        // Function to start a single animation cycle
        void StartRippleAnimation()
        {
            if (target == null || !target.activeInHierarchy) return;

            // Reset to initial state
            target.transform.localScale = originalScale;
            canvasGroup.alpha = 1f;

            // Scale up quickly (using 1/3 of the total duration)
            LeanTween.scale(target, Vector3.one * maxScale, animationDuration)
                .setEase(LeanTweenType.easeOutExpo); // Changed to easeOutExpo for faster initial movement

            // Fade out over the full duration
            LeanTween.alphaCanvas(canvasGroup, 0f, animationDuration)
                .setEase(LeanTweenType.easeOutQuad)
                .setOnComplete(StartRippleAnimation);
        }

        // Start the first animation cycle
        StartRippleAnimation();
    }

    public static void StopAnimating(GameObject target)
    {
        if (target == null) return;

        // Cancel all tweens
        LeanTween.cancel(target);

        // Try to reset the object if it still exists
        try
        {
            // Reset scale
            target.transform.localScale = Vector3.one;

            // Reset alpha if CanvasGroup exists
            var canvasGroup = target.GetComponent<CanvasGroup>();
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 1f;
            }
        }
        catch (MissingReferenceException)
        {
            // Object was already destroyed, nothing to do
        }
    }

    public static void MakeNudge(GameObject selectedOption)
    {
        LeanTween.moveX(selectedOption, selectedOption.transform.position.x + 0.1f, 0.05f)
                    .setEase(LeanTweenType.easeInOutSine)
                    .setLoopPingPong((int)(0.5f / 0.05f))
                    .setOnComplete(() => selectedOption.transform.position = selectedOption.transform.position);
    }

    // Animates UI element in a curved path using quadratic Bezier curve
    public static void AnimateCurvedUIMovement(RectTransform rectTransform, Vector2 startPosition, Vector2 endPosition, float curveOffset, float duration, System.Action onComplete = null)
    {
        Vector2 controlPoint = new Vector2(
            startPosition.x + curveOffset,
            startPosition.y - (startPosition.y - endPosition.y) * 0.5f
        );

        LeanTween.value(rectTransform.gameObject, 0f, 1f, duration)
            .setOnUpdate((float t) =>
            {
                Vector2 newPosition = Vector2.Lerp(
                    Vector2.Lerp(startPosition, controlPoint, t),
                    Vector2.Lerp(controlPoint, endPosition, t),
                    t
                );
                rectTransform.anchoredPosition = newPosition;
            })
            .setOnComplete(() => onComplete?.Invoke())
            .setEase(LeanTweenType.easeInOutSine);
    }

    // Fades UI element's alpha
    public static void FadeUIElement(
        RectTransform rectTransform,
        float startAlpha,
        float endAlpha,
        float duration,
        System.Action onComplete = null)
    {
        LeanTween.alpha(rectTransform, endAlpha, duration)
            .setOnComplete(() => onComplete?.Invoke());
    }

    /*
        // Utility function for delayed calls
        public static int DelayedCall(float delay, System.Action action)
        {
            return LeanTween.delayedCall(delay, action).id;
        }
    */
    // Cancel a specific delayed call
    public static void CancelDelayedCall(int id)
    {
        LeanTween.cancel(id);
    }

    // Cancel all animations for a specific GameObject
    public static void CancelAllAnimationsForObject(GameObject gameObject)
    {
        LeanTween.cancel(gameObject);
    }
}
