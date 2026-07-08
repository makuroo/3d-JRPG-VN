using Core;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace Character.Enemy
{
    public enum MoveState
    {
        Idle,
        Move
    }

    public class EnemyWander : MonoBehaviour
    {
        private static readonly int IsMoving = Animator.StringToHash("IsMoving");
        private Vector2 _targetPos;
        [FormerlySerializedAs("_speed")] [SerializeField] private float _moveSpeed;
        [SerializeField] private float _maxIdleTime = 5f;
        [SerializeField] private float _wanderRadius = 2f;
        private MoveState _currentState = MoveState.Idle;
        private Vector2 _initialPos;
    
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _visualTransform;

        private bool _isPaused;
        
        private Tween _idleTween;

        private void Start()
        {
            _initialPos = transform.position;
            _idleTween = DOVirtual.DelayedCall(Random.Range(1, _maxIdleTime), GetTargetPos);
        }

        private void Update()
        {
            if (_isPaused || GameManager.Instance.CurrentState != GameState.Exploration)
            {
                return;
            }
         
            switch (_currentState)
            {
                case MoveState.Idle:
                    HandleIdle();
                    break;
                case MoveState.Move:
                    HandleMoving();
                    break;
            }
        }

        private void HandleMoving()
        {
            Flip();
            _animator.SetBool(IsMoving, true);
            transform.position = Vector2.MoveTowards(transform.position, _targetPos, 
                Time.deltaTime * _moveSpeed);
            if (Vector2.Distance(transform.position, _targetPos) < 0.1f)
            {
                _animator.SetBool(IsMoving, false);
                _currentState = MoveState.Idle;
                _idleTween = DOVirtual.DelayedCall(Random.Range(1, _maxIdleTime), GetTargetPos);
            }

        }

        private void HandleIdle()
        {
            _animator.SetBool(IsMoving, false);
        }

        private void Flip()
        {
            if (_targetPos.x > transform.position.x)
            {
                _visualTransform.localEulerAngles= new Vector3(_visualTransform.localEulerAngles.x, 0, _visualTransform.localEulerAngles.z);
            }
            else if (_targetPos.x < transform.position.x)
            {
                _visualTransform.localEulerAngles = new Vector3(_visualTransform.localEulerAngles.x, 180, _visualTransform.localEulerAngles.z);
            }
        }

        private void GetTargetPos()
        {
            _targetPos = _initialPos + (Random.insideUnitCircle * _wanderRadius);
            _currentState = MoveState.Move;
        }

        //Called from fungus
        public void PauseAI(bool isPaused)
        {
            _animator.SetBool(IsMoving, !isPaused);
            _isPaused = isPaused;

            if (isPaused)
            {
                _idleTween.Pause();
            }
            else
            {
                _idleTween.Play();
            }
        }
    }
}