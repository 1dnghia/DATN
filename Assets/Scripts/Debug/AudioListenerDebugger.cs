using UnityEngine;

public class AudioListenerDebugger : MonoBehaviour
{
    [Header("Debug Audio Listeners")]
    public bool autoCheck = true;
    public float checkInterval = 1f;
    
    private void Start()
    {
        if (autoCheck)
        {
            InvokeRepeating(nameof(CheckAudioListeners), 0f, checkInterval);
        }
    }
    
    [ContextMenu("Check Audio Listeners Now")]
    public void CheckAudioListeners()
    {
        AudioListener[] listeners = FindObjectsByType<AudioListener>(FindObjectsSortMode.None);
        
        Debug.Log($"=== AUDIO LISTENER CHECK ===");
        Debug.Log($"Total Audio Listeners found: {listeners.Length}");
        
        for (int i = 0; i < listeners.Length; i++)
        {
            AudioListener listener = listeners[i];
            string status = listener.enabled ? "ENABLED" : "DISABLED";
            string sceneName = listener.gameObject.scene.name;
            
            Debug.Log($"[{i}] {listener.gameObject.name} in scene '{sceneName}' - {status}");
        }
        
        if (listeners.Length > 1)
        {
            Debug.LogWarning($"⚠️ MULTIPLE AUDIO LISTENERS DETECTED! Only 1 should be enabled.");
        }
        else if (listeners.Length == 1)
        {
            Debug.Log($"✅ Perfect! Only 1 Audio Listener found.");
        }
        else
        {
            Debug.LogError($"❌ No Audio Listener found! Audio won't work.");
        }
        
        Debug.Log($"========================");
    }
    
    private void OnDestroy()
    {
        CancelInvoke();
    }
}