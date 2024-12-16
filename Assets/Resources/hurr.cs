using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip soundClip;

    // This method is called by the Animation Event
    public void PlaySound()
    {
        if (audioSource != null && soundClip != null)
        {
            Debug.Log(Color.yellow + "Playing the audio");
            audioSource.PlayOneShot(soundClip);
        }
        else
        {
            Debug.LogWarning("AudioSource or AudioClip is missing!");
        }
    }
}
