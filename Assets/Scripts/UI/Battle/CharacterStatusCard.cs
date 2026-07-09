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
        [SerializeField] private Image _hpFill;
        
        public void Initialize(BattleCharacterData data)
        {
            _nameText.text = data.CharacterDataSo.CharacterName;
            _hpText.text = data.Stat.CurrentHealth  + "/" + data.Stat.MaxHealth;
            _hpFill.fillAmount = data.Stat.CurrentHealth / data.Stat.MaxHealth;

            data.CharacterCombat.OnTakeDamage += UpdateStatusUI;
        }

        private void UpdateStatusUI(float maxHealth, float currentHealth)
        {
            _hpText.text = currentHealth  + "/" + maxHealth;
            _hpFill.fillAmount = currentHealth / maxHealth;
        }
    }
}
