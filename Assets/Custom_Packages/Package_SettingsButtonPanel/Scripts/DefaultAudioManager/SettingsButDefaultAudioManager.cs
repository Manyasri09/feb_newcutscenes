using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// Handles core audio functionality, including toggling music and sound effects
public class SettingsButDefaultAudioManager : MonoBehaviour, ISettingsButPanelAudioController
{
    // Singleton instance for global access
    private static SettingsButDefaultAudioManager instance;

    [SerializeField] private AudioSource musicSource; // AudioSource for background music
    [SerializeField] private AudioSource soundSource; // AudioSource for sound effects

    // Keys used to save and load preferences in PlayerPrefs
    private const string MUSIC_ENABLED_KEY = "MusicEnabled";
    private const string SOUND_ENABLED_KEY = "SoundEnabled";

    // Events that notify listeners when the audio state changes
    public event Action<bool> OnMusicStateChanged;
    public event Action<bool> OnSoundStateChanged;

    // Fields to track the current state of music and sound
    private bool isMusicEnabled;
    private bool isSoundEnabled;

    // Public properties to access the current audio states
    public bool IsMusicEnabled => isMusicEnabled;
    public bool IsSoundEnabled => isSoundEnabled;

    // Unity's Awake method, used to initialize the singleton instance
    void Awake()
    {
        // Ensure only one instance of AudioManager exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persist this object across scenes
            LoadAudioPreferences(); // Load saved audio preferences
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    // Loads audio preferences from PlayerPrefs and applies the initial state
    private void LoadAudioPreferences()
    {
        isMusicEnabled = PlayerPrefs.GetInt(MUSIC_ENABLED_KEY, 1) == 1; // Default to enabled
        isSoundEnabled = PlayerPrefs.GetInt(SOUND_ENABLED_KEY, 1) == 1; // Default to enabled

        ApplyMusicState(); // Apply the music state
        ApplySoundState(); // Apply the sound state
    }

    // Toggles the music state and updates PlayerPrefs
    public void ToggleMusic()
    {
        isMusicEnabled = !isMusicEnabled; // Toggle the state
        PlayerPrefs.SetInt(MUSIC_ENABLED_KEY, isMusicEnabled ? 1 : 0); // Save the state
        PlayerPrefs.Save(); // Ensure changes are written
        ApplyMusicState(); // Apply the new state
        OnMusicStateChanged?.Invoke(isMusicEnabled); // Notify listeners
    }

    // Toggles the sound state and updates PlayerPrefs
    public void ToggleSound()
    {
        isSoundEnabled = !isSoundEnabled; // Toggle the state
        PlayerPrefs.SetInt(SOUND_ENABLED_KEY, isSoundEnabled ? 1 : 0); // Save the state
        PlayerPrefs.Save(); // Ensure changes are written
        ApplySoundState(); // Apply the new state
        OnSoundStateChanged?.Invoke(isSoundEnabled); // Notify listeners
    }

    // Applies the current music state by muting/unmuting the AudioSource
    private void ApplyMusicState()
    {
        if (musicSource != null)
        {
            musicSource.mute = !isMusicEnabled; // Mute if music is disabled
        }
    }

    // Applies the current sound state by muting/unmuting the AudioSource
    private void ApplySoundState()
    {
        if (soundSource != null)
        {
            soundSource.mute = !isSoundEnabled; // Mute if sound is disabled
        }
    }

    // Public getter for accessing the AudioManager instance
    public static ISettingsButPanelAudioController Instance => instance;
}
