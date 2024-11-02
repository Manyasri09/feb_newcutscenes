using UnityEngine;
using UnityEngine.UI;

public class DraggableObjectPrompt : MonoBehaviour
{
    [SerializeField] private float maxScale = 2f;
    [SerializeField] private float animationDuration = 1f;

    private void OnEnable()
    {
        // Make sure we're not already destroyed
        if (this != null && gameObject != null && gameObject.activeInHierarchy)
        {
            startRipple();
            Actions.onDrag += stopRipple;
        }
    }
    private void OnDisable()
    {
        // Clean up event subscription
        Actions.onDrag -= stopRipple;

        // Only try to stop the ripple if we still exist
        if (this != null && gameObject != null)
        {
            stopRipple();
        }
    }
    private void OnDestroy()
    {
        // Make sure to clean up when object is destroyed
        Actions.onDrag -= stopRipple;
        stopRipple();
    }

    private void startRipple()
    {
        if (this == null || gameObject == null) return;

        gameObject.GetComponent<Image>().enabled = true;
        AnimationHelper.CreateRipple(this.gameObject, maxScale, animationDuration);
    }
    private void stopRipple()
    {
        if (this == null || gameObject == null) return;
        AnimationHelper.StopAnimating(this.gameObject);

        gameObject.GetComponent<Image>().enabled = false;
    }
}
