# Chapter-Day Package for Unity

## Overview
The Chapter-Day Package is a flexible UI system for Unity that implements a chapter-based content navigation system. It follows the Model-View-Controller (MVC) pattern to provide a clean, maintainable architecture for managing chapter and day-based content progression.

## Features
- MVC architecture for clean separation of concerns
- Flexible chapter and day navigation system
- Built-in lock/unlock functionality
- Easy to extend and customize
- Unity-friendly serializable data models
- Interface-based design for easy modification and testing
- Includes mock data provider for immediate testing
- Simple initialization system

## Installation
1. Import the package into your Unity project
2. Add the necessary prefabs to your scene:
   - ChaptersParentPrefab
   - DaysParentPrefab
   - ChapterUI prefab
   - DayUI prefab

## Quick Start
1. Create a new scene or use an existing one
2. Add all required prefabs to your scene
3. Create an empty GameObject and name it "GameInitializer"
4. Attach the GameInitializer script to this GameObject
5. Reference your ChapterDayViewManager in the GameInitializer's inspector
6. Press Play to see the system working with mock data

## Mock Data
The package includes a MockDataProvider that creates:
- Chapter 1 (Unlocked): 4 days (2 unlocked, 2 locked)
- Chapter 2 (Unlocked): 3 days (1 unlocked, 2 locked)
- Chapter 3 (Locked): 3 days (all locked)

## Architecture

### Models
- `ChapterModel`: Represents a chapter containing multiple days
- `DayModel`: Represents a single day of content
- `IChapterDayDataProvider`: Interface for data retrieval implementation

### Views
- `ChapterDayViewManager`: Main view manager implementing IChapterDayView
- `ChapterSelectionUI`: Manages the chapter selection interface
- `DaySelectionUI`: Manages the day selection interface within a chapter
- `ChapterUI`: Individual chapter UI component
- `DayUI`: Individual day UI component

### Controller
- `ChapterDayController`: Orchestrates interactions between views and data

### Initialization
- `GameInitializer`: Handles system setup and component wiring
- `MockDataProvider`: Provides sample data for testing

## Setup Guide

1. **Scene Setup**
   - Create a new scene or use an existing one
   - Add the ChapterDayViewManager to your scene
   - Configure the necessary prefab references in the inspector
   - Add the GameInitializer component to an empty GameObject

2. **Custom Data Provider Implementation**
   ```csharp
   public class YourDataProvider : IChapterDayDataProvider
   {
       public List<ChapterModel> GetChapters()
       {
           // Implement your data retrieval logic here
           return chapters;
       }
   }
   ```

3. **Custom Initialization**
   ```csharp
   // If you need custom initialization:
   var dataProvider = new YourDataProvider();
   var viewManager = FindObjectOfType<ChapterDayViewManager>();
   var controller = new ChapterDayController(dataProvider, viewManager);
   viewManager.Initialize(controller);
   ```

## Usage Example

```csharp
// Creating a basic chapter structure
var chapter = new ChapterModel
{
    chapterName = "Chapter 1",
    isLocked = false,
    days = new List<DayModel>
    {
        new DayModel { dayName = "Day 1", isLocked = false },
        new DayModel { dayName = "Day 2", isLocked = true }
    }
};
```

## Customization

### Custom Day Content
Extend the `DayModel` class to include additional content:

```csharp
public class CustomDayModel : DayModel
{
    public string contentPath;
    public int difficultyLevel;
    // Add your custom properties
}
```

### Custom UI Elements
You can create custom UI prefabs by:
1. Duplicating the existing prefabs
2. Modifying the visual elements
3. Updating the references in ChapterDayViewManager

## Best Practices
- Implement your own `IChapterDayDataProvider` for data persistence
- Keep UI prefabs modular and reusable
- Follow the established MVC pattern when extending functionality
- Use the provided interfaces for custom implementations
- Test with MockDataProvider before implementing custom data provider

## Requirements
- Unity 2020.3 or higher
- TextMeshPro (for UI elements)

## Known Limitations
- Single-level chapter hierarchy
- Basic lock/unlock functionality
- UI prefabs require manual setup

