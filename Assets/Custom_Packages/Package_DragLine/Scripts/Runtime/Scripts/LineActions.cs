using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LineActions
{
    public static event Action OnLineStarted;
    public static event Action<string> OnLineEnded;
    public static event Action<int> TriggerNodeAnimation;
    public static void TriggerLineStarted()
    {
        OnLineStarted?.Invoke();
    }

    public static void TriggerLineEnded(string outputText)
    {
        OnLineEnded?.Invoke(outputText);
    }

    public static void TriggerAnimation(int index)
    {
        TriggerNodeAnimation?.Invoke(index);
    }
}
