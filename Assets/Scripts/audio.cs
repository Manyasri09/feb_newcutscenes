using UnityEngine;

public class Audio : MonoBehaviour
{
    public AudioSource backgroundAudio; // Background music AudioSource
    public AudioSource introAudio;      // Intro music AudioSource

    private float backgroundOriginalVolume; // To store the original background music volume

    void Start()
    {
        // Check if audio sources are assigned
        if (backgroundAudio == null || introAudio == null)
        {
            Debug.LogError("Audio sources are not assigned in the Inspector.");
            return;
        }

        // Store the original background music volume
        backgroundOriginalVolume = backgroundAudio.volume;

        // Play intro music
        introAudio.loop = true; // Ensure intro music loops
        introAudio.Play();

        // Reduce background music volume
        backgroundAudio.volume = backgroundOriginalVolume * 0.3f; // Reduce volume to 30%
        backgroundAudio.loop = true; // Ensure background music loops
        backgroundAudio.Play();

        // Stop intro music and restore background music volume after 15 seconds
        Invoke(nameof(StopIntroMusic), 8f);
    }

    void StopIntroMusic()
    {
        if (introAudio.isPlaying)
        {
            introAudio.Stop(); // Stop intro music
            Debug.Log("Intro music stopped.");
        }

        // Restore background music volume
        backgroundAudio.volume = backgroundOriginalVolume;
        Debug.Log("Background music volume restored.");
    }
}
