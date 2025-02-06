using UnityEngine;
using TMPro;
using UnityEngine.UI;  // Make sure you have this to use Button

public class WordFlipper : MonoBehaviour
{
    public TMP_Text[] wordTexts; // Array to hold "टॉर्च", "चालू", "करो"
    public AudioSource audioSource; // AudioSource for playing clips
    public AudioClip englishAudio; // English audio clip

    private bool[] isWordFlipped; // Array to track the state of each word (flipped or not)
    private int flippedWordsCount = 0; // Counter for flipped words

    private void Start()
    {
        // Initialize arrays
        isWordFlipped = new bool[wordTexts.Length];

        // Add click listeners to each word
        for (int i = 0; i < wordTexts.Length; i++)
        {
            int index = i; // Capture the index for the listener
            Button btn = wordTexts[i].gameObject.AddComponent<Button>(); // Add a button component dynamically

            // Add onClick event to each word
            btn.onClick.AddListener(() => OnWordSelected(index));
        }
    }

    // This method will be called when a word is clicked
    public void OnWordSelected(int wordIndex)
    {
        // Flip the word
        if (!isWordFlipped[wordIndex]) // If the word has not been flipped yet
        {
            // Change the text of the word to English
            switch (wordIndex)
            {
                case 0: wordTexts[wordIndex].text = "torch"; break;  // "टॉर्च" to "torch"
                case 1: wordTexts[wordIndex].text = "turn"; break;  // "चालू" to "on"
                case 2: wordTexts[wordIndex].text = "on"; break;  // "करो" to "do"
            }

            // Mark this word as flipped
            isWordFlipped[wordIndex] = true;
            flippedWordsCount++;

            // Check if all words are flipped, if yes, play audio
            if (flippedWordsCount == wordTexts.Length)
            {
                PlayAudio();
            }
        }
    }

    // Play the audio once all words are flipped
    private void PlayAudio()
    {
        // Play the English audio
        audioSource.clip = englishAudio;
        audioSource.Play();
    }
}
