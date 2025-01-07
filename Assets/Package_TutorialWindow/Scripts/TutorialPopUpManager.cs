using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;

public class TutorialPopUpManager : MonoBehaviour
{
    [Header("UI References")]
    [Header("TMP Text")]
    [SerializeField] private TMP_Text headingTextTMP;      // UI Text for the heading
    [SerializeField] private TMP_Text descriptionTextTMP; // UI Text for the description

    [Header("Normal Text")]
    
    [SerializeField] private Text headingText; // UI Text for the heading
    [SerializeField] private Text descriptionText; // UI Text for the description

    [Space(15)]
    [SerializeField] private RawImage videoDisplay; // UI Raw Image for the video
    
    [SerializeField] private Button closeButton;   // Close button
    [SerializeField] private VideoPlayer videoPlayer; // VideoPlayer component

    [Header("Pop-Up Data")]
    [SerializeField] private TutorialPopUpData tutorialData; // ScriptableObject for pop-up data

    private void Awake()
    {
        closeButton.onClick.AddListener(ClosePopUp);

        if (tutorialData != null)
        {
            InitializePopUp(tutorialData);
        }
    }

    public void InitializePopUp(TutorialPopUpData popUpData)
    {
        // Update UI elements with data from the ScriptableObject
        headingText.text = popUpData.heading;
        descriptionText.text = popUpData.description;

        headingTextTMP.text = popUpData.heading;
        descriptionTextTMP.text = popUpData.description;

        if (popUpData.tutorialVideo != null)
        {
            videoPlayer.clip = popUpData.tutorialVideo;
            videoPlayer.isLooping = true;
            videoPlayer.Play();
        }

        gameObject.SetActive(true);
    }

    private void ClosePopUp()
    {
        // Stop the video and hide the pop-up
        videoPlayer.Stop();
        gameObject.SetActive(false);
    }
}
