using UnityEngine;

public class Doodle_movement_1 : MonoBehaviour
{
    public GameObject handObject; // Assign the Hand Object in the Inspector
    private Animator handAnimator;

    void Start()
    {
        if (handObject != null)
        {
            handObject.SetActive(false); // Hide hand at start
            handAnimator = handObject.GetComponent<Animator>(); // Get the animator
        }
    }

    // Called from the Animation Event when Doodle's animation ends
    public void TriggerHandAnimation()
    {
        if (handObject != null)
        {
            handObject.SetActive(true); // Show hand
            if (handAnimator != null)
            {
                handAnimator.SetTrigger("ShowHand"); // Start hand animation
            }
        }
        else
        {
            Debug.LogError("Hand object is not assigned!");
        }
    }
}

