using UnityEngine;
using System.Collections;

public class AnimationSequenceController : MonoBehaviour
{
    public Animator animator1;
    public Animator animator2;
    public Animator animator3;

    void Start()
    {
        StartCoroutine(PlayAnimationsInSequence());
    }

    private IEnumerator PlayAnimationsInSequence()
    {
        animator1.SetTrigger("Play");
        yield return new WaitForSeconds(GetAnimationLength(animator1));

        animator2.SetTrigger("Play");
        yield return new WaitForSeconds(GetAnimationLength(animator2));

        animator3.SetTrigger("Play");
    }

    private float GetAnimationLength(Animator animator)
    {
        return animator.runtimeAnimatorController.animationClips[0].length;
    }
}
