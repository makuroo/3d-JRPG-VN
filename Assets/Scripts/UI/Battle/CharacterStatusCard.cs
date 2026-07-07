using Battle;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Battle
{
    public class CharacterStatusCard : MonoBehaviour
    {
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] protected  TMP_Text _hpText;
        [SerializeField] protected  TMP_Text _mpText;
        [SerializeField] private Image _hpFill;
        [SerializeField] private Image _mpFill;

        private BattleCharacterData _data;
        public void Initialize(BattleCharacterData data)
        {
            _data = data;
            _nameText.text = data.CharacterDataSo.CharacterName;
            _hpText.text = data.Stat.CurrentHealth  + "/" + data.Stat.MaxHealth;
            _hpFill.fillAmount = data.Stat.CurrentHealth / data.Stat.MaxHealth;

            _data.CharacterStat.OnStatChange += UpdateStatusUI;
        }

        private void UpdateStatusUI()
        {
            _hpText.text = _data.Stat.CurrentHealth  + "/" + _data.Stat.MaxHealth;
            _hpFill.fillAmount = _data.Stat.CurrentHealth / _data.Stat.MaxHealth;
        }
    }
}
