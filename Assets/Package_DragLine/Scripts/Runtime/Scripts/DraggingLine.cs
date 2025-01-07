using System;
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
    // [SerializeField] private Button resetButton; // Button to reset the line drawing.


    // Private fields for managing state during interaction.
    private List<GameObject> selectedNodes = new List<GameObject>(); // List of nodes selected during the drag.
    private List<Vector3> linePositions = new List<Vector3>(); // Positions of the line points.
    private bool isDragging = false; // Flag to check if the user is currently dragging.
    private Vector3 currentMousePos; // Current position of the mouse in world space.
    private int outputTextCounter = 0; // Counter to keep track of the output text index.
    [SerializeField] private int requiredNodeCount = 3;

    public bool IsSolved { get; private set; } // Property to indicate if the task is solved.

    HashSet<string> addedTexts = new HashSet<string>(); // Create a HashSet to store the texts that have already been added

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
        // resetButton.onClick.AddListener(ResetLine);
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

    private void HandleMouseDown()
    {
        // Detect if a valid node is clicked and start dragging.
        GameObject node = GetNodeUnderMouse(Input.mousePosition);
        if (node != null && outputTextCounter < requiredNodeCount)
        {
            StartDragging(node);
            LineActions.TriggerLineStarted(); // Trigger event when the line starts.
        }   
        
    }

    private void HandleMouseDrag()
    {
        // Update the line as the user drags the mouse.
        GameObject node = GetNodeUnderMouse(Input.mousePosition);
        if (node != null && !selectedNodes.Contains(node))
        {
            AddNodeToLine(node); // Add a new node to the line if not already selected.
        }
        else
        {
            UpdateMousePosition(Input.mousePosition); // Update the current mouse position.
        }
    }

    private void HandleMouseUp()
    {
        // Finalize the line when the mouse button is released.
        isDragging = false;

        // Check if there is a node below mouse pointer
        GameObject nodeUnderMouse = GetNodeUnderMouse(Input.mousePosition);
        if (nodeUnderMouse == null)
        {
            Debug.Log("There is no node under the mouse. Ignore this line.");
            UpdateLineRenderer();
            return;
        }
        else if (selectedNodes.Contains(nodeUnderMouse))
        {
            UpdateLineRenderer();
        }

        // Check if pattern is complete
        if (outputTextCounter >= requiredNodeCount)
        {
            string result = string.Join(" ", selectedNodes.Select(n => n.GetComponentInChildren<Text>().text));
            Debug.Log($"<color=yellow>Pattern: {result}</color>");
            LineActions.TriggerLineEnded(result);
        }
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
