using UnityEngine;
using TMPro;
using System.Collections;
using System.Linq;
using UnityEngine.UI;
using GlobalAudioManagerPackage;

public class SceneTwoAudioController : MonoBehaviour
{
    public Text eventText; // Assign your TextMeshPro object here
    public AudioSource audioSource; // Assign your AudioSource here
    public string displayMessage = "रानी की यात्रा में अंग्रेजी में सवाल और चुनौतियां शामिल होंगी। उसे आगे बढ़ने में मदद करें।";
    public float wordDisplayDelay = 2f; // Delay between each word
    public float dimVolumeLevel = 0.2f; // Volume level to dim other audio sources

    public float normalVolume = 1f; // Duration of the dim effect

    public SceneChanger sceneChanger; // Reference to the SceneChanger script

    private AudioSource[] allAudioSources; // Store all audio sources in the scene
    private float[] originalVolumes; // Store the original volumes of the audio sources

    private void Start()
    {
        if (eventText != null)
        {
            eventText.gameObject.SetActive(false); // Hide the text initially
        }

        // // Get all AudioSources in the scene
        // allAudioSources = FindObjectsOfType<AudioSource>();
        // originalVolumes = new float[allAudioSources.Length];

        // // Save the original volume levels
        // for (int i = 0; i < allAudioSources.Length; i++)
        // {
        //     originalVolumes[i] = allAudioSources[i].volume;
        // }

        // Debug.Log(displayMessage);

       GlobalAudioManager.Instance.PlayMusic(GlobalAudioManager.Instance.AudioConfig.cutSceneTwo.BackgroundMusic);
       GlobalAudioManager.Instance.PlayAmbient(GlobalAudioManager.Instance.AudioConfig.cutSceneTwo.AmbientSounds);


    }

    // Method to be called by Animation Event
    public void TriggerEvent()
    {
        // Dim other audio sources
        // DimOtherAudio(true);
        GlobalAudioManager.Instance.SetAmbientVolume(dimVolumeLevel); // Dim ambient sounds
        GlobalAudioManager.Instance.SetMusicVolume(dimVolumeLevel); // Dim music sounds


        // Play the audio
        // if (audioSource != null && audioSource.clip != null)
        // {
        //     audioSource.Play();
        //     StartCoroutine(RestoreOtherAudioAfterPlayback(audioSource.clip.length));
        // }

        GlobalAudioManager.Instance.PlayVoiceOver(GlobalAudioManager.Instance.AudioConfig.cutSceneTwo.CutsceneVO);
        StartCoroutine(RestoreOtherAudioAfterPlayback(audioSource.clip.length));
        
        
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
            eventText.text = HindiCorrector.Correct(eventText.text);
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
        while (GlobalAudioManager.Instance.IsVoiceOverPlaying())
        {
            yield return null;
        }
        // yield return new WaitForSeconds(playbackDuration);

        // Restore other audio sources' volumes
        // DimOtherAudio(false);
        // Restore original volumes after the playback
        // GlobalAudioManager.Instance.SetAmbientVolume(normalVolume); // Dim ambient sounds
        // GlobalAudioManager.Instance.SetMusicVolume(normalVolume); // Dim music sounds
        yield return new WaitForSeconds(0.1f);
        GlobalAudioManager.Instance.StopAmbient(); // stop ambient sounds
        GlobalAudioManager.Instance.StopMusic(); // stop music sounds

        sceneChanger.SwitchScene();

    }
}
