using UnityEngine;

public class AnimationSoundController : MonoBehaviour
{
    [Tooltip("The AudioSource component to play the sound.")]
    public AudioSource audioSource;

    /// <summary>
    /// Play the sound.
    /// This method will be called from an animation event.
    /// </summary>
    public void PlaySound()
    {
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning($"AudioSource or AudioClip is missing on {gameObject.name}!");
        }
    }
}
