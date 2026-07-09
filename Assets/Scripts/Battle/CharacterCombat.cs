using System;
using Character;
using Core;
using DG.Tweening;
using Interface;
using UI.Battle;
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

        public void TakeDamage(float damage)
        {
            if (_stat.Stat.CurrentHealth <=0) return;
            _stat.Stat.CurrentHealth -= damage;
            OnTakeDamage?.Invoke(_stat.Stat.MaxHealth, _stat.Stat.CurrentHealth);

            Debug.Log(_view.Owner.Team == Team.Player && (_stat.Stat.CurrentHealth / _stat.Stat.MaxHealth) < .3f);
            if (_view.Owner.Team == Team.Player && (_stat.Stat.CurrentHealth / _stat.Stat.MaxHealth) < .3f)
            {
                PostProcessManager.Instance.ActivateVignette(.3f,Color.red, 5f);
            }
            
            _view.PlayAnimation("Hit");
            
            if (_stat.Stat.CurrentHealth <= 0)
            {
                _view.PlayAnimation("Dead");
                OnDeath?.Invoke(_view.Owner);
            }
        }
    }
}
