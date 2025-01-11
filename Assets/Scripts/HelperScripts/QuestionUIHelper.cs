using ezygamers.cmsv1;
using ezygamers.dragndropv1;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Static utility class providing helper methods for managing question-related UI components.
/// Handles setting and resetting various UI elements like text, images, audio, and drag-drop options.
/// </summary>
public class QuestionUIHelper
{
    /// <summary>
    /// Sets the text of a UI Text component if it exists.
    /// </summary>
    /// <param name="uiTextComponent">The Text component to modify.</param>
    /// <param name="text">The text to display.</param>
    public static void SetText(Text uiTextComponent, string text)
    {
        if (uiTextComponent != null)
        {
            //uiTextComponent.text = string.IsNullOrEmpty(hindiText) ? englishText : $"{hindiText}\n{englishText}";
            uiTextComponent.text = text;
        }
    }

    /// <summary>
    /// Configures multiple option holders with corresponding image, text, and drop handler data.
    /// Matches option data to UI containers and sets up drag-drop functionality.
    /// </summary>
    /// <param name="imageOptions">List of option data containing sprites, text, and IDs.</param>
    /// <param name="OptionHolders">List of UI containers for the options.</param>
    /// <param name="dropHandlers">List of drop handlers for drag-drop functionality.</param>
    /// <remarks>
    /// Only processes the minimum count between available options and option holders to prevent index errors.
    /// Each option holder gets configured with:
    /// - The option's image sprite
    /// - The option's text
    /// - The corresponding drop handler's option ID
    /// </remarks>
    public static void SetOptionsData(List<OptionGeneric> imageOptions, List<OptionContainer> OptionHolders, List<DropHandler> dropHandlers)
    {
        if (imageOptions != null && OptionHolders != null)
        {
            int count = Mathf.Min(imageOptions.Count, OptionHolders.Count);
            for (int i = 0; i < count; i++)
            {
                QuestionUIHelper.SetImage(OptionHolders[i].image, imageOptions[i].sprite);
                QuestionUIHelper.SetText(OptionHolders[i].optionText, imageOptions[i].text);
                dropHandlers[i].OptionID = imageOptions[i].optionID;
            }
        }
    }

    /// <summary>
    /// Sets the option ID for a specific drop handler component.
    /// Used for identifying valid drag-drop matches.
    /// </summary>
    /// <param name="dropHandler">The drop handler component to modify.</param>
    /// <param name="id">The option ID to assign.</param>
    public static void SetDraggableID(DropHandler dropHandler, string id)
    {
        dropHandler.OptionID = id;
    }
    
    /// <summary>
    /// Sets the sprite of a UI Image component if both the component and sprite exist.
    /// </summary>
    /// <param name="uiImageComponent">The Image component to modify.</param>
    /// <param name="imageSprite">The sprite to display.</param>
    public static void SetImage(Image uiImageComponent, Sprite imageSprite)
    {
        if (uiImageComponent != null && imageSprite != null)
        {
            uiImageComponent.sprite = imageSprite;
        }
    }

    /// <summary>
    /// Sets the audio clip for an AudioSource component if both exist.
    /// Does not automatically play the audio.
    /// </summary>
    /// <param name="audioSource">The AudioSource component to modify.</param>
    /// <param name="audioClip">The audio clip to assign.</param>
    public static void SetAudio(AudioSource audioSource, AudioClip audioClip)
    {
        if (audioSource != null && audioClip != null)
        {
            audioSource.clip = audioClip;
            //audioSource.Play();
        }
    }

    /// <summary>
    /// Resets an Image component's sprite to null if the component exists.
    /// Useful for clearing images between questions or during resets.
    /// </summary>
    /// <param name="uiImageComponent">The Image component to reset.</param>
    public static void ResetImage(Image uiImageComponent)
    {
        if (uiImageComponent != null)
        {
            uiImageComponent.sprite = null;
        }
    }

    /// <summary>
    /// Stops any playing audio and clears the audio clip from an AudioSource component.
    /// Ensures complete audio reset between questions or during cleanup.
    /// </summary>
    /// <param name="audioSource">The AudioSource component to reset.</param>
    public static void ResetAudio(AudioSource audioSource)
    {
        if (audioSource != null)
        {
            audioSource.Stop();
            audioSource.clip = null;
        }
    }

    /// <summary>
    /// Clears the text of a UI Text component if it exists.
    /// Sets the text to an empty string.
    /// </summary>
    /// <param name="uiTextComponent">The Text component to reset.</param>
    public static void ResetText(Text uiTextComponent)
    {
        if (uiTextComponent != null)
        {
            uiTextComponent.text = string.Empty;
        }
    }
}