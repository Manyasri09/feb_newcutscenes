using UnityEngine;

public class SimpleAudioPlayer : MonoBehaviour
{
    public AudioClip audioClip; // The audio clip to play
    private AudioSource audioSource; // The AudioSource component

    void Start()
    {
        // Add an AudioSource component to the GameObject if it doesn't exist
        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Assign the audio clip to the AudioSource
        audioSource.clip = audioClip;
    }

    public void PlayAudio()
    {
        // Play the audio
        if (audioClip != null)
        {
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("No audio clip assigned!");
        }
    }
}
