using Interface;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Character.Player
{
   public class PlayerInteraction : MonoBehaviour
   {
      [SerializeField] private InputActionAsset _actionAsset;
      [ShowInInspector, ReadOnly]
      private IInteractable _interactable;

      private void OnEnable()
      {
         _actionAsset.FindAction("Interact").Enable();
         _actionAsset.FindAction("Interact").performed += ProcessInteraction;
      }

      private void OnDisable()
      {
         _actionAsset.FindAction("Interact").performed-= ProcessInteraction;
      }

      private void OnTriggerEnter2D(Collider2D other)
      {
         if (other.TryGetComponent(out IInteractable interactable))
         {
            _interactable = interactable;
         }
      }

      private void OnTriggerExit2D(Collider2D other)
      {
         if (other.TryGetComponent(out IInteractable interactable) && _interactable == interactable)
         {
            _interactable = null;
         }
      }

      private void ProcessInteraction(InputAction.CallbackContext context)
      {
         if (_interactable == null)
         {
            Debug.Log("No IInteractable");
            return;
         }
         _interactable.Interact();
      }
   }
}
