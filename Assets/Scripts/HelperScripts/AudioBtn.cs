using System.Collections;
using System.Collections.Generic;
using GlobalAudioManagerPackage;
using UnityEngine;

public class AudioBtn : MonoBehaviour
{
    // Declare a static AudioSource variable
    // private static AudioSource audioSource;
    // Start is called before the first frame update
    private void Start()
    {
        // // Get the AudioSource component from the GameObject
        // audioSource = GetComponent<AudioSource>();
        // // Set the playOnAwake property to false, so the audio doesn't play when the GameObject is created
        // audioSource.playOnAwake = false;  // Don't play on start
        // // Set the loop property to false, so the audio doesn't loop
        // audioSource.loop = false;
        // // Set the volume property to 1.0f
        // audioSource.volume = 1.0f;
    }
    // Play the audio when the button is clicked
    public static void PlayAudioOnClick()
    {
        // Check if the audioSource has a clip
        // if (audioSource.clip!=null)
        // {
        //     // Play the audio
        //     audioSource.Play();
        //     // Log a message to the console
        //     Debug.Log("audio played...");
        // }

        GlobalAudioManager.Instance.PlayVoiceOver();
    }
}
