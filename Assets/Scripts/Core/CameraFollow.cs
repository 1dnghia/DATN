using UnityEngine;

/// <summary>
/// Simple Camera Follow for Unity 2D games
/// Player always at screen center
/// </summary>
public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    [Tooltip("Player GameObject để camera follow")]
    public Transform target;
    
    [Header("Follow Settings")]
    [Tooltip("Khoảng cách offset từ player (x, y, z)")]
    public Vector3 offset = new Vector3(0, 0, -10);
    
    [Header("Optional Settings")]
    [Tooltip("Tự động tìm Player nếu target null")]
    public bool autoFindPlayer = true;
    
    private void Start()
    {
        // Auto-find player if not assigned
        if (target == null && autoFindPlayer)
        {
            FindPlayer();
        }
        
        // Set initial position if target found
        if (target != null)
        {
            transform.position = target.position + offset;
        }
    }
    
    private void LateUpdate()
    {
        if (target == null) return;
        
        // Player luôn ở giữa màn hình - instant follow
        // Update camera FIRST to prevent UI jitter
        transform.position = target.position + offset;
    }
    
    private void FindPlayer()
    {
        // Try to find Player GameObject
        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            target = player.transform;
            return;
        }
        
        // Try to find PlayerController component
        PlayerController playerController = FindFirstObjectByType<PlayerController>();
        if (playerController != null)
        {
            target = playerController.transform;
            return;
        }

    }
    
    /// <summary>
    /// Set new target for camera to follow
    /// </summary>
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
    
    /// <summary>
    /// Set camera offset
    /// </summary>
    public void SetOffset(Vector3 newOffset)
    {
        offset = newOffset;
    }
}
