using System;
using Character;
using Core;
using Interface;
using UnityEngine;

namespace Battle
{
    public class CharacterCombat : MonoBehaviour, IDamageable
    {
        private CharacterStat _stat;
        private BattleCharacterView _view;
        public Action<BattleCharacterData> OnDeath;
        public Action<float,float> OnTakeDamage;

        private void Start()
        {
            _stat = GetComponent<CharacterStat>();
            _view = GetComponentInChildren<BattleCharacterView>();
        }

        private void OnDisable()
        {
            OnDeath -= BattleManager.Instance.RemoveUnitFromQueue;
        }

        public void TakeDamage(float damage)
        {
            if (_stat.Stat.CurrentHealth <=0) return;
            _stat.Stat.CurrentHealth -= damage;
            OnTakeDamage?.Invoke(_stat.Stat.MaxHealth, _stat.Stat.CurrentHealth);
            
            if (_stat.Stat.CurrentHealth <= 0)
            {
                OnDeath?.Invoke(_view.Owner);
            }
        }
    }
}
