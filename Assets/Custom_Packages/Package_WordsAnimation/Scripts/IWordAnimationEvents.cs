using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines an interface for handling events related to word animations.
/// </summary>
/// <param name="wordIndex">The index of the word that triggered the animation event.</param>
public interface IWordAnimationEvents 
{
    void OnWordAnimationTriggered(int wordIndex);
}