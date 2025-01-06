using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;

public class TutorialPopUpManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text headingText;      // UI Text for the heading
    [SerializeField] private RawImage videoDisplay; // UI Raw Image for the video
    [SerializeField] private TMP_Text descriptionText; // UI Text for the description
    [SerializeField] private Button closeButton;   // Close button
    [SerializeField] private VideoPlayer videoPlayer; // VideoPlayer component
    [SerializeField] private GameObject TutorialPanel; // Tutorial Panel

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
        TutorialPanel.SetActive(false);
    }
}
