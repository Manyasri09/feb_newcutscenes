using UnityEngine;

namespace GlobalAudioManagerPackage
{

    [System.Serializable]
    public class CutSceneAudioClips
    {
        public AudioClip AmbientSounds;
        public AudioClip BackgroundMusic;
        public AudioClip extras;
        public AudioClip CutsceneVO;
    }

    [CreateAssetMenu(fileName = "AudioConfig", menuName = "MyAudioPackage/AudioConfig")]
    public class GlobalAudioConfig : ScriptableObject
    {
        [Header("Menu Music")]
        public AudioClip mainMenuMusic;
        public AudioClip settingsMenuMusic;

        [Header("Gameplay Music")]
        public AudioClip levelBackgroundMusic;
        public AudioClip bossBattleMusic;

        public AudioClip CoinAudio;

        [Header("Common SFX")]
        public AudioClip buttonClickSFX;
        public AudioClip coinPickupSFX;
        public AudioClip correctAnswerSFX;
        public AudioClip incorrectAnswerSFX;
        public AudioClip victorySFX;
        public AudioClip defeatSFX;

        [Header("Cutscene / Voice Overs")]

        public CutSceneAudioClips cutSceneOne;
        public CutSceneAudioClips cutSceneTwo;
        public CutSceneAudioClips cutSceneThree;
        // Or use lists/dictionaries if you have many clips
    }
}