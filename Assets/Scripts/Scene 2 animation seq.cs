using UnityEngine;

public class AnimationTrigger : MonoBehaviour
{
    public GameObject firstObject; // Reference to the first object
    public Animator secondObjectAnimator; // Animator of the second object
    public GameObject secondObject; // Reference to the second object
    public string secondAnimationName; // Name of the second animation

    public void PlaySecondAnimation()
    {
        // Deactivate or remove the first object
        Destroy(firstObject); // Use Destroy to remove the object
        // OR
        // firstObject.SetActive(false); // Use this to deactivate the object instead of destroying it

        // Activate the second object
        secondObject.SetActive(true);

        // Play the second object's animation
        secondObjectAnimator.Play(secondAnimationName);
    }
}
