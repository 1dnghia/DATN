using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadingManager : MonoBehaviour
{
    public static LoadingManager Instance;
    
    [Header("Loading UI Components")]
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private Slider loadingBar;
    [SerializeField] private TextMeshProUGUI loadingText;
    [SerializeField] private TextMeshProUGUI progressText;
    
    [Header("Loading Animation")]
    [SerializeField] private float dotAnimationSpeed = 0.5f;
    
    private Coroutine loadingAnimationCoroutine;
    private Canvas loadingCanvas;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // Find and setup loading canvas
            SetupLoadingCanvas();
            
            // Ensure loading panel starts hidden
            HideLoading();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void SetupLoadingCanvas()
    {
        // Find the loading canvas (should be a child or sibling of loading panel)
        if (loadingPanel != null)
        {
            loadingCanvas = loadingPanel.GetComponentInParent<Canvas>();
            if (loadingCanvas != null)
            {
                Debug.Log($"Loading Canvas found: {loadingCanvas.gameObject.name}");
                
                // Ensure high sort order to appear above all other UI
                loadingCanvas.sortingOrder = 9999;
                Debug.Log($"Loading Canvas sort order set to: {loadingCanvas.sortingOrder}");
                
                // No need for DontDestroyOnLoad since PersistentScene will always be loaded
                Debug.Log("Loading Canvas will persist with PersistentScene");
            }
            else
            {
                Debug.LogWarning("Loading Canvas not found! Make sure LoadingPanel is child of a Canvas.");
            }
        }
    }

    
    public void ShowLoading()
    {
        if (loadingPanel != null)
        {
            loadingPanel.SetActive(true);
            Debug.Log("Loading UI shown");
        }
        
        // Reset loading bar
        if (loadingBar != null)
        {
            loadingBar.value = 0f;
        }
        
        // Start loading text animation
        if (loadingAnimationCoroutine != null)
        {
            StopCoroutine(loadingAnimationCoroutine);
        }
        loadingAnimationCoroutine = StartCoroutine(AnimateLoadingText());
        
        // Update progress text
        UpdateProgressText(0f);
    }
    
    public void HideLoading()
    {
        if (loadingPanel != null)
        {
            loadingPanel.SetActive(false);
            Debug.Log("Loading UI hidden");
        }
        
        // Stop loading text animation
        if (loadingAnimationCoroutine != null)
        {
            StopCoroutine(loadingAnimationCoroutine);
            loadingAnimationCoroutine = null;
        }
    }
    
    public void UpdateLoadingProgress(float progress)
    {
        // Update loading bar
        if (loadingBar != null)
        {
            loadingBar.value = progress;
        }
        
        // Update progress text
        UpdateProgressText(progress);
    }
    
    private void UpdateProgressText(float progress)
    {
        if (progressText != null)
        {
            int percentage = Mathf.RoundToInt(progress * 100f);
            progressText.text = $"{percentage}%";
        }
    }
    
    private IEnumerator AnimateLoadingText()
    {
        string baseText = "Loading";
        int dotCount = 0;
        
        while (true)
        {
            if (loadingText != null)
            {
                string dots = new string('.', dotCount);
                loadingText.text = baseText + dots;
            }
            
            dotCount = (dotCount + 1) % 4; // Cycle through 0, 1, 2, 3 dots
            yield return new WaitForSeconds(dotAnimationSpeed);
        }
    }
    
    // Utility methods
    public bool IsLoadingVisible()
    {
        return loadingPanel != null && loadingPanel.activeSelf;
    }
    
    public void ForceHideLoading()
    {
        // Force hide loading UI (for emergency cases)
        HideLoading();
        Debug.Log("Loading UI force hidden");
    }
    
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public void DebugLoadingStatus()
    {
        Debug.Log("=== Loading UI Status ===");
        Debug.Log($"Loading Panel Active: {(loadingPanel != null ? loadingPanel.activeSelf.ToString() : "NULL")}");
        Debug.Log($"Loading Canvas: {(loadingCanvas != null ? loadingCanvas.name : "NULL")}");
        Debug.Log($"Canvas Sort Order: {(loadingCanvas != null ? loadingCanvas.sortingOrder.ToString() : "NULL")}");
        Debug.Log($"Animation Running: {(loadingAnimationCoroutine != null)}");
        Debug.Log("========================");
    }
}