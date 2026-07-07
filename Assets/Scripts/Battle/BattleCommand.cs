using System.Collections;
using Character;
using DG.Tweening;
using UnityEngine;

namespace Battle
{
    public abstract class BattleCommand
    {
        protected BattleCharacterData Source;
        protected BattleCharacterData Target;

        protected BattleCommand(BattleCharacterData source, BattleCharacterData target)
        {
            Source = source;
            Target = target;
        }

        public abstract IEnumerator Execute();
    }

    public class BasicAttackCommand : BattleCommand
    {
        public BasicAttackCommand(BattleCharacterData source, BattleCharacterData target) : base(source, target)
        {
            Source = source;
            Target = target;
        }

        public override IEnumerator Execute()
        {
            var view = Source.BattleCharacterView;
            var originalPos = view.transform.position;
            var facingDirection = view.Owner.Team == Team.Player? Vector3.right : Vector3.left;
            var moveForwardTween =
                Source.BattleCharacterView.transform.DOMove(
                    view.transform.position + facingDirection * 3f, 0.2f);
        
            yield return moveForwardTween.WaitForCompletion();
            view.PlayAnimation("Attack");
            //make sure we enter attack state
            yield return null;
        
            yield return new WaitUntil(() => view.IsAnimationPlaying());
        
            var combat = Target.CharacterStat.GetComponent<CharacterCombat>();
            
            combat.TakeDamage(Source.Stat.Damage);
        
            var moveBackwardTween =
                Source.BattleCharacterView.transform.DOMove(
                    originalPos, 0.2f);
        
            yield return moveBackwardTween.WaitForCompletion();
            view.PlayAnimation("Idle");
        }
    }
}