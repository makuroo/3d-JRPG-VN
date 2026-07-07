using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Data.SfxData
{
    [CreateAssetMenu(fileName = "SfxDatabase", menuName = "Scriptable Objects/SfxDatabase")]
    public class SfxDatabase : SerializedScriptableObject
    {
        public Dictionary<string, AudioClip[]> DataCollection = new();

        public AudioClip GetClipById(string id)
        {

            if (DataCollection.TryGetValue(id, out AudioClip[] clips))
            {
                return clips[Random.Range(0, DataCollection[id].Length)];
            }
            Debug.Log($"No clip of id {id}");
            return null;
        }
    }
}
