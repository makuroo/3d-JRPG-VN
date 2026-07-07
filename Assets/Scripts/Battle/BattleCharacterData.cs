using Character;
using Data.CharacterData;

namespace Battle
{
    public class BattleCharacterData
    {
        public CharacterDataSo CharacterDataSo { get; }
        public RuntimeStat Stat { get; }
        public BattleCharacterView BattleCharacterView { get; set; }
        public CharacterStat CharacterStat { get; set; }
    
        public Team Team { get; set; }

        public BattleCharacterData(CharacterDataSo dataSo)
        {
            CharacterDataSo = dataSo;
            Stat = new RuntimeStat(dataSo);
        }
    }
}
