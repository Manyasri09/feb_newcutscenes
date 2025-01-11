using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayButtonScreen : MonoBehaviour
{
    [SerializeField] private GameObject coinAmount; // Reference to the GameObject displaying coin amount
    [SerializeField] private Button playButton; // Reference to the Play Button

    /// <summary>
    /// Displays the coin amount and activates the play button.
    /// </summary>
    public void DisplayPlayButton()
    {
        // Check if both references are valid
        if (playButton != null && coinAmount != null)
        {
            // Activate the coin amount display
            coinAmount.SetActive(true); 
            // Activate the play button
            playButton.gameObject.SetActive(true); 
        }
    }
}
