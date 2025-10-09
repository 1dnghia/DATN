using UnityEngine;

public class AudioListenerManager : MonoBehaviour
{
    private void Awake()
    {
        // Ensure only one Audio Listener exists in the scene
        EnsureSingleAudioListener();
    }
    
    private void EnsureSingleAudioListener()
    {
        AudioListener[] listeners = FindObjectsByType<AudioListener>(FindObjectsSortMode.None);
        
        if (listeners.Length > 1)
        {
            Debug.LogWarning($"Found {listeners.Length} Audio Listeners. Disabling extras...");
            
            // Keep the first one, disable the rest
            for (int i = 1; i < listeners.Length; i++)
            {
                listeners[i].enabled = false;
                Debug.Log($"Disabled Audio Listener on: {listeners[i].gameObject.name}");
            }
            
            Debug.Log($"Audio Listener kept on: {listeners[0].gameObject.name}");
        }
        else if (listeners.Length == 0)
        {
            Debug.LogError("No Audio Listener found! Audio won't work properly.");
        }
        else
        {
            Debug.Log($"Single Audio Listener found on: {listeners[0].gameObject.name} - OK");
        }
    }
    
    // Method to manually check and fix Audio Listeners
    [ContextMenu("Fix Audio Listeners")]
    public void FixAudioListeners()
    {
        EnsureSingleAudioListener();
    }
}