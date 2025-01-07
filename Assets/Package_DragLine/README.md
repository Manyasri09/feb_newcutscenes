# Dragging Line Unity Package

## Overview
The Dragging Line package enables users to implement an interactive line-drawing mechanic in Unity. This tool is ideal for applications such as connecting nodes in puzzles, circuit simulations, graph pathfinding, or other interactive systems requiring draggable connections between elements.

## Features
- **Interactive Line Drawing:** Enables users to draw lines interactively between nodes using Unity's `LineRenderer`.
- **Node Selection:** Nodes can be assigned unique identifiers and values for pattern validation.
- **Pattern Validation:** Validates the sequence of connected nodes against customizable criteria.
- **Customizable Line Appearance:** Adjust line width, material, and other properties through the Unity Editor.
- **UI Integration:** Displays connected patterns as text output using Unity's UI system.

## Installation
1. Download the package and add it to your Unity project by copying the script files into the desired folder in the `Assets` directory.
2. Ensure your Unity project includes the following components:
   - **EventSystem:** Required for detecting user interactions with nodes.
   - **LineRenderer:** Attach this component to the GameObject using the `DraggingLine` script.
   - **Text Component:** Used to display the output pattern.

## How to Use
### Setup Nodes
1. Use the **NodePrefab** to create a node in your scene.
2. Ensure the text component of the prefab is assigned to the **NodeText** field in the `LineDrawNode` script.

### Setup Dragging Line
1. Use the **LineRenderer** prefab from the `Prefabs` folder to create a LineRenderer GameObject.
2. Configure the following properties in the Inspector:
   - **Line Renderer:** Drag and assign the attached `LineRenderer` component.
   - **Output Text:** Assign a `Text` UI element to display the connected pattern.
   - **Line Material:** Assign a material for rendering the line.
   - **Line Width:** Set the thickness of the line.
   - **Main Camera:** Drag the main camera used for rendering the scene.

### Connecting Nodes
1. During gameplay, click on a node to start drawing a line.
2. Drag to connect the line to other nodes.
3. Release the mouse button to finalize the connection and record the pattern.

### Pattern Validation
- Customize the `ValidatePattern` method in the `DraggingLine` script to implement your desired validation logic. By default, it checks if the connected pattern matches a specific sequence, such as `Please Help Me !`.

## Example Use Case
1. Place four nodes in a grid and assign them the following values:
   - Node 1: "Please"
   - Node 2: "Help"
   - Node 3: "Me"
   - Node 4: "!"
2. During gameplay, connect the nodes in the correct order.
3. The connected pattern will be displayed as `Please Help Me !`, marking the puzzle as solved.

## Dependencies
- Unity 2021.3 or higher (tested on Unity 2021.3.10f1).
- Ensure the **EventSystem** is active in your scene.

## Customization
- **Line Appearance:** Modify the `LineRenderer` component to customize the line's color, material, or width.
- **Validation Logic:** Extend the `ValidatePattern` method to create more complex validation rules.
- **Dynamic Nodes:** Programmatically generate nodes by instantiating GameObjects with the `LineDrawNode` script.

## Troubleshooting
1. **Line Not Drawing:**
   - Ensure a `LineRenderer` component is attached and assigned in the Inspector.
   - Confirm that nodes are tagged with **Node**.
2. **Nodes Not Detected:**
   - Verify that the **EventSystem** is active in the scene.
   - Ensure colliders are correctly configured on nodes for hit detection.
3. **Validation Fails:**
   - Check that node IDs and values are correctly assigned.

## License
This package is open-source and free for personal and commercial use. Attribution is appreciated but not required.
