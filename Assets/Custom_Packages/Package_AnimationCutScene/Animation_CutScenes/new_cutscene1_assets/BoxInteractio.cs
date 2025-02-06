using UnityEngine;
using System.Collections;

public class BoxInteractio : MonoBehaviour
{
    private Animator animator;
    public GameObject handIcon; // Assign Hand Icon in Inspector

    void Start()
    {
        animator = GetComponent<Animator>(); // Get the Animator component
    }

    void OnMouseDown()
    {
        if (animator != null)
        {
            animator.SetTrigger("Play"); // Trigger the animation
            
            if (handIcon != null)
            {
                handIcon.SetActive(false); // Hide the hand icon
            }

            // Invoke method to check when animation ends
            StartCoroutine(WaitForAnimation());
        }
        else
        {
            Debug.LogError("Animator not found on " + gameObject.name);
        }
    }

    private IEnumerator WaitForAnimation()
    {
        // Wait until the animation is finished
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        Debug.Log(gameObject.name + " animation finished!");
    }
}
