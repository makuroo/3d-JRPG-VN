using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Interface;
using UnityEngine;

namespace Battle
{
    public abstract class BattleCommand
    {
        [SerializeReference] protected List<IActionStep> _actionSteps;
        protected BattleCharacterData _source;
        protected BattleCharacterData _target;

        protected BattleCommand(BattleCharacterData source, BattleCharacterData target)
        {
            _source = source;
            _target = target;
        }

        public abstract IEnumerator Execute();
    }

    public class ActionCommand : BattleCommand
    {
        public ActionCommand(BattleCharacterData source, BattleCharacterData target, List<IActionStep> actionSteps) : base(source, target)
        {
            _source = source;
            _target = target;
            _actionSteps = actionSteps;
        }

        public override IEnumerator Execute()
        {
            foreach (var actionStep in _actionSteps)
            {
                yield return actionStep.Execute(_source, _target);
            }
        }
    }
}