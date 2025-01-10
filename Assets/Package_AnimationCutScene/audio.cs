using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameAudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource backgroundAudio;  // Background music AudioSource
    public AudioSource sentenceAudio;    // AudioSource for the sentence audio
    public AudioSource keyframeAudio;    // AudioSource for keyframe-triggered audio

    [Header("Volume Settings")]
    [Range(0f, 1f)] public float backgroundDimVolume = 0.2f;  // Volume when dimmed
    [Range(0f, 1f)] public float backgroundNormalVolume = 1f;  // Normal background volume

    [Header("Text Display")]
    public GameObject textObjectPrefab;  // Prefab for the text object
    public Transform textParent;         // Parent transform for the text object
    public string sentence = "गाँव की खामोशी मे चलते हुए, रानी ने एक रहसयमयी आवाज़ सुनी...";  // The sentence to display

    private GameObject currentTextObject; // Reference to the dynamically created text object

    /// <summary>
    /// Initializes the background audio and sets up the audio playback.
    /// This method is called when the GameObject is first created.
    /// </summary>
        private void Start()
    {
        // Play background music on loop
        if (backgroundAudio != null)
        {
            backgroundAudio.loop = true;
            backgroundAudio.volume = backgroundNormalVolume;
            backgroundAudio.Play(); 
        }
    }

    /// <summary>
    /// Triggered by an Animation Event at a keyframe.
    /// </summary>
    public void PlayKeyframeAudio()
    {
        if (keyframeAudio != null)
        {
            Debug.Log("Playing keyframe audio..."); // Optional: Log the event for debugging
            DimBackgroundAudio();
            keyframeAudio.Play();
        }
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
            Debug.Log("Playing sentence audio..."); // Optional: Log the event for debugging
            DimBackgroundAudio();
            sentenceAudio.volume = backgroundNormalVolume;
            sentenceAudio.Play();

            // Dynamically create the text object
            currentTextObject = Instantiate(textObjectPrefab, textParent);
            Text textMesh = currentTextObject.GetComponent<Text>();

            // Start displaying the sentence word by word
            StartCoroutine(DisplayTextWithAudio(sentence, textMesh));

            float waitTime = 0f;
            // Wait until the sentence audio finishes
            while (sentenceAudio.isPlaying)
            {
                waitTime += Time.deltaTime;
                if (waitTime % 1f < Time.deltaTime) // Log every second
                {
                    Debug.Log($"Still waiting... Time: {waitTime:F1}s, Audio playing: {sentenceAudio.isPlaying}, Time position: {sentenceAudio.time}/{sentenceAudio.clip.length}");
                }
                yield return null;
            }

            RestoreBackgroundAudio(); // Restore background audio after sentence audio
        }
    }

    IEnumerator DisplayTextWithAudio(string sentence, Text textMesh)
    {
        string[] words = sentence.Split(' '); // Split sentence into words
        textMesh.text = ""; // Clear the text initially

        foreach (string word in words)
        {
            textMesh.text += word + " "; // Display one word at a time
            yield return new WaitForSeconds(0.5f); // Adjust the delay between words as needed
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
