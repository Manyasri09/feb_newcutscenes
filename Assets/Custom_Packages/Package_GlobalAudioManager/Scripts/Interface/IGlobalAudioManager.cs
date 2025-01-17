using UnityEngine;

namespace GlobalAudioManagerPackage
{
    /// <summary>
    /// Defines the core functionality for a game-wide audio manager.
    /// This allows swapping out implementations without changing client code.
    /// </summary>
    public interface IGlobalAudioManager
    {
        void PlayMusic(AudioClip clip, bool loop = true, float volume = 1f);
        void PlaySFX(AudioClip clip, float volume = 1f);
        void StopMusic();
        void SetMusicVolume(float volume);
        void SetSFXVolume(float volume);

        // Example scalable methods:
        void FadeInMusic(AudioClip clip, float duration, bool loop = true, float targetVolume = 1f);
        void FadeOutMusic(float duration);
    }
}