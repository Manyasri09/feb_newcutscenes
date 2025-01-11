using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineEventListener : MonoBehaviour
{
    

    private void OnEnable()
    {
        LineActions.OnLineStarted += HandleLineStarted;
        LineActions.OnLineEnded += HandleLineEnded;
        
    }

    private void OnDisable()
    {
        LineActions.OnLineStarted -= HandleLineStarted;
        LineActions.OnLineEnded -= HandleLineEnded;
       
    }

    private void HandleLineStarted()
    {
        Debug.Log("Line drawing started!");
    }

    private void HandleLineEnded(string output)
    {
        Debug.Log("Line drawing ended!");
    }

    

    private void HandleLineUpdated(Vector3[] linePositions)
    {
        Debug.Log($"Line updated with {linePositions.Length} positions.");
    }
}
