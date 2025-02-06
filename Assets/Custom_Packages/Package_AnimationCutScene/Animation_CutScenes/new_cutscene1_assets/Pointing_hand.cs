using UnityEngine;

public class Pointing_hand : MonoBehaviour
{
    public GameObject handIcon; // Assign the hand sprite in the Inspector

    void Start()
    {
        ShowHandIcon(); // Show the hand when the scene starts
    }

    public void ShowHandIcon()
    {
        if (handIcon != null)
        {
            handIcon.SetActive(true); // Make sure the hand is visible
        }
        else
        {
            Debug.LogError("Hand Icon is not assigned!");
        }
    }

    public void HideHandIcon()
    {
        if (handIcon != null)
        {
            handIcon.SetActive(false); // Hide the hand when needed
        }
    }
}
