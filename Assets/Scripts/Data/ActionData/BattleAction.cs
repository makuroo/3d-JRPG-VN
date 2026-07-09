using System.Collections.Generic;
using Battle;
using Interface;
using UnityEngine;

namespace Data.ActionData
{
    [CreateAssetMenu(fileName = "BattleAction", menuName = "Scriptable Objects/Battle Action")]
    public class BattleAction : BattleActionSO
    {
        [SerializeReference] private List<IActionStep> _actionSteps = new();
        public override BattleCommand CreateBattleCommand(BattleCharacterData source, BattleCharacterData target)
        {
            return new ActionCommand(source, target, _actionSteps);
        }
    }
}
