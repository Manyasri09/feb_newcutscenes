using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class HandDraggingPrompt : MonoBehaviour
{
    [Header("Prompt Settings")]
    [SerializeField] private bool enableAutoPrompt = true;
    [SerializeField] private float totalAnimationDuration = -1f; // -1 for infinite loop

    [Header("References")]
    [SerializeField] private Image handPromptImage;
    [SerializeField] private List<Transform> nodes; // List of nodes for the hand to follow

    [Header("Animation Settings")]
    [SerializeField] private float idleTimeThreshold = 5f;         // Time before showing prompt
    [SerializeField] private float animationDurationPerSegment = 1f; // Duration to move between two nodes
    [SerializeField] private Vector3 handOffset = Vector3.zero;   // Offset for the hand's position

    private RectTransform handRectTransform;
    private bool isAnimating;
    private Coroutine idleCheckCoroutine;
    private Coroutine animationCoroutine;

    private void Awake()
    {
        ValidateComponents();
        InitializePrompt();
    }

    private void Start()
    {
        if (enableAutoPrompt)
        {
            ScheduleAnimation();
        }
        LineActions.OnLineStarted += EndAnimation;
    }

    private void OnDestroy()
    {
        StopAnimation();
    }

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

    private void InitializePrompt()
    {
        handPromptImage.enabled = false;
        isAnimating = false;
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

        // Reset hand position to the first node with the offset
        handRectTransform.position = nodes[0].position + handOffset;

        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
        }
        animationCoroutine = StartCoroutine(AnimateHandRoutine());
    }

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

                // Linear interpolation between two nodes with the offset
                handRectTransform.position = Vector3.Lerp(
                    startNode.position + handOffset,
                    endNode.position + handOffset,
                    t
                );
                yield return null;
            }

            // Snap to the end node with the offset
            handRectTransform.position = endNode.position + handOffset;
        }

        // End of animation routine
        handPromptImage.enabled = false; // Hide hand prompt at the last node
        isAnimating = false;

        if (enableAutoPrompt && totalAnimationDuration < 0)
        {
            ScheduleAnimation(); // Reschedule animation if in infinite loop mode
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
    }

    private void EndAnimation()
    {
        StopAnimation();
        enableAutoPrompt = false;
    }
}
