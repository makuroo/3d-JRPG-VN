using Core;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Battle
{
    public class BattleCharacterView : MonoBehaviour
    {
        [SerializeField] private CinemachineCamera _unitCamera;
        [SerializeField] private Animator _animator;
        [SerializeField] private EventTrigger _triggerEvent;
        public BattleCharacterData Owner { get; set; }

        private void Start()
        {
            var entry = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerClick
            };
            
            entry.callback.AddListener((data) => { ProcessCommand((PointerEventData)data); });
            _triggerEvent.triggers.Add(entry);
        }
    
        public void FocusOnUnit(bool focus)
        {
            _unitCamera.Priority.Value = focus ? 10 : 0;
            Debug.Log($"{focus} is focusing, camera priority is {_unitCamera.Priority.Value}", this);
            //_selectedHighlight.gameObject.SetActive(show);
        }

        public void SetSelectable(bool selectable)
        {
            if (!_triggerEvent)
            {
                Debug.LogWarning("Trigger Event not set",this);
                return;
            }
            _triggerEvent.enabled = selectable;
        }

        public void PlayAnimation(string targetAnim)
        {
            _animator.Play(targetAnim);
        }
    
        public bool IsAnimationPlaying()
        {
            return _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && !_animator.IsInTransition(0);
        }

        public void PlayAttackAudio()
        {
            AudioManager.Instance.PlaySfx(Owner.CharacterDataSo.AttackSFX, transform.position);
        }

        public void PlayDamagedAudio()
        {
            AudioManager.Instance.PlaySfx(Owner.CharacterDataSo.DamagedSFX, transform.position);
        }

        private void ProcessCommand(PointerEventData data)
        {
            BattleManager.Instance.ExecuteActionCommand(Owner);
        }
    }
}
