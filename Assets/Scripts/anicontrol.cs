using System.Collections;
using UnityEngine;

public class SequentialAnimatorController : MonoBehaviour
{
    public GameObject[] animationObjects; // Array of GameObjects with animations

    private void Start()
    {
        StartCoroutine(PlayAnimationsSequentially());
    }

    private IEnumerator PlayAnimationsSequentially()
    {
        for (int i = 0; i < animationObjects.Length; i++)
        {
            GameObject obj = animationObjects[i];

            // Activate the current object
            obj.SetActive(true);

            // Get the Animator component
            Animator animator = obj.GetComponent<Animator>();
            if (animator == null)
            {
                Debug.LogWarning($"GameObject {obj.name} does not have an Animator component!");
                continue;
            }

            // Play the animation and get the length of the clip
            float clipLength = GetCurrentAnimationClipLength(animator);

            // Wait for the animation to complete
            yield return new WaitForSeconds(clipLength);

            if (i == 3) // For animation 4 (index 3)
            {
                // For animation 4, stop the Animator to freeze at its last frame
                animator.enabled = false;
            }
            else if (i < 3)
            {
                // Deactivate objects for animations 1, 2, and 3 after they finish
                obj.SetActive(false);
            }
            else
            {
                // For animation 5 onward, stop the Animator and keep the last frame
                animator.enabled = false;
            }
        }

        Debug.Log("All animations have finished playing in sequence.");
    }

    private float GetCurrentAnimationClipLength(Animator animator)
    {
        if (animator.runtimeAnimatorController == null || 
            animator.runtimeAnimatorController.animationClips.Length == 0)
        {
            Debug.LogWarning($"Animator on {animator.gameObject.name} has no animation clips!");
            return 0;
        }

        // Get the first animation clip length (assuming one clip per object)
        return animator.runtimeAnimatorController.animationClips[0].length;
    }
}
