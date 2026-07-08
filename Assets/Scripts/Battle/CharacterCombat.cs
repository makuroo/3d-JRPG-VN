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

        private void Start()
        {
            _stat = GetComponent<CharacterStat>();
            _view = GetComponentInChildren<BattleCharacterView>();
        }

        public void TakeDamage(float damage)
        {
            var sprite = _view.GetComponent<SpriteRenderer>();
            
            if (_stat.Stat.CurrentHealth <=0) return;
            _stat.Stat.CurrentHealth -= damage;
            _stat.OnStatChange?.Invoke();
           
            //Show damage popup
            // var popup =VfxManager.Instance.SpawnFromPool("DamagePopup", transform.position, 1f);
            // popup.GetComponent<DamagePopup>().Initialize(damage.ToString());
            // _view.PlayDamagedAudio();
            
            //unit alpha fade
            // sprite.DOFade(.5f, 0.2f).OnComplete(() =>
            // {
            //     sprite.DOFade(1, .2f);
            // });
            
            //punch backward
            //transform.DOPunchPosition(-transform.right,.3f,1);
            
            _view.PlayAnimation("Hit");
            
            if (_stat.Stat.CurrentHealth <= 0)
            {
                _view.PlayAnimation("Dead");
            }
        }
    }
}
