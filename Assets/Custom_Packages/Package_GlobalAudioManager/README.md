# Global Audio Manager Package

The Global Audio Manager Package is a Unity-based tool designed to simplify audio management for games. 
It provides functionalities to manage background music, sound effects (SFX), and audio configurations 
globally across your project. This package supports volume controls, audio fading, and more, 
all in an easy-to-use Singleton architecture.

---

## Features

- **Global Audio Management**: Centralized handling of background music and SFX.
- **Audio Fading**: Smooth fade-in and fade-out effects for music transitions.
- **Custom Audio Configurations**: Define and use audio configurations via ScriptableObjects.
- **Singleton Pattern**: Ensures a single instance of the audio manager.
- **Volume Controls**: Adjustable music and SFX volumes.
- **Scalability**: Easily extendable interface for custom audio behavior.

---

## Installation

1. Clone or download the Global Audio Manager Package.
2. Place the package files in your Unity project's `Assets` folder.
3. Create an `AudioConfig` ScriptableObject via `Assets > Create > MyAudioPackage > AudioConfig`.
4. Assign audio clips in the `AudioConfig` asset.

---

## Usage

### 1. Setting Up the Audio Manager

1. Create an empty GameObject in your Unity scene and attach the `GlobalAudioManager` script or use GlobalAudioManager prefab from the package
2. Assign the required `AudioSource` components to the **Music Source** and **SFX Source** fields in the Inspector.
3. Link your `AudioConfig` asset to the **Audio Config** field.

### 2. Playing Background Music

```csharp
GlobalAudioManager.Instance.PlayMusic(GlobalAudioManager.Instance.AudioConfig.mainMenuMusic);
```

### 3. Playing Sound Effects

```csharp
GlobalAudioManager.Instance.PlaySFX(GlobalAudioManager.Instance.AudioConfig.buttonClickSFX);
```

### 4. Adjusting Volume

```csharp
GlobalAudioManager.Instance.SetMusicVolume(0.5f); // Set music volume to 50%
GlobalAudioManager.Instance.SetSFXVolume(0.8f);  // Set SFX volume to 80%
```

### 5. Fading Music

- **Fade In**
  ```csharp
  GlobalAudioManager.Instance.FadeInMusic(GlobalAudioManager.Instance.AudioConfig.levelBackgroundMusic, duration: 3f);
  ```

- **Fade Out**
  ```csharp
  GlobalAudioManager.Instance.FadeOutMusic(duration: 2f);
  ```

---

## Components

### Scripts

1. **GlobalAudioConfig**
   - A `ScriptableObject` for storing and organizing audio clips.
   - Accessible via the `AudioConfig` property in the `GlobalAudioManager`.

2. **GlobalAudioManager**
   - Core class for managing audio playback and settings.
   - Implements the `IGlobalAudioManager` interface.

3. **IGlobalAudioManager**
   - Interface defining the essential audio management methods.
   - Allows for scalable and interchangeable implementations.

---

## Example Audio Configuration

You can create an `AudioConfig` ScriptableObject and assign your audio clips for various categories such as:

- **Menu Music**: Main menu, settings menu.
- **Gameplay Music**: Level background, boss battle.
- **Common SFX**: Button clicks, coin pickups, victory/defeat sounds.
- **Voice Overs**: Cutscenes or narration.

---

## Notes

- Ensure that all `AudioSource` components are properly configured in the Unity Inspector.
- Use the `Default Execution Order` attribute to initialize the audio manager early in the game lifecycle.
- Consider extending the package with crossfade functionality for smoother music transitions.

---

## Contributing

Contributions are welcome! Feel free to open issues or submit pull requests for improvements and bug fixes.

---

## License

This project is licensed under the MIT License. See the `LICENSE` file for details.

---

## Contact

For questions or suggestions, feel free to reach out to [Your Email or GitHub Profile].
