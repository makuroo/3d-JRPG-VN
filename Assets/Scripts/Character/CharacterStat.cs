using System;
using Data.CharacterData;
using UnityEngine;
using UnityEngine.Serialization;

namespace Character
{
    public enum Team
    {
        Player,
        Enemy
    }

    public class RuntimeStat
    {
        public float MaxHealth;
        public float Speed;
        public float Damage;
        public float Def;
        private float _currentHealth;

        public float CurrentHealth
        {
            get => _currentHealth;
            set => _currentHealth = Mathf.Clamp(value, 0, MaxHealth);
        }
    
        public RuntimeStat(CharacterDataSo data)
        {
            MaxHealth = data.MaxHealth;
            Speed = data.Speed;
            Damage = data.Damage;
            Def = data.Def;
        
            CurrentHealth = data.MaxHealth;
        }
    
        public override string ToString() 
        {
            return $"Max Health: {MaxHealth}, Damage: {Damage}, Speed: {Speed}, CurrentHealth: {CurrentHealth}, Def: {Def}";
        }
    }

    public class CharacterStat : MonoBehaviour
    {
        [FormerlySerializedAs("baseData")] [SerializeField] private CharacterDataSo _baseData;
        private RuntimeStat _stat;
        public RuntimeStat Stat => _stat;

        public CharacterDataSo BaseData => _baseData;

        private void Awake()
        {
            _stat = new RuntimeStat(_baseData);
        }

        public void SetRuntimeStat(RuntimeStat stat)
        {
            _stat = stat;
        }
    }
}