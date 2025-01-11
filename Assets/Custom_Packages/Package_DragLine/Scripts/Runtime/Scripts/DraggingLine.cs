using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(LineRenderer))]
public class DraggingLine : MonoBehaviour
{
    // Serialized fields to configure the script from the Unity Inspector.
    [SerializeField] private LineRenderer lineRenderer; // The LineRenderer component used to draw the line.
    [SerializeField] private List<Text> outputText; // List of Text components for displaying node data.
    [SerializeField] private Material lineMaterial; // Material used for the line.
    [SerializeField] private float lineWidth = 0.1f; // Width of the line.
    [SerializeField] private Camera mainCamera; // Reference to the main camera.
    [SerializeField] private string requiredTag = "Node"; // Tag used to identify valid nodes.
    [SerializeField] private Button resetButton; // Button to reset the line drawing.
    [SerializeField] private int requiredNodeCount = 3;


    // Private fields for managing state during interaction.
    private List<GameObject> selectedNodes = new List<GameObject>(); // List of nodes selected during the drag.
    private List<Vector3> linePositions = new List<Vector3>(); // Positions of the line points.
    private bool isDragging = false; // Flag to check if the user is currently dragging.
    private Vector3 currentMousePos; // Current position of the mouse in world space.
    private int outputTextCounter = 0; // Counter to keep track of the output text index.
    private Vector3 lastMousePosition; // Stores the mouse position from the previous frame to calculate the drag path
    private bool triggerCalled;

   

    private void Start()
    {
        // Ensure the LineRenderer component is assigned and configured.
        if (lineRenderer == null)
        {
            lineRenderer = GetComponent<LineRenderer>();
            if (lineRenderer == null)
            {
                Debug.LogError("LineRenderer is missing. Please attach one to the GameObject.");
                enabled = false;
                return;
            }
        }
        ConfigureLineRenderer();
        selectedNodes.Clear();
        linePositions.Clear();
        triggerCalled = false;
    }

    private void Update()
    {
        // Handle mouse input to start, update, and end the line drawing.
        if (Input.GetMouseButtonDown(0))
        {
            HandleMouseDown();
        }

        if (Input.GetMouseButton(0) && isDragging)
        {
            HandleMouseDrag();
        }

        if (Input.GetMouseButtonUp(0))
        {
            HandleMouseUp();
        }
    }


    /// <summary>
    /// Handles the continuous mouse drag operation, checking for nodes along the drag path
    /// and updating the line renderer accordingly.
    /// </summary>
    private void HandleMouseDrag()
    {
        Vector3 currentMousePos = Input.mousePosition;
        
        // Only check for nodes in the path if we have a valid last position
        // This prevents checking on the first frame of dragging
        if (lastMousePosition != Vector3.zero)
        {
            CheckNodesInPath(lastMousePosition, currentMousePos);
        }

        // Check for a node directly under the current mouse position
        GameObject node = GetNodeUnderMouse(currentMousePos);
        if (node != null && !selectedNodes.Contains(node))
        {
            AddNodeToLine(node);
        }
        else
        {
            // If no new node is found, just update the visual line to follow the mouse
            UpdateMousePosition(currentMousePos);
        }

        // Store current position for next frame's path checking
        lastMousePosition = currentMousePos;
    }

    /// <summary>
    /// Checks for nodes along the path between two points to catch nodes during fast dragging.
    /// This prevents missing nodes when the mouse moves quickly between frames.
    /// </summary>
    /// <param name="startPos">Starting position of the path segment</param>
    /// <param name="endPos">Ending position of the path segment</param>
    private void CheckNodesInPath(Vector3 startPos, Vector3 endPos)
    {
        // Calculate the direction and total distance of this path segment
        Vector3 direction = endPos - startPos;
        float distance = direction.magnitude;
        direction.Normalize();

        // Calculate how many points to check along the path
        // Smaller divisor = more precise but more performance intensive
        // Larger divisor = less precise but better performance
        int steps = Mathf.CeilToInt(distance / 10f); // Adjust this value based on your needs
        
        // Check multiple points along the path for nodes
        for (int i = 1; i < steps; i++)
        {
            // Calculate position to check, interpolating between start and end positions
            Vector3 pointToCheck = startPos + (direction * (distance * i / steps));
            GameObject node = GetNodeUnderMouse(pointToCheck);
            
            // If we found a new valid node, add it to our line
            if (node != null && !selectedNodes.Contains(node))
            {
                AddNodeToLine(node);
            }
        }
    }

    /// <summary>
    /// Handles the initial mouse down event, starting the drag operation if a valid node is clicked.
    /// </summary>
    private void HandleMouseDown()
    {
        // Initialize the last mouse position for path checking
        lastMousePosition = Input.mousePosition;
        
        GameObject node = GetNodeUnderMouse(Input.mousePosition);
        if (node != null && outputTextCounter < requiredNodeCount)
        {
            StartDragging(node);
            LineActions.TriggerLineStarted();
        }
    }

