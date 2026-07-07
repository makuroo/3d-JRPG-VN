using Core;
using Data.ActionData;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.Battle
{
    public class ActionButton : MonoBehaviour
    {
        [SerializeField] private TMP_Text _actionNameText;
        [SerializeField] protected Button _actionButton;
        [SerializeField] private Image _selectedIcon;
        private bool _isSelected;

        public void Setup(BattleActionSO actionData, UnityAction<BattleActionSO> onClick)
        {
            _actionNameText.text = actionData.Name;
            _actionButton.onClick.RemoveAllListeners();
            _actionButton.onClick.AddListener(()=>
            {
                onClick.Invoke(actionData);
                AudioManager.Instance.PlaySfx("ButtonClick",transform.position);
                _isSelected = true;
            });
        }

        public void ShowSelectedIcon()
        {
            _selectedIcon.gameObject.SetActive(true);
        }

        public void HideSelectedIcon()
        {
            if(_isSelected) return;
            _selectedIcon.gameObject.SetActive(false);
        }
    }
}