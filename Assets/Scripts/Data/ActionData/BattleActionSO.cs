using Battle;
using UnityEngine;

namespace Data.ActionData
{
    [CreateAssetMenu(fileName = "ActionData", menuName = "Scriptable Objects/ActionData")]
    public abstract class BattleActionSO : ScriptableObject
    {
        public string Name;
        public string Description;
        public abstract BattleCommand CreateBattleCommand(BattleCharacterData source, BattleCharacterData target);
    }
}
