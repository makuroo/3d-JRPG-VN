using System.Collections.Generic;
using Battle;
using Data.CharacterData;
using Data.PartyDatabase;
using UnityEngine;

namespace Core
{
   public class PartyManager : MonoBehaviour
   { 
      [SerializeField] private PartyDatabaseSO databaseSo;
      public static PartyManager Instance;
      private readonly Dictionary<CharacterDataSo, List<BattleCharacterData>> _runtimePartyData = new();
      private void Awake()
      {
         if (Instance == null)
         {
            Instance = this;
         }
         else
         {
            Destroy(gameObject);
         }
         DontDestroyOnLoad(gameObject);
      }

      public List<BattleCharacterData> GetParty(CharacterDataSo leaderData)
      {
         if (_runtimePartyData.TryGetValue(leaderData, out var party)) return party;
      
         _runtimePartyData.Add(leaderData, new List<BattleCharacterData>());

         var partyCharacterSo = databaseSo.Database[leaderData];
         foreach (var partyData in partyCharacterSo)
         {
            var runtimeData = new BattleCharacterData(partyData);
            _runtimePartyData[leaderData].Add(runtimeData);
         }
      
         return _runtimePartyData[leaderData];
      }
   }
}
