```markdown
# Unity Options Panel Package

This Unity package provides a customizable options panel that allows players to manage audio settings and navigate between scenes in your game. It is designed to be modular and easy to integrate into any Unity project.

## Features

- **Audio Controls**
  - Toggle music and sound effects on or off.
  - Visual feedback through icons that update based on the current state.

- **Navigation Buttons**
  - Pre-configured buttons for navigating between scenes (e.g., Home, Settings, Credits).
  - Easily extendable to include additional navigation buttons.

- **Dynamic Level Display**
  - Display the current level using a `TMP_Text` component.

## How to Use

### 1. Import the Package
- Add the scripts and related assets to your Unity project.
- Ensure you have the following dependencies:
  - [TextMeshPro](https://docs.unity3d.com/Packages/com.unity.textmeshpro@latest)
  - A scene management system for loading scenes.

### 2. Setting Up the Options UI
1. **Create a UI Canvas**:
   - Add a Canvas to your scene.
   - Create buttons for music toggle, sound toggle, and navigation.

2. **Assign the `SettingsPanelUI` Script**:
   - Attach the `SettingsPanelUI` script to an empty GameObject in the scene.

3. **Configure the Inspector Fields**:
   - Drag and drop UI elements (buttons and icons) into the appropriate fields in the `SettingsPanelUI` component.
   - Assign scene names for navigation buttons.

4. **Implement the Audio Controller**:
   - Create a class that implements the `ISettingsButPanelAudioController` interface.
   - Set `AudioManager.Instance` to reference your implementation.

### 3. Adding Audio Icons
- Use the `AudioToggleUIState` to manage enabled/disabled icons for music and sound buttons:
  - Assign `enabledSprite` and `disabledSprite` for visual feedback.

### 4. Navigation Configuration
- Define scene names for `homeSceneName`, `settingsSceneName`, and `creditsSceneName` in the Inspector.

## Code Overview

### `SettingsPanelUI` Script
Handles:
- Audio toggle button logic and UI updates.
- Navigation button setup and scene loading.
- Level display text updates.

### `ISettingsButPanelAudioController` Interface
Defines:
- Properties and methods for controlling music and sound.
- Events to notify listeners about state changes.

### Example of `ISettingsButPanelAudioController` Implementation
```csharp
public class AudioManager : MonoBehaviour, ISettingsButPanelAudioController
{
    public static AudioManager Instance;

    public bool IsMusicEnabled { get; private set; }
    public bool IsSoundEnabled { get; private set; }

    public event System.Action<bool> OnMusicStateChanged;
    public event System.Action<bool> OnSoundStateChanged;

    private void Awake()
    {
        Instance = this;
    }

    public void ToggleMusic()
    {
        IsMusicEnabled = !IsMusicEnabled;
        OnMusicStateChanged?.Invoke(IsMusicEnabled);
    }

    public void ToggleSound()
    {
        IsSoundEnabled = !IsSoundEnabled;
        OnSoundStateChanged?.Invoke(IsSoundEnabled);
    }
}
```

## Example Scene Structure
- A `MainMenu` scene for the home screen.
- A `Settings` scene for additional options.
- A `Credits` scene to display acknowledgments.

## License
This package is licensed under the MIT License. Feel free to use and modify it for your projects.

## Contributions
Contributions, suggestions, and improvements are welcome! Feel free to open a pull request or issue.

## Contact
For support or inquiries, please contact [Your Contact Information].

```
