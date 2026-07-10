using System.Collections.Generic;
using Battle;
using Data.CharacterData;
using Data.UnitDatabase;
using UnityEngine;
using System.Linq;

namespace Core
{
   public class PartyManager : MonoBehaviour
   { 
      [SerializeField] private UnitDatabaseSO databaseSo;
      public static PartyManager Instance;
      private readonly Dictionary<CharacterDataSo, List<BattleCharacterData>> _runtimePartyData = new();
      private HashSet<CharacterDataSo> _pickedUnits = new();

      public List<BattleCharacterData> PlayerPartyData = new List<BattleCharacterData>();
      public List<BattleCharacterData> EnemyPartyData = new List<BattleCharacterData>();
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

      public void InitializePartyData(CharacterDataSo pickedUnit)
      {
         _pickedUnits.Clear();
         EnemyPartyData.Clear();
         PlayerPartyData.Clear();
         PlayerPartyData = GetParty(pickedUnit);
         EnemyPartyData = GetParty();
      }

      private  List<BattleCharacterData> GetParty(CharacterDataSo includedCharacter = null, int maxPartySize = 2)
      {
         var result = new List<BattleCharacterData>();
         for (int i = 0; i < maxPartySize; i++)
         {
            if (includedCharacter && !_pickedUnits.Contains(includedCharacter))
            {
               var battleData = new BattleCharacterData(includedCharacter);
               result.Add(battleData);
               _pickedUnits.Add(includedCharacter);
               Debug.Log(battleData.CharacterDataSo.CharacterName);
               continue;
            }
            
            var filteredList = databaseSo.Database.Except(_pickedUnits).ToList();
            if (filteredList.Count <= 0)
            {
               Debug.LogError("Not enough units");
               return new List<BattleCharacterData>();
            }

            var pickedUnit = filteredList[Random.Range(0, filteredList.Count)];
            result.Add(new BattleCharacterData(pickedUnit));
            _pickedUnits.Add(pickedUnit);
            Debug.Log(pickedUnit.CharacterName);
         }
         return result;
      }
   }
}
