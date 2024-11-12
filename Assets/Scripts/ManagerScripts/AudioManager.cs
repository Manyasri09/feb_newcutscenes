using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip correctClip;
    [SerializeField] private AudioClip wrongClip;
    [SerializeField] private AudioClip bkgClip;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource musicSource;

    private void OnEnable()
    {
        //audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;  // Don't play on start
        audioSource.loop = false;
        audioSource.volume = 1.0f;
        musicSource.volume = 0.65f;
    }

    public void PlayCorrectAudio()
    {
        audioSource.clip = correctClip;
        audioSource.Play();
        Invoke("StopAudio", 2.5f);
    }
    public void PlayWrongAudio()
    {
        audioSource.clip = wrongClip;
        audioSource.Play();
    }

    public void PlayBkgMusic()
    {
        musicSource.clip = bkgClip;
        musicSource.Play();
    }


    void StopAudio()
    {
        // Stop the audio playback
        audioSource.Stop();
    }
}
