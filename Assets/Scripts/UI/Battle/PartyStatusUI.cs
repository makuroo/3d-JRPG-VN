using System.Collections.Generic;
using Battle;
using Core;
using UnityEngine;

namespace UI.Battle
{
    public class PartyStatusUI : MonoBehaviour
    {
        [SerializeField] private CharacterStatusCard _statusCardPrefab;
        private List<BattleCharacterData> _party = new List<BattleCharacterData>();
        private void OnEnable()
        {
            BattleManager.Instance.OnPartyInitialized += SetupCard;
        }

        private void OnDisable()
        {
            BattleManager.Instance.OnPartyInitialized -= SetupCard;
        }
    
        private void SetupCard(List<BattleCharacterData> party)
        {
            _party = party;
            foreach (var data in _party)
            {
                var card = Instantiate(_statusCardPrefab,transform);
                card.Initialize(data);
            }
        }
    }
}
