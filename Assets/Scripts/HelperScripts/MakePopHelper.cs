using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakePopHelper : MonoBehaviour
{
    [SerializeField] private Vector3 targetScale = new Vector3(1.1f, 1.1f, 1.1f);
    [SerializeField] private float pulseSpeed = 4.0f;

    private void OnEnable()
    {
        startPOP();
    }
    private void OnDisable()
    {
        stopPOP();
    }

    private void startPOP()
    {
        AnimationHelper.StartPulse(this.gameObject, targetScale, pulseSpeed);
    }
    private void stopPOP()
    {
        AnimationHelper.StopAnimating(this.gameObject);
    }
}
