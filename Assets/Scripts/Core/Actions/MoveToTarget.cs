using System;
using System.Collections;
using System.Collections.Generic;
using Battle;
using DG.Tweening;
using Interface;
using UnityEngine;

namespace Core.Actions
{
    [System.Serializable]
    public class MoveToTarget:IActionStep
    {
        [SerializeField] private float _duration;
        [SerializeField] private float _stopRange;
        [SerializeField] private bool _toSpawnPos;
        public IEnumerator Execute(BattleCharacterData source, BattleCharacterData target)
        {
            var direction = Vector3.zero;
            direction   = source.SpawnPos - target.BattleCharacterView.transform.position;
            direction.y = 0;
            direction.Normalize();

            Tween moveTween;
            if (_toSpawnPos)
            {
                moveTween = source.BattleCharacterView.transform.DOLocalMove(source.SpawnPos, _duration);
            }
            else
            {
                moveTween = source.BattleCharacterView.transform.DOMove(
                    target.BattleCharacterView.transform.position + direction * _stopRange,
                    _duration);
            }
            
            yield return moveTween.WaitForCompletion();
        }
    }

    [System.Serializable]
    public class PlayAnimation : IActionStep
    {
        [SerializeField] private string _animationName;
        public IEnumerator Execute(BattleCharacterData source, BattleCharacterData target)
        {
            if (string.IsNullOrEmpty(_animationName))
            {
                Debug.LogError($"PlayAnimation called without animation name");
                yield break;
            }
            source.BattleCharacterView.PlayAnimation(_animationName);
      
            //make sure we have entered the animation state
            yield return null;
            yield return new WaitUntil(() => source.BattleCharacterView.IsAnimationCompleted());
        }
    }
    
    [Serializable]
    public class ParallelSteps : IActionStep
    {
        [SerializeReference] private List<IActionStep> _actionSteps;
        public IEnumerator Execute(BattleCharacterData source, BattleCharacterData target)
        {
            var runner = BattleManager.Instance;
            var coroutines = new List<Coroutine>();
            foreach (var step in _actionSteps)
            {
                var routine = runner.StartCoroutine(step.Execute(source, target));
                coroutines.Add(routine);
            }

            foreach (var routine in coroutines)
            {
                yield return routine;
            }
        }
    }
    
    [Serializable] 
    public class DamageWithDelay : IActionStep
    {
        [SerializeField] private float _delay;
        public IEnumerator Execute(BattleCharacterData source, BattleCharacterData target)
        {
            yield return new WaitForSeconds(_delay);
            target.BattleCharacterView.Owner.CharacterStat.GetComponent<CharacterCombat>()
                .TakeDamage(source.CharacterStat.Stat.Damage);
        }
    }
    
    [Serializable]
    public class SpawnVfx : IActionStep
    {
        [SerializeField] private string _vfxId;
        [SerializeField] private float _activeDuration;
        [SerializeField] private bool _targetSelf;
        public IEnumerator Execute(BattleCharacterData source, BattleCharacterData target)
        {
            if (string.IsNullOrEmpty(_vfxId))
            {
                Debug.LogError($"SpawnVfx called without VFX ID");
                yield break;
            }

            if (_targetSelf)
            {
                VfxManager.Instance.SpawnFromPool(_vfxId,source.BattleCharacterView.transform.position,_activeDuration);
            }
            else
            {
                VfxManager.Instance.SpawnFromPool(_vfxId,target.BattleCharacterView.transform.position,_activeDuration);
            }
            yield return new WaitForSeconds(_activeDuration);
        }
    }
}
