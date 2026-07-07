using Core;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Battle
{
    public class BattleCharacterView : MonoBehaviour
    {
        [SerializeField] private Transform _selectedHighlight;
        [SerializeField] private Animator _animator;
        [SerializeField] private Button _selectButton;
        [SerializeField] private EventTrigger _triggerEvent;
        private bool _isClickable;
        public BattleCharacterData Owner { get; set; }

        private void Start()
        {
            _selectButton.onClick.AddListener((() =>
            {
                BattleManager.Instance.ExecuteActionCommand(Owner);
            }));
        }
    
        public void ShowHighlight(bool show)
        {
            _selectedHighlight.gameObject.SetActive(show);
        }

        public void SetSelectable(bool selectable)
        {
            if (!_triggerEvent)
            {
                Debug.LogWarning("Trigger Event not set",this);
                return;
            }
            _selectButton.interactable = selectable;
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
    }
}
