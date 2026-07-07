using System.Collections.Generic;
using Data.VfxData;
using DG.Tweening;
using UnityEngine;

namespace Core
{
    public class VfxManager : MonoBehaviour
    {
        public static VfxManager Instance;
        [SerializeField] private VfxData _vfxDatabase;
        private Dictionary<string, Queue<GameObject>> _pool = new();

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

            foreach (var key in _vfxDatabase.VfxCollection.Keys)
            {
                var tempPool = new Queue<GameObject>();
                for (int i = 0; i < _vfxDatabase.VfxCollection[key].count; i++)
                {
                    var spawned = Instantiate(_vfxDatabase.VfxCollection[key].prefab, transform, true);
                    spawned.SetActive(false);
                    tempPool.Enqueue(spawned);
                }
                _pool.Add(key, tempPool);
            }
        }

        public GameObject SpawnFromPool(string id, Vector3 position, float duration = -1)
        {
            if (_pool.TryGetValue(id, out var pool))
            {
               var spawned = pool.Count > 0 ? pool.Dequeue() : Instantiate(_vfxDatabase.VfxCollection[id].prefab);
                
                spawned.transform.position = position;

                if (duration >= 0)
                {
                    DOVirtual.DelayedCall(duration, () => ReturnToPool(id, spawned));
                }
                
                spawned.SetActive(true);

                return spawned;
            }
            Debug.LogWarning($"Pool with id {id} was not found");
            return null;
        }

        private void ReturnToPool(string id, GameObject go)
        {
            if(go == null) return;
            
            go.SetActive(false);
            _pool[id].Enqueue(go);
        }
    }
}