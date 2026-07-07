using Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Character.Player
{
   public class PlayerMovement : MonoBehaviour
   {
      [SerializeField] private InputActionAsset _actionAsset;
      [SerializeField] private float _speed;
      [SerializeField] private Animator _animator;

      private Vector2 _inputDirection;
      private Rigidbody2D _rigidbody;
      private int VELOCITY = Animator.StringToHash("velocityMagnitude");
      private void Awake()
      {

         _rigidbody = GetComponent<Rigidbody2D>();
      }

      private void OnEnable()
      {
         _actionAsset.FindAction("Move").performed += Move;
         _actionAsset.FindAction("Move").canceled += Stop;
         GameManager.Instance.OnGameStateChange += ForceStop;
      }

      private void OnDisable()
      {
         _actionAsset.FindAction("Move").performed -= Move;
         _actionAsset.FindAction("Move").canceled -= Stop;
         GameManager.Instance.OnGameStateChange -= ForceStop;
      }

      private void Move(InputAction.CallbackContext context)
      {
         _inputDirection = context.ReadValue<Vector2>();
      
      }

      private void Stop(InputAction.CallbackContext context)
      {
         _inputDirection = Vector2.zero;
      }

      private void Update()
      {
         if(GameManager.Instance.CurrentState == GameState.Cutscene) 
         {
            _inputDirection = Vector2.zero;
            return;
         }
         _animator.SetFloat(VELOCITY, _inputDirection.magnitude);
         Checkflip();
      }

      private void FixedUpdate()
      {
         _rigidbody.linearVelocity = _inputDirection * _speed;
      }

      private void Checkflip()
      {
         if (_inputDirection.x > 0)
         {
            transform.eulerAngles = new Vector3(0, 0, 0);
         }
         else if (_inputDirection.x < 0)
         {
            transform.eulerAngles = new Vector3(0, 180, 0 );
         }
      }

      private void ForceStop(GameState state)
      {
         if (state != GameState.Cutscene) return;
         _inputDirection = Vector2.zero;
         _animator.SetFloat(VELOCITY, 0);
      }
   }
}
