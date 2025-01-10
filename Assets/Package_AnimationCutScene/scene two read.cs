using UnityEngine;
using TMPro;
using System.Collections;
using System.Linq;
using UnityEngine.UI;

public class AnimationEventManager : MonoBehaviour
{
    public Text eventText; // Assign your TextMeshPro object here
    public AudioSource audioSource; // Assign your AudioSource here
    public string displayMessage = "रानी की यात्रा में अंग्रेजी के सवाल और चुनौतियां होंगी। उसे आगे बढ़ाने में मदद करें।";
    public float wordDisplayDelay = 0.5f; // Delay between each word
    public float dimVolumeLevel = 0.2f; // Volume level to dim other audio sources

    private AudioSource[] allAudioSources; // Store all audio sources in the scene
    private float[] originalVolumes; // Store the original volumes of the audio sources

    private void Start()
    {
        if (eventText != null)
        {
            eventText.gameObject.SetActive(false); // Hide the text initially
        }

        // Get all AudioSources in the scene
        allAudioSources = FindObjectsOfType<AudioSource>();
        originalVolumes = new float[allAudioSources.Length];

        // Save the original volume levels
        for (int i = 0; i < allAudioSources.Length; i++)
        {
            originalVolumes[i] = allAudioSources[i].volume;
        }
    }

    // Method to be called by Animation Event
    public void TriggerEvent()
    {
        // Dim other audio sources
        DimOtherAudio(true);

        // Play the audio
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Play();
            StartCoroutine(RestoreOtherAudioAfterPlayback(audioSource.clip.length));
        }

        // Start displaying the text word by word
        if (eventText != null)
        {
            eventText.gameObject.SetActive(true); // Show the text object
            StartCoroutine(DisplayTextWordByWord(displayMessage));
        }
    }

    private IEnumerator DisplayTextWordByWord(string message)
    {
        eventText.text = ""; // Clear any previous text
        string[] words = message.Split(' '); // Split the sentence into words

        foreach (string word in words)
        {
            eventText.text += word + " "; // Add the next word
            yield return new WaitForSeconds(wordDisplayDelay); // Wait before adding the next word
        }

        // Keep the text visible until the scene ends (no need to hide it)
    }

    private void DimOtherAudio(bool dim)
    {
        for (int i = 0; i < allAudioSources.Length; i++)
        {
            if (allAudioSources[i] != audioSource)
            {
                allAudioSources[i].volume = dim ? dimVolumeLevel : originalVolumes[i];
            }
        }
    }

    private IEnumerator RestoreOtherAudioAfterPlayback(float playbackDuration)
    {
        yield return new WaitForSeconds(playbackDuration);

        // Restore other audio sources' volumes
        DimOtherAudio(false);
    }
}
