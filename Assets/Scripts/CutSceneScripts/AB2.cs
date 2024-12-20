using UnityEngine;

public class AnimationAudioHelper : MonoBehaviour
{
    public AudioSource audioSource; // Reference to the audio source
    public AudioClip animationClip; // Reference to the audio clip

    void Start()
    {
        // Assign the audio clip to the audio source
        if (audioSource != null && animationClip != null)
        {
            audioSource.clip = animationClip;
        }
    }

    // Function to play audio
    public void PlayAudio()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }

    // Function to stop audio
    public void StopAudio()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }
}
