using Core;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Character.Enemy
{
    public class EnemyCombat : MonoBehaviour
    {
        private CharacterStat _stat;
        
        private void Awake()
        {
            _stat = GetComponent<CharacterStat>();
        }
        
        public void PickCombatMove()
        {
            DOVirtual.DelayedCall(.5f, () =>
            {
                var move = _stat.BaseData.Actions[Random.Range(0, _stat.BaseData.Actions.Count)];
                BattleManager.Instance.OnActionSelected(move);
                BattleManager.Instance.ExecuteActionCommand(BattleManager.Instance.GetRandomPlayerUnit());
            });
        }
    }
}