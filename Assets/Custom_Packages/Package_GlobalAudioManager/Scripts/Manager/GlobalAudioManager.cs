using System.Collections;
using System;
using UnityEngine;


namespace GlobalAudioManagerPackage
{
    [DefaultExecutionOrder(-100)]
    public class GlobalAudioManager : MonoBehaviour, IGlobalAudioManager, ISettingsButPanelAudioController
    {
        public static GlobalAudioManager Instance { get; private set; }

        [Header("Audio Sources")]
        [SerializeField] private AudioSource musicSource;  // For background music
        [SerializeField] private AudioSource sfxSource;    // For one-shot sound effects
        [SerializeField] private AudioSource voiceOverSource; // For voice-over audio
        [SerializeField] private AudioSource ambientSource; // For ambient sounds
        [SerializeField] private AudioSource extras; // For cutscene audio

        [Header("Audio Config")]
        [SerializeField] private GlobalAudioConfig audioConfig;
        public GlobalAudioConfig AudioConfig => audioConfig;

        [Header("Default Settings")]
        [Range(0f, 1f)]
        [SerializeField] private float defaultMusicVolume = 1f;
        [Range(0f, 1f)]
        [SerializeField] private float defaultSFXVolume = 1f;

        private bool isMusicEnabled;
        private bool isSoundEnabled;

        public bool IsMusicEnabled => isMusicEnabled;
        public bool IsSoundEnabled => isSoundEnabled;

        public event Action<bool> OnMusicStateChanged;
        public event Action<bool> OnSoundStateChanged;

        private const string MUSIC_ENABLED_KEY = "MusicEnabled";
        private const string SOUND_ENABLED_KEY = "SoundEnabled";



        // Active fade coroutine (if any) to avoid starting multiple fades simultaneously
        private Coroutine currentFadeCoroutine = null;

        private void Awake()
        {
            // Implement Singleton pattern
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Initialize volumes
            musicSource.volume = defaultMusicVolume;
            sfxSource.volume = defaultSFXVolume;
            LoadAudioPreferences(); // Load saved audio preferences
        }

        private void LoadAudioPreferences()
        {
            isMusicEnabled = PlayerPrefs.GetInt(MUSIC_ENABLED_KEY, 1) == 1; // Default to enabled
            isSoundEnabled = PlayerPrefs.GetInt(SOUND_ENABLED_KEY, 1) == 1; // Default to enabled

            ApplyMusicState(); // Apply the music state
            ApplySoundState(); // Apply the sound state
        }

        public void ToggleMusic()
        {
            isMusicEnabled = !isMusicEnabled; // Toggle the state
            PlayerPrefs.SetInt(MUSIC_ENABLED_KEY, isMusicEnabled ? 1 : 0); // Save the state
            PlayerPrefs.Save(); // Ensure changes are written
            ApplyMusicState(); // Apply the new state
            OnMusicStateChanged?.Invoke(isMusicEnabled); // Notify listeners
        }

        private void ApplyMusicState()
        {
            if (musicSource != null)
            {
                musicSource.mute = !isMusicEnabled; // Mute if music is disabled
                ambientSource.mute = !isMusicEnabled; // Mute if music is disabled
            }
        }

        public void ToggleSound()
        {
            isSoundEnabled = !isSoundEnabled; // Toggle the state
            PlayerPrefs.SetInt(SOUND_ENABLED_KEY, isSoundEnabled ? 1 : 0); // Save the state
            PlayerPrefs.Save(); // Ensure changes are written
            ApplySoundState(); // Apply the new state
            OnSoundStateChanged?.Invoke(isSoundEnabled); // Notify listeners
        }

        private void ApplySoundState()
        {
            if (sfxSource != null)
            {
                sfxSource.mute = !isSoundEnabled; // Mute if sound is disabled
                extras.mute = !isSoundEnabled; // Mute if sound is disabled
                voiceOverSource.mute = !isSoundEnabled; // Mute if sound is disabled
            }
        }

        #region Basic Methods

        public void PlayMusic(AudioClip clip, bool loop = true, float volume = 1f)
        {
            if (clip == null) return;
            musicSource.clip = clip;
            musicSource.loop = loop;
            musicSource.volume = Mathf.Clamp01(volume);
            musicSource.Play();
        }

