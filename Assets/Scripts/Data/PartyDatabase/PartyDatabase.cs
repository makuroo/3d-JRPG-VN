using System.Collections.Generic;
using Data.CharacterData;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Data.PartyDatabase
{
    [CreateAssetMenu(fileName = "PartyDatabase", menuName = "Scriptable Objects/PartyDatabase")]
    public class PartyDatabaseSO : SerializedScriptableObject
    {
        public Dictionary<CharacterDataSo, List<CharacterDataSo>> Database = new();
    
    }
}
