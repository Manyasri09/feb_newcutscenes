using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordAnimationManager : MonoBehaviour, IWordAnimationEvents
{
    [SerializeField] private List<GameObject> words; // List of word to be animated
    [SerializeField] private List<GameObject> targets; // Target object where words will move to
    
    public WordAnimationSettings wordAnimationSettings; // ScriptableObject containing animation settings

    private List<Vector3> initialPositions = new List<Vector3>(); // List to store initial positions of words

    private float moveDuration; // Duration of the move animation
    private float moveDelay; // Delay before moving words
    private float delay; // Delay before starting the animation
    private float staggerDelay; // Delay between each word animation

    private float scaleDownFactor; // Scale factor for the scale down animation
    private float scaleDownDuration; // Duration of the scale down animation
    private float scaleUpFactor; // Scale factor for the scale up animation
    private float scaleUpDuration; // Duration of the scale up animation

    private int targetIndex = 0; // Index of the target object

    
    /// <summary>
    /// Triggers the animation for the word at the specified index.
    /// </summary>
    /// <param name="wordIndex">The index of the word to animate.</param>
    public void OnWordAnimationTriggered(int wordIndex)
    {
        AnimateWord(wordIndex);
    }
    

    private void Start()
    {
        // Store initial positions and rotations
        StoreInitialPositions();

        // Set animation settings
        SetAnimationSettings();
    }

    /// <summary>
    /// Sets the animation settings for the word animation based on the values in the WordAnimationSettings scriptable object.
    /// </summary>
    // This method sets the animation settings for the word animation
    private void SetAnimationSettings()
    {
        // Set the move duration to the value in the word animation settings
        moveDuration = wordAnimationSettings.moveDuration;
        // Set the move delay to the value in the word animation settings
        moveDelay = wordAnimationSettings.moveDelay;
        // Set the delay to the value in the word animation settings
        delay = wordAnimationSettings.delay;
        // Set the stagger delay to the value in the word animation settings
        staggerDelay = wordAnimationSettings.staggerDelay;

        // Set the scale down factor to the value in the word animation settings
        scaleDownFactor = wordAnimationSettings.scaleDownFactor;
        // Set the scale down duration to the value in the word animation settings
        scaleDownDuration = wordAnimationSettings.scaleDownDuration;
        // Set the scale up factor to the value in the word animation settings
        scaleUpFactor = wordAnimationSettings.scaleUpFactor;
        // Set the scale up duration to the value in the word animation settings
        scaleUpDuration = wordAnimationSettings.scaleUpDuration;
    }

    /// <summary>
    /// Stores the initial positions of the words in the `words` list.
    /// </summary>
    public void StoreInitialPositions()
    {
        // Iterate through the words and add their current positions to the `initialPositions` list
        for (int i = 0; i < words.Count; i++)
        {
            initialPositions.Add(words[i].transform.position);
        }
    }
    
    /// <summary>
    /// Resets the positions of the words in the `words` list to their initial positions.
    /// </summary>
    public void ResetWords()
    {
        // Iterate through the words and set their positions to the corresponding initial positions
        for (int i = 0; i < words.Count; i++)
        {
            words[i].transform.position = initialPositions[i];
        }
    }

    /// <summary>
    /// Calculates the world-space position of the target GameObject based on its RectTransform and the Canvas it is parented to.
    /// </summary>
    /// <param name="target">The GameObject whose world-space position should be calculated.</param>
    /// <returns>The world-space position of the target GameObject.</returns>
    private Vector3 GetTargetWorldPosition(GameObject target)
    {
        RectTransform targetRectTransform = target.GetComponent<RectTransform>();

        if (targetRectTransform == null)
        {
            Debug.LogError("Target RectTransform is not assigned!");
            return Vector3.zero;
        }

        Canvas canvas = targetRectTransform.GetComponentInParent<Canvas>();

        if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            return targetRectTransform.position; 
        }
        else
        {
            RectTransformUtility.PixelAdjustRect(targetRectTransform, canvas);
            return targetRectTransform.position;
        }
    }

    /// <summary>
    /// Animates the words in the `words` list by moving them to their respective target positions.
    /// If the `targets` list is empty, a debug error is logged and the method returns without performing any animation.
    /// The animation of each word is staggered by the `staggerDelay` value.
    /// </summary>
    public void AnimateWords()
    {
        if (targets.Count == 0)
        {
            Debug.LogError("Targets is not assigned!");
            return;
        }

        for (int i = 0; i < words.Count; i++)
        {
            // Move the coin to the target position
            Animate(words[i]);
            delay += staggerDelay;
        }

    }

    /// <summary>
    /// Animates a GameObject by moving it to a target position and performing a "pop" animation sequence.
    /// The target position is determined by the `GetTargetWorldPosition` method, and the animation is staggered by the `delay` and `moveDelay` values.
    /// The "pop" animation sequence scales the GameObject down, then up, and finally back to its original scale.
    /// </summary>
    /// <param name="word">The GameObject to be animated.</param>
    private void Animate(GameObject word)
    {

        LeanTween.move(word, GetTargetWorldPosition(targets[targetIndex]), moveDuration)
        .setDelay(delay + moveDelay)
        .setOnComplete(() => {
            // Pop animation sequence
            LeanTween.scale(word, Vector3.one * scaleDownFactor, scaleDownDuration)  // Scale down
            .setEaseOutQuad()
            .setOnComplete(() => {
                LeanTween.scale(word, Vector3.one * scaleUpFactor, scaleUpDuration)  // Scale up
                .setEaseOutQuad()
                .setOnComplete(() => {
                    LeanTween.scale(word, Vector3.one, 0.1f)  // Scale back to normal
                        .setEaseInQuad();
                });
            });
            
        });

        targetIndex = (targetIndex + 1) % targets.Count;
    }

    
    /// <summary>
    /// Adds a clickable Button component to each word GameObject in the `words` list, and attaches a click event handler that calls the `AnimateWord` method with the corresponding index.
    /// This allows the user to click on each word to trigger its animation.
    /// </summary>
    private void MakeWordsClickable()
    {
        
        for (int i = 0; i < words.Count; i++)
        {
            int index = i; 
            Button button = words[i].GetComponent<Button>();
            if (button == null)
            {
                button = words[i].AddComponent<Button>();
            }
            button.onClick.AddListener(() => AnimateWord(index));
        }
    }

    /// <summary>
    /// Animates a word by creating a copy of the word GameObject, setting its position and scale to match the original, and then animating the copy using the `Animate` method.
    /// This allows the original word GameObject to remain unchanged while a copy is animated.
    /// </summary>
    /// <param name="index">The index of the word in the `words` list to be animated.</param>
    private void AnimateWord(int index)
    {
        if (index < targets.Count)
        {
            // Create a copy of the word
            GameObject wordCopy = Instantiate(words[index], words[index].transform.position, Quaternion.identity);
            wordCopy.transform.SetParent(words[index].transform.parent); // Keep same parent as original
            wordCopy.transform.localScale = words[index].transform.localScale; // Set the same scale as original
        
            // Animate the copy instead of the original
            Animate(wordCopy);
        }
    }


    
}
