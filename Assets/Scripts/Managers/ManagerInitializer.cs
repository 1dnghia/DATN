using UnityEngine;

public class ManagerInitializer : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private SceneGameManager sceneGameManager;
    [SerializeField] private SceneManager sceneManager;
    [SerializeField] private LoadingManager loadingManager;
    [SerializeField] private AudioListenerManager audioListenerManager;
    
    private void Start()
    {
        
    }
    
    
}