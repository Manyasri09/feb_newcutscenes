using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordAnimationBridge : MonoBehaviour
{
    [SerializeField] private WordAnimationManager wordAnimationManager; 

    // Subscribe to the animation trigger event when the component is enabled.
    private void OnEnable()
    {
        LineActions.TriggerNodeAnimation += TriggerAnimation; 
    }

    // Unsubscribe from the event when the component is disabled to avoid memory leaks.
    private void OnDisable()
    {
        LineActions.TriggerNodeAnimation -= TriggerAnimation; 
    }

    // This method is called by the LineActions class when an animation trigger event occurs. 
    // It forwards the event to the WordAnimationManager, allowing it to handle the animation logic.
    private void TriggerAnimation(int index)
    {
        wordAnimationManager.OnWordAnimationTriggered(index); 
    }
}