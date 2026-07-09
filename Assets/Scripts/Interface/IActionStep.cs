using System.Collections;
using Battle;

namespace Interface
{
    public interface IActionStep
    {
        public IEnumerator Execute(BattleCharacterData source, BattleCharacterData target);
    }
}