    /// <summary>
    /// Handles the mouse up event, finalizing the pattern if enough nodes were collected
    /// regardless of whether the mouse is over a node.
    /// </summary>
    private void HandleMouseUp()
    {
        isDragging = false;
        // Reset the last mouse position since we're no longer dragging
        lastMousePosition = Vector3.zero;

        // Check if we've collected enough nodes for a complete pattern
        // This happens regardless of where the mouse is released
        if (outputTextCounter >= requiredNodeCount && !triggerCalled)
        {
            string result = string.Join(" ", selectedNodes.Select(n => n.GetComponentInChildren<Text>().text));
            Debug.Log($"<color=yellow>Pattern: {result}</color>");
            LineActions.TriggerLineEnded(result);
            triggerCalled = true;
        }
        
        UpdateLineRenderer();
    }

    private void StartDragging(GameObject node)
    {
        // Start the drag process and add the initial node.
        isDragging = true;
        AddNodeToLine(node);
    }

    private void AddNodeToLine(GameObject node)
    {
        if (!selectedNodes.Contains(node))
        {
            LineActions.TriggerAnimation(node.GetComponent<LineDrawNode>().nodeID);
            // Add a node to the selected list and update the line.
            selectedNodes.Add(node);
            SetOutputText(node); // Set the output text for the node.
            Vector3 nodePosition = node.transform.position;
            nodePosition.z = 0; // Ensure the position is in the same plane.
    
            // Add the node position multiple times to smooth the line.
            linePositions.Add(nodePosition);
            linePositions.Add(nodePosition);
            linePositions.Add(nodePosition);
            linePositions.Add(nodePosition);
            UpdateLineRenderer();
        }
    }

    private void UpdateMousePosition(Vector3 mousePosition)
    {
        // Update the current mouse position in world space.
        currentMousePos = Camera.main.ScreenToWorldPoint(mousePosition);
        currentMousePos.z = 0;
        UpdateLineRenderer();
    }

    public void ResetLine()
    {
        // Clear the line and reset state for a new interaction.
        lineRenderer.positionCount = 0;
        selectedNodes.Clear();
        linePositions.Clear();
        outputTextCounter = 0;
        foreach (var item in outputText)
        {
            item.text = "";
        }
    }

    private void UpdateLineRenderer()
    {
        // Update the LineRenderer to match the current line positions.
        if (!lineRenderer) return;

        // Calculate the number of points in the line renderer. If the user is dragging, add an extra point at the mouse position.
        int pointCount = linePositions.Count + (isDragging ? 1 : 0);
        lineRenderer.positionCount = pointCount;

        // Set the positions of the line renderer to match the current line positions.
        for (int i = 0; i < linePositions.Count; i++)
        {
            lineRenderer.SetPosition(i, linePositions[i]);
        }

        // If the user is dragging, extend the line to the mouse position.
        if (isDragging)
        {
            lineRenderer.SetPosition(pointCount - 1, currentMousePos); // Extend the line to the mouse position.
        }
    }

    private GameObject GetNodeUnderMouse(Vector2 mousePosition)
    {
        // Perform a raycast to find a valid node under the mouse pointer.
        PointerEventData eventData = new PointerEventData(EventSystem.current) { position = mousePosition };
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (RaycastResult result in results)
        {
            // Check if the game object has the required tag.
            if (result.gameObject.CompareTag(requiredTag))
            {
                // Return the game object if it has the required tag.
                return result.gameObject;
            }
        }

        // Return null if no game object with the required tag is found.
        return null;
    }
    private void SetOutputText(GameObject node)
    {
        // Log the text of the node
        Debug.Log($"<color=yellow>nodeText: {node.GetComponentInChildren<Text>().text}</color>");

        // Check if the outputTextCounter is 0
        if (outputTextCounter == 0)
        {
            // Set the text of the current outputText to the text of the current node
            outputText[outputTextCounter].text = node.GetComponentInChildren<Text>().text;

            // Increment the outputTextCounter
            outputTextCounter++;
        }

        // Check if the text of the previous node is not equal to the text of the current node
        if (!outputText[outputTextCounter - 1].text.Equals( node.GetComponentInChildren<Text>().text))
        {
            // Log the text of the current node
            Debug.Log($"<color=red>nodeText: {node.GetComponentInChildren<Text>().text}</color>");

            // Set the text of the current outputText to the text of the current node
            outputText[outputTextCounter].text = node.GetComponentInChildren<Text>().text;

            // Increment the outputTextCounter
            outputTextCounter++;
        }
    }

 
    private void ConfigureLineRenderer()
    {
        // Set up the LineRenderer with the specified material and width.
        if (lineMaterial != null)
        {
            lineRenderer.material = lineMaterial;
        }
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.useWorldSpace = true;
    }
}