        public void PlayAmbient(AudioClip clip, bool loop = true, float volume = 1f)
        {
            if (clip == null) return;
            ambientSource.clip = clip;
            ambientSource.loop = loop;
            ambientSource.volume = Mathf.Clamp01(volume);
            ambientSource.Play();
        }
        public void PlaySFX(AudioClip clip, float volume = 1f)
        {
            if (clip == null) return;
            sfxSource.PlayOneShot(clip, Mathf.Clamp01(volume));
        }

        public void PlayExtras(AudioClip clip, float volume = 1f)
        {
            if (clip == null) return;
            extras.PlayOneShot(clip, Mathf.Clamp01(volume));
        }

        public void SetVoiceOverClip(AudioClip clip)
        {
            if (clip == null) return;
            voiceOverSource.clip = clip;
        }

        public void PlayVoiceOver(float volume = 1f)
        {
            if (voiceOverSource.clip == null) return;
            voiceOverSource.Play();
        }

        public void PlayVoiceOver(AudioClip clip, float volume = 1f)
        {
            if (clip == null) return;
            voiceOverSource.clip = clip;
            voiceOverSource.Play();
        }

        public bool IsVoiceOverPlaying()
        {
            return voiceOverSource.isPlaying;
        }

        public void StopMusic()
        {
            if (musicSource.isPlaying)
            {
                musicSource.Stop();
            }
        }

        public void StopAmbient()
        {
            if (ambientSource.isPlaying)
            {
                ambientSource.Stop();
            }
        }

        public void StopSFX()
        {
            if (sfxSource.isPlaying)
            {
                sfxSource.Stop();   
            }
        }

        public void SetMusicVolume(float volume)
        {
            musicSource.volume = Mathf.Clamp01(volume);
        }
        public void SetAmbientVolume(float volume)
        {
            ambientSource.volume = Mathf.Clamp01(volume);
        }

        public void SetSFXVolume(float volume)
        {
            sfxSource.volume = Mathf.Clamp01(volume);
        }

        #endregion

        #region Fade Methods

        /// <summary>
        /// Fades in a new music track over the specified duration.
        /// If a track is already playing, it stops immediately (or you could crossfade if you wish).
        /// </summary>
        /// <param name="clip">AudioClip to play.</param>
        /// <param name="duration">Time to reach target volume.</param>
        /// <param name="loop">Whether to loop the clip.</param>
        /// <param name="targetVolume">Volume level to fade to (0f - 1f).</param>
        public void FadeInMusic(AudioClip clip, float duration, bool loop = true, float targetVolume = 1f)
        {
            // Stop any ongoing fade
            if (currentFadeCoroutine != null)
            {
                StopCoroutine(currentFadeCoroutine);
            }

            // Immediately stop any current music
            musicSource.Stop();

            // Reset volume to 0, start playing the new clip
            musicSource.clip = clip;
            musicSource.loop = loop;
            musicSource.volume = 0f;
            musicSource.Play();

            // Start fading up
            currentFadeCoroutine = StartCoroutine(FadeMusicRoutine(targetVolume, duration));
        }

        /// <summary>
        /// Fades out the currently playing music over a specified duration.
        /// </summary>
        /// <param name="duration">Time to go from current volume to 0f.</param>
        public void FadeOutMusic(float duration)
        {
            if (currentFadeCoroutine != null)
            {
                StopCoroutine(currentFadeCoroutine);
            }

            // Fade to 0 volume
            currentFadeCoroutine = StartCoroutine(FadeMusicRoutine(0f, duration, stopAtEnd:true));
        }

        private IEnumerator FadeMusicRoutine(float targetVolume, float duration, bool stopAtEnd = false)
        {
            float startVolume = musicSource.volume;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;
                // Smooth step could be used: float smoothedT = t*t*(3f - 2f*t);
                musicSource.volume = Mathf.Lerp(startVolume, targetVolume, t);
                yield return null;
            }

            // Final volume
            musicSource.volume = targetVolume;

            // Optionally stop the music if we're fading out
            if (stopAtEnd && targetVolume <= 0.01f)
            {
                musicSource.Stop();
            }

            currentFadeCoroutine = null;
        }

        #endregion
    }
}