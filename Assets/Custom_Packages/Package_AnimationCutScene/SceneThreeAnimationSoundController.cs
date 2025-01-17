using UnityEngine;
using GlobalAudioManagerPackage;

public class SceneThreeAnimationSoundController : MonoBehaviour
{
    [Tooltip("The AudioSource component to play the sound.")]
    // public AudioSource audioSource;

    public AudioClip soundClip;

    /// <summary>
    /// Play the sound.
    /// This method will be called from an animation event.
    /// </summary>
    public void PlaySound()
    {
        if (soundClip != null)
        {
            // audioSource.Play();
            GlobalAudioManager.Instance.PlaySFX(soundClip); // Use the GlobalAudioManager to play the sound
        }
        else
        {
            Debug.LogWarning($"AudioSource or AudioClip is missing on {gameObject.name}!");
        }
    }
}
