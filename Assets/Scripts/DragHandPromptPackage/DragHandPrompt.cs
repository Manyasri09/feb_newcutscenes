using UnityEngine;
using UnityEngine.UI;

namespace IdlePromptSystem
{
    public class DragHandPrompt : MonoBehaviour
    {
        public bool enableIdlePrompt = true;
        [Header("References")]

        public Image handPromptImage;
        //public GameObject textPrompt;

        [SerializeField] private float idleTimeThreshold = 3f;      // Time before showing prompt
        [SerializeField] private float animationDuration = 1.5f;    // Duration of single animation
        [SerializeField] private float delayBetweenLoops = 0.5f;   // Delay before repeating
        [SerializeField] private float verticalDistance = 1000f;    // Distance to move down
        [SerializeField] private float curveOffset = 150f;          // How much the curve bends to the right

        private float lastInputTime;
        private bool isAnimating;
        private Vector2 startPosition;
        private Vector2 endPosition;
        private RectTransform handRectTransform;
        private bool isPromptVisible;

        private void Start()
        {
            if (handPromptImage == null)
            {
                Debug.LogError("Hand Prompt Image not assigned to DragHandPrompt script!");
                enabled = false;
                return;
            }

            handRectTransform = handPromptImage.rectTransform;

            // Store the initial position set in the scene
            startPosition = handRectTransform.anchoredPosition;
            // Calculate end position (same x, but 1200 units down)
            endPosition = new Vector2(startPosition.x, startPosition.y - verticalDistance);

            // Hide prompt initially
            SetPromptVisibility(false);

            // Initialize input time
            ResetIdleTimer();
        }

        private void OnEnable()
        {
            ResetIdleTimer();
            SetPromptVisibility(false);
            isAnimating = false;
        }

        private void Update()
        {
            if (!enableIdlePrompt) return;

            if (HasUserInput())
            {
                if (isPromptVisible)
                {
                    StopAnimation();
                }
                ResetIdleTimer();
                return;
            }

            if (Time.time - lastInputTime >= idleTimeThreshold && !isAnimating)
            {
                StartAnimation();
            }
        }

        private bool HasUserInput()
        {
            return Input.anyKey || Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0 ||
                   Input.touchCount > 0;
        }

        private void ResetIdleTimer()
        {
            lastInputTime = Time.time;
        }

        private void StartAnimation()
        {
            if (isAnimating) return;
            //textPrompt.SetActive(true);
            isAnimating = true;
            SetPromptVisibility(true);
            AnimatePrompt();
        }

        private void StopAnimation()
        {
            if (!isAnimating) return;
            //textPrompt.SetActive(false);
            LeanTween.cancel(handRectTransform.gameObject);
            SetPromptVisibility(false);
            isAnimating = false;
        }

        private void SetPromptVisibility(bool visible)
        {
            if (handPromptImage != null)
            {
                handPromptImage.enabled = visible;
                isPromptVisible = visible;
            }
        }

        private void AnimatePrompt()
        {
            // Reset to start position
            handRectTransform.anchoredPosition = startPosition;

            // Calculate control point for curved motion
            // Midpoint between start and end, but offset to the right for a curved path
            Vector2 midPoint = (startPosition + endPosition) * 0.5f;
            midPoint.x += curveOffset; // Offset to the right for a curved path

            // Animate using LeanTween
            LeanTween.value(handRectTransform.gameObject, 0f, 1f, animationDuration)
                .setOnUpdate((float t) =>
                {
                    // Quadratic Bezier curve
                    Vector2 m1 = Vector2.Lerp(startPosition, midPoint, t);
                    Vector2 m2 = Vector2.Lerp(midPoint, endPosition, t);
                    handRectTransform.anchoredPosition = Vector2.Lerp(m1, m2, t);
                })
                .setEaseInOutQuad()
                .setOnComplete(() =>
                {
                    // Fade out
                    LeanTween.alpha(handRectTransform, 0f, 0.3f)
                        .setOnComplete(() =>
                        {
                            // Delay before next loop
                            LeanTween.delayedCall(delayBetweenLoops, () =>
                            {
                                if (isAnimating)
                                {
                                    // Reset alpha here, just before starting the next animation
                                    Color c = handPromptImage.color;
                                    c.a = 1f;
                                    handPromptImage.color = c;

                                    AnimatePrompt();
                                }
                            });
                        });
                });
        }
    }
}