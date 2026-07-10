using System;
using System.Collections.Generic;
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

        private void OnDisable()
        {
            Owner.CharacterCombat.OnDeath -= PlayDeathAnimation;
            Owner.CharacterCombat.OnTakeDamage -= PlayHitAnimation;
        }

        public void Initialize(BattleCharacterData owner)
        {
            Owner = owner;
            owner.CharacterCombat.OnDeath += PlayDeathAnimation;
            owner.CharacterCombat.OnTakeDamage += PlayHitAnimation;
        }

        public void FocusOnUnit(bool focus)
        {
            _unitCamera.Priority.Value = focus ? 10 : 0;
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
    
        public bool IsAnimationCompleted()
        {
            return _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && !_animator.IsInTransition(0);
        }

        private void ProcessCommand(PointerEventData data)
        {
            BattleManager.Instance.ExecuteActionCommand(Owner);
        }

        private void PlayDeathAnimation(BattleCharacterData _)
        {
            PlayAnimation("Dead");
        }

        private void PlayHitAnimation(float maxHealth, float currHealth)
        {
            PlayAnimation("Hit");
        }
    }
}
