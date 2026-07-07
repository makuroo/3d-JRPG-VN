using System.Collections.Generic;
using Character;
using Core;
using Data.CharacterData;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Battle
{
    public class UnitPlacer : SerializedMonoBehaviour
    {
        [SerializeField] private Dictionary<int, List<Transform>> _playerUnitStations;
        [SerializeField] private Dictionary<int, List<Transform>> _enemyUnitStations;
    
        private List<BattleCharacterData> _currentBattleCharacterData;
        private void OnEnable()
        {
            BattleManager.Instance.OnPrepareBattle += SetPlacement;
        }

        private void OnDisable()
        {
            BattleManager.Instance.OnPrepareBattle -= SetPlacement;
        }

        private void SetPlacement(CharacterDataSo playerData, CharacterDataSo enemyData)
        {
            var playerParty = PartyManager.Instance.GetParty(playerData);
            var enemyParty = PartyManager.Instance.GetParty(enemyData);
        
            //store mutated BattleCharacterData
            var playerUnits = PlaceUnits(playerParty, _playerUnitStations, Team.Player);
            var enemyUnits = PlaceUnits(enemyParty, _enemyUnitStations, Team.Enemy);
        
            BattleManager.Instance.InitializeParty(playerUnits,enemyUnits);
        }
    
        private List<BattleCharacterData> PlaceUnits(List<BattleCharacterData> party, Dictionary<int, List<Transform>> partyStations, Team team)
        {
            List<BattleCharacterData> spawnedUnits = new List<BattleCharacterData>();
            if (partyStations.TryGetValue(party.Count, out var stationList))
            {
                for (int i = 0; i < Mathf.Min(stationList.Count,party.Count); i++)
                { 
                    var unit = Instantiate(party[i].CharacterDataSo.CombatPrefab, stationList[i]);
            
                    if(unit == null) continue;
                    var unitStat = unit.GetComponent<CharacterStat>();
                    unitStat.SetRuntimeStat(party[i].Stat);
                
                    var unitView = unit.GetComponentInChildren<BattleCharacterView>();
                    unitView.Owner = party[i];
                
                    party[i].Team = team;
                    party[i].BattleCharacterView = unitView;
                    party[i].CharacterStat = unitStat;
                
                    spawnedUnits.Add(party[i]);
                }
            }  
        
            return spawnedUnits;
        }
    }
}
