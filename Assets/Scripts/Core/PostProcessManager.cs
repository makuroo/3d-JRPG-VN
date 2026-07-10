using System;
using System.Collections.Generic;
using Battle;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Core
{
    public class PostProcessManager : MonoBehaviour
    {
        public static PostProcessManager Instance;

        [SerializeField] private Volume _volume;
        private Vignette _vignette;
        
        private Tween _vignetteStartTween;
        private Tween _vignetteEndTween;
        
        private List<BattleCharacterData> _subscribedCharacters = new List<BattleCharacterData>();
        
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

            if (_volume)
            {
                _volume.profile.TryGet(out _vignette);
            }

            BattleManager.Instance.OnPartyInitialized += SubscribeToPlayerUnits;
        }

        private void OnDisable()
        {
            BattleManager.Instance.OnPartyInitialized -= SubscribeToPlayerUnits;
            UnsubscribeAll();
        }

        public void ActivateVignette(float intensity, Color color, float duration)
        {
            _vignette.color.value = color;
           
            if (_vignetteStartTween == null)
            {
                _vignetteStartTween = DOVirtual.Float(0, intensity, 0.5f, value =>
                {
                    _vignette.intensity.value = value;
                }).SetAutoKill(false);
            }
            else
            {
                _vignetteStartTween.Restart();
            }
            
            if (duration > -1)
            {
                if (_vignetteEndTween == null)
                {
                    _vignetteEndTween = DOVirtual.DelayedCall(duration, () =>
                    {
                        DOVirtual.Float(_vignette.intensity.value, 0, 0.2f, value =>
                        {
                            _vignette.intensity.value = value;
                        });
                    }).SetAutoKill(false);
                }
                else
                {
                    _vignetteEndTween.Restart();
                }
            }
        }

        private void SubscribeToPlayerUnits(List<BattleCharacterData> playerParty)
        {
            UnsubscribeAll();
            foreach (var player in playerParty)
            {
                player.CharacterCombat.OnTakeDamage += TryApplyLowHpEffect;
                _subscribedCharacters.Add(player);
            }
        }

        private void TryApplyLowHpEffect(float maxHealth, float currHealth)
        {
            if (currHealth / maxHealth <= .3f)
            {
                ActivateVignette(.3f,Color.red, 3f);
            }
        }

        private void UnsubscribeAll()
        {
            foreach (var player in _subscribedCharacters)
            {
                player.CharacterCombat.OnTakeDamage -= TryApplyLowHpEffect;
            }
            _subscribedCharacters.Clear();
        }
    }
}
