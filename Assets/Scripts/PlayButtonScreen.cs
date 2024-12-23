using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayButtonScreen : MonoBehaviour
{
    [SerializeField] private GameObject coinAmount;
    [SerializeField] private Button playButton;


    public void DisplayPlayButton()
    {
        if (playButton != null && coinAmount != null)
        {
            coinAmount.SetActive(true);
            playButton.gameObject.SetActive(true);
        }
    }

}
