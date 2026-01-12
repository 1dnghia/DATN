using UnityEngine;
using UnityEngine.UI;

namespace Vampire
{
    public class DialogBox : MonoBehaviour
    {
        [SerializeField] private DialogBox previousDialog, nextDialog;
        [SerializeField] private Button closeButton;
        [SerializeField] private MainMenu mainMenu;

        private void Awake()
        {
            if (closeButton != null)
            {
                closeButton.onClick.AddListener(Close);
            }
        }

        private void OnDestroy()
        {
            if (closeButton != null)
            {
                closeButton.onClick.RemoveListener(Close);
            }
        }

        public virtual void Open()
        {
            Debug.Log($"DialogBox.Open called on {gameObject.name}");
            gameObject.SetActive(true);
            Debug.Log($"DialogBox.Open finished - Active: {gameObject.activeSelf}");
        }

        public virtual void Close()
        {
            gameObject.SetActive(false);
            
            // If there's a previous dialog, open it instead of main menu
            if (previousDialog != null)
            {
                previousDialog.Open();
            }
            // Otherwise show main menu when closing
            else if (mainMenu != null)
            {
                mainMenu.ShowMainMenu();
            }
        }

        public void Return()
        {
            previousDialog?.Open();
            Close();
        }

        public void Continue()
        {
            nextDialog?.Open();
            Close();
        }
    }
}
