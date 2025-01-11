# Word Animation Unity Package

## Overview
The **Word Animation Unity Package** provides a flexible and reusable system for animating words in Unity. This package is perfect for projects that involve UI-based word animations, such as educational games or interactive text-based applications.

The package allows you to:
- Animate words by moving them to target positions with scaling effects.
- Customize animation settings using ScriptableObjects.
- Trigger animations programmatically or via UI button clicks.

---

## Features

- **Word Movement Animation**: Words move to specified targets with customizable duration and delays.
- **Scaling Effects**: Words scale down and up during animations to create a "pop" effect.
- **ScriptableObject Settings**: Centralized animation configuration using the `WordAnimationSettings` ScriptableObject.
- **Event Handling**: Trigger animations programmatically using the `IWordAnimationEvents` interface.
- **UI Integration**: Words can be made clickable to trigger animations via button events.

---

## Installation

1. Clone or download the repository.
2. Import the package into your Unity project.
3. Ensure you have the [LeanTween library](https://assetstore.unity.com/packages/tools/animation/leantween-3595) installed, as it is used for animations.

---

## Usage

### 1. Setup

1. Add the `WordAnimationManager` script to a GameObject in your scene.
2. Assign the `words` and `targets` GameObject lists in the Inspector.
3. Create a `WordAnimationSettings` ScriptableObject:
   - Right-click in the Project window and select `Create > WordAnimation > Settings`.
   - Customize the animation settings in the ScriptableObject.
4. Assign the `WordAnimationSettings` ScriptableObject to the `WordAnimationManager`.

### 2. Programmatic Trigger

Implement the `IWordAnimationEvents` interface to trigger animations programmatically:
```csharp
public class Example : MonoBehaviour, IWordAnimationEvents
{
    [SerializeField] private WordAnimationManager wordAnimationManager;

    public void OnWordAnimationTriggered(int wordIndex)
    {
        wordAnimationManager.AnimateWord(wordIndex);
    }
}
```

### 3. Make Words Clickable

Use the `MakeWordsClickable` method to enable word animations via UI button clicks:
```csharp
wordAnimationManager.MakeWordsClickable();
```

---

## Script Breakdown

### WordAnimationManager.cs

- **Responsibilities**:
  - Store and reset initial positions of words.
  - Animate words with movement and scaling effects.
  - Handle staggered animations.
  - Integrate with UI buttons for interaction.

- **Key Methods**:
  - `AnimateWords()`: Animates all words to their target positions.
  - `AnimateWord(int index)`: Animates a specific word.
  - `ResetWords()`: Resets words to their initial positions.

### WordAnimationSettings.cs

- **Responsibilities**:
  - Store animation configuration values (durations, delays, scaling factors).
  - Simplify customization through ScriptableObject architecture.

### IWordAnimationEvents.cs

- **Purpose**: Provides an interface for triggering word animations programmatically.

---

## Dependencies

- **LeanTween**: A lightweight and efficient animation library for Unity. Download it from the Unity Asset Store: [LeanTween](https://assetstore.unity.com/packages/tools/animation/leantween-3595).

---

## Example

1. Create a UI with words as `GameObjects`.
2. Set up targets where the words should animate to.
3. Customize the animation settings in the ScriptableObject.
4. Use the `AnimateWords` method to animate all words:
```csharp
wordAnimationManager.AnimateWords();
```

---

## License
This package is licensed under the MIT License. Feel free to use and modify it as needed.

---

## Support
For questions or issues, please create a GitHub issue or contact the developer.


