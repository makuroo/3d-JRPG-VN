using Battle;
using UnityEngine;

namespace Data.ActionData
{
    [CreateAssetMenu(fileName = "AttackBattleAction", menuName = "Scriptable Objects/AttackBattleAction")]
    public class AttackBattleAction : BattleActionSO
    {
        public override BattleCommand CreateBattleCommand(BattleCharacterData source, BattleCharacterData target)
        {
            return new BasicAttackCommand(source, target);
        }
    }
}
