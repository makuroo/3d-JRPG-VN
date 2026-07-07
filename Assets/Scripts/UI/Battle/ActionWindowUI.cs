using System.Collections.Generic;
using Core;
using Data.ActionData;
using UnityEngine;

namespace UI.Battle
{
    public class ActionWindowUI : MonoBehaviour
    {
        [SerializeField] private ActionButton _actionButtonPrefab;
        [SerializeField] private Transform _actionButtonParent;
        public void PopulateWindow(List<BattleActionSO> actions)
        {
            foreach (Transform t in _actionButtonParent)
            {
                Destroy(t.gameObject);
            }
     
            foreach (BattleActionSO action in actions)
            {
                var button = Instantiate(_actionButtonPrefab, _actionButtonParent);
                button.Setup(action, OnSelectAction);
            }
        }

        private void OnSelectAction(BattleActionSO action)
        {
            if (BattleManager.Instance.SelectedBattleAction)
            {
                return;
            }
            BattleManager.Instance.OnActionSelected(action);
        }
    }
}