using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WordAnimationSettings", menuName = "WordAnimation/Settings")]
public class WordAnimationSettings : ScriptableObject
{
    [Header("Movement Animation Settings")]
    public float moveDuration = 1f; // Duration to move words
    public float moveDelay = 0.5f; // Delay before moving words
    public float delay = 0f; // Delay before starting the animation
    public float staggerDelay = 0.1f; // Delay between animations of each word

    [Header("Scaling Animation Settings")]
    public float scaleDownFactor = 0.4f; // Factor to scale down words
    public float scaleDownDuration = 0.3f; // Duration to scale down words
    public float scaleUpFactor = 1.2f; // Factor to scale up words
    public float scaleUpDuration = 0.3f; // Duration to scale up words
}
