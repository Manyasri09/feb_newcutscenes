using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using GlobalAudioManagerPackage;

public class SceneOneAudioController : MonoBehaviour
{

    

    [Header("Audio Sources")]
    public AudioSource backgroundAudio;  // Background music AudioSource
    public AudioSource sentenceAudio;    // AudioSource for the sentence audio
    public AudioSource keyframeAudio;    // AudioSource for keyframe-triggered audio

    [Header("Volume Settings")]
    [Range(0f, 1f)] public float backgroundDimVolume = 0.2f;  // Volume when dimmed
    [Range(0f, 1f)] public float backgroundNormalVolume = 1f;  // Normal background volume
    [Range(0f, 1f)] public float ambientVolume = 1f;  // Normal background volume
    [Range(0f, 1f)] public float sfxVolume = 1f;  // Normal background volume

    [Header("Text Display")]
    public Text DisplayTextField;  // Prefab for the text object
    public Transform textParent;         // Parent transform for the text object
    private string sentence = "गाँव की खामोशी में चलते हुए, रानी ने एक रहस्यमयी आवाज़ सुनी ....";  // The sentence to display

    [Header("Scene Management")]
    public SceneChanger sceneChanger;  // Reference to the SceneManager

    private GameObject currentTextObject; // Reference to the dynamically created text object

    /// <summary>
    /// Initializes the background audio and sets up the audio playback.
    /// This method is called when the GameObject is first created.
    /// </summary>
    private void Start()
    {
        // Play background music on loop
        // if (backgroundAudio != null)
        // {
        //     backgroundAudio.loop = true;
        //     backgroundAudio.volume = backgroundNormalVolume;         //TODO: Remove extra code
        //     backgroundAudio.Play(); 
        //     // sentenceAudio.Play();
        //     // keyframeAudio.Play();
        // }
        // else
        // {
        //     Debug.Log("Background audio not set.");
        // }

        // Debug.Log("Sentence: " + sentence);

        GlobalAudioManager.Instance.PlayAmbient(
            GlobalAudioManager.Instance.AudioConfig.cutSceneOne.AmbientSounds,
            true,
            ambientVolume);

        GlobalAudioManager.Instance.PlayMusic(
            GlobalAudioManager.Instance.AudioConfig.cutSceneOne.BackgroundMusic, 
            true, 
            backgroundNormalVolume); // Play the background music
        
    }

    /// <summary>
    /// Triggered by an Animation Event at a keyframe.
    /// </summary>
    public void PlayKeyframeAudio()
    {
        // if (keyframeAudio != null)
        // {
        //     Debug.Log("Playing keyframe audio..."); // Optional: Log the event for debugging   //TODO: Remove extra code
        //     DimBackgroundAudio();
        //     keyframeAudio.Play();
        // }
        GlobalAudioManager.Instance.SetMusicVolume(backgroundDimVolume);
        GlobalAudioManager.Instance.PlaySFX(GlobalAudioManager.Instance.AudioConfig.cutSceneOne.extras, sfxVolume);
    }

    /// <summary>
    /// Triggered by an Animation Event at the end of the animation.
    /// </summary>
    public void PlaySentenceAudioAndDisplayText()
    {
        Debug.Log("Playing sentence audio and displaying text..."); // Optional: Log the event for debugging
        StartCoroutine(PlayAudioAndDisplayText());
    }

    IEnumerator PlayAudioAndDisplayText()
    {
        // Play sentence audio
        if (sentenceAudio != null)
        {

            // if (keyframeAudio != null && keyframeAudio.clip != null)
            // {
            //     Debug.Log($"Audio clip details - Name: {keyframeAudio.clip.name}");
            //     Debug.Log($"Samples: {keyframeAudio.clip.samples}");
            //     Debug.Log($"Channels: {keyframeAudio.clip.channels}");
            //     Debug.Log($"Frequency: {keyframeAudio.clip.frequency}");
            //     Debug.Log($"Length: {keyframeAudio.clip.length}");
            // }

            // if (sentenceAudio != null && sentenceAudio.clip != null)
            // {
            //     Debug.Log($"Audio clip details - Name: {sentenceAudio.clip.name}");  //TODO: Remove extra code
            //     Debug.Log($"Samples: {sentenceAudio.clip.samples}");
            //     Debug.Log($"Channels: {sentenceAudio.clip.channels}");
            //     Debug.Log($"Frequency: {sentenceAudio.clip.frequency}");
            //     Debug.Log($"Length: {sentenceAudio.clip.length}");
            // }


            // Debug.Log("Playing sentence audio..."); // Optional: Log the event for debugging
            // DimBackgroundAudio();
            // sentenceAudio.volume = backgroundNormalVolume;
            // sentenceAudio.Play();

            GlobalAudioManager.Instance.SetMusicVolume(backgroundDimVolume);
            GlobalAudioManager.Instance.PlayVoiceOver(GlobalAudioManager.Instance.AudioConfig.cutSceneOne.CutsceneVO);

            // if (currentTextObject == null)
            // {
            //     currentTextObject = Instantiate(textObjectPrefab, textParent);
            // }
            // // Dynamically create the text object
            // currentTextObject = Instantiate(textObjectPrefab, textParent);


            Text textMesh = DisplayTextField;

            // Start displaying the sentence word by word
            StartCoroutine(DisplayTextWithAudio(sentence, textMesh));

            // float waitTime = 0f;
            // Wait until the sentence audio finishes
            while (GlobalAudioManager.Instance.IsVoiceOverPlaying())
            {
                // waitTime += Time.deltaTime;
                // if (waitTime % 1f < Time.deltaTime) // Log every second
                // {
                //     Debug.Log($"Still waiting... Time: {waitTime:F1}s, Audio playing: {sentenceAudio.isPlaying}, Time position: {sentenceAudio.time}/{sentenceAudio.clip.length}");
                // }
                yield return null;
            }

            GlobalAudioManager.Instance.SetMusicVolume(backgroundNormalVolume); // RestoreBackgroundAudio(); // Restore background audio after sentence audio

            // Wait for a short duration before changing the first cutscene
            yield return new WaitForSeconds(3.0f);
            GlobalAudioManager.Instance.StopMusic();
            GlobalAudioManager.Instance.StopAmbient();
            sceneChanger.SwitchScene(); // Change to the next scene

        }
    }

    IEnumerator DisplayTextWithAudio(string sentence, Text textMesh)
    {
        string[] words = sentence.Split(' '); // Split sentence into words
        textMesh.text = ""; // Clear the text initially

        foreach (string word in words)
        {
            textMesh.text += word + " "; // Display one word at a time
            textMesh.text = HindiCorrector.Correct(textMesh.text);
            yield return new WaitForSeconds(0.3f); // Adjust the delay between words as needed
        }
    }

    private void DimBackgroundAudio()
    {
        if (backgroundAudio != null)
        {
            Debug.Log("Dimming background audio..."); // Optional: Log the event for debugging
            backgroundAudio.volume = backgroundDimVolume; // Dim background audio
        }
    }

    private void RestoreBackgroundAudio()
    {
        if (backgroundAudio != null)
        {
            backgroundAudio.volume = backgroundNormalVolume; // Restore background audio volume
        }
    }
}
