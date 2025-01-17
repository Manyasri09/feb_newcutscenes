using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using VContainer;
using GlobalAudioManagerPackage;

// Serializable class to manage the UI state of audio toggles
// Contains references to the UI elements and sprites needed for audio state visualization
[System.Serializable]
public class AudioToggleUIState
{
    public Image icon;              // Reference to the UI image that shows the audio state
    public Sprite enabledSprite;    // Sprite to display when audio is enabled
    public Sprite disabledSprite;   // Sprite to display when audio is disabled
}

// Main class to handle all UI interactions in the settings panel
public class SettingsPanelUI : MonoBehaviour
{
    // Audio control button references and their corresponding UI states
    [Header("Audio Controls")]
    [SerializeField] private Button musicToggleButton;    // Button to toggle music
    [SerializeField] private Button soundToggleButton;    // Button to toggle sound effects
    [SerializeField] private AudioToggleUIState musicUI;  // UI state for music toggle
    [SerializeField] private AudioToggleUIState soundUI;  // UI state for sound toggle

    // Navigation button references
    [Header("Navigation Buttons")]
    [SerializeField] private Button homeButton;       // Button to return to main menu
    [SerializeField] private Button settingsButton;   // Button to open settings
    [SerializeField] private Button creditsButton;    // Button to view credits

    // Scene names for navigation
    [Header("Scene Names")]
    [SerializeField] private string homeSceneName = "MainMenu";        // Name of the main menu scene
    [SerializeField] private string settingsSceneName = "Settings";    // Name of the settings scene
    [SerializeField] private string creditsSceneName = "Credits";      // Name of the credits scene

    public Button settingsCloseButton;  // Reference to the close button in the settings panel

    public TMP_Text levelText;  // Reference to the TextMeshPro component displaying the level

    private ISettingsButPanelAudioController audioController;  // Reference to the audio controller interface

    // Initialize all UI elements and their listeners
    void Start()
    {
        InitializeAudioControls();
        SetupNavigationButtons();
    }

    // Set up audio controls and their event listeners
    private void InitializeAudioControls()
    {
        audioController = GlobalAudioManager.Instance;  // Assuming AudioManager implements ISettingsButPanelAudioController

        if (audioController != null)
        {
            // Set up music toggle button and its event listener
            if (musicToggleButton != null)
            {
                musicToggleButton.onClick.AddListener(() => {
                    Debug.Log("Music button clicked");
                    audioController.ToggleMusic();
                });
                audioController.OnMusicStateChanged += UpdateMusicUI;
            }

            // Set up sound toggle button and its event listener
            if (soundToggleButton != null)
            {
                soundToggleButton.onClick.AddListener(() => {
                    Debug.Log("Sound button clicked");
                    audioController.ToggleSound();
                });
                audioController.OnSoundStateChanged += UpdateSoundUI;
            }

            UpdateUI();  // Initialize UI states
        }
    }

    // Set up navigation button click listeners
    private void SetupNavigationButtons()
    {
        // Set up home button navigation
        if (homeButton != null)
        {
            homeButton.onClick.AddListener(() => {
                Debug.Log("Home button clicked");
                LoadScene(homeSceneName);
            });
        }

        // Set up settings button navigation
        if (settingsButton != null)
        {
            settingsButton.onClick.AddListener(() => {
                Debug.Log("Settings button clicked");
                LoadScene(settingsSceneName);
            });
        }

        // Set up credits button navigation
        if (creditsButton != null)
        {
            creditsButton.onClick.AddListener(() => {
                Debug.Log("Credits button clicked");
                LoadScene(creditsSceneName);
            });
        }
    }

    // Load a new scene by name
    private void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Update all UI elements to reflect current state
    private void UpdateUI()
    {
        if (audioController != null)
        {
            UpdateMusicUI(audioController.IsMusicEnabled);
            UpdateSoundUI(audioController.IsSoundEnabled);
        }
    }

    // Update music toggle button UI based on state
    private void UpdateMusicUI(bool isEnabled)
    {
        if (musicUI.icon != null)
        {
            musicUI.icon.sprite = isEnabled ? musicUI.enabledSprite : musicUI.disabledSprite;
        }
    }

    // Update sound toggle button UI based on state
    private void UpdateSoundUI(bool isEnabled)
    {
        if (soundUI.icon != null)
        {
            soundUI.icon.sprite = isEnabled ? soundUI.enabledSprite : soundUI.disabledSprite;
        }
    }

    // Update the level display text
    private void SetLevelText(int level)
    {
        levelText.text = "Level " + level.ToString();
    }

    // Clean up event listeners when the object is destroyed
    void OnDestroy()
    {
        if (audioController != null)
        {
            audioController.OnMusicStateChanged -= UpdateMusicUI;
            audioController.OnSoundStateChanged -= UpdateSoundUI;
        }
    }
}