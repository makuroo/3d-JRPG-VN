using Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Character.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private InputActionAsset _actionAsset;

        private void OnEnable()
        {
            if (GameManager.Instance)
            {
                GameManager.Instance.OnGameStateChange += HandleInput;
            }
        }

        private void OnDisable()
        {
            if (GameManager.Instance)
            {
                GameManager.Instance.OnGameStateChange -= HandleInput;
            }
        }

        private void HandleInput(GameState state)
        {
            if (state != GameState.Exploration)
            {
                _actionAsset.FindActionMap("Player").Disable();
            }
            else
            {
                _actionAsset.FindActionMap("Player").Enable();
            }
        }
    }
}

