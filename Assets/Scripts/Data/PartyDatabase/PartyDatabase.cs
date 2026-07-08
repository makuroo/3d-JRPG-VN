using System.Collections.Generic;
using Data.CharacterData;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Data.UnitDatabase
{
    [CreateAssetMenu(fileName = "UnitDatabase", menuName = "Scriptable Objects/UnitDatabase")]
    public class UnitDatabaseSO : SerializedScriptableObject
    {
        public List<CharacterDataSo> Database = new();
    
    }
}
