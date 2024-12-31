# Tutorial Pop-Up Package

This Unity package provides a customizable tutorial pop-up system for mobile and desktop games. It includes a pop-up prefab, a `TutorialPopUpData` ScriptableObject for managing data, and a demo scene to help you quickly integrate the system into your project.

## Features

- **Customizable Layout**: Easily modify the heading, video, and description.
- **ScriptableObject Integration**: Use `TutorialPopUpData` to configure your tutorial content.
- **Looping Video Support**: Automatically plays tutorial videos in a loop.
- **Demo Scene**: Example setup to showcase how to use the system.

## Package Contents

- **Prefabs**
  - `TutorialPopUpPrefab`: The UI pop-up prefab with heading, video, description, and close button.
- **ScriptableObjects**
  - `Default_TutorialPopUpData`: Example data for the tutorial pop-up.
- **Scripts**
  - `TutorialPopUpManager`: Main script to control the pop-up.
  - `TutorialPopUpData`: ScriptableObject definition.
- **Demo Scene**
  - A scene that demonstrates how to set up and use the tutorial pop-up.

## Installation

1. Import the package into your Unity project.
2. Locate the `TutorialPopUpPrefab` and `Default_TutorialPopUpData` in the package folder.

## How to Use

### Setting Up the Tutorial Pop-Up

1. **Drag and Drop the Prefab**: Place the `TutorialPopUpPrefab` in your scene.
2. **Attach the Script**: The `TutorialPopUpManager` script is already attached to the prefab.

### Configuring Tutorial Content

1. Create a new `TutorialPopUpData` ScriptableObject:
   - Right-click in the Project window and select `Create > TutorialPopUpData`.
   - Fill in the fields for heading, tutorial video, and description.

2. Assign the ScriptableObject to the `TutorialPopUpManager`:
   - Select the `TutorialPopUpPrefab` in the scene.
   - Drag your custom `TutorialPopUpData` into the `Tutorial Data` field on the `TutorialPopUpManager` component.

### Using the Demo Scene

1. Open the included demo scene to see an example setup.
2. Play the scene to view the tutorial pop-up in action.

### Example Code to Trigger the Pop-Up

To trigger the pop-up programmatically:

```csharp
public class Example : MonoBehaviour
{
    [SerializeField] private TutorialPopUpManager popUpManager;
    [SerializeField] private TutorialPopUpData tutorialData;

    public void ShowTutorial()
    {
        popUpManager.InitializePopUp(tutorialData);
    }
}
```

## Customization

- **UI Elements**: Modify the `TutorialPopUpPrefab` to change the layout or style.
- **Data**: Create multiple `TutorialPopUpData` assets for different tutorials.

## Performance

The system is optimized for mobile and desktop platforms. Video playback uses Unity's `VideoPlayer` component, which is lightweight and efficient for looping videos.

## Notes

- Ensure that your video files are compatible with Unity's `VideoPlayer`.
- You can preload video assets into memory if needed for larger projects.

## Support

For any issues or feature requests, please contact the package maintainer or submit a report via the Unity Asset Store.

## License

This package is distributed under the MIT License. See the LICENSE file for details.
