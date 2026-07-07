using System;
using Fungus;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UI.System
{
    public class QuitUI : MonoBehaviour
    {
        [SerializeField] private InputActionAsset _actionAsset;
        [SerializeField] private Transform _quitPanel;
        [SerializeField] private GraphicRaycaster _raycaster;
        private void OnEnable()
        {
            _actionAsset.FindAction("Quit").Enable();
            _actionAsset.FindAction("Quit").performed += TogglePanel;
        }

        private void OnDisable()
        {
            _actionAsset.FindAction("Quit").performed -= TogglePanel;
        }

        private void TogglePanel(InputAction.CallbackContext context)
        {
            _quitPanel.gameObject.SetActive(!_quitPanel.gameObject.activeSelf);
            _raycaster.enabled = _quitPanel.gameObject.activeSelf;
        }

        public void Quit()
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
            Application.Quit();
        }

        public void Cancel()
        {
            _quitPanel.gameObject.SetActive(false);
            _raycaster.enabled = false;
        }
    }
}
