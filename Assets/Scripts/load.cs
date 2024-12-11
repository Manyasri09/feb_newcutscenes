using UnityEngine;
using UnityEngine.SceneManagement; // To load scenes
using UnityEngine.UI; // For Button reference

public class SceneLoader : MonoBehaviour
{
    public Button loadButton; // Reference to the UI button
    public string[] sceneNames; // Array of scene names for each scene

    private int currentSceneIndex = 0; // Track the current scene index

    void Start()
    {
        // Ensure the button triggers the LoadNextScene method
        loadButton.onClick.AddListener(LoadNextScene);
    }

    void LoadNextScene()
    {
        // Check if there are more scenes to load
        if (currentSceneIndex < sceneNames.Length)
        {
            // Load the next scene by name
            SceneManager.LoadScene(sceneNames[currentSceneIndex]);

            // Increment the scene index to move to the next scene
            currentSceneIndex++;
        }
        else
        {
            // If no more scenes, you can either loop or disable the button
            Debug.Log("No more scenes to load.");
            loadButton.interactable = false; // Disable button after last scene
        }
    }
}
