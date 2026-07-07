using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Data.VfxData
{
    [System.Serializable]
    public class PoolableData
    {
        public GameObject prefab;
        public int count;
    }
    
    [CreateAssetMenu(fileName = "VfxData", menuName = "Scriptable Objects/VfxDatabase")]
    public class VfxData : SerializedScriptableObject
    {
        public Dictionary<string, PoolableData> VfxCollection = new();

        public GameObject GetPrefabById(string id)
        {
            return VfxCollection.TryGetValue(id, out var value) ? value.prefab : null;
        }
    }
}
