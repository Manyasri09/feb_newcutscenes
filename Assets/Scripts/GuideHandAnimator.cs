using UnityEngine;

public class GuideHandController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Vector2 offset = new Vector2(10f, 10f); // Offset from cursor position
    [SerializeField] private bool hideSystemCursor = true;
    [SerializeField] private float smoothSpeed = 10f; // Lower = smoother movement
    
    private Camera mainCamera;
    private Vector3 targetPosition;
    
    void Start()
    {
        mainCamera = Camera.main;
        
        if (hideSystemCursor)
        {
            Cursor.visible = false;
        }
        
        // Set initial position to cursor
        UpdateTargetPosition();
        transform.position = GetCursorWorldPosition();
    }
    
    void Update()
    {
        UpdateTargetPosition();
        
        // Smoothly move the hand to the target position
        transform.position = Vector3.Lerp(
            transform.position, 
            targetPosition, 
            smoothSpeed * Time.deltaTime
        );
    }
    
    private void UpdateTargetPosition()
    {
        targetPosition = GetCursorWorldPosition();
    }
    
    private Vector3 GetCursorWorldPosition()
    {
        // Get cursor position in screen space
        Vector3 mousePos = Input.mousePosition;
        
        // Convert to world space
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(new Vector3(
            mousePos.x + offset.x,
            mousePos.y + offset.y,
            -mainCamera.transform.position.z // Place at camera's near clip plane
        ));
        
        // Maintain the sprite's z position if needed
        worldPos.z = transform.position.z;
        
        return worldPos;
    }
    
    // Optional: Add this method if you want to dynamically change the offset
    public void SetOffset(Vector2 newOffset)
    {
        offset = newOffset;
    }
}